using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCardMobGroup
    {
        public string MobInx { get; }

        public uint CardMobGroup { get; }

        public CollectCardMobGroup(SHNResult pResult, int i)
        {
            MobInx = pResult.Read<string>(i, "CC_MobInx");
            CardMobGroup = pResult.Read<uint>(i, "CC_CardMobGroup");
        }
    }
}
