using System.IO;
using System.Text;

namespace DragonFiesta.Networking.Packet
{
    public class FiestaPacketReader : PacketReader
    {
        public FiestaPacketReader(MemoryStream Stream, Encoding Encoder) : base(Stream, Encoder)
        {
        }
    }
}