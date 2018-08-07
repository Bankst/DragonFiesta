using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildTournament
{
    public class GuildTournamentRequire
    {
        public byte MinLv { get; }

        public ushort MinMem { get; }

        public uint JoinMoney { get; }

        public GuildTournamentRequire(SHNResult pResult, int i)
        {
            MinLv = pResult.Read<byte>(i, "MinLv");
            MinMem = pResult.Read<ushort>(i, "MinMem");
            JoinMoney = pResult.Read<uint>(i, "JoinMoney");
        }
    }
}
