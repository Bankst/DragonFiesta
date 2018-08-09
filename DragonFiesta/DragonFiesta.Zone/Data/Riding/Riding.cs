using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Riding
{
    public class Riding
    {
        public ushort Handle { get; }

        public string ItemID { get; }

        public string Name { get; }

        public string BodyType { get; }

        public string Shape { get; }

        public ushort UseTime { get; }

        public string FeedType { get; }

        public string Texture { get; }

        public ushort FeedGauge { get; }

        public ushort HGauge { get; }

        public ushort InitHGauge { get; }

        public ushort Tick { get; }

        public ushort UGauge { get; }

        public ushort RunSpeed { get; }

        public ushort FootSpeed { get; }

        public ushort CastingTime { get; }

        public uint CoolTime { get; }

        public string IconFileN { get; }

        public ushort IconIndex { get; }

        public string ImageN { get; }

        public string ImageH { get; }

        public string ImageE { get; }

        public string DummyA { get; }

        public string DummyB { get; }


        public Riding(SHNResult pResult, int i)
        {
            Handle = pResult.Read<ushort>(i, "Handle");
            ItemID = pResult.Read<string>(i, "ItemID");
            Name = pResult.Read<string>(i, "Name");
            BodyType = pResult.Read<string>(i, "BodyType");
            Shape = pResult.Read<string>(i, "Shape");
            UseTime = pResult.Read<ushort>(i, "UseTime");
            FeedType = pResult.Read<string>(i, "FeedType");
            Texture = pResult.Read<string>(i, "Texture");
            FeedGauge = pResult.Read<ushort>(i, "FeedGauge");
            HGauge = pResult.Read<ushort>(i, "HGauge");
            InitHGauge = pResult.Read<ushort>(i, "InitHGauge");
            Tick = pResult.Read<ushort>(i, "Tick");
            UGauge = pResult.Read<ushort>(i, "UGauge");
            RunSpeed = pResult.Read<ushort>(i, "RunSpeed");
            FootSpeed = pResult.Read<ushort>(i, "FootSpeed");
            CastingTime = pResult.Read<ushort>(i, "CastingTime");
            CoolTime = pResult.Read<uint>(i, "CoolTime");
            IconFileN = pResult.Read<string>(i, "IconFileN");
            IconIndex = pResult.Read<ushort>(i, "IconIndex");
            IconFileN = pResult.Read<string>(i, "IconFileN");
            ImageN = pResult.Read<string>(i, "ImageN");
            ImageH = pResult.Read<string>(i, "ImageH");
            ImageE = pResult.Read<string>(i, "ImageE");
            DummyA = pResult.Read<string>(i, "DummyA");
            DummyB = pResult.Read<string>(i, "DummyB");
        }
    }
}
