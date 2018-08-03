using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.KingdomQuest
{
    public class KingdomQuestMap
    {
        public byte NumOfMap { get; }

        public string BaseMap { get; }

        public string Map { get; }

        public sbyte Clear { get; }

        public KingdomQuestMap(SHNResult pResult, int i)
        {
            NumOfMap = pResult.Read<byte>(i, "NumOfMap");
            BaseMap = pResult.Read<string>(i, "BaseMap");
            Map = pResult.Read<string>(i, "Map");
            Clear = pResult.Read<sbyte>(i, "Clear");
        }
    }
}
