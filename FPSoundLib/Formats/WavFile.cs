using System.Text;
using FPSoundLib.Utils;

namespace FPSoundLib.Formats
{
	public class WavFile : SoundFile
	{
		// 1-4: Marks the file as a riff file. Characters are each 1 byte long.
		public string ChunkId { get; }

		// 5-8: Size of the overall file - 8 bytes, in bytes (32-bit integer).
		public int ChunkSize { get; }

		// 9-12: File Type Header. For our purposes, it always equals “WAVE”.
		public string Format { get; }

		// 13-16: Format chunk marker. Includes trailing null
		public string FormatMarker { get; }

		// 17-20: Length of format data as listed above
		public int FormatDataLength { get; }

		// 21-22: Type of format (1 is PCM) - 2 byte integer
		public short AudioFormat { get; }

		// 23-24: Number of Channels - 2 byte integer
		public short NumChannels { get; }

		// 25-28: Sample Rate - 32 byte integer.
		public int SampleRate { get; }

		// 29-32: (Sample Rate * BitsPerSample * Channels) / 8.
		public int ByteRate { get; }

		// 33-34: (BitsPerSample * Channels) / 8.
		public short BlockAlign { get; }

		// 35-36: Bits per sample
		public short BitsPerSample { get; }

		// 37-40: “data” chunk header. Marks the beginning of the data section.
		public string DataChunkHeader { get; }

		// 41-44: Size of the data section.
		public int DataSize { get; }

		// The actual sound data.
		public SoundData Data { get; }

		public InfoChunk? Info { get; }

		public WavFile(byte[] fileBuffer, FileInfo? fileInfo = null) : base(fileBuffer, FileType.Wav)  
		{
			if (fileInfo != null)
			{
				Metadata metadata = Metadata;
				metadata.FileName = fileInfo.Name;
				metadata.FilePath = fileInfo.FullName;
				metadata.FileSize = fileInfo.Length;
				Metadata = metadata;
			}

			ChunkId = Encoding.ASCII.GetString(fileBuffer, 0, 4);
			ChunkSize = BitConverter.ToInt32(fileBuffer, 4);
			Format = Encoding.ASCII.GetString(fileBuffer, 8, 4);
			FormatMarker = Encoding.ASCII.GetString(fileBuffer, 12, 4);
			FormatDataLength = BitConverter.ToInt32(fileBuffer, 16);
			AudioFormat = BitConverter.ToInt16(fileBuffer, 20);
			NumChannels = BitConverter.ToInt16(fileBuffer, 22);
			SampleRate = BitConverter.ToInt32(fileBuffer, 24);
			ByteRate = BitConverter.ToInt32(fileBuffer, 28);
			BlockAlign = BitConverter.ToInt16(fileBuffer, 32);
			BitsPerSample = BitConverter.ToInt16(fileBuffer, 34);

			int cursor = 36;
			string header = Encoding.ASCII.GetString(fileBuffer, cursor, 4);
			while (header != "data" && cursor < fileBuffer.Length)
			{
				if (header.Equals("LIST", StringComparison.CurrentCultureIgnoreCase))
				{
					string listTypeId = Encoding.ASCII.GetString(fileBuffer, cursor + 8, 4);
					if (listTypeId.Equals("INFO", StringComparison.CurrentCultureIgnoreCase))
						Info = new InfoChunk(fileBuffer[cursor..]);
				}
				int size = BitConverter.ToInt32(fileBuffer, cursor + 4);
				cursor += size + 8;
				header = Encoding.ASCII.GetString(fileBuffer, cursor, 4);
			}

			DataChunkHeader = header;
			cursor += 4;
			DataSize = BitConverter.ToInt32(fileBuffer, cursor);
			cursor += 4;
			Data = new SoundData(DataSize, SampleRate, NumChannels, BitsPerSample, BlockAlign, fileBuffer[cursor..]);

		}

		public override string ToString()
		{
			return
				base.ToString() +
				$"ChunkID: {ChunkId}\n" +
				$"ChunkSize: {ChunkSize}\n" +
				$"Format: {Format}\n" +
				$"FormatMarker: {FormatMarker}\n" +
				$"FormatDataLength: {FormatDataLength}\n" +
				$"AudioFormat: {AudioFormat}\n" +
				$"NumChannels: {NumChannels}\n" +
				$"SampleRate: {SampleRate}\n" +
				$"ByteRate: {ByteRate}\n" +
				$"BlockAlign: {BlockAlign}\n" +
				$"BitsPerSample: {BitsPerSample}\n" +
				$"{Info?.ToString() ?? ""}" +
				$"DataChunkHeader: {DataChunkHeader}\n" +
				$"DataSize: {DataSize}\n" +
				$"DataChunk:\n" +
				$"\t{Data.ToString().Replace("\n", "\n\t")}\n";
		}
	}
}

/*
Positions	Description
   1-4	Marks the file as a riff file. Characters are each 1 byte long.
   5-8	Size of the overall file - 8 bytes, in bytes (32-bit integer). Typically, you’d fill this in after creation.
   9 -12	File Type Header. For our purposes, it always equals “WAVE”.
   13-16	Format chunk marker. Includes trailing null
   17-20	Length of format data as listed above
   21-22	Type of format (1 is PCM) - 2 byte integer
   23-24	Number of Channels - 2 byte integer
   25-28	Sample Rate - 32 byte integer. Common values are 44100 (CD), 48000 (DAT). Sample Rate = Number of Samples per second, or Hertz.
   29-32	(Sample Rate * BitsPerSample * Channels) / 8.
   33-34	(BitsPerSample * Channels) / 8.1 - 8 bit mono2 - 8 bit stereo/16 bit mono4 - 16 bit stereo
   35-36	Bits per sample
   37-40	“data” chunk header. Marks the beginning of the data section.
   41-44	Size of the data section.

 */