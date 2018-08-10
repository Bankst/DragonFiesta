using DragonFiesta.Utils.IO.SHN;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Data.KingdomQuest
{
    public class KingdomQuestRew
    {
        public uint ID { get; }

        public string IndexString { get; }

        public string KQBoxItemIDX { get; }

        public Dictionary<byte, short> Reward { get; private set; }

        public KingdomQuestRew(SHNResult pResult, int row)
        {
            ID = pResult.Read<uint>(row, "ID");
            IndexString = pResult.Read<string>(row, "IndexString");
            KQBoxItemIDX = pResult.Read<string>(row, "KQBoxItemIDX");

            Reward = new Dictionary<byte, short>();
            for (var i = 0; i < 27; i++)
            {
                var colname = i == 0 ? "Reward" : $"Unknown: {i}";
                Reward.Add((byte)(i + 1), pResult.Read<short>(row, colname));
            }
        }
    }
}
