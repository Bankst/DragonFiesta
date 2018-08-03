using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.KingdomQuest
{
    public class KQVoteMajorityRate
    {
        public byte VoteAgreeRate { get; }

        public KQVoteMajorityRate(SHNResult pResult, int i)
        {
            VoteAgreeRate = pResult.Read<byte>(i, "VoteAgreeRate");
        }
    }
}
