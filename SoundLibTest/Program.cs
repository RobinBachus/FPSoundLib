using BachLib.Logging.Enums;
using FPSoundLib;
using FPSoundLib.Formats;
using FPSoundLib.Utils.DLinkList;

namespace SoundLibTest
{
	internal class Program
	{
		private static void Main()
		{
			try
			{
				using Player player = new(LogLevel.Debug);
				Thread.Sleep(1000);
				if (player.LoadFromFile("Resources/click.wav") is not WavFile wavFile)
					return;
				wavFile.Metadata.AddTag("sfx");
				wavFile.Metadata.AddTag("music");
				Console.WriteLine(wavFile);

				Dictionary<byte, int> byteCount = new();
				foreach (Node<byte[]> b in wavFile.Data.Samples)
				{
					foreach (byte bb in b.Value)
					{
						if (!byteCount.TryAdd(bb, 1))
							byteCount[bb]++;
					}
				}

				foreach (KeyValuePair<byte, int> kvp in byteCount)
				{
					Console.WriteLine($"{kvp.Key:X}; {kvp.Value}");
				}

				// Testing the renderer thread
				Thread.Sleep(1000);
				Player.Play(wavFile);
				Thread.Sleep(10000);
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