using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.AttendReward
{
    public class AttendReward
    {
        public byte AR_ID { get; }

        public uint AR_Type { get; }

        public byte AR_Count { get; }

        public string AR_ItemInx { get; }

        public AttendReward(SHNResult pResult, int i)
        {
            AR_ID = pResult.Read<byte>(i, "AR_ID");
            AR_Type = pResult.Read<uint>(i, "AR_Type");
            AR_Count = pResult.Read<byte>(i, "AR_Count");
            AR_ItemInx = pResult.Read<string>(i, "AR_ItemInx");
        }
    }
}
