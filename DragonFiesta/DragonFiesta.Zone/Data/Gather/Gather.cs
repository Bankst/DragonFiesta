using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Gather
{
    public class Gather
    {
        public ushort GatherID { get; }

        public string Index { get; }

        public uint Type { get; }

        public string NeededTool0 { get; }

        public string NeededTool1 { get; }

        public string NeededTool2 { get; }

        public string EqipItemView { get; }

        public uint AniNumber { get; }

        public uint Gauge { get; }

        public Gather(SHNResult pResult, int i)
        {
            GatherID = pResult.Read<ushort>(i, "GatherID");
            Index = pResult.Read<string>(i, "Index");
            Type = pResult.Read<uint>(i, "Type");
            NeededTool0 = pResult.Read<string>(i, "NeededTool0");
            NeededTool1 = pResult.Read<string>(i, "NeededTool1");
            NeededTool2 = pResult.Read<string>(i, "NeededTool2");
            EqipItemView = pResult.Read<string>(i, "EqipItemView");
            AniNumber = pResult.Read<uint>(i, "AniNumber");
            Gauge = pResult.Read<uint>(i, "Gauge");
        }
    }
}
