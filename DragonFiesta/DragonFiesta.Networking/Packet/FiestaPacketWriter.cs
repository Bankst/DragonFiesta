using System;
using System.IO;
using System.Text;

namespace DragonFiesta.Networking.Packet
{
    public class FiestaPacketWriter : PacketWriter
    {
        public FiestaPacketWriter(MemoryStream Stream, Encoding Encode) : base(Stream, Encode)
        {
        }

        public void WriteStringLen(string pValue, bool addNullTerminator = false)
        {
            if (addNullTerminator) pValue += char.MinValue;
            if (pValue.Length > 0xFF)
            {
                throw new Exception("Too long!");
            }
            Write<byte>((byte)pValue.Length);
            Write<byte[]>(Encoding.GetBytes(pValue));
            // NOTE: Some messages might be NULL terminated!
        }
    }
}