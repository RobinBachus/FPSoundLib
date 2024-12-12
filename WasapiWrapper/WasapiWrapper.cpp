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

#define LOG(message) \
	if (logging_enabled_)\
		std::cout << message << std::endl;

HRESULT wasapi_wrapper::init(Nullable<bool> enable_log)
{
	IMMDeviceEnumerator* p_enumerator = nullptr;


	if (enable_log.HasValue)
		logging_enabled_ = enable_log.Value;
	

	// ReSharper disable CppInconsistentNaming
	const auto CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
	const auto IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
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

	if (logging_enabled_)
	{
		hr = log_device_info(default_device);
		EXIT_ON_ERROR(hr, "log_device_info");
	}

	IAudioClient* audio_client = nullptr;
	hr = audio_client_init(default_device, audio_client);
	EXIT_ON_ERROR(hr, "audio_client_init");

	audio_client_ = audio_client;

	initialized = true;

	return 0;
}

void wasapi_wrapper::dispose()
{
	if (!initialized) return;

	LOG("Disposing WasapiWrapper...");

	if (default_device_ != nullptr)
	{
		LOG("Releasing default device");
		SAFE_RELEASE(default_device_);
	}

	if (audio_client_ != nullptr)
	{
		LOG("Releasing audio client");
		SAFE_RELEASE(audio_client_);
	}

	LOG("WasapiWrapper disposed");
	initialized = false;
}

renderer^ wasapi_wrapper::get_renderer()
{
	if (!initialized)
		throw gcnew InvalidOperationException("WasapiWrapper not initialized");

	return gcnew renderer(default_device_, audio_client_);
}

HRESULT wasapi_wrapper::log_device_info(IMMDevice* device)
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

HRESULT wasapi_wrapper::audio_client_init(IMMDevice* device, IAudioClient*& audio_client)
{
	// ReSharper disable once CppInconsistentNaming
	const auto IID_IAudioClient = __uuidof(IAudioClient);

	REFERENCE_TIME default_period;
	REFERENCE_TIME minimum_period;
	WAVEFORMATEX* wave_format;

	HRESULT hr = device->Activate(IID_IAudioClient, CLSCTX_ALL, nullptr, reinterpret_cast<void**>(&audio_client));
	EXIT_ON_ERROR(hr, "ActivateAudioDevice");

	hr = audio_client->GetDevicePeriod(&default_period, &minimum_period);
	EXIT_ON_ERROR(hr, "GetDevicePeriod");

	hr = audio_client->GetMixFormat(&wave_format);
	EXIT_ON_ERROR(hr, "GetMixFormat");

	hr = audio_client->Initialize(AUDCLNT_SHAREMODE_SHARED, AUDCLNT_STREAMFLAGS_EVENTCALLBACK, minimum_period, 0,
	                              wave_format, nullptr);
	EXIT_ON_ERROR(hr, "InitializeAudioClient");

	return 0;
}
