using DragonFiesta.Game.Stats;
using DragonFiesta.Providers.Items;

namespace DragonFiesta.Zone.Game.Stats
{
    public class FreeStats : iStatsChanger
    {
        //It adds damage to the end of an attack, after the defenders defense is taken into account
        // Every free stat point you put into strength increases the END damage output by 1.2 points.
        public byte Str { get; set; }

        public double WeaponDamageP { get; set; }

        //For the first 50 points of free stat End you add , your shield block% is increased by .1%, from 51 on it adds .05% block%.
        //Every free stat end point adds .5 to the defense reduction part.
        public byte End { get; set; }

        public double ShieldBlockP { get; set; }
        public double WeaponDefenseP { get; set; }

        // todo: recalc
        // For the first 33 points Aim% is increased by .3%, from 33 to 67 Aim% is increased by .2%, and from 67 on Aim% is incresed by .1%.
        // For the first 50 points Evasion% is increased by .2%, after the first 50 it is only increased by .1%.
        public byte Dex { get; set; }

        public double AimP { get; set; }
        public double EvasionP { get; set; }

        //It adds damage to the end of an attack, after the defenders defense is taken into account
        //every stat point added into this increases the end output damage of magical attacks by 1.2 points.
        public byte Int { get; set; }

        public double MagicDamageP { get; set; }

        //For the first 25 Spirit points you recieve .2% critical chance, for every point after 25 you get a .1% added chance of performing a critical attack.
        //Each point added to free stats here increases the magic damage reduction by .5.
        public byte Spr { get; set; }

        public double CritP { get; set; }
        public double MagicDefenseP { get; set; }

        //iStatsChanger
        StatsHolder iStatsChanger.Stats => new StatsHolder()
        {
	        Str = Str,
	        End = End,
	        Dex = Dex,
	        Int = Int,
	        Spr = Spr,
        };
    }
}