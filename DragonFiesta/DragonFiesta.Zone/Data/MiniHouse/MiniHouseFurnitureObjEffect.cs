using DragonFiesta.Utils.IO.SHN;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    public class MiniHouseFurnitureObjEffect
    {
        public ushort Handle { get; }

        public string ItemID { get; }

        public uint EffectEnum { get; }

        public string EffectIndex { get; }

        public Dictionary<byte, int> ApplyRange { get; }

        public Dictionary<byte, int> UseRange { get; }

        public string NeedItem { get; }

        public uint NeedMoney { get; }

        public string EffectName { get; }

        public string EffectSound { get; }

        public MiniHouseFurnitureObjEffect(SHNResult pResult, int row)
        {
            Handle = pResult.Read<ushort>(row, "Handle");
            ItemID = pResult.Read<string>(row, "ItemID");
            EffectEnum = pResult.Read<uint>(row, "EffectEnum");
            EffectIndex = pResult.Read<string>(row, "EffectIndex");

            ApplyRange = new Dictionary<byte, int>();
            for (var i = 0; i < 2; i++)
            {
                var colname = i == 0 ? "ApplyRange" : $"Unknown: {i}";
                ApplyRange.Add((byte)(i + 1), pResult.Read<short>(row, colname));
            }

            UseRange = new Dictionary<byte, int>();
            for (var i = 0; i < 3; i++)
            {
                var colname = i == 0 ? "UseRange" : $"Unknown: {i}";
                UseRange.Add((byte)(i + 1), pResult.Read<short>(row, colname));
            }

            NeedItem = pResult.Read<string>(row, "NeedItem");
            NeedMoney = pResult.Read<uint>(row, "NeedMoney");
            EffectName = pResult.Read<string>(row, "EffectName");
            EffectSound = pResult.Read<string>(row, "EffectSound");

        }
    }
}
