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
        }
    }
}
