using System.Reflection;

using static BachLib.Utils.Formatting;

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
		public List<string> Tags { get; set; } = new();

		public FileType FileType { get; set; } = FileType.Unknown;

		public string? Title { get; set; } = null;
		public string? Artist { get; set; } = null;
		public string? Album { get; set; } = null;
		public string? Genre { get; set; } = null;
		public string? FileName { get; set; } = null;
		public long? FileSize { get; set; } = null;
		public string? Duration { get; set; } = null;
		public string? FilePath { get; set; } = null;

		public readonly void AddTag(string tag) => Tags.Add(tag);

		public override readonly string ToString()
		{
			string str = "Metadata:\n";

			foreach (PropertyInfo property in GetType().GetProperties())
			{
				object? value = property.GetValue(this);
				if (value == null)
					continue;

				if (property.PropertyType == typeof(List<string>))
				{
					var values = (List<string>)value;
					if (values.Count == 0)
						continue;
					str += $"\t{property.Name}: {string.Join(", ", values)}\n";
				}
				else if (property.Name == "FileSize")
					str += $"\t{property.Name}: {DataSizeToHumanReadable((long)value)}\n";
				else
					str += $"\t{property.Name}: {value}\n";
			}

			return str;
		}
	}
}
