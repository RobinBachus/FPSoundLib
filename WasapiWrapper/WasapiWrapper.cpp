#include "pch.h"
#include "WasapiWrapper.h"

#define EXIT_ON_ERROR(hr, process)  \
			  if (FAILED(hr)) { \
				  std::cout << process << " Failed with HRESULT: " << std::hex << hr << std::endl; \
				  return hr; \
			  }

#define SAFE_RELEASE(punk)  \
			  if ((punk) != NULL)  \
				{ (punk)->Release(); (punk) = NULL; }

int wasapi_wrapper::init()
{
	IMMDeviceEnumerator* p_enumerator = nullptr;

	REFERENCE_TIME default_period;
	REFERENCE_TIME minimum_period;
	WAVEFORMATEX* mix_format;


	// ReSharper disable CppInconsistentNaming
	const auto CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
	const auto IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
	const auto IID_IAudioClient = __uuidof(IAudioClient);
	// ReSharper restore CppInconsistentNaming

	HRESULT hr = CoCreateInstance(
		CLSID_MMDeviceEnumerator, nullptr,
		CLSCTX_ALL, IID_IMMDeviceEnumerator,
		reinterpret_cast<void**>(&p_enumerator));

	EXIT_ON_ERROR(hr, "CoCreateInstance");

	IMMDevice* default_device = nullptr;
	hr = p_enumerator->GetDefaultAudioEndpoint(eRender, eConsole, &default_device);
	EXIT_ON_ERROR(hr, "EnumAudioEndpoints");

	default_device_ = default_device;

	log_device_info(default_device);

	IAudioClient* audio_client = nullptr;
	hr = default_device->Activate(IID_IAudioClient, CLSCTX_ALL, nullptr, reinterpret_cast<void**>(&audio_client));
	EXIT_ON_ERROR(hr, "ActivateAudioDevice");

	audio_client_ = audio_client;

	hr = audio_client_->GetDevicePeriod(&default_period, &minimum_period);
	EXIT_ON_ERROR(hr, "GetDevicePeriod");

	hr = audio_client_->GetMixFormat(&mix_format);
	EXIT_ON_ERROR(hr, "GetMixFormat");

	hr = audio_client_->Initialize(AUDCLNT_SHAREMODE_SHARED, 0, minimum_period, 0, mix_format, nullptr);
	EXIT_ON_ERROR(hr, "InitializeAudioClient");

	initialized = true;

	return 0;
}

void wasapi_wrapper::dispose()
{
	if (!initialized) return;

	std::cout << "Disposing WasapiWrapper..." << std::endl;

	if (default_device_ != nullptr)
	{
		std::cout << "Releasing default device" << std::endl;
		SAFE_RELEASE(default_device_);
	}

	if (audio_client_ != nullptr)
	{
		std::cout << "Releasing audio client" << std::endl;
		SAFE_RELEASE(audio_client_);
	}

	std::cout << "WasapiWrapper disposed" << std::endl;
	initialized = false;
}

int wasapi_wrapper::log_device_info(IMMDevice* device)
{
	IPropertyStore* p_props = nullptr;
	LPWSTR pwsz_id = nullptr;

	HRESULT hr = device->GetId(&pwsz_id);
	EXIT_ON_ERROR(hr, "GetId");

	hr = device->OpenPropertyStore(STGM_READ, &p_props);
	EXIT_ON_ERROR(hr, "OpenPropertyStore");

	PROPVARIANT device_name;
	PropVariantInit(&device_name);

	hr = p_props->GetValue(PKEY_Device_FriendlyName, &device_name);
	EXIT_ON_ERROR(hr, "GetValue");

	std::wcout << "Endpoint ID: " << pwsz_id << std::endl;
	std::wcout << "Endpoint name: " << device_name.pwszVal << std::endl;


	CoTaskMemFree(pwsz_id);
	pwsz_id = nullptr;
	auto _ = PropVariantClear(&device_name);
	SAFE_RELEASE(p_props)
	return 0;
}
