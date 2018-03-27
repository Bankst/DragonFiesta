using System.IO;

namespace DragonFiesta.World.Data.Settings
{
    public class KeyMapOptions
    {
        public ushort KeyType { get; private set; }
        public byte ExtendKey { get; set; }

        public char ASCIIKey { get;  set; }


        public KeyMapOptions(BinaryReader Reader)
        {
            KeyType = Reader.ReadUInt16();
            ExtendKey = Reader.ReadByte();
            ASCIIKey = Reader.ReadChar();
        }

        public KeyMapOptions(SQLResult Result,int i)
        {
            KeyType = Result.Read<ushort>(i, "KeyType");
            ExtendKey = Result.Read<byte>(i, "ExtendKey");
            ASCIIKey = Result.Read<char>(i, "ASCIIKey");
        }
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(KeyType);
            Writer.Write(ExtendKey);
            Writer.Write(ASCIIKey);
        }
    }
}
