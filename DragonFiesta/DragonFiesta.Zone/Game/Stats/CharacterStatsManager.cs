using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Data.Stats;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.Game.Stats
{
    public sealed class CharacterStatsManager : StatsManager
    {
        public ZoneCharacter Character { get; private set; }

        public CharacterStatsManager(ZoneCharacter Character) : base()
        {
            this.Character = Character;
            AddStatsChanger(Character.Info.FreeStats.Stats, false);
        }

        //here get class base stats..
        public override StatsHolder BaseStats => Character.Info.LevelParameter.Stats;

        private FreeStatData FreeStats => Character.Info.LevelParameter.FreeStats;

        protected override void DisposeInternal()
        {
            Character = null;
        }

        
        //TODO CALCULATE Stats Data Korrectly
        public override void Update_STR()
        {
            base.Update_STR();
            //calc free stat str
            Character.Info.FreeStats.Stats.WeaponDamageP = (Character.Info.FreeStats.StatPoints_STR * FreeStats.WeaponDamage);
            //GameLog.Write(GameLogType.Debug, "Weapon Damage: {0}%", Character.FreeStats.WeaponDamageP);
        
            SH04Handler.SendUpdateCharacterStats(Character, StatsType.STR,
                StatsType.Damage_Min,
                StatsType.Damage_Max,
                StatsType.Bonus_Damage);

        }

        public override void Update_DEX()
        {
            base.Update_DEX();
            //calc free stat dex
            Character.Info.FreeStats.Stats.AimP = (Character.Info.FreeStats.StatPoints_DEX * FreeStats.Aim);
            Character.Info.FreeStats.Stats.EvasionP = (Character.Info.FreeStats.StatPoints_DEX * FreeStats.Evasion);
            // Character.Info.FreeStats.EvasionP = (Character.Info.FreeStats.Dex * 0.2d);
            //GameLog.Write(GameLogType.Debug, "Aim: {0}%, Evasion: {1}%", Character.FreeStats.AimP, Character.FreeStats.EvasionP);

            SH04Handler.SendUpdateCharacterStats(Character, StatsType.DEX,
                       StatsType.Aim,
                       StatsType.Evasion,

                       StatsType.Bonus_Aim,
                       StatsType.Bonus_Evasion);
        }

        public override void Update_END()
        {
            base.Update_END();
            //calc free stat end
           // Character.Info.FreeStats.ShieldBlockP = (Character.Info.FreeStats.End * 0.1d);
          //  Character.Info.FreeStats.WeaponDefenseP = (Character.Info.FreeStats.End * 0.5d);
            //GameLog.Write(GameLogType.Debug, "Shield block: {0}%, Weapon defense: {1}%", Character.FreeStats.ShieldBlockP, Character.FreeStats.WeaponDefenseP);

            SH04Handler.SendUpdateCharacterStats(Character, StatsType.END,
                StatsType.Defense,
                StatsType.MaxHP,
                StatsType.Bonus_MaxHP,
                StatsType.Bonus_Defense,
                StatsType.BlockRate);
        }

        public override void Update_INT()
        {
            base.Update_INT();
            //calc free stat int
            Character.Info.FreeStats.Stats.MagicDamageP = (Character.Info.FreeStats.Stats.Int * FreeStats.MagicDamage);
            //GameLog.Write(GameLogType.Debug, "Magic Damage: {0}%", Character.FreeStats.MagicDamageP);

            SH04Handler.SendUpdateCharacterStats(Character, StatsType.INT,
                        StatsType.MagicDamage_Min,
                        StatsType.MagicDamage_Max,
                        StatsType.Bonus_MagicDamage);
        }

        public override void Update_SPR()
        {
            base.Update_SPR();
            //calc free stat spr
          
            Character.Info.FreeStats.Stats.CritP = (Character.Info.FreeStats.Stats.Spr * FreeStats.CriticalRate);
            Character.Info.FreeStats.Stats.MagicDefenseP = (Character.Info.FreeStats.Stats.Spr * FreeStats.MagicDefense);
            //GameLog.Write(GameLogType.Debug, "Crit: {0}%, Magic Defense: {1}%", Character.FreeStats.CritP, Character.FreeStats.MagicDefenseP);


            SH04Handler.SendUpdateCharacterStats(Character, StatsType.SPR,
                StatsType.MagicDefense,
                StatsType.MaxSP,

                StatsType.Bonus_MaxSP,
                StatsType.Bonus_MagicDefense,
                StatsType.CriticalRate);
        }

        public override int GetStatByType(StatsType Type)
        {
            switch (Type)
            {



                case StatsType.Bonus_Damage:
                    return (int)Character.Info.FreeStats.Stats.WeaponDamageP;
                case StatsType.Bonus_Defense:
                    return (int)Character.Info.FreeStats.Stats.WeaponDefenseP;

                case StatsType.Bonus_MagicDamage:
                    return (int)Character.Info.FreeStats.Stats.MagicDamageP;
                case StatsType.Bonus_MagicDefense:
                    return (int)Character.Info.FreeStats.Stats.MagicDefenseP;

                case StatsType.Bonus_Aim:
                    return (int)(Character.Info.FreeStats.Stats.AimP * 10);
                case StatsType.Bonus_Evasion:
                    return (int)(Character.Info.FreeStats.Stats.EvasionP * 10);

                case StatsType.CriticalRate:
                    return (int)(Character.Info.FreeStats.Stats.CritP * 10);
                case StatsType.BlockRate:
                    return (int)(Character.Info.FreeStats.Stats.ShieldBlockP * 10);

                case StatsType.Bonus_MaxHP:
                    return (int)(Character.Info.FreeStats.Stats.End * 5);
                case StatsType.Bonus_MaxSP:
                    return (int)(Character.Info.FreeStats.Stats.Spr * 5);
                //TODO LP
                default:
                    return base.GetStatByType(Type);
            }
        }

        
    }
}