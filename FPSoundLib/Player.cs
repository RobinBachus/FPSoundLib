using FPSoundLib.Utils;
using FPSoundLib.Utils.DLinkList;

namespace FPSoundLib
{
	public class Player
	{
		private readonly List<SoundFile> _soundFiles = new();

		public Player()
		{
			Console.WriteLine("Loading fpsl...");
			Console.WriteLine("Loading WASAPI... \n");
			int hr = wasapi_wrapper.init(true);

			if (hr != 0) 
				throw new OperationCanceledException("Failed to initialize WASAPI, init cancelled") {HResult = hr};

			Console.WriteLine("\nWASAPI loaded successfully.");
		}

		public SoundFile LoadFromFile(string path)
		{
			path = Path.GetFullPath(path);

			string ext = new FileInfo(path).Extension.Remove(0, 1);

			_ = Enum.TryParse(ext, true, out FileType fileType);

			FileStream file = File.OpenRead(path);
			byte[] fileBuffer = new byte[file.Length];
			_ = file.Read(fileBuffer, 0, (int)file.Length);
			file.Dispose();

			_soundFiles.Add(SoundFile.Create(fileBuffer, fileType));
			return _soundFiles.Last();

		}
		
		public void Dispose() => wasapi_wrapper.dispose();

		~Player() => Dispose();
		
	}
}