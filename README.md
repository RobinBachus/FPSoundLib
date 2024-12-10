# FPSoundLib

FPSoundLib is a personal project aimed at learning and experimenting with C#, C++, Windows APIs, and audio rendering. This .NET audio library uses a C# wrapper around a C++/CLI WASAPI (Windows Audio Session API) implementation.

This project is so far from complete that it doesn't do the primary thing it was designed to do: play audio. However, it does have a basic file loader for WAV files and a rudimentary metadata system. 

On the c++ side, it has the beginnings of a WASAPI implementation. It can create a WASAPI audio client and has a thread that signals when data should be loaded to the renderer.

## Features

- C# wrapper for easy integration with .NET applications.
- Utilizes C++/CLI for high-performance audio processing.
- Implements WASAPI for audio rendering on Windows.

## Usage
Here is a basic example of how to use FPSoundLib in your C# project:

```C#
using FPSoundLib;
using FPSoundLib.Formats;

class Program
{
    static void Main()
    {
        using Player player = new();
        if (player.LoadFromFile("Resources/CantinaBandCompressed.wav") is not WavFile wavFile)
            return;
        wavFile.Metadata.AddTag("sfx");
        wavFile.Metadata.AddTag("music");
        Console.WriteLine(wavFile);
    }
}
```

In this example, we create a new `Player` and load a WAV file from disk. We then print the formatted data of the loaded file.

<details>

<summary>Example output</summary>

```yml
Summary of file AWAAYKHC (CantinaBandCompressed.wav):
Metadata:
        Tags: sfx, music
        FileType: Wav
        FileName: CantinaBandCompressed.wav
        FileSize: 46 KB
        FilePath: D:\example\absolute\filepath\CantinaBandCompressed.wav
ChunkID: RIFF
ChunkSize: 48070
Format: WAVE
FormatMarker: fmt
FormatDataLength: 16
AudioFormat: 1
NumChannels: 1
SampleRate: 8000
ByteRate: 16000
BlockAlign: 2
BitsPerSample: 16
InfoChunk:
        ISFT: Lavf61.1.100
DataChunkHeader: data
DataSize: 48000
DataChunk:
        04 00  04 00  05 00  05 00  02 00  01 00  00 00  00 00  00 00  FF FF  FD FF  FE FF  FB FF  FC FF  FC FF  F8 FF
        F9 FF  F6 FF  F4 FF  F5 FF  F4 FF  F4 FF  F6 FF  F1 FF  F0 FF  F1 FF  F0 FF  F1 FF  EF FF  EE FF  EE FF  ED FF
        EC FF  EB FF  EA FF  E9 FF  E9 FF  E7 FF  E8 FF  E8 FF  E7 FF  E8 FF  E9 FF  E9 FF  E9 FF  E9 FF  E8 FF  E9 FF
        EA FF  E9 FF  EB FF  EB FF  EC FF  ED FF  EE FF  ED FF  F0 FF  F0 FF  F3 FF  F4 FF  F3 FF  F5 FF  F5 FF  F5 FF
        F7 FF  F8 FF  F6 FF  F8 FF  F5 FF  F6 FF  F8 FF  F7 FF  F8 FF  F7 FF  F5 FF  F4 FF  F3 FF  F1 FF  F6 FF  F4 FF
        F3 FF  F4 FF  F2 FF  F2 FF  F2 FF  F1 FF  F2 FF  F1 FF  F0 FF  F1 FF  F1 FF  EE FF  F0 FF  F0 FF  EF FF  F2 FF
        EF FF  EE FF  F1 FF  F1 FF  F0 FF  F1 FF  F0 FF  F0 FF  F0 FF  EE FF  F0 FF  EF FF  F0 FF  F2 FF  F0 FF  F1 FF
        F1 FF  F0 FF  F2 FF  F3 FF  F4 FF  F6 FF  F7 FF  F6 FF  F8 FF  F8 FF  F9 FF  F9 FF  F8 FF  FB FF  FA FF  FA FF
        FB FF  FC FF  FC FF  FE FF  00 00  00 00  03 00  04 00  03 00  03 00  01 00  03 00  05 00  05 00  06 00  07 00
        06 00  07 00  08 00  07 00  0A 00  07 00  08 00  09 00  07 00  08 00  08 00  07 00  08 00  09 00  08 00  09 00
        0B 00  0B 00  0B 00  0A 00  0A 00  0A 00  0A 00  09 00  0A 00  09 00  0A 00  0B 00  07 00  0A 00  08 00  08 00
        08 00  07 00  05 00  05 00  07 00  01 00  03 00  00 00  01 00  02 00  03 00  02 00  03 00  03 00  02 00  04 00
        04 00  05 00  04 00  04 00  06 00  05 00  05 00  06 00  06 00  09 00  08 00  0A 00  0A 00  0A 00  0A 00  08 00
        09 00  08 00  0A 00  08 00  08 00  09 00  06 00  06 00  06 00  07 00  07 00  06 00  04 00  03 00  02 00  01 00
        01 00  FF FF  FD FF  FB FF  FC FF  F9 FF  F8 FF  F6 FF  F7 FF  F6 FF  F2 FF  F4 FF  F2 FF  EF FF  F1 FF  F0 FF
        F0 FF  EF FF  EE FF  EF FF  F1 FF  F0 FF  F3 FF  F0 FF  F3 FF  F3 FF  F3 FF  F6 FF  F6 FF  F5 FF  F4 FF  F6 FF
        F6 FF  F7 FF  F5 FF  F6 FF  F6 FF  FA FF  FA FF  FA FF  FD FF  FC FF  FE FF  FC FF  FE FF  02 00  02 00  01 00
        03 00  03 00  04 00  07 00  05 00  0A 00  0A 00  09 00  0C 00  0D 00  0E 00  0F 00  10 00  10 00  11 00  10 00
        11 00  14 00  13 00  13 00  16 00  15 00  19 00  19 00  19 00  19 00  1A 00  1B 00  1F 00  1D 00  1D 00  1D 00
        1D 00  1F 00  1E 00  1F 00  22 00  21 00  22 00  20 00  22 00  21 00  22 00  22 00  23 00  25 00  22 00  22 00
        ...

```
</details>
