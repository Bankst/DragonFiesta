using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.SpamerReport
{
    public class SpamerReport
    {
        public uint SR_Term { get; }

        public ushort SR_Number { get; }

        public string SR_Message { get; }

        public SpamerReport(SHNResult pResult, int i)
        {
            SR_Term = pResult.Read<uint>(i, "SR_Term");
            SR_Number = pResult.Read<ushort>(i, "SR_Number");
            SR_Message = pResult.Read<string>(i, "SR_Message");
        }
    }
}