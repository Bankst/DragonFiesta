using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    public class MiniHouse
    {
        public ushort Handle { get; }

        public string ItemID { get; }

        public string DummyType { get; }

        public string Backimage { get; }

        public ushort KeepTime_Hour { get; }

        public ushort HPTick { get; }

        public ushort SPTick { get; }

        public ushort HPRecovery { get; }

        public ushort SPRecovery { get; }

        public ushort Casting { get; }

        public byte Slot { get; }

        public MiniHouse(SHNResult pResult, int row)
        {
            Handle = pResult.Read<ushort>(row, "Handle");
            ItemID = pResult.Read<string>(row, "ItemID");
            DummyType = pResult.Read<string>(row, "DummyType");
            Backimage = pResult.Read<string>(row, "Backimage");
            KeepTime_Hour = pResult.Read<ushort>(row, "KeepTime_Hour");
            HPTick = pResult.Read<ushort>(row, "HPTick");
            SPTick = pResult.Read<ushort>(row, "SPTick");
            HPRecovery = pResult.Read<ushort>(row, "HPRecovery");
            SPRecovery = pResult.Read<ushort>(row, "SPRecovery");
            Casting = pResult.Read<ushort>(row, "Casting");
            Slot = pResult.Read<byte>(row, "Slot");
        }
    }
}
