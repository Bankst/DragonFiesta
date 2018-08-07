using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildAcademy
{
    public class GuildAcademy
    {
        public string BuffName { get; }

        public uint LeastJoinTime { get; }

        public sbyte RankAggregationTime { get; }

        public GuildAcademy(SHNResult pResult, int i)
        {
            BuffName = pResult.Read<string>(i, "BuffName");
            LeastJoinTime = pResult.Read<uint>(i, "LeastJoinTime");
            RankAggregationTime = pResult.Read<sbyte>(i, "RankAggregationTime");
        }
    }
}
