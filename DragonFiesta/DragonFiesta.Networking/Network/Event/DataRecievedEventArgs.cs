namespace DragonFiesta.Networking.Network
{
    public class DataRecievedEventArgs
    {
        public byte[] PacketData { get; private set; }

        public DataRecievedEventArgs(byte[] packetData)
        {
            this.PacketData = packetData;
        }

        ~DataRecievedEventArgs()
        {
            PacketData = null;
        }
    }
}