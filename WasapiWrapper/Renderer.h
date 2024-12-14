#pragma once

#include <Audioclient.h>
#include <mmdeviceapi.h>

using namespace System;


public ref class renderer sealed
{
public:
	bool started = false;

	renderer(IMMDevice* device, IAudioClient* audio_client);
	~renderer();

	void start();
	void stop();

	void load_next_chunk(array<byte>^ chunk);

	event EventHandler^ OnLoadNextChunkReady;

private:
	IMMDevice* device_;
	IAudioClient* audio_client_;
	IAudioRenderClient* render_client_;
	HANDLE render_event_;

	Threading::Mutex^ mtx_;
	Threading::AutoResetEvent^ cv_;

	Threading::Thread^ render_thread_;

	array<byte>^ current_chunk_ = gcnew array<byte>(0);
	array<byte>^ next_chunk_ = gcnew array<byte>(0);

	bool new_chunk_loaded = false;

	void on_load_next_chunk_ready();
	void render_audio_chunk(array<byte>^ chunk);

	void start_thread();
};


