using System.Text;

namespace FPSoundLib
{
	public class WavFile(byte[] fileBuffer): SoundFile(fileBuffer)
	{
		// 1-4: Marks the file as a riff file. Characters are each 1 byte long.
		public string ChunkID { get; } = Encoding.ASCII.GetString(fileBuffer, 0, 4);

		// 5-8: Size of the overall file - 8 bytes, in bytes (32-bit integer).
		public int ChunkSize { get; } = BitConverter.ToInt32(fileBuffer, 4);

		// 9-12: File Type Header. For our purposes, it always equals “WAVE”.
		public string Format { get; } = Encoding.ASCII.GetString(fileBuffer, 8, 4);

		// 13-16: Format chunk marker. Includes trailing null
		public string FormatMarker { get; } = Encoding.ASCII.GetString(fileBuffer, 12, 4);

		// 17-20: Length of format data as listed above
		public int FormatDataLength { get; } = BitConverter.ToInt32(fileBuffer, 16);

		// 21-22: Type of format (1 is PCM) - 2 byte integer
		public short AudioFormat { get; } = BitConverter.ToInt16(fileBuffer, 20);

		// 23-24: Number of Channels - 2 byte integer
		public short NumChannels { get; } = BitConverter.ToInt16(fileBuffer, 22);

		// 25-28: Sample Rate - 32 byte integer.
		public int SampleRate { get; } = BitConverter.ToInt32(fileBuffer, 24);

		// 29-32: (Sample Rate * BitsPerSample * Channels) / 8.
		public int ByteRate { get; } = BitConverter.ToInt32(fileBuffer, 28);

		// 33-34: (BitsPerSample * Channels) / 8.
		public short BlockAlign { get; } = BitConverter.ToInt16(fileBuffer, 32);

		// 35-36: Bits per sample
		public short BitsPerSample { get; } = BitConverter.ToInt16(fileBuffer, 34);

		// 37-40: “data” chunk header. Marks the beginning of the data section.
		public string DataChunkHeader { get; } = Encoding.ASCII.GetString(fileBuffer, 36, 4);

		// 41-44: Size of the data section.
		public int DataSize { get; } = BitConverter.ToInt32(fileBuffer, 40);

		public override string ToString()
		{
			return $"ChunkID: {ChunkID}\n" +
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
				$"DataChunkHeader: {DataChunkHeader}\n" +
				$"DataSize: {DataSize}\n";
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