using BachLib.Logging;
using FPSoundLib.Utils;
using FPSoundLib.Utils.DLinkList;

namespace FPSoundLib
{
	public class Player
	{
		private readonly List<SoundFile> _soundFiles = [];
		private static renderer? _renderer;

		public Player()
		{
			Console.WriteLine("Loading fpsl...");
			Console.WriteLine("Loading WASAPI... \n");

			// Initialize the WASAPI wrapper
			int hr = wasapi_wrapper.init(true);
			if (hr != 0)
				throw new OperationCanceledException("Failed to initialize WASAPI, init cancelled") { HResult = hr };

			// Retrieve the renderer
			_renderer = wasapi_wrapper.get_renderer() ?? throw new NotSupportedException("Failed to get renderer, init cancelled") { HResult = hr };

			// Example of loading audio data into the renderer
			byte i = 0;
			_renderer.OnLoadNextChunkReady += (_, _) =>
			{
				// Load the next chunk of audio data
				_renderer.load_next_chunk([(byte)(++i % 9 + 48)]); // ASCII 0-9
			};

			// Uncomment to start the renderer with initial data
			_renderer.start([0]);

			Logger.Log("\nWASAPI loaded successfully.");
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

		public static void Dispose()
		{
			wasapi_wrapper.dispose();
			_renderer?.stop();
			_renderer?.Dispose();
		}

		~Player() => Dispose();
	}
}