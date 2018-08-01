using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCardReward
    {
        public ushort RewardID { get; }

        public uint CardRewardType { get; }

        public ushort CardLot { get; }

        public string RewardItemInx { get; }

        public ushort RewardLot { get; }

        public CollectCardReward(SHNResult pResult, int i)
        {
            RewardID = pResult.Read<ushort>(i, "CC_RewardID");
            CardRewardType = pResult.Read<uint>(i, "CC_CardRewardType");
            CardLot = pResult.Read<ushort>(i, "CC_CardLot");
            RewardItemInx = pResult.Read<string>(i, "CC_RewardItemInx");
            RewardLot = pResult.Read<ushort>(i, "CC_RewardLot");
        }
    }
}
