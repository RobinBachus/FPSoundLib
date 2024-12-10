using BachLib.Logging;
using BachLib.Logging.Enums;
using FPSoundLib.Utils;

namespace FPSoundLib
{
    public class Player : IDisposable
    {
        private readonly List<SoundFile> _soundFiles = [];
        private static renderer? _renderer;
        private bool _disposed;

        public static LogLevel LogLevel
        {
            get => Logger.Level;
            set => Logger.Level = value;
        }

        public Player(LogLevel logLevel = LogLevel.Error)
        {
            Logger.Level = logLevel;

            Logger.Log("Loading fpsl...");
            Logger.Log("Loading WASAPI... \n", LogLevel.Debug);


            // Initialize the WASAPI wrapper
            int hr = wasapi_wrapper.init(LogLevel < LogLevel.Info);
            if (hr != 0)
                throw new OperationCanceledException("Failed to initialize WASAPI, init cancelled") { HResult = hr };

            // Retrieve the renderer
            _renderer = wasapi_wrapper.get_renderer() ??
                        throw new NotSupportedException("Failed to get renderer, init cancelled") { HResult = hr };

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

            // TODO: Handle parsing errors
            _ = Enum.TryParse(ext, true, out FileType fileType);

            using FileStream file = File.OpenRead(path);
            byte[] fileBuffer = new byte[file.Length];

            // TODO: Handle file reading errors
            _ = file.Read(fileBuffer, 0, (int)file.Length);

            _soundFiles.Add(SoundFile.Create(fileBuffer, fileType, fileInfo));
            return _soundFiles.Last();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Dispose managed resources
                _soundFiles.Clear();
            }

            // Dispose unmanaged resources
            if (_renderer != null)
            {
                _renderer.stop();
                _renderer.Dispose();
                _renderer = null;
            }

            wasapi_wrapper.dispose();

            _disposed = true;
        }

        ~Player()
        {
            if (_disposed)
                return;

            Logger.Log("Warning: Player was not disposed properly.", LogLevel.Warn);
            Dispose(false);
        }
    }
}