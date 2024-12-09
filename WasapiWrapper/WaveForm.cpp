#include "pch.h"
#include "WaveForm.h"

WAVEFORMATEX wave_form::to_waveformatex()
{
	WAVEFORMATEX wave_format = { 0 };
	wave_format.wFormatTag = this->wFormatTag;
	wave_format.nChannels = this->nChannels;
	wave_format.nSamplesPerSec = this->nSamplesPerSec;
	wave_format.nAvgBytesPerSec = this->n_avg_bytes_per_sec();
	wave_format.nBlockAlign = this->n_block_align();
	wave_format.wBitsPerSample = this->wBitsPerSample;
	wave_format.cbSize = this->cbSize;

	return wave_format;
}
