using System.IO;
using System.Text;

namespace DragonFiesta.Utils.Shine
{
	public class SHNDataReader : BinaryReader
	{
		public SHNDataReader(Stream input) : base(input)
		{
		}

		public string ReadPaddedString(int length)
		{
			var Value = string.Empty;
			var Offset = 0;
			var Bytes = ReadBytes(length);

			while (Offset < length && Bytes[Offset] != 0x00)
			{
				Offset++;
			}

			if (length > 0)
			{
				Value = Encoding.UTF8.GetString(Bytes, 0, Offset);
			}

			return Value;
		}

		public long Seek(long offset, SeekOrigin origin)
		{
			return BaseStream.Seek(offset, origin);
		}

		public long Skip(long offset)
		{
			return Seek(offset, SeekOrigin.Current);
		}
	}
}
