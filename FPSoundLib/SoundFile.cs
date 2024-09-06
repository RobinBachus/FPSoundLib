using FPSoundLib.Formats;
using FPSoundLib.Utils;


namespace FPSoundLib
{
	public abstract class SoundFile(byte[] fileBuffer, FileType fileType)
	{
		public Metadata Metadata { get; protected set; } = new() { FileType = fileType };
		public string Id
		{
			get
			{
				Random random = new(GetHashCode());
				string id = "";
				for (int i = 0; i < 8; i++)
					id += (char)random.Next(65, 90);
				return id;
			}
		}

		protected byte[] FileBuffer = fileBuffer;

		/// <summary>
		/// Create a new SoundFile object based on the file type.
		/// </summary>
		/// <param name="fileBuffer"> The file buffer to create the SoundFile object from. </param>
		/// <param name="fileType"> The file type of the file buffer. </param>
		/// <param name="fileInfo"> Optional file info. </param>
		/// <returns> A new SoundFile object. </returns>
		/// <exception cref="NotSupportedException"> Thrown when the file type is not known or not supported. </exception>
		/// <exception cref="NotImplementedException"> Thrown when the file type is not yet supported. </exception>
		public static SoundFile Create(byte[] fileBuffer, FileType fileType, FileInfo? fileInfo = null)
		{
			return fileType switch
			{
				FileType.Wav => new WavFile(fileBuffer, fileInfo),
				FileType.Unknown => throw new FormatException("File type not known"),
				_ => throw new NotSupportedException($"File type '{fileType.ToString().ToLower()}' not yet supported.")
			};
		}

		public override string ToString()
		{
			string fileName = Metadata.FileName != null ? $" ({Metadata.FileName})" : "";

			return $"Summary of file {Id}{fileName}:\n{Metadata}";
		}
	}
}