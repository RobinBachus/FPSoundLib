#include "pch.h"

#include "WasapiWrapper.h"
#include "iostream"

int wasapi_wrapper::test()
{
	std::cout << "Bridge to sound-card created!" << std::endl;
	return 0;
}

int wasapi_wrapper::init()
{
	return 0;
}
