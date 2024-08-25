using FPSoundLib.Utils;

namespace FPSoundLib
{
	public abstract class SoundFile(byte[] fileBuffer, FileType fileType)
	{
		public Metadata Metadata { get; } = new() { FileType = fileType };

		protected byte[] FileBuffer = fileBuffer;

		/// <summary>
		/// Create a new SoundFile object based on the file type.
		/// </summary>
		/// <param name="fileBuffer"> The file buffer to create the SoundFile object from. </param>
		/// <param name="fileType"> The file type of the file buffer. </param>
		/// <returns> A new SoundFile object. </returns>
		/// <exception cref="NotSupportedException"> Thrown when the file type is not known or not supported. </exception>
		/// <exception cref="NotImplementedException"> Thrown when the file type is not yet supported. </exception>
		public static SoundFile Create(byte[] fileBuffer, FileType fileType)
		{
			return fileType switch
			{
				FileType.Wav => new WavFile(fileBuffer),
				FileType.Unknown => throw new FormatException("File type not known"),
				_ => throw new NotSupportedException($"File type '{fileType.ToString().ToLower()}' not yet supported.")
			};
		}
	}
}