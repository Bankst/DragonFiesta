using System;
using System.IO;
using System.Text;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNBinaryReader : BinaryReader
    {
        private Encoding SHNEncoding;

        private Byte[] Buffer = new Byte[0x100];

        public SHNBinaryReader(Stream S, Encoding SE) : base(S) { SHNEncoding = SE; }

        public String ReadString(Int32 Bytes)
        {
            if (Bytes > 0) { return ReadString((UInt32)Bytes); }

            return String.Empty;
        }

        public String ReadString(UInt32 Bytes) { return PReadString(Bytes).TrimEnd(new Char[1]); }

        private String PReadString(UInt32 Bytes)
        {
            String ReturnString = String.Empty;

            if (Bytes > 0x100) { ReturnString = ReadString((UInt32)(Bytes - 0x100)); }

            Read(Buffer, 0, (Int32)Bytes);

            return ReturnString + SHNEncoding.GetString(Buffer, 0, (Int32)Bytes);
        }

        public override string ReadString()
        {
            Int32 Count = 0;

            for (Byte Counter = ReadByte(); Counter != 0; Counter = ReadByte())
            {
                Buffer[Count++] = Counter;

                if (Count >= 0x100) { break; }
            }

            String ReturnString = SHNEncoding.GetString(Buffer, 0, Count);

            if (Count == 0x100) { ReturnString = ReturnString + ReadString(); }

            return ReturnString;
        }
    }
}
