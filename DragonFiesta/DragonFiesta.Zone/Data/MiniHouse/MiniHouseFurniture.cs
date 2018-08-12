using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    public class MiniHouseFurniture
    {
        public ushort Handle { get; }

        public string ItemID { get; }

        public string FurnitureType { get; }

        public string InvenType { get; }

        public uint GameType { get; }

        public byte CanSet { get; }

        public string Backimage { get; }

        public byte WALL { get; }

        public byte BOTTOM { get; }

        public byte CEILING { get; }

        public byte IsAnimation { get; }

        public MiniHouseFurniture(SHNResult pResult, int row)
        {

        }
    }
}
