using System;
using System.IO;
using System.Text;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNBinaryWriter : BinaryWriter
    {
        private Encoding SHNEncoding;

        public SHNBinaryWriter(Stream S, Encoding SE) : base(S) { SHNEncoding = SE; }

        public void WriteString(String Text, Int32 Length)
        {
            if (Length == -1)
            {
                Write(SHNEncoding.GetBytes(Text));

                Write((Byte)0);
            }
            else
            {
                Byte[] StringBytes = SHNEncoding.GetBytes(Text);
                Byte[] DestinationArray = new Byte[Length];

                Array.Copy(StringBytes, DestinationArray, Math.Min(Length, StringBytes.Length));

                Write(DestinationArray);
            }
        }
    }
}
