using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.Guild
{
    public class GuildLevelScoreData
    {
        public ushort CheckLevel { get; }

        public ushort AddLevelScore { get; }

        public GuildLevelScoreData(SHNResult pResult, int i)
        {
            CheckLevel = pResult.Read<ushort>(i, "CheckLevel");
            AddLevelScore = pResult.Read<ushort>(i, "AddLevelScore");
        }
    }
}
