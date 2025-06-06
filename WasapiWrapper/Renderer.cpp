#include "pch.h"
#include "Renderer.h"

#include <iostream>

#define EXIT_ON_ERROR(hr, message)  \
	if (FAILED(hr)) { \
		std::cout << " Failed with HRESULT: " << std::hex << hr << std::endl; \
		throw gcnew InvalidOperationException(gcnew String(message)); \
	}

renderer::renderer(IMMDevice* device, IAudioClient* audio_client)
{
	audio_client_ = audio_client;

	// Create an event handle for event-driven buffering
	render_event_ = CreateEventW(nullptr, FALSE, FALSE, nullptr);
	if (render_event_ == nullptr)
	{
		const DWORD l_error = GetLastError();
		std::cout << "Failed to create event handle: " << l_error << std::endl;
		throw gcnew InvalidOperationException("Failed to create event handle");
	}

	// Set the event handle
	HRESULT hr = audio_client_->SetEventHandle(render_event_);
	EXIT_ON_ERROR(hr, "Failed to set event handle");

	// Get the render client
	IAudioRenderClient* render_client = nullptr;
	hr = audio_client_->GetService(__uuidof(IAudioRenderClient), reinterpret_cast<void**>(&render_client));
	EXIT_ON_ERROR(hr, "Failed to get render client");

	render_client_ = render_client;

	mtx_ = gcnew Threading::Mutex();
	cv_ = gcnew Threading::AutoResetEvent(false);
}

renderer::~renderer()
{
	if (render_client_)
	{
		render_client_->Release();
		render_client_ = nullptr;
	}

	if (render_event_)
	{
		CloseHandle(render_event_);
		render_event_ = nullptr;
	}

	if (render_thread_)
	{
		started = false;
		render_thread_->Join();
	}
}

void renderer::start()
{
	render_thread_ = gcnew Threading::Thread(gcnew Threading::ThreadStart(this, &renderer::start_thread));
	render_thread_->Name = "renderer";
	render_thread_->Start();

	started = true;

	if (FAILED(audio_client_->Start()))
	{
		throw gcnew InvalidOperationException("Failed to start audio client");
	}

	
}

void renderer::stop()
{
	started = false;
	if (FAILED(audio_client_->Stop()))
	{
		throw gcnew InvalidOperationException("Failed to stop audio client");
	}
}

// TODO: Pass metadata here
void renderer::load_next_chunk(array<byte>^ chunk)
{
	mtx_->WaitOne();
	next_chunk_ = chunk;
	new_chunk_loaded = true;
	cv_->Set();
	mtx_->ReleaseMutex();
}

void renderer::on_load_next_chunk_ready()
{
	OnLoadNextChunkReady(this, EventArgs::Empty);
}

void renderer::render_audio_chunk(array<byte>^ chunk)
{
	UINT32 buffer_frame_count;
	HRESULT hr = audio_client_->GetBufferSize(&buffer_frame_count);
	if (FAILED(hr))
	{
		throw gcnew InvalidOperationException("Failed to get buffer size");
	}

	BYTE* data;
	hr = render_client_->GetBuffer(buffer_frame_count, &data);
	if (FAILED(hr))
	{
		throw gcnew InvalidOperationException("Failed to get buffer");
	}

	// Copy the chunk data to the buffer
	const pin_ptr<byte> pinned_chunk = &chunk[0];
	memcpy(data, pinned_chunk, chunk->Length);

	hr = render_client_->ReleaseBuffer(buffer_frame_count, 0);
	if (FAILED(hr))
	{
		throw gcnew InvalidOperationException("Failed to release buffer");
	}
}


// TODO: Move thread to Bridge or separate class
void renderer::start_thread()
{
	while (started)
	{
		mtx_->WaitOne();
		if (new_chunk_loaded)
		{
			current_chunk_ = next_chunk_;
			new_chunk_loaded = false;
			mtx_->ReleaseMutex();

			if (current_chunk_->Length != 0)
			{
				// TODO: Use metadata to determine how to parse data: 8-bit mono, 8-bit stereo, 16-bit mono, 16-bit stereo
				// See load_next_chunk TODO.
				// Metadata needed: bits per sample, channels, block align, sample rate

				// This only works for 16-bit mono data (out of bounds with 8 mono)
				for (int i = 0; i < current_chunk_->Length; i += 2)
				{
					// Parse PCM data (little endian)
					const int16_t sample = static_cast<int16_t>((current_chunk_[i + 1] << 8) | current_chunk_[i]);
					std::wcout << sample << std::endl;
				}

				on_load_next_chunk_ready();
				Sleep(50);
			}
		}
		else
		{
			mtx_->ReleaseMutex();
			cv_->WaitOne();
		}
	}
}
