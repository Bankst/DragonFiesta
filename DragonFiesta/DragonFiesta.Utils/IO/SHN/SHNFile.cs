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
        public String LoadPath;
        public MethodInfo CryptoMethod;
        public Encoding SHNEncoding;

        public SHNType Type;
        public DataTable Table;
        public List<SHNColumn> SHNColumns = new List<SHNColumn>();

        private Byte[] CryptoHeader;
        private Byte[] Data;
        private Int32 DataLength;
        private UInt32 Header;
        private UInt32 RowCount;
        private UInt32 DefaultRowLength;
        private UInt32 ColumnCount;

        public Int16 QuestHeader;

        public SHNFile(String LP, MethodInfo CM)
        {
            LoadPath = LP;

            CryptoMethod = CM;

            try { Type = (SHNType)Enum.Parse(typeof(SHNType), Path.GetFileNameWithoutExtension(LoadPath)); }
            catch { Type = SHNType.Unknown; }
        }

        public Byte[] ReadData(Boolean Decrypt)
        {
            SHNBinaryReader SHNReader;

            using (SHNReader = new SHNBinaryReader(File.OpenRead(LoadPath), SHNEncoding))
            {
                CryptoHeader = SHNReader.ReadBytes(32);
                DataLength = SHNReader.ReadInt32();
                Data = SHNReader.ReadBytes(DataLength - 36);
            }

            if (Decrypt) { CryptoMethod.Invoke(this, new Object[] { Data }); }

            return Data;
        }

        public void WriteData(String WritePath, Boolean Encrypt)
        {
            if (File.Exists(WritePath)) { File.Move(WritePath, String.Format("C:\\SHNBackups\\{0}{1}.shn", Path.GetFileNameWithoutExtension(WritePath), DateTime.Now.ToString("dd-MM-yyyy.hh-mm-ss-fff"))); }

            SHNBinaryWriter SHNWriter;

            if (Encrypt) { CryptoMethod.Invoke(this, new Object[] { Data }); }

            using (SHNWriter = new SHNBinaryWriter(File.Create(WritePath), SHNEncoding))
            {
                SHNWriter.Write(CryptoHeader);
                SHNWriter.Write(Data.Length + 36);
                SHNWriter.Write(Data);
            }

            LoadPath = WritePath;
        }

        private String ReadScript(BinaryReader Reader)
        {
            Byte[] ScriptData = new Byte[256];

            Int32 Index = 0;
            Byte ReadByte;

            while ((ReadByte = Reader.ReadByte()) != 0)
            {
                if (ScriptData.Length == Index) { Array.Resize(ref ScriptData, ScriptData.Length * 2); }

                ScriptData[Index++] = ReadByte;
            }

            return Encoding.ASCII.GetString(ScriptData, 0, Index);
        }

        private String ByteArrayToHex(Byte[] Array)
        {
            String Hex = BitConverter.ToString(Array);
            return Hex.Replace("-", "");
        }


        private void WriteScript(String Script, BinaryWriter Writer)
        {
            Writer.Write(Encoding.ASCII.GetBytes(Script));
            Writer.Write((Byte)0);
        }

        private Byte[] HexToByteArray(String Hex)
        {
            Int32 Chars = Hex.Length;

            Byte[] Array = new Byte[Chars / 2];

            for (Int32 Counter = 0; Counter < Chars; Counter += 2) { Array[Counter / 2] = Convert.ToByte(Hex.Substring(Counter, 2), 16); }

            return Array;
        }

        public void Read()
        {
            Table = new DataTable();

            SHNBinaryReader SHNReader;

            using (SHNReader = new SHNBinaryReader(File.OpenRead(LoadPath), SHNEncoding))
            {
                CryptoHeader = SHNReader.ReadBytes(32);
                DataLength = SHNReader.ReadInt32();
                Data = SHNReader.ReadBytes(DataLength - 36);
            }

            CryptoMethod.Invoke(this, new Object[] { Data });

            SHNReader = new SHNBinaryReader(new MemoryStream(Data), SHNEncoding);

            Header = SHNReader.ReadUInt32();
            RowCount = SHNReader.ReadUInt32();
            DefaultRowLength = SHNReader.ReadUInt32();
            ColumnCount = SHNReader.ReadUInt32();

            Int32 UnknownCount = 0;
            Int32 IDCount = 0;

            for (UInt32 Counter = 0; Counter < ColumnCount; Counter++)
            {
                String Name = SHNReader.ReadString(48);
                UInt32 Type = SHNReader.ReadUInt32();
                Int32 Length = SHNReader.ReadInt32();

                if(Name.Length == 0 || String.IsNullOrWhiteSpace(Name))
                {
                    Name = $"Unknown: {UnknownCount}";

                    UnknownCount++;
                }

                SHNColumn NewSHNColumn = new SHNColumn()
                {
                    ID = IDCount,
                    Name = Name,
                    Type = Type,
                    Length = Length
                };

                SHNColumns.Add(NewSHNColumn);

                DataColumn NewDataColumn = new DataColumn()
                {
                    ColumnName = Name,
                    DataType = NewSHNColumn.GetType()
                };

                Table.Columns.Add(NewDataColumn);

                IDCount++;
            }

            Object[] Values = new Object[ColumnCount];

            for (UInt32 RowCounter = 0; RowCounter < RowCount; RowCounter++)
            {
                SHNReader.ReadUInt16();

                foreach (SHNColumn Column in SHNColumns)
                {
                    switch (Column.Type)
                    {
                        case 1:
                            {
                                Values[Column.ID] = SHNReader.ReadByte();
                                break;
                            }
                        case 2:
                            {
                                Values[Column.ID] = SHNReader.ReadUInt16();
                                break;
                            }
                        case 3:
                            {
                                Values[Column.ID] = SHNReader.ReadUInt32();
                                break;
                            }
                        case 5:
                            {
                                Values[Column.ID] = SHNReader.ReadSingle();
                                break;
                            }
                        case 9:
                            {
                                Values[Column.ID] = SHNReader.ReadString(Column.Length);
                                break;
                            }
                        case 11:
                            {
                                Values[Column.ID] = SHNReader.ReadUInt32();
                                break;
                            }
                        case 12:
                            {
                                Values[Column.ID] = SHNReader.ReadByte();
                                break;
                            }
                        case 13:
                            {
                                Values[Column.ID] = SHNReader.ReadInt16();
                                break;
                            }
                        case 0x10:
                            {
                                Values[Column.ID] = SHNReader.ReadByte();
                                break;
                            }
                        case 0x12:
                            {
                                Values[Column.ID] = SHNReader.ReadUInt32();
                                break;
                            }
                        case 20:
                            {
                                Values[Column.ID] = SHNReader.ReadSByte();
                                break;
                            }
                        case 0x15:
                            {
                                Values[Column.ID] = SHNReader.ReadInt16();
                                break;
                            }
                        case 0x16:
                            {
                                Values[Column.ID] = SHNReader.ReadInt32();
                                break;
                            }
                        case 0x18:
                            {
                                Values[Column.ID] = SHNReader.ReadString(Column.Length);
                                break;
                            }
                        case 0x1a:
                            {
                                Values[Column.ID] = SHNReader.ReadString();
                                break;
                            }
                        case 0x1b:
                            {
                                Values[Column.ID] = SHNReader.ReadUInt32();
                                break;
                            }
                        case 0x1d:
                            {
                                Values[Column.ID] = String.Concat(SHNReader.ReadUInt32(), ":", SHNReader.ReadUInt32());
                                break;
                            }
                    }
                }

                Table.Rows.Add(Values);
            }
        }

        public void Write(String WritePath)
        {
            if(File.Exists(WritePath)) { File.Move(WritePath, String.Format("C:\\SHNBackups\\{0}{1}.shn", Path.GetFileNameWithoutExtension(WritePath), DateTime.Now.ToString("dd-MM-yyyy.hh-mm-ss-fff"))); }

            MemoryStream SHNStream = new MemoryStream();
            SHNBinaryWriter SHNWriter = new SHNBinaryWriter(SHNStream, SHNEncoding);

            SHNWriter.Write(Header);
            SHNWriter.Write(Table.Rows.Count);
            SHNWriter.Write(GetColumnLengths());
            SHNWriter.Write(Table.Columns.Count);

            foreach (SHNColumn Column in SHNColumns)
            {
                if (Column.Name.StartsWith("Unknown")) { SHNWriter.Write(new Byte[48]); }
                else { SHNWriter.WriteString(Column.Name, 48); }

                SHNWriter.Write(Column.Type);
                SHNWriter.Write(Column.Length);
            }

            foreach (DataRow Row in Table.Rows)
            {
                Int64 Position = SHNWriter.BaseStream.Position;

                SHNWriter.Write((UInt16)0);

                foreach (SHNColumn Column in SHNColumns)
                {
                    Object ColumnValue = Row.ItemArray[Column.ID];

                    if (ColumnValue == null) { ColumnValue = "0"; }

                    switch (Column.Type)
                    {
                        case 1:
                            {
                                if (ColumnValue is String) { ColumnValue = Byte.Parse((String)ColumnValue); }

                                SHNWriter.Write((Byte)ColumnValue);

                                break;
                            }
                        case 2:
                            {
                                if (ColumnValue is String) { ColumnValue = UInt16.Parse((String)ColumnValue); }

                                SHNWriter.Write((UInt16)ColumnValue);

                                break;
                            }
                        case 3:
                            {
                                if (ColumnValue is String) { ColumnValue = UInt32.Parse((String)ColumnValue); }

                                SHNWriter.Write((UInt32)ColumnValue);

                                break;
                            }
                        case 5:
                            {
                                if (ColumnValue is String) { ColumnValue = Single.Parse((String)ColumnValue); }

                                SHNWriter.Write((Single)ColumnValue);

                                break;
                            }
                        case 9:
                            {
                                if (String.IsNullOrWhiteSpace(ColumnValue.ToString())) { SHNWriter.WriteString(ColumnValue.ToString(), Column.Length); }
                                else { SHNWriter.WriteString((String)ColumnValue, Column.Length); }

                                break;
                            }
                        case 11:
                            {
                                if (ColumnValue is String) { ColumnValue = UInt32.Parse((String)ColumnValue); }

                                SHNWriter.Write((UInt32)ColumnValue);

                                break;
                            }
                        case 12:
                            {
                                if (ColumnValue is String) { ColumnValue = Byte.Parse((String)ColumnValue); }

                                SHNWriter.Write((Byte)ColumnValue);

                                break;
                            }
                        case 13:
                            {
                                if (ColumnValue is String) { ColumnValue = Int16.Parse((String)ColumnValue); }

                                SHNWriter.Write((Int16)ColumnValue);

                                break;
                            }
                        case 0x10:
                            {
                                if (ColumnValue is String) { ColumnValue = Byte.Parse((String)ColumnValue); }

                                SHNWriter.Write((Byte)ColumnValue);

                                break;
                            }
                        case 0x12:
                            {
                                if (ColumnValue is String) { ColumnValue = UInt32.Parse((String)ColumnValue); }

                                SHNWriter.Write((UInt32)ColumnValue);

                                break;
                            }
                        case 20:
                            {
                                if (ColumnValue is String) { ColumnValue = SByte.Parse((String)ColumnValue); }

                                SHNWriter.Write((SByte)ColumnValue);

                                break;
                            }
                        case 0x15:
                            {
                                if (ColumnValue is String) { ColumnValue = Int16.Parse((String)ColumnValue); }

                                SHNWriter.Write((Int16)ColumnValue);

                                break;
                            }
                        case 0x16:
                            {
                                if (ColumnValue is String) { ColumnValue = Int32.Parse((String)ColumnValue); }

                                SHNWriter.Write((Int32)ColumnValue);

                                break;
                            }
                        case 0x18:
                            {
                                SHNWriter.WriteString((String)ColumnValue, Column.Length);

                                break;
                            }
                        case 0x1a:
                            {
                                SHNWriter.WriteString((String)ColumnValue, -1);

                                break;
                            }
                        case 0x1b:
                            {
                                if (ColumnValue is String) { ColumnValue = UInt32.Parse((String)ColumnValue); }

                                SHNWriter.Write((UInt32)ColumnValue);

                                break;
                            }
                        case 0x1d:
                            {
                                if (!ColumnValue.ToString().Contains(":")) { break; }

                                String[] Combined = ColumnValue.ToString().Split(':');

                                SHNWriter.Write(UInt32.Parse(Combined[0]));
                                SHNWriter.Write(UInt32.Parse(Combined[1]));

                                break;
                            }
                    }

                    Int64 MPosition = SHNWriter.BaseStream.Position - Position;
                    Int64 Start = SHNWriter.BaseStream.Position;

                    SHNWriter.BaseStream.Seek(Position, SeekOrigin.Begin);
                    SHNWriter.Write((UInt16)MPosition);
                    SHNWriter.BaseStream.Seek(Start, SeekOrigin.Begin);
                }
            }

            Byte[] UnecryptedData = SHNStream.GetBuffer();
            Byte[] EncryptedData = new Byte[SHNStream.Length];

            Array.Copy(UnecryptedData, EncryptedData, SHNStream.Length);

            CryptoMethod.Invoke(this, new Object[] { EncryptedData });

            SHNWriter.Close();
            SHNWriter = new SHNBinaryWriter(File.Create(WritePath), SHNEncoding);
            SHNWriter.Write(CryptoHeader);
            SHNWriter.Write(EncryptedData.Length + 36);
            SHNWriter.Write(EncryptedData);
            SHNWriter.Close();

            LoadPath = WritePath;
        }

        private UInt32 GetColumnLengths()
        {
            UInt32 Start = 2;

            foreach (SHNColumn Column in SHNColumns) { Start += (UInt32)Column.Length; }

            return Start;
        }

        public void DisallowRowChanges() { Table.RowChanged += Table_RowChanged; }

        private void Table_RowChanged(Object Sender, DataRowChangeEventArgs Args) { Table.RejectChanges(); }
    }
}
