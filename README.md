# FPSoundLib

FPSoundLib is a personal project aimed at learning and experimenting with C#, C++, Windows APIs, and audio rendering. This .NET audio library uses a C# wrapper around a C++/CLI WASAPI (Windows Audio Session API) implementation.

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
