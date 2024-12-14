using BachLib.Logging.Enums;
using FPSoundLib;
using FPSoundLib.Formats;

namespace SoundLibTest
{
	internal class Program
	{
		private static void Main()
		{
			try
			{
				using Player player = new(LogLevel.Debug);
				Thread.Sleep(1000);
				if (player.LoadFromFile("Resources/CantinaBandCompressed.wav") is not WavFile wavFile)
					return;
				wavFile.Metadata.AddTag("sfx");
				wavFile.Metadata.AddTag("music");
				Console.WriteLine(wavFile);
				// Testing the renderer thread
				Thread.Sleep(1000);
				Player.Play(wavFile);
				Thread.Sleep(10000);
			}
			catch (Exception e) when (e is NotSupportedException or FormatException or OperationCanceledException)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"\n{e.GetType()}: {e.Message} (0x{e.HResult:X})");
				Console.ResetColor();
			}

		}
	}
}