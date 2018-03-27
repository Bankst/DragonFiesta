using DragonFiesta.World.Data.Settings;
using System.IO;

namespace DragonFiesta.World.Game.Settings
{
    public class CharacterKeyMapOptions : KeyMapOptions
    {
        public CharacterKeyMapOptions(BinaryReader Reader) : base(Reader)
        {

        }


        public CharacterKeyMapOptions(SQLResult Result, int i)
            : base(Result, i)
        {
        }

        public void Write(FiestaPacket Packet)
        {
            Packet.Write<ushort>(KeyType);
            Packet.Write<byte>(ExtendKey);
            Packet.Write<char>(base.ASCIIKey);
        }
    }
}
