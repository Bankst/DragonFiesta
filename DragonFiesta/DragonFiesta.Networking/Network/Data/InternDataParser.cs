using System;

namespace DragonFiesta.Networking.Network
{
    public class InternDataParser : DataParserBase
    {
        private int GetSizeOfNextPacket(byte[] ReadBuffer, int Offset) => BitConverter.ToInt32(ReadBuffer, Offset) + 4;

        public override void ParseNext(byte[] ReadBuffer, ref int Offset, int ReadLength)
        {
            int Length = ReadLength;

            while (CanReadNextPacket(ReadBuffer, Offset, Length))
            {
                ReadNextPacket(ReadBuffer, ref Offset, ref Length);
            }
        }

        private bool CanReadNextPacket(byte[] ReadBuffer, int ReadOffset, int Length)
        {
            if (Length == 0)
                return false;

            return Length == GetSizeOfNextPacket(ReadBuffer, ReadOffset);
        }

        private void ReadNextPacket(byte[] buffer, ref int offset, ref int ReadLength)
        {
            int bodySize = GetSizeOfNextPacket(buffer, offset);

            byte[] packetBody = new byte[bodySize];
            Buffer.BlockCopy(
                buffer,
                offset + 4,
                packetBody,
                0,
                bodySize);

            offset += bodySize;

            InvokeDataRecv(new DataRecievedEventArgs(packetBody));
        }
    }
}