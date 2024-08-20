#pragma warning disable IDE0301 // Disable prefer simplified collection initialization

namespace FPSoundLib.Utils
{
	/// <summary>
	/// Holds metadata for a sound file.
	/// </summary>
	public struct Metadata()
	{
		/// <summary>
		/// Arbitrary tags that can be used to categorize the sound file.
		/// </summary>
		public string[] Tags { get; set; } = Array.Empty<string>();
		public FileType FileType { get; set; } = FileType.Unknown;

		public string? Title { get; set; } = null;
		public string? Artist { get; set; } = null;
		public string? Album { get; set; } = null;
		public string? Genre { get; set; } = null;
	}
}

#pragma warning restore IDE0301