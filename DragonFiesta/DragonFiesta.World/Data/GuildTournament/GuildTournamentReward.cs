using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildTournament
{
    public class GuildTournamentReward
    {
        public byte Rank { get; }

        public byte RewardGroup { get; }

        public uint RewardType { get; }

        public uint Value1 { get; }

        public uint Value2 { get; }

        public uint Value3 { get; }

        public uint IO_Str { get; }

        public uint IO_Con { get; }

        public uint IO_Dex { get; }

        public uint IO_Int { get; }

        public uint IO_Men { get; }


        public GuildTournamentReward(SHNResult pResult, int i)
        {

        }
    }
}
