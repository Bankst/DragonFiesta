
public sealed class StatsHolder
{
    public int Str { get; set; }
    public int End { get; set; }
    public int Dex { get; set; }
    public int Int { get; set; }
    public int Spr { get; set; }
    public int MaxHP { get; set; }
    public int MaxSP { get; set; }

    public int MaxLP { get; set; }
    public int Aim { get; set; }
    public int Evasion { get; set; }
    public MinMax<int> WeaponDamage { get; set; }
    public int WeaponDefense { get; set; }
    public MinMax<int> MagicDamage { get; set; }
    public int MagicDefense { get; set; }
    public int BlockRate { get; set; }
    public int CriticalRate { get; set; }
    public MinMax<int> CriticalWeaponDamage { get; set; }
    public MinMax<int> CriticalMagicDamage { get; set; }
    public ushort WalkSpeed { get; set; }
    public ushort RunSpeed { get; set; }
    public double IncreaseSpeedPercent { get; set; }
    public double IncreaseCastingTime { get; set; }
    public double IncreaseHPPercent { get; set; }
    public double IncreaseSPPercent { get; set; }
    public double IncreaseStrPercent { get; set; }
    public double IncreaseEndPercent { get; set; }
    public double IncreaseDexPercent { get; set; }
    public double IncreaseIntPercent { get; set; }
    public double IncreaseSprPercent { get; set; }
    public double IncreaseDefensePercent { get; set; }
    public StatsHolder()
    {
        WeaponDamage = new MinMax<int>();
        MagicDamage = new MinMax<int>();
        CriticalWeaponDamage = new MinMax<int>();
        CriticalMagicDamage = new MinMax<int>();
    }
    ~StatsHolder()
    {
        WeaponDamage = null;
        MagicDamage = null;
        CriticalWeaponDamage = null;
        CriticalMagicDamage = null;
    }
    public static StatsHolder operator +(StatsHolder s1, StatsHolder s2)
    {
        var newStats = new StatsHolder()
        {
            Str = s1.Str + s2.Str,
            End = s1.End + s2.End,
            Dex = s1.Dex + s2.Dex,
            Int = s1.Int + s2.Int,
            Spr = s1.Spr + s2.Spr,
            MaxHP = s1.MaxHP + s2.MaxHP,
            MaxSP = s1.MaxSP + s2.MaxSP,
            MaxLP = s1.MaxLP + s2.MaxLP,
            Aim = s1.Aim + s2.Aim,
            Evasion = s1.Evasion + s2.Evasion,
            WeaponDamage = new MinMax<int>((int)(s1.WeaponDamage.Min + s2.WeaponDamage.Min), (int)(s1.WeaponDamage.Max + s2.WeaponDamage.Max)),
            WeaponDefense = (int)(s1.WeaponDefense + s2.WeaponDefense),
            MagicDamage = new MinMax<int>((int)(s1.MagicDamage.Min + s2.MagicDamage.Min), (int)(s1.MagicDamage.Max + s2.MagicDamage.Max)),
            MagicDefense = (int)(s1.MagicDefense + s2.MagicDefense),
            BlockRate = (int)(s1.BlockRate + s2.BlockRate),
            CriticalRate = (int)(s1.CriticalRate + s2.CriticalRate),
            CriticalWeaponDamage = new MinMax<int>((int)(s1.CriticalWeaponDamage.Min + s2.CriticalWeaponDamage.Min), (int)(s1.CriticalWeaponDamage.Max + s2.CriticalWeaponDamage.Max)),
            CriticalMagicDamage = new MinMax<int>((int)(s1.CriticalMagicDamage.Min + s2.CriticalMagicDamage.Min), (int)(s1.CriticalMagicDamage.Max + s2.CriticalMagicDamage.Max)),
            WalkSpeed = (ushort)(s1.WalkSpeed + s2.WalkSpeed),
            RunSpeed = (ushort)(s1.RunSpeed + s2.RunSpeed),
            IncreaseCastingTime = (s1.IncreaseCastingTime + s2.IncreaseCastingTime),
            IncreaseSpeedPercent = (s1.IncreaseSpeedPercent + s2.IncreaseSpeedPercent),
            IncreaseHPPercent = (s1.IncreaseHPPercent + s2.IncreaseHPPercent),
            IncreaseSPPercent = (s1.IncreaseSPPercent + s2.IncreaseSPPercent),
            IncreaseStrPercent = (s1.IncreaseStrPercent + s2.IncreaseStrPercent),
            IncreaseEndPercent = (s1.IncreaseEndPercent + s2.IncreaseEndPercent),
            IncreaseDexPercent = (s1.IncreaseDexPercent + s2.IncreaseDexPercent),
            IncreaseIntPercent = (s1.IncreaseIntPercent + s2.IncreaseIntPercent),
            IncreaseSprPercent = (s1.IncreaseSprPercent + s2.IncreaseSprPercent),
            IncreaseDefensePercent = (s1.IncreaseDefensePercent + s2.IncreaseDefensePercent),
        };
        return newStats;
    }
    public static StatsHolder operator -(StatsHolder s1, StatsHolder s2)
    {
        var newStats = new StatsHolder()
        {
            Str = s1.Str + s2.Str,
            End = s1.End + s2.End,
            Dex = s1.Dex + s2.Dex,
            Int = s1.Int + s2.Int,
            Spr = s1.Spr + s2.Spr,
            MaxHP = s1.MaxHP + s2.MaxHP,
            MaxSP = s1.MaxSP + s2.MaxSP,
            MaxLP = s1.MaxLP + s2.MaxLP,
            Aim = s1.Aim + s2.Aim,
            Evasion = s1.Evasion + s2.Evasion,
            WeaponDamage = new MinMax<int>((int)(s1.WeaponDamage.Min - s2.WeaponDamage.Min), (int)(s1.WeaponDamage.Max - s2.WeaponDamage.Max)),
            WeaponDefense = (short)(s1.WeaponDefense - s2.WeaponDefense),
            MagicDamage = new MinMax<int>((int)(s1.MagicDamage.Min - s2.MagicDamage.Min), (int)(s1.MagicDamage.Max - s2.MagicDamage.Max)),
            MagicDefense = (int)(s1.MagicDefense - s2.MagicDefense),
            BlockRate = (int)(s1.BlockRate - s2.BlockRate),
            CriticalRate = (int)(s1.CriticalRate - s2.CriticalRate),
            CriticalWeaponDamage = new MinMax<int>((int)(s1.CriticalWeaponDamage.Min - s2.CriticalWeaponDamage.Min), (int)(s1.CriticalWeaponDamage.Max - s2.CriticalWeaponDamage.Max)),
            CriticalMagicDamage = new MinMax<int>((int)(s1.CriticalMagicDamage.Min - s2.CriticalMagicDamage.Min), (int)(s1.CriticalMagicDamage.Max - s2.CriticalMagicDamage.Max)),
            WalkSpeed = (ushort)(s1.WalkSpeed - s2.WalkSpeed),
            RunSpeed = (ushort)(s1.RunSpeed - s2.RunSpeed),
            IncreaseCastingTime = (s1.IncreaseCastingTime - s2.IncreaseCastingTime),
            IncreaseSpeedPercent = (s1.IncreaseSpeedPercent - s2.IncreaseSpeedPercent),
            IncreaseHPPercent = (s1.IncreaseHPPercent - s2.IncreaseHPPercent),
            IncreaseSPPercent = (s1.IncreaseSPPercent - s2.IncreaseSPPercent),
            IncreaseStrPercent = (s1.IncreaseStrPercent - s2.IncreaseStrPercent),
            IncreaseEndPercent = (s1.IncreaseEndPercent - s2.IncreaseEndPercent),
            IncreaseDexPercent = (s1.IncreaseDexPercent - s2.IncreaseDexPercent),
            IncreaseIntPercent = (s1.IncreaseIntPercent - s2.IncreaseIntPercent),
            IncreaseSprPercent = (s1.IncreaseSprPercent - s2.IncreaseSprPercent),
            IncreaseDefensePercent = (s1.IncreaseDefensePercent - s2.IncreaseDefensePercent),
        };
        return newStats;
    }

}