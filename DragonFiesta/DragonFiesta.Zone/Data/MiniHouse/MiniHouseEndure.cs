using DragonFiesta.Utils.IO.SHN;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    public class MiniHouseEndure
    {
        public ushort Handle { get; }

        public Dictionary<byte, short> Endure { get; private set; }


        public MiniHouseEndure(SHNResult pResult, int row)
        {
            Handle = pResult.Read<ushort>(row, "Handle");
            Endure = new Dictionary<byte, short>();
            for (var i = 0; i < 2; i++)
            {
                var colname = i == 0 ? "Endure" : $"Unknown: {i}";
                Endure.Add((byte)(i + 1), pResult.Read<short>(row, colname));
            }
        }
    }
}
