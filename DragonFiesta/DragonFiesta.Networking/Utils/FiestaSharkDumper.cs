using DragonFiesta.Utils.Core;
using System;
using System.IO;

namespace DragonFiesta.Networking.Utils
{
    public class FiestaSharkDumper
    {
        private FileStream DumpFile { get; set; }

        private BinaryWriter StreamWrite { get; }
        private object LockThreadObject { get; set; }

        private string Path { get; set; }
        private string DumpDirectory = @"PacketDumps";

        private ushort PacketCounter = 0;

        private ushort DumpCounter = 1;

        private ushort SessionId { get; set; }

        public FiestaSharkDumper(ushort SessionId)
        {
            if (!Directory.Exists(DumpDirectory))
                Directory.CreateDirectory(DumpDirectory);

            LockThreadObject = new object();
            this.SessionId = SessionId;

            LoadPath();

            DumpFile = new FileStream(Path, FileMode.Append, FileAccess.Write);
            StreamWrite = new BinaryWriter(DumpFile);

            StreamWrite.Write((ushort)PacketCounter);
            StreamWrite.Write((ushort)SessionId);
        }

        private void LoadPath()
        {
            while (true)
            {
                string path = DumpDirectory + $"\\{ServerMainBase.InternalInstance.ServerType}_PacketDump_{ DateTime.Now.ToString("MM_dd_yyyy")}_{SessionId}_{DumpCounter}.msb";

                if (!File.Exists(path))
                {
                    Path = path;
                    break;
                }
                DumpCounter++;
            }
        }

        public void DumpPacket(FiestaPacket Packet, bool OutBound)
        {
            lock (LockThreadObject)
            {
                ushort mOpcode = (ushort)((Packet.Header << 10) + (Packet.Type & 1023));
                byte[] buffer = new byte[Packet.GetBuffer().Length + 12];
                ushort size = (ushort)(Packet.GetBuffer().Length);
                if (OutBound) size |= 0x8000;
                long ticks = DateTime.Now.Ticks;
                buffer[0] = (byte)ticks;
                buffer[1] = (byte)(ticks >> 8);
                buffer[2] = (byte)(ticks >> 16);
                buffer[3] = (byte)(ticks >> 24);
                buffer[4] = (byte)(ticks >> 32);
                buffer[5] = (byte)(ticks >> 40);
                buffer[6] = (byte)(ticks >> 48);
                buffer[7] = (byte)(ticks >> 56);
                buffer[8] = (byte)size;
                buffer[9] = (byte)(size >> 8);
                buffer[10] = (byte)mOpcode;
                buffer[11] = (byte)(mOpcode >> 8);
                Buffer.BlockCopy(Packet.GetBuffer(), 0, buffer, 12, Packet.GetBuffer().Length);

                StreamWrite.Write(buffer);
                StreamWrite.Flush();

                if (PacketCounter >= 200)
                    DumpCounter++;

                PacketCounter++;
            }
        }
    }
}