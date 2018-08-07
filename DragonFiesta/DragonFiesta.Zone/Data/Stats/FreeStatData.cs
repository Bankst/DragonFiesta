using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Stats
{
    public class FreeStatData
    {

        public byte Level { get; private set; }

        public int WeaponDamage { get; private set; }

        public int MagicDamage { get; private set; }

        public short WeaponDefense { get; private set; }

        public short BlockRate { get; private set; }

        public short MaxHP { get; private set; }

        public short MaxSP { get; private set; }

        public short Aim { get; private set; }
        public short Evasion { get; private set; }
        public short MagicDefense { get; private set; }
        public short CriticalRate { get; private set; }


        public FreeStatData(SQLResult Result, int i)
        {
            Level = Result.Read<byte>(i, "Level");

            WeaponDamage = Result.Read<int>(i, "WCAbsolute");
            MagicDamage = Result.Read<int>(i, "MAAbsolute");

            WeaponDefense = Result.Read<short>(i, "ACAbsolute");
            BlockRate = Result.Read<short>(i, "BlockRate");
            MaxHP = Result.Read<short>(i, "MaxHP");

            Aim = Result.Read<short>(i, "THRate");
            Evasion = Result.Read<short>(i, "TBRate");
            MagicDefense = Result.Read<short>(i, "MRAbsolute");
            CriticalRate = Result.Read<short>(i, "CriRate");
            MaxSP = Result.Read<short>(i, "MaxSP");
        }
    }
}
