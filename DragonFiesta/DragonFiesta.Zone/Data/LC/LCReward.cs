using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.LC
{
    public class LCReward
    {
        public byte LCR_Group { get; }

        public string Item_Inx { get; }

        public byte LCR_Lot { get; }

        public LCReward(SHNResult pResult, int row)
        {
            LCR_Group = pResult.Read<byte>(row, "LCR_Group");
            Item_Inx = pResult.Read<string>(row, "Item_Inx");
            LCR_Lot = pResult.Read<byte>(row, "LCR_Lot");
        }
    }
}
