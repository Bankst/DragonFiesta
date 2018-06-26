using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Maps
{  
    public sealed class MapInfo
    {
        public ushort ID { get; private set; }
        public string Index { get; private set; }
        public string Name { get; private set; }

        public bool HasLevelCondition { get { return LevelCondition != null; } }

        public FieldLvCondition LevelCondition { get; set; }

        public Position Regen { get; private set; }
        public MapType Type { get; private set; }

        public bool IsInSide { get; private set; }
        public double ViewRange { get; private set; }

        public MapInfo(SHNResult rResult, int rIndex)
        {
            ID = rResult.Read<ushort>(rIndex, "ID");
            Index = rResult.Read<string>(rIndex, "MapName");
            Name = rResult.Read<string>(rIndex, "Name");
            Regen = new Position(rResult.Read<uint>(rIndex, "RegenX"), rResult.Read<uint>(rIndex, "RegenY"));
            Type = (MapType)rResult.Read<byte>(rIndex, "KingdomMap");
            IsInSide = rResult.Read<bool>(rIndex, "InSide");
            ViewRange = rResult.Read<uint>(rIndex, "Sight");
        }
    }
}