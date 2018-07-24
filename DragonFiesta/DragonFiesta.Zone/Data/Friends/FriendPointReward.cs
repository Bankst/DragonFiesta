using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Friends
{

    public class FriendPointReward
    {
        public ushort ItemId { get; private set; }

        public byte Amount { get; private set; }

        public ushort Rate { get; private set; }

        public FriendPointReward(SQLResult Result,int i)
        {
            ItemId = Result.Read<ushort>(i, "ItemId");
            Amount = Result.Read<byte>(i, "Amount");
            Rate = Result.Read<ushort>(i, "FPRate");
        }
    }
}
