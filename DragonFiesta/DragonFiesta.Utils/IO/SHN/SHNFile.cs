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
                CryptoHeader = SHNReader.ReadBytes(0x20);
                DataLength = SHNReader.ReadInt32();
                Data = SHNReader.ReadBytes(DataLength - 0x24);
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
                SHNWriter.Write((Int32)(Data.Length + 0x24));
                SHNWriter.Write(Data);
            }

            LoadPath = WritePath;
        }

        public void ReadQuest()
        {
            using (BinaryReader SHNReader = new BinaryReader(File.OpenRead(LoadPath)))
            {
                QuestHeader = SHNReader.ReadInt16();

                if (QuestHeader == 4)
                {
                    Table = TableCreator.CreateDefaultQuestDataOdin();

                    Int16 TotalQuests = SHNReader.ReadInt16();

                    for (Int32 Counter = 0; Counter < TotalQuests; Counter++)
                    {
                        List<Object> QuestData = new List<Object>();

                        SHNReader.ReadUInt32();

                        UInt16 QuestID = SHNReader.ReadUInt16();

                        QuestData.Add(QuestID);
                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadUInt16());

                        Byte[] UnkData0 = SHNReader.ReadBytes(1);

                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData1 = SHNReader.ReadBytes(3);

                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadByte());

                        Byte[] UnkData2 = SHNReader.ReadBytes(17);

                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData3 = SHNReader.ReadBytes(1);

                        QuestData.Add(SHNReader.ReadUInt16());

                        Byte[] UnkData4 = SHNReader.ReadBytes(2);

                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadByte());

                        Byte[] UnkData5 = SHNReader.ReadBytes(22);

                        for (Int32 MobCounter = 0; MobCounter < 5; MobCounter++)
                        {
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadUInt16());
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadByte());
                        }

                        for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                        {
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadByte());
                            QuestData.Add(SHNReader.ReadUInt16());
                            QuestData.Add(SHNReader.ReadUInt16());
                        }

                        QuestData.Add(SHNReader.ReadUInt32());

                        for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                        {
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                        }

                        for (Int32 RewardCounter = 0; RewardCounter < 12; RewardCounter++)
                        {
                            QuestData.Add(SHNReader.ReadByte());

                            Byte Type = SHNReader.ReadByte();

                            QuestData.Add(Type);
                            QuestData.Add(SHNReader.ReadUInt16());

                            if (Type == 2)
                            {
                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add((UInt64)0);

                                SHNReader.ReadBytes(4);
                            }
                            else if (Type == 3)
                            {
                                SHNReader.ReadBytes(4);

                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add((UInt16)0);
                                QuestData.Add((UInt64)0);

                                SHNReader.ReadBytes(2);
                            }
                            else
                            {
                                QuestData.Add((UInt16)0);
                                QuestData.Add((UInt16)0);
                                QuestData.Add(SHNReader.ReadUInt64());
                            }
                        }

                        SHNReader.BaseStream.Position += 6;
                        Byte[] RewardData = SHNReader.ReadBytes(18);

                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ByteArrayToHex(UnkData0));
                        QuestData.Add(ByteArrayToHex(UnkData1));
                        QuestData.Add(ByteArrayToHex(UnkData2));
                        QuestData.Add(ByteArrayToHex(UnkData3));
                        QuestData.Add(ByteArrayToHex(UnkData4));
                        QuestData.Add(ByteArrayToHex(UnkData5));
                        QuestData.Add(ByteArrayToHex(RewardData));

                        Table.Rows.Add(QuestData.ToArray());
                    }
                }
                else if (QuestHeader == 6)
                {
                    Table = TableCreator.CreateDefaultQuestDataHK();

                    Int16 TotalQuests = SHNReader.ReadInt16();

                    for (Int32 Counter = 0; Counter < TotalQuests; Counter++)
                    {
                        List<Object> QuestData = new List<Object>();

                        SHNReader.ReadUInt32();

                        UInt16 QuestID = SHNReader.ReadUInt16();

                        QuestData.Add(QuestID);
                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadUInt16());

                        Byte[] UnkData0 = SHNReader.ReadBytes(1);

                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData1 = SHNReader.ReadBytes(2);
                        
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadByte());
                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData2 = SHNReader.ReadBytes(1);

                        QuestData.Add(SHNReader.ReadUInt16());
                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData3 = SHNReader.ReadBytes(19);

                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData4 = SHNReader.ReadBytes(1);

                        QuestData.Add(SHNReader.ReadUInt16());

                        Byte[] UnkData5 = SHNReader.ReadBytes(2);

                        QuestData.Add(SHNReader.ReadBoolean());
                        QuestData.Add(SHNReader.ReadByte());

                        Byte[] UnkData6 = SHNReader.ReadBytes(24);

                        QuestData.Add(SHNReader.ReadBoolean());

                        Byte[] UnkData7 = SHNReader.ReadBytes(3);

                        for (Int32 MobCounter = 0; MobCounter < 5; MobCounter++)
                        {
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadUInt16());
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadByte());
                            QuestData.Add(SHNReader.ReadByte());
                            QuestData.Add(SHNReader.ReadByte());
                        }

                        for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                        {
                            QuestData.Add(SHNReader.ReadBoolean());
                            QuestData.Add(SHNReader.ReadByte());
                            QuestData.Add(SHNReader.ReadUInt16());
                            QuestData.Add(SHNReader.ReadUInt16());
                        }

                        QuestData.Add(SHNReader.ReadUInt32());

                        for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                        {
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                            QuestData.Add(SHNReader.ReadUInt32());
                        }

                        Byte[] MobDropData = SHNReader.ReadBytes(40);

                        for (Int32 RewardCounter = 0; RewardCounter < 12; RewardCounter++)
                        {
                            QuestData.Add(SHNReader.ReadByte());

                            Byte Type = SHNReader.ReadByte();

                            QuestData.Add(Type);
                            QuestData.Add(SHNReader.ReadUInt16());

                            if (Type == 2)
                            {
                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add((UInt64)0);

                                SHNReader.ReadBytes(4);
                            }
                            else if (Type == 3)
                            {
                                SHNReader.ReadBytes(4);

                                QuestData.Add(SHNReader.ReadUInt16());
                                QuestData.Add((UInt16)0);
                                QuestData.Add((UInt64)0);

                                SHNReader.ReadBytes(2);
                            }
                            else
                            {
                                QuestData.Add((UInt16)0);
                                QuestData.Add((UInt16)0);
                                QuestData.Add(SHNReader.ReadUInt64());
                            }
                        }

                        SHNReader.BaseStream.Position += 6;
                        Byte[] RewardData = SHNReader.ReadBytes(14);

                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ReadScript(SHNReader));
                        QuestData.Add(ByteArrayToHex(UnkData0));
                        QuestData.Add(ByteArrayToHex(UnkData1));
                        QuestData.Add(ByteArrayToHex(UnkData2));
                        QuestData.Add(ByteArrayToHex(UnkData3));
                        QuestData.Add(ByteArrayToHex(UnkData4));
                        QuestData.Add(ByteArrayToHex(UnkData5));
                        QuestData.Add(ByteArrayToHex(UnkData6));
                        QuestData.Add(ByteArrayToHex(UnkData7));
                        QuestData.Add(ByteArrayToHex(MobDropData));
                        QuestData.Add(ByteArrayToHex(RewardData));

                        Table.Rows.Add(QuestData.ToArray());
                    }
                }
            }
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

        public void WriteQuest(String WritePath)
        {
            if (File.Exists(WritePath)) { File.Move(WritePath, String.Format("C:\\SHNBackups\\{0}{1}.shn", Path.GetFileNameWithoutExtension(WritePath), DateTime.Now.ToString("dd-MM-yyyy.hh-mm-ss-fff"))); }

            BinaryWriter SHNWriter = new BinaryWriter(File.Open(WritePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
            SHNWriter.Write(QuestHeader);
            SHNWriter.Write((Int16)Table.Rows.Count);

            if (QuestHeader == 4)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    UInt32 DataLength = 592;
                    DataLength += (UInt32)(Row.ItemArray[226].ToString().Length + 1);
                    DataLength += (UInt32)(Row.ItemArray[227].ToString().Length + 1);
                    DataLength += (UInt32)(Row.ItemArray[228].ToString().Length + 1);

                    SHNWriter.Write(DataLength);
                    SHNWriter.Write((UInt16)Row.ItemArray[0]);
                    SHNWriter.Write((UInt16)Row.ItemArray[1]);
                    SHNWriter.Write((UInt16)Row.ItemArray[2]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[229].ToString()));
                    SHNWriter.Write((Byte)Row.ItemArray[3]);
                    SHNWriter.Write((Boolean)Row.ItemArray[4]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[230].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[5]);
                    SHNWriter.Write((Byte)Row.ItemArray[6]);
                    SHNWriter.Write((Byte)Row.ItemArray[7]);
                    SHNWriter.Write((Boolean)Row.ItemArray[8]);
                    SHNWriter.Write((UInt16)Row.ItemArray[9]);
                    SHNWriter.Write((Boolean)Row.ItemArray[10]);
                    SHNWriter.Write((Byte)Row.ItemArray[11]);
                    SHNWriter.Write((UInt16)Row.ItemArray[12]);
                    SHNWriter.Write((Byte)Row.ItemArray[13]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[231].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[14]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[232].ToString()));
                    SHNWriter.Write((UInt16)Row.ItemArray[15]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[233].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[16]);
                    SHNWriter.Write((Byte)Row.ItemArray[17]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[234].ToString()));

                    Int32 ColumnID = 18;

                    for (Int32 MobCounter = 0; MobCounter < 5; MobCounter++)
                    {
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                    }

                    for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                    {
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                    }

                    SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);

                    for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                    {
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                    }

                    for (Int32 RewardCounter = 0; RewardCounter < 12; RewardCounter++)
                    {
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);

                        Byte Type = (Byte)Row.ItemArray[ColumnID++];

                        SHNWriter.Write(Type);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);

                        if (Type == 2)
                        {
                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                            ColumnID++;

                            SHNWriter.Write(new Byte[4]);
                        }
                        else if (Type == 3)
                        {
                            SHNWriter.Write(new Byte[4]);

                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);

                            ColumnID = ColumnID + 2;

                            SHNWriter.Write(new Byte[2]);
                        }
                        else
                        {
                            ColumnID = ColumnID + 2;
                            SHNWriter.Write((UInt64)Row.ItemArray[ColumnID++]);
                        }
                    }

                    SHNWriter.Write((UInt16)(Row.ItemArray[226].ToString().Length + 1));
                    SHNWriter.Write((UInt16)(Row.ItemArray[228].ToString().Length + 1));
                    SHNWriter.Write((UInt16)(Row.ItemArray[227].ToString().Length + 1));
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[235].ToString()));

                    WriteScript(Row.ItemArray[226].ToString(), SHNWriter);
                    WriteScript(Row.ItemArray[227].ToString(), SHNWriter);
                    WriteScript(Row.ItemArray[228].ToString(), SHNWriter);
                }
            }
            else if (QuestHeader == 6)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    UInt32 DataLength = 672;
                    DataLength += (UInt32)(Row.ItemArray[239].ToString().Length + 1);
                    DataLength += (UInt32)(Row.ItemArray[240].ToString().Length + 1);
                    DataLength += (UInt32)(Row.ItemArray[241].ToString().Length + 1);

                    SHNWriter.Write(DataLength);
                    SHNWriter.Write((UInt16)Row.ItemArray[0]);
                    SHNWriter.Write((UInt16)Row.ItemArray[1]);
                    SHNWriter.Write((UInt16)Row.ItemArray[2]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[242].ToString()));
                    SHNWriter.Write((Byte)Row.ItemArray[3]);
                    SHNWriter.Write((Boolean)Row.ItemArray[4]);
                    SHNWriter.Write((Boolean)Row.ItemArray[5]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[243].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[6]);
                    SHNWriter.Write((Boolean)Row.ItemArray[7]);
                    SHNWriter.Write((Boolean)Row.ItemArray[8]);
                    SHNWriter.Write((Byte)Row.ItemArray[9]);
                    SHNWriter.Write((Byte)Row.ItemArray[10]);
                    SHNWriter.Write((Boolean)Row.ItemArray[11]);
                    SHNWriter.Write((UInt16)Row.ItemArray[12]);
                    SHNWriter.Write((Boolean)Row.ItemArray[13]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[244].ToString()));
                    SHNWriter.Write((UInt16)Row.ItemArray[14]);
                    SHNWriter.Write((Boolean)Row.ItemArray[15]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[245].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[16]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[246].ToString()));
                    SHNWriter.Write((UInt16)Row.ItemArray[17]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[247].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[18]);
                    SHNWriter.Write((Byte)Row.ItemArray[19]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[248].ToString()));
                    SHNWriter.Write((Boolean)Row.ItemArray[20]);
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[249].ToString()));

                    Int32 ColumnID = 21;

                    for (Int32 MobCounter = 0; MobCounter < 5; MobCounter++)
                    {
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                    }

                    for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                    {
                        SHNWriter.Write((Boolean)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                    }

                    SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);

                    for (Int32 ItemCounter = 0; ItemCounter < 10; ItemCounter++)
                    {
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                        SHNWriter.Write((UInt32)Row.ItemArray[ColumnID++]);
                    }

                    SHNWriter.Write(HexToByteArray(Row.ItemArray[250].ToString()));

                    for (Int32 RewardCounter = 0; RewardCounter < 12; RewardCounter++)
                    {
                        SHNWriter.Write((Byte)Row.ItemArray[ColumnID++]);

                        Byte Type = (Byte)Row.ItemArray[ColumnID++];

                        SHNWriter.Write(Type);
                        SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);

                        if (Type == 2)
                        {
                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);
                            ColumnID++;

                            SHNWriter.Write(new Byte[4]);
                        }
                        else if (Type == 3)
                        {
                            SHNWriter.Write(new Byte[4]);

                            SHNWriter.Write((UInt16)Row.ItemArray[ColumnID++]);

                            ColumnID = ColumnID + 2;

                            SHNWriter.Write(new Byte[2]);
                        }
                        else
                        {
                            ColumnID = ColumnID + 2;
                            SHNWriter.Write((UInt64)Row.ItemArray[ColumnID++]);
                        }
                    }

                    SHNWriter.Write((UInt16)(Row.ItemArray[239].ToString().Length + 1));
                    SHNWriter.Write((UInt16)(Row.ItemArray[241].ToString().Length + 1));
                    SHNWriter.Write((UInt16)(Row.ItemArray[240].ToString().Length + 1));
                    SHNWriter.Write(HexToByteArray(Row.ItemArray[251].ToString()));

                    WriteScript(Row.ItemArray[239].ToString(), SHNWriter);
                    WriteScript(Row.ItemArray[240].ToString(), SHNWriter);
                    WriteScript(Row.ItemArray[241].ToString(), SHNWriter);
                }
            }

            SHNWriter.Close();

            LoadPath = WritePath;
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
                CryptoHeader = SHNReader.ReadBytes(0x20);
                DataLength = SHNReader.ReadInt32();
                Data = SHNReader.ReadBytes(DataLength - 0x24);
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
                String Name = SHNReader.ReadString(0x30);
                UInt32 Type = SHNReader.ReadUInt32();
                Int32 Length = SHNReader.ReadInt32();

                if(Name.Length == 0 || String.IsNullOrWhiteSpace(Name))
                {
                    Name = String.Format("Unknown: {0}", UnknownCount);

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
                if (Column.Name.StartsWith("Unknown")) { SHNWriter.Write(new Byte[0x30]); }
                else { SHNWriter.WriteString(Column.Name, 0x30); }

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

                    if (ColumnValue == null) { ColumnValue = (String)"0"; }

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
            SHNWriter.Write((Int32)(EncryptedData.Length + 0x24));
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
