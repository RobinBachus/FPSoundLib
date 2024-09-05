using System.Text;

namespace FPSoundLib.Utils
{
	public readonly struct InfoChunk
	{
		public string ChunkId { get; }
		public int ChunkSize { get; }
		public string ListTypeId { get; }

		public Dictionary<string, string> Data { get; }

		public InfoChunk(byte[] fileBuffer)
		{
			ChunkId = Encoding.ASCII.GetString(fileBuffer, 0, 4);

			if (ChunkId != "LIST")
				throw new FormatException("Invalid chunk ID");

			ChunkSize = BitConverter.ToInt32(fileBuffer, 4);
			ListTypeId = Encoding.ASCII.GetString(fileBuffer, 8, 4);

			if (ListTypeId != "INFO")
				throw new FormatException("Invalid LIST type ID");

			Data = new Dictionary<string, string>();

			int i = 12;
			while (i < ChunkSize)
			{
				string id = Encoding.ASCII.GetString(fileBuffer, i, 4);
				i += 4;

				int length = BitConverter.ToInt32(fileBuffer, i);
				i += 4;

				Data[id] = Encoding.ASCII.GetString(fileBuffer, i, length);
				i += length;
			}
		}

		public override string ToString()
		{
			string str = "InfoChunk:\n";
			foreach ((string fieldId, string value) in Data)
			{
				str += $"\t{fieldId}: {value}\n";
			}

			return str;
		}
	}
}


