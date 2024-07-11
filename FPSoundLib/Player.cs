namespace FPSoundLib
{
	public class Player
	{
		private List<SoundFile> _soundFiles = new List<SoundFile>();

		public static void Init()
		{
			Console.WriteLine("Loading fpsl");
		}

		public WavFile LoadWavFile(string path)
		{
			byte[] fileBuffer;
			try
			{
				fileBuffer = File.ReadAllBytes(path);
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
			_soundFiles.Add(new WavFile(fileBuffer));
			return (WavFile)_soundFiles.Last();
		}
	}
}
