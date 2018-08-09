using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Produce
{
    public class Produce
    {
        public ushort ProductID { get; }

        public string ProduceIndex { get; }

        public string Product { get; }

        public uint Lot { get; }

        public string Raw0 { get; }

        public uint Quantity0 { get; }

        public string Raw1 { get; }

        public uint Quantity1 { get; }

        public string Raw2 { get; }

        public uint Quantity2 { get; }

        public string Raw3 { get; }

        public uint Quantity3 { get; }

        public string Raw4 { get; }

        public uint Quantity4 { get; }

        public string Raw5 { get; }

        public uint Quantity5 { get; }

        public string Raw6 { get; }

        public uint Quantity6 { get; }

        public string Raw7 { get; }

        public uint Quantity7 { get; }

        public uint MasteryType { get; }

        public uint MasteryGain { get; }

        public uint NeededMasteryType { get; }

        public uint NeededMasteryGain{ get; }

        public Produce(SHNResult pResult, int i)
        {
            ProductID = pResult.Read<ushort>(i, "ProductID");
            ProduceIndex = pResult.Read<string>(i, "ProduceIndex");
            Product = pResult.Read<string>(i, "Product");
            Lot = pResult.Read<uint>(i, "Lot");
            Raw0 = pResult.Read<string>(i, "Raw0");
            Quantity0 = pResult.Read<uint>(i, "Quantity0");
            Raw1 = pResult.Read<string>(i, "Raw1");
            Quantity1 = pResult.Read<uint>(i, "Quantity1");
            Raw2 = pResult.Read<string>(i, "Raw2");
            Quantity2 = pResult.Read<uint>(i, "Quantity2");
            Raw3 = pResult.Read<string>(i, "Raw3");
            Quantity3 = pResult.Read<uint>(i, "Quantity3");
            Raw4 = pResult.Read<string>(i, "Raw4");
            Quantity4 = pResult.Read<uint>(i, "Quantity4");
            Raw5 = pResult.Read<string>(i, "Raw5");
            Quantity5 = pResult.Read<uint>(i, "Quantity5");
            Raw6 = pResult.Read<string>(i, "Raw6");
            Quantity6 = pResult.Read<uint>(i, "Quantity6");
            Raw7 = pResult.Read<string>(i, "Raw7");
            Quantity7 = pResult.Read<uint>(i, "Quantity7");
            MasteryType = pResult.Read<uint>(i, "MasteryType");
            MasteryGain = pResult.Read<uint>(i, "MasteryGain");
            NeededMasteryType = pResult.Read<uint>(i, "NeededMasteryType");
            NeededMasteryGain = pResult.Read<uint>(i, "NeededMasteryGain");
        }
    }
}
