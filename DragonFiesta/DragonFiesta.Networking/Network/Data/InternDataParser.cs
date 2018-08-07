using System;

namespace DragonFiesta.Networking.Network
{
    public class InternDataParser : DataParserBase
    {
        private int GetSizeOfNextPacket(byte[] ReadBuffer, int Offset) => BitConverter.ToInt32(ReadBuffer, Offset) + 4;

        public override void ParseNext(byte[] ReadBuffer, ref int Offset, int ReadLength)
        {
            int Lenght = ReadLength;

            while (CanReadNextPacket(ReadBuffer, Offset, Lenght))
            {
                ReadNextPacket(ReadBuffer, ref Offset, ref Lenght);
            }
        }

        private bool CanReadNextPacket(byte[] ReadBuffer, int ReadOffset, int Lenght)
        {
            if (Lenght == 0)
                return false;

            return Lenght == GetSizeOfNextPacket(ReadBuffer, ReadOffset);
        }

        private void ReadNextPacket(byte[] buffer, ref int offset, ref int ReadLenght)
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