using FPSoundLib.Utils;
using FPSoundLib.Utils.DLinkList;

namespace FPSoundLib
{
	public class Player
	{
		private readonly List<SoundFile> _soundFiles = new();

		public static void Init()
		{
			Console.WriteLine("Loading fpsl");
			// Console.WriteLine(wasapi_wrapper.test() );

			DLinkList<int> list = new()
			{
				1,
				2,
				3
			};

			foreach (var node in list)
			{
				Console.WriteLine(node.Data);
			}

			Console.WriteLine("\nAdding 4, 5, 6\n");

			// BUG: Original list when setting first element
			
			list[0] = new Node<int>(4);
			list[1] = new Node<int>(5);
			list[2] = new Node<int>(6);

			foreach (var node in list)
			{
				Console.WriteLine(node.Data);
			}

		}

		public SoundFile LoadFromFile(string path)
		{
			string ext = new FileInfo(path).Extension.Remove(0, 1);

			_ = Enum.TryParse(ext, true, out FileType fileType);

			FileStream file = File.OpenRead(path);
			byte[] fileBuffer = new byte[file.Length];
			_ = file.Read(fileBuffer, 0, (int)file.Length);
			file.Dispose();

			_soundFiles.Add(SoundFile.Create(fileBuffer, fileType));
			return _soundFiles.Last();

		}

	}
}