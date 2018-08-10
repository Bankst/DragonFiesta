using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.LC
{
    public class LCGroupRate
    {
        public ushort Item_ID { get; }

        public byte LCR_Group { get; }

        public uint LCR_Rate { get; }

        public LCGroupRate(SHNResult pResult, int row)
        {
            Item_ID = pResult.Read<ushort>(row, "Item_ID");
            LCR_Group = pResult.Read<byte>(row, "LCR_Group");
            LCR_Rate = pResult.Read<uint>(row, "LCR_Rate");
        }
    }
}
