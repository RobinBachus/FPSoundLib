#include "pch.h"
#include "Renderer.h"

#include <iostream>
#include <ostream>

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
        DWORD lerror = GetLastError();
        std::cout << "Failed to create event handle: " << lerror << std::endl;
        throw gcnew InvalidOperationException("Failed to create event handle");
    }

    // Set the event handle
    HRESULT hr = audio_client_->SetEventHandle(render_event_);
    EXIT_ON_ERROR(hr, "Failed to set event handle");

    // Get the render client
    IAudioRenderClient* render_client = nullptr;
    hr = audio_client_->GetService(__uuidof(IAudioRenderClient), (void**)&render_client);
    EXIT_ON_ERROR(hr, "Failed to get render client");

    render_client_ = render_client;
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
}

void renderer::start(array<byte>^ entry_chunk)
{
    load_next_chunk(entry_chunk);

    HRESULT hr = audio_client_->Start();
    if (FAILED(hr))
    {
        throw gcnew InvalidOperationException("Failed to start audio client");
    }

    while (true)
    {
        if (!new_chunk_loaded)
            continue;

        // Copy next chunk to current chunk
        current_chunk_ = gcnew array<byte>(next_chunk_->Length);
        next_chunk_->CopyTo(current_chunk_, 0);
        new_chunk_loaded = false;

        if (current_chunk_->Length == 0)
            break;

        // Render the current chunk
        render_audio_chunk(current_chunk_);

        if (current_chunk_[0] == 16)
            break;

        OnLoadNextChunkReady(this, EventArgs::Empty);
    }

    hr = audio_client_->Stop();
    if (FAILED(hr))
    {
        throw gcnew InvalidOperationException("Failed to stop audio client");
    }
}

void renderer::stop()
{
    HRESULT hr = audio_client_->Stop();
    if (FAILED(hr))
    {
        throw gcnew InvalidOperationException("Failed to stop audio client");
    }
}

void renderer::load_next_chunk(array<byte>^ chunk)
{
    next_chunk_ = chunk;
    new_chunk_loaded = true;
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
    pin_ptr<byte> pinned_chunk = &chunk[0];
    memcpy(data, pinned_chunk, chunk->Length);

    hr = render_client_->ReleaseBuffer(buffer_frame_count, 0);
    if (FAILED(hr))
    {
        throw gcnew InvalidOperationException("Failed to release buffer");
    }
}
