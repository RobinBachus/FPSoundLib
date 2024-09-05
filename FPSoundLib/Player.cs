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

			renderer? renderer = wasapi_wrapper.get_renderer() ?? throw new NotSupportedException("Failed to get renderer, init cancelled") { HResult = hr };
			
			byte i = 0;
			renderer.OnLoadNextChunkReady += (sender, e) =>
			{
				renderer.load_next_chunk(new byte[1] {++i});
			};

			renderer.start(new byte[1] { 0 });

			Console.WriteLine("\nWASAPI loaded successfully.");
		}

		public SoundFile LoadFromFile(string path)
		{
			path = Path.GetFullPath(path);
			FileInfo fileInfo = new(path);

			string ext = fileInfo.Extension.Remove(0, 1);

			_ = Enum.TryParse(ext, true, out FileType fileType);

			FileStream file = File.OpenRead(path);
			byte[] fileBuffer = new byte[file.Length];
			_ = file.Read(fileBuffer, 0, (int)file.Length);
			file.Dispose();

			_soundFiles.Add(SoundFile.Create(fileBuffer, fileType, fileInfo));
			return _soundFiles.Last();

		}
		
		public static void Dispose() => wasapi_wrapper.dispose();

		~Player() => Dispose();
		
	}
}