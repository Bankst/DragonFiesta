using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Data.Characters
{
    public sealed class DefaultCharacterItemOptions
    {
        public byte Class { get; private set; }

        public ushort ItemID { get; private set; }

        public int index { get; private set; }

        public long Value { get; private set; }

        public DefaultCharacterItemOptions(SQLResult pRes, int i)
        {
            Class = pRes.Read<byte>(i, "Class");
            ItemID = pRes.Read<ushort>(i, "ItemID");
            index = pRes.Read<int>(i, "OptionIndex");
            Value = pRes.Read<long>(i, "OptionValue");
        }
    }
}