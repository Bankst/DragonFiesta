using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.Guild
{
    public class GuildGradeScoreData
    {
        public short GradeScore { get; }

        public GuildGradeScoreData(SHNResult pResult, int i)
        {
            GradeScore = pResult.Read<short>(i, "GradeScore");
        }
    }
}
