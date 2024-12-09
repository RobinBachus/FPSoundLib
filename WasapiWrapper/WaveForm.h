#pragma once
#include <cstdint>
#include <mmeapi.h>

#define WORD uint16_t
#define DWORD uint32_t

ref struct wave_form sealed
{
public:
	WORD wFormatTag = WAVE_FORMAT_PCM;
	WORD nChannels = 2;
	DWORD nSamplesPerSec = 44100;
	WORD wBitsPerSample = 16;
	WORD cbSize = 0;

	WAVEFORMATEX to_waveformatex();

	DWORD n_avg_bytes_per_sec()
	{
		return nSamplesPerSec * n_block_align();
	}

	WORD n_block_align()
	{
		return nChannels * wBitsPerSample / 8;
	}
};
