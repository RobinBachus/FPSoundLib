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
				Player.Init();
				if (player.LoadFromFile(Path.GetFullPath("tone.wav")) is WavFile wavFile)
					Console.WriteLine(wavFile);
			}
			catch (NotImplementedException e)
			{
				Console.WriteLine(e.Message);
			}
			catch (NotSupportedException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
