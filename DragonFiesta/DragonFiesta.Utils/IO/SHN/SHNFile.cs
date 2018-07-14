using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNFile
    {
        public string LoadPath;
        public MethodInfo CryptoMethod;
        public Encoding SHNEncoding;

        public SHNType Type;
        public DataTable Table;
        public List<SHNColumn> SHNColumns = new List<SHNColumn>();

        private byte[] _cryptoHeader;
        private byte[] _data;
        private int _dataLength;
        private uint _header;
        private uint _rowCount;
        private uint _defaultRowLength;
        private uint _columnCount;

        public SHNFile(string lp, MethodInfo cm)
        {
            LoadPath = lp;

            CryptoMethod = cm;

            try { Type = (SHNType)Enum.Parse(typeof(SHNType), Path.GetFileNameWithoutExtension(LoadPath) ?? throw new InvalidOperationException()); }
            catch { Type = SHNType.Unknown; }
        }

	    public void Read()
        {
            Table = new DataTable();

            SHNBinaryReader shnReader;

            using (shnReader = new SHNBinaryReader(File.OpenRead(LoadPath), SHNEncoding))
            {
                _cryptoHeader = shnReader.ReadBytes(32);
                _dataLength = shnReader.ReadInt32();
                _data = shnReader.ReadBytes(_dataLength - 36);
            }

            CryptoMethod.Invoke(this, new object[] { _data });

            shnReader = new SHNBinaryReader(new MemoryStream(_data), SHNEncoding);

            _header = shnReader.ReadUInt32();
            _rowCount = shnReader.ReadUInt32();
            _defaultRowLength = shnReader.ReadUInt32();
            _columnCount = shnReader.ReadUInt32();

            int unknownCount = 0;
            int idCount = 0;

            for (uint counter = 0; counter < _columnCount; counter++)
            {
                string name = shnReader.ReadString(48);
                uint type = shnReader.ReadUInt32();
                int length = shnReader.ReadInt32();

                if(name.Length == 0 || string.IsNullOrWhiteSpace(name))
                {
                    name = $"Unknown: {unknownCount}";

                    unknownCount++;
                }

                SHNColumn newSHNColumn = new SHNColumn()
                {
                    ID = idCount,
                    Name = name,
                    Type = type,
                    Length = length
                };

                SHNColumns.Add(newSHNColumn);

                DataColumn newDataColumn = new DataColumn()
                {
                    ColumnName = name,
                    DataType = newSHNColumn.GetType()
                };

                Table.Columns.Add(newDataColumn);

                idCount++;
            }

            object[] values = new object[_columnCount];

            for (uint rowCounter = 0; rowCounter < _rowCount; rowCounter++)
            {
                shnReader.ReadUInt16();

                foreach (SHNColumn column in SHNColumns)
                {
                    switch (column.Type)
                    {
                        case 1:
                            {
                                values[column.ID] = shnReader.ReadByte();
                                break;
                            }
                        case 2:
                            {
                                values[column.ID] = shnReader.ReadUInt16();
                                break;
                            }
                        case 3:
                            {
                                values[column.ID] = shnReader.ReadUInt32();
                                break;
                            }
                        case 5:
                            {
                                values[column.ID] = shnReader.ReadSingle();
                                break;
                            }
                        case 9:
                            {
                                values[column.ID] = shnReader.ReadString(column.Length);
                                break;
                            }
                        case 11:
                            {
                                values[column.ID] = shnReader.ReadUInt32();
                                break;
                            }
                        case 12:
                            {
                                values[column.ID] = shnReader.ReadByte();
                                break;
                            }
                        case 13:
                            {
                                values[column.ID] = shnReader.ReadInt16();
                                break;
                            }
                        case 0x10:
                            {
                                values[column.ID] = shnReader.ReadByte();
                                break;
                            }
                        case 0x12:
                            {
                                values[column.ID] = shnReader.ReadUInt32();
                                break;
                            }
                        case 20:
                            {
                                values[column.ID] = shnReader.ReadSByte();
                                break;
                            }
                        case 0x15:
                            {
                                values[column.ID] = shnReader.ReadInt16();
                                break;
                            }
                        case 0x16:
                            {
                                values[column.ID] = shnReader.ReadInt32();
                                break;
                            }
                        case 0x18:
                            {
                                values[column.ID] = shnReader.ReadString(column.Length);
                                break;
                            }
                        case 0x1a:
                            {
                                values[column.ID] = shnReader.ReadString();
                                break;
                            }
                        case 0x1b:
                            {
                                values[column.ID] = shnReader.ReadUInt32();
                                break;
                            }
                        case 0x1d:
                            {
                                values[column.ID] = string.Concat(shnReader.ReadUInt32(), ":", shnReader.ReadUInt32());
                                break;
                            }
                    }
                }

                Table.Rows.Add(values);
            }
        }

        public void Write(string writePath)
        {
            if(File.Exists(writePath)) { File.Move(writePath,
	            $"C:\\SHNBackups\\{Path.GetFileNameWithoutExtension(writePath)}{DateTime.Now.ToString("dd-MM-yyyy.hh-mm-ss-fff")}.shn"); }

            MemoryStream shnStream = new MemoryStream();
            SHNBinaryWriter shnWriter = new SHNBinaryWriter(shnStream, SHNEncoding);

            shnWriter.Write(_header);
            shnWriter.Write(Table.Rows.Count);
            shnWriter.Write(GetColumnLengths());
            shnWriter.Write(Table.Columns.Count);

            foreach (SHNColumn column in SHNColumns)
            {
                if (column.Name.StartsWith("Unknown")) { shnWriter.Write(new byte[48]); }
                else { shnWriter.WriteString(column.Name, 48); }

                shnWriter.Write(column.Type);
                shnWriter.Write(column.Length);
            }

            foreach (DataRow row in Table.Rows)
            {
                long position = shnWriter.BaseStream.Position;

                shnWriter.Write((ushort)0);

                foreach (SHNColumn column in SHNColumns)
                {
                    object columnValue = row.ItemArray[column.ID];

                    if (columnValue == null) { columnValue = "0"; }

                    switch (column.Type)
                    {
                        case 1:
                            {
                                if (columnValue is string) { columnValue = byte.Parse((string)columnValue); }

                                shnWriter.Write((byte)columnValue);

                                break;
                            }
                        case 2:
                            {
                                if (columnValue is string) { columnValue = ushort.Parse((string)columnValue); }

                                shnWriter.Write((ushort)columnValue);

                                break;
                            }
                        case 3:
                            {
                                if (columnValue is string) { columnValue = uint.Parse((string)columnValue); }

                                shnWriter.Write((uint)columnValue);

                                break;
                            }
                        case 5:
                            {
                                if (columnValue is string) { columnValue = float.Parse((string)columnValue); }

                                shnWriter.Write((float)columnValue);

                                break;
                            }
                        case 9:
                            {
                                if (string.IsNullOrWhiteSpace(columnValue.ToString())) { shnWriter.WriteString(columnValue.ToString(), column.Length); }
                                else { shnWriter.WriteString((string)columnValue, column.Length); }

                                break;
                            }
                        case 11:
                            {
                                if (columnValue is string) { columnValue = uint.Parse((string)columnValue); }

                                shnWriter.Write((uint)columnValue);

                                break;
                            }
                        case 12:
                            {
                                if (columnValue is string) { columnValue = byte.Parse((string)columnValue); }

                                shnWriter.Write((byte)columnValue);

                                break;
                            }
                        case 13:
                            {
                                if (columnValue is string) { columnValue = short.Parse((string)columnValue); }

                                shnWriter.Write((short)columnValue);

                                break;
                            }
                        case 0x10:
                            {
                                if (columnValue is string) { columnValue = byte.Parse((string)columnValue); }

                                shnWriter.Write((byte)columnValue);

                                break;
                            }
                        case 0x12:
                            {
                                if (columnValue is string) { columnValue = uint.Parse((string)columnValue); }

                                shnWriter.Write((uint)columnValue);

                                break;
                            }
                        case 20:
                            {
                                if (columnValue is string) { columnValue = sbyte.Parse((string)columnValue); }

                                shnWriter.Write((sbyte)columnValue);

                                break;
                            }
                        case 0x15:
                            {
                                if (columnValue is string) { columnValue = short.Parse((string)columnValue); }

                                shnWriter.Write((short)columnValue);

                                break;
                            }
                        case 0x16:
                            {
                                if (columnValue is string) { columnValue = int.Parse((string)columnValue); }

                                shnWriter.Write((int)columnValue);

                                break;
                            }
                        case 0x18:
                            {
                                shnWriter.WriteString((string)columnValue, column.Length);

                                break;
                            }
                        case 0x1a:
                            {
                                shnWriter.WriteString((string)columnValue, -1);

                                break;
                            }
                        case 0x1b:
                            {
                                if (columnValue is string) { columnValue = uint.Parse((string)columnValue); }

                                shnWriter.Write((uint)columnValue);

                                break;
                            }
                        case 0x1d:
                            {
                                if (!columnValue.ToString().Contains(":")) { break; }

                                string[] combined = columnValue.ToString().Split(':');

                                shnWriter.Write(uint.Parse(combined[0]));
                                shnWriter.Write(uint.Parse(combined[1]));

                                break;
                            }
                    }

                    long mPosition = shnWriter.BaseStream.Position - position;
                    long start = shnWriter.BaseStream.Position;

                    shnWriter.BaseStream.Seek(position, SeekOrigin.Begin);
                    shnWriter.Write((ushort)mPosition);
                    shnWriter.BaseStream.Seek(start, SeekOrigin.Begin);
                }
            }

            byte[] unecryptedData = shnStream.GetBuffer();
            byte[] encryptedData = new byte[shnStream.Length];

            Array.Copy(unecryptedData, encryptedData, shnStream.Length);

            CryptoMethod.Invoke(this, new object[] { encryptedData });

            shnWriter.Close();
            shnWriter = new SHNBinaryWriter(File.Create(writePath), SHNEncoding);
            shnWriter.Write(_cryptoHeader);
            shnWriter.Write(encryptedData.Length + 36);
            shnWriter.Write(encryptedData);
            shnWriter.Close();

            LoadPath = writePath;
        }

        private uint GetColumnLengths()
        {
            uint start = 2;

            foreach (SHNColumn column in SHNColumns) { start += (uint)column.Length; }

            return start;
        }

        public void DisallowRowChanges() { Table.RowChanged += Table_RowChanged; }

        private void Table_RowChanged(object sender, DataRowChangeEventArgs args) { Table.RejectChanges(); }
    }
}
