using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildAcademy
{
    public class GuildAcademyLevelUp
    {
        public byte Level { get; }

        public uint Point { get; }

        public uint BuffTime { get; }

        public GuildAcademyLevelUp(SHNResult pResult, int i)
        {
            Level = pResult.Read<byte>(i, "Level");
            Point = pResult.Read<uint>(i, "Point");
            BuffTime = pResult.Read<uint>(i, "BuffTime");
        }
    }
}
