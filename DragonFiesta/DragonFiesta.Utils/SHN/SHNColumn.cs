using System;
using System.Data;

namespace DragonFiesta.Utils.SHN
{
	public class SHNColumn : DataColumn
	{
		public int Length { get; }
		public byte Type { get; }

		public SHNColumn(SHNDataReader reader, ref int unkColumnCount)
		{
			var Name = reader.ReadPaddedString(48);

			if (Name.Trim().Length < 2)
			{
				ColumnName = $"UnkCol{unkColumnCount}";
				unkColumnCount++;
			}
			else
			{
				ColumnName = Name;
			}

			Type = (byte)reader.ReadUInt32();
			DataType = GetType(Type);
			Length = reader.ReadInt32();
		}

		public static Type GetType(byte type)
		{
			switch (type)
			{
				case 1:
				case 12:
					return typeof(byte);
				case 2:
					return typeof(ushort);
				case 3:
				case 11:
					return typeof(uint);
				case 5:
					return typeof(float);
				case 0x15:
				case 13:
					return typeof(short);
				case 0x10:
					return typeof(byte);
				case 0x12:
				case 0x1b:
					return typeof(uint);
				case 20:
					return typeof(sbyte);
				case 0x16:
					return typeof(int);
				case 0x18:
				case 0x1a:
				case 9:
					return typeof(string);
				default:
					return typeof(object);
			}
		}
	}
}
