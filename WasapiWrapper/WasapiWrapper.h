#pragma once

#include "pch.h"
#include <iostream>
#include <mmdeviceapi.h>
#include <functiondiscoverykeys_devpkey.h>
#include <propvarutil.h>
#include <combaseapi.h>
#include <Audioclient.h>

#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "propsys.lib")
#pragma comment(lib, "mmdevapi.lib")

using namespace System;

public ref class wasapi_wrapper sealed 
{
public:
	/**
	 * \brief Sets up the wasapi bridge
	 * \return 0 if successful, 1 if failed
	 */
	static int init();
	static void dispose();

	static bool initialized = false;

private:
	static IMMDevice* default_device_;
	static IAudioClient* audio_client_;

	static int log_device_info(IMMDevice* device);
};

