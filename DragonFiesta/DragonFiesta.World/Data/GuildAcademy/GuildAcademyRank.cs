using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildAcademy
{
    public class GuildAcademyRank
    {
        public sbyte Rank { get; }

        public uint Fame { get; }

        public string BuffName { get; }

        public GuildAcademyRank(SHNResult pResult, int i)
        {
            Rank = pResult.Read<sbyte>(i, "Rank");
            Fame = pResult.Read<uint>(i, "Fame");
            BuffName = pResult.Read<string>(i, "BuffName");
        }
    }
}
