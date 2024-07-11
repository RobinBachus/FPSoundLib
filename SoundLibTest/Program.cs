using FPSoundLib;

namespace SoundLibTest
{
	internal class Program
	{
		private static void Main()
		{
			Player player = new();
			try
			{
				WavFile wavFile = player.LoadWavFile(Path.GetFullPath("tone.wav"));
				Console.WriteLine(wavFile);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("File not found.");
			}
		}
	}
}
