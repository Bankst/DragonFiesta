using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.SHN
{
	public class SHNFile : DataTable
	{
		public uint ColumnCount { get; private set; }
		public uint RowCount { get; private set; }
		public string FileName { get; }
		public uint Header { get; private set; }
		public byte[] CryptHeader { get; private set; }

		private uint DefaultRowLength;

		public SHNFile(string filePath)
		{
			FileName = filePath;
			TableName = Path.GetFileName(filePath);

			Read();
		}

		private void Read()
		{
			if (!File.Exists(FileName))
			{
				throw new FileNotFoundException("The file could not be found", FileName);
			}

			byte[] Bytes;

			using (var FileHandle = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var Reader = new BinaryReader(FileHandle))
				{
					CryptHeader = Reader.ReadBytes(32);

					var Length = Reader.ReadInt32() - 36;
					Bytes = Reader.ReadBytes(Length);

					var Xor = (byte)Length;

					for (var i = Length - 1; i >= 0; i--)
					{
						Bytes[i] = (byte)(Bytes[i] ^ Xor);

						var XorNext = (byte)i;
						XorNext = (byte)(XorNext & 0xF);
						XorNext = (byte)(XorNext + 0x55);
						XorNext = (byte)(XorNext ^ (byte)((byte)i * 0xB));
						XorNext = (byte)(XorNext ^ Xor);
						XorNext = (byte)(XorNext ^ 0xAA);

						Xor = XorNext;
					}
				}
			}

			using (var Stream = new MemoryStream(Bytes))
			{
				using (var Reader = new SHNDataReader(Stream))
				{
					Header = Reader.ReadUInt32();
					RowCount = Reader.ReadUInt32();
					DefaultRowLength = Reader.ReadUInt32();
					ColumnCount = Reader.ReadUInt32();

					/* LOAD COLUMNS */
					var UnknownColumnCount = 0;
					var Length = 2;

					for (var i = 0; i < ColumnCount; i++)
					{
						var NewColumn = new SHNColumn(Reader, ref UnknownColumnCount);
						Length += NewColumn.Length;

						Columns.Add(NewColumn);
					}

					if (Length != DefaultRowLength)
					{
						throw new Exception("The default row length does not fit");
					}

					/* LOAD ROWS */
					var NewRow = new object[ColumnCount];

					for (uint i = 0; i < RowCount; i++)
					{
						uint RowLength = Reader.ReadUInt16();

						for (var x = 0; x < ColumnCount; x++)
						{
							var RowColumn = (SHNColumn)Columns[x];

							switch (RowColumn.Type)
							{
								case 1:
								case 12:
								case 16:
									NewRow[x] = Reader.ReadByte();
									break;
								case 2:
									NewRow[x] = Reader.ReadUInt16();
									break;
								case 3:
								case 11:
								case 18:
								case 27:
									NewRow[x] = Reader.ReadUInt32();
									break;
								case 5:
									NewRow[x] = Reader.ReadSingle();
									break;
								case 9:
								case 24:
									NewRow[x] = Reader.ReadPaddedString(RowColumn.Length);
									break;
								case 13:
								case 21:
									NewRow[x] = Reader.ReadInt16();
									break;
								case 20:
									NewRow[x] = Reader.ReadSByte();
									break;
								case 22:
									NewRow[x] = Reader.ReadInt32();
									break;
								case 26:
									NewRow[x] = Reader.ReadPaddedString((int)(RowLength - DefaultRowLength + 1));
									break;
								default:
									throw new Exception("New SHN column type found");
							}
						}

						Rows.Add(NewRow);
					}
				}
			}
		}
	}
}
