﻿using System;

namespace DragonFiesta.Networking.Network
{
    public sealed class FiestaDataParser : DataParserBase
    {
        private ushort GetSizeOfNextPacket(byte[] ReadBuffer, int Offset)
        {
            if (ReadBuffer[Offset] != 0)
                return ReadBuffer[Offset];
            else
                return BitConverter.ToUInt16(ReadBuffer, Offset + 1);
        }

        private int GetHeaderSize(byte[] ReadBuffer, int Offset) => ReadBuffer[Offset] != 0 ? 1 : 3;

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
            int headerSize = GetHeaderSize(ReadBuffer, ReadOffset);
            if (Length <= headerSize)
                return false;
            ushort bodySize = GetSizeOfNextPacket(ReadBuffer, ReadOffset);
            int packetSize = headerSize + bodySize;
            return Length >= packetSize;
        }

        private void ReadNextPacket(byte[] buffer, ref int offset, ref int ReadLength)
        {
            int headerSize = GetHeaderSize(buffer, offset);
            int bodySize = GetSizeOfNextPacket(buffer, offset);
            int packetSize = headerSize + bodySize;

            byte[] packetBody = new byte[bodySize];
            Buffer.BlockCopy(
                buffer,
                offset + headerSize,
                packetBody,
                0,
                bodySize);

            ReadLength -= packetSize;

            offset += packetSize;

            InvokeDataRecv(new DataRecievedEventArgs(packetBody));
        }
    }
}