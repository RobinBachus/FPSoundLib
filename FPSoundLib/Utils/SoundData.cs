using FPSoundLib.Utils.DLinkList;

namespace FPSoundLib.Utils
{
	public readonly struct SoundData
	{
		public int TotalBytes { get; }
		public int SampleRate { get; }
		public int Channels { get; }
		public int BitsPerSample { get; }
		public int BlockAlign { get; }

		public DLinkList<byte[]> Samples { get; }

		public SoundData(int totalBytes, int sampleRate, int channels, int bitsPerSample, int blockAlign, IReadOnlyList<byte> data)
		{
			TotalBytes = totalBytes;
			SampleRate = sampleRate;
			Channels = channels;
			BitsPerSample = bitsPerSample;
			BlockAlign = blockAlign;


			Samples = new DLinkList<byte[]>();
			for (int i = 0; i < data.Count; i += blockAlign)
			{
				byte[] sample = new byte[blockAlign];
				for (int j = 0; j < blockAlign; j++)
				{
					sample[j] = data[i + j];
				}

				Samples.Add(sample);
			}
		}

		public override string ToString()
		{
			string str = "";

			int i = 0;
			foreach (Node<byte[]> sample in Samples)
			{
				if (i >= (16 * 20)) 
					return $"{str}..."; 

				str = sample.Value.Aggregate(str, (current, b) => current + $"{b:X2} ");
				
				str += ++i % 16 == 0 ? "\n" : " ";
			}

			return str;
		}

		public void PrintSamples(CancellationToken? token = null)
		{
			int i = 0;
			foreach (Node<byte[]> sample in Samples)
			{
				if (token?.IsCancellationRequested ?? false)
					break;
				string str = sample.Value.Aggregate("", (current, b) => current + $"{b:X2} ");
				str += ++i % 16 == 0 ? "\n" : " ";
				Console.Write(str);
			}
		}
	}
}
