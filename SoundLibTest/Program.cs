using FPSoundLib;

namespace SoundLibTest
{
	internal class Program
	{
		private static void Main()
		{
			try
			{
				Player player = new();
				if (player.LoadFromFile("tone.wav") is WavFile wavFile)
					Console.WriteLine(wavFile);
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
