using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Items;
using DragonFiesta.Utils.Config;
using DragonFiesta.Zone.Data.Stats;

namespace DragonFiesta.Zone.Data.Characters
{
    public class CharacterLevelParameter
    {

        public byte Level { get; private set; }

        public StatsHolder Stats { get; set; }

        public FreeStatData FreeStats { get; set; }
        /// <summary>
        /// The amount of HP you get for a stone.
        /// </summary>
        public uint StoneHP { get; private set; }
        /// <summary>
        /// The amount of SP you get for a stone.
        /// </summary>
        public uint StoneSP { get; private set; }
        public ushort MaxHPStones { get; private set; }
        public ushort MaxSPStones { get; private set; }


        public uint PriceHPStone { get; private set; }
        public uint PriceSPStone { get; private set; }


        public CharacterLevelParameter(SQLResult pResult, int i)
        {
            Level = pResult.Read<byte>(i, "Level");
            Stats = new StatsHolder()
            {
                Str = pResult.Read<short>(i, "Strength"),
                End = pResult.Read<short>(i, "Constitution"),
                Dex = pResult.Read<short>(i, "Dexterity"),
                Int = pResult.Read<short>(i, "Intelligence"),
                Spr = pResult.Read<short>(i, "MentalPower"),
                MaxHP = pResult.Read<int>(i, "MaxHP"),
                MaxSP = pResult.Read<int>(i, "MaxSP"),
                MaxLP = pResult.Read<int>(i, "MaxLightPower"),
                WalkSpeed = GameConfiguration.Instance.WalkSetting.WalkSpeed,
                RunSpeed = GameConfiguration.Instance.WalkSetting.RunSpeed,
            };

            StoneHP = pResult.Read<uint>(i, "SoulHP");
            StoneSP = pResult.Read<uint>(i, "SoulSP");
            MaxHPStones = pResult.Read<ushort>(i, "MAXSoulHP");
            MaxSPStones = pResult.Read<ushort>(i, "MAXSoulSP");
            PriceHPStone = pResult.Read<uint>(i, "PriceHPStone");
            PriceSPStone = pResult.Read<uint>(i, "PriceSPStone");
        }
    }
}
