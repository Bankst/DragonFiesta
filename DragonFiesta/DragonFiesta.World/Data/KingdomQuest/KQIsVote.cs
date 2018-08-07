using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.KingdomQuest
{
    public class KQIsVote
    {
        public short ID { get; }

        public byte IsVote { get; }

        public KQIsVote(SHNResult pResult, int i)
        {
            ID = pResult.Read<short>(i, "ID");
            IsVote = pResult.Read<byte>(i, "IsVote");
        }
    }
}
