#pragma once

#include "pch.h"
#include <iostream>
#include <mmdeviceapi.h>
#include <functiondiscoverykeys_devpkey.h>
#include <propvarutil.h>
#include <combaseapi.h>
#include <Audioclient.h>

#include "Renderer.h"

#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "propsys.lib")
#pragma comment(lib, "mmdevapi.lib")

using namespace System;
using namespace System::Runtime::InteropServices;

public ref class wasapi_wrapper sealed 
{
public:
	/**
	 * \brief Sets up the wasapi bridge
	 * \param enable_log Whether to log information during initialization.
	 *	Default is false
	 * \return 0 if successful, otherwise an error code
	 */
	static HRESULT init([Optional] Nullable<bool> enable_log);
	static void dispose();

	/**
	 * \brief Gets a renderer object used for streaming audio
	 * \throws InvalidOperationException if the wasapi bridge has not been initialized
	 * \return A renderer instance
	 */
	static renderer^ get_renderer();

	static bool initialized = false;

private:
	static IMMDevice* default_device_;
	static IAudioClient* audio_client_;
	static bool logging_enabled_ = false;

	[[nodiscard]] static HRESULT log_device_info(IMMDevice* device);
	[[nodiscard]] static HRESULT audio_client_init(IMMDevice* device, IAudioClient*& audio_client);
};

