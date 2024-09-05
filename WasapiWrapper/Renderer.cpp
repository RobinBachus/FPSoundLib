#include "pch.h"
#include "Renderer.h"

#include <iostream>
#include <ostream>


renderer::renderer(IMMDevice* device, IAudioClient* audio_client)
{
	device_ = device;
	audio_client_ = audio_client;

	IAudioRenderClient* render_client = nullptr;

	const HRESULT hr = audio_client_->GetService(__uuidof(IAudioRenderClient), reinterpret_cast<void**>(&render_client));

	if (FAILED(hr))
	{
		throw gcnew MemberAccessException("Failed to get audio render client");
	}
}

renderer::~renderer()
{
	if (render_client_)
	{
		render_client_->Release();
		render_client_ = nullptr;
	}
}

void renderer::start(array<byte>^ entry_chunk)
{
	load_next_chunk(entry_chunk);

	while (true)
	{
		if (!New_chunk_loaded)
			continue;

		Array::Clear(current_chunk_);
		current_chunk_ = gcnew array<byte>(next_chunk_->Length);
		next_chunk_->CopyTo(current_chunk_, 0);
		Array::Clear(next_chunk_);

		if (current_chunk_->Length == 0)
			break;

		if (current_chunk_[0] == 16)
			break;

		std::cout << std::hex << current_chunk_[0] % 0x10 << std::endl;

		OnLoadNextChunkReady(this, EventArgs::Empty);
	}
}

void renderer::stop()
{
}

void renderer::load_next_chunk(array<byte>^ chunk)
{
	next_chunk_ = chunk;
}

void renderer::on_load_next_chunk_ready()
{
	OnLoadNextChunkReady(this, EventArgs::Empty);
}
