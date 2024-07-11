using FPSoundLib;

namespace SoundLibTest
{
	internal class Program
	{
		static void Main(string[] args)
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
