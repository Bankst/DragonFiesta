using System;
using System.Threading;

namespace DragonFiesta.Game.Stats
{
    public abstract class StatsManager
    {
        /// <summary>
        /// The base stats (eg: the class params of characters or stats from mobinfo of mobs)
        /// </summary>
        public abstract StatsHolder BaseStats { get; }

        /// <summary>
        /// The normal stats based on the BaseStats
        /// </summary>
        public StatsHolder NormalStats { get; private set; }

        /// <summary>
        /// The full stats based on the NormalStats and increased by the StatsChangers
        /// </summary>
        public StatsHolder FullStats { get; private set; }

        public SecureWriteCollection<iStatsChanger> StatsChangers { get; private set; }
        public bool IsDisposed => (_isDisposedInt > 0);
	    private int _isDisposedInt;
        private Func<iStatsChanger, bool> _statsChangerFuncAdd;
        private Func<iStatsChanger, bool> _statsChangerFuncRemove;
        private Action _statsChangerFuncClear;
        protected object ThreadLocker { get; private set; }

        protected StatsManager()
        {
            NormalStats = new StatsHolder();
            FullStats = new StatsHolder();
            StatsChangers = new SecureWriteCollection<iStatsChanger>(out _statsChangerFuncAdd, out _statsChangerFuncRemove, out _statsChangerFuncClear);
            ThreadLocker = new object();
        }
        public void Dispose()
        {
	        if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	        DisposeInternal();
	        NormalStats = null;
	        FullStats = null;
	        StatsChangers.Dispose();
	        StatsChangers = null;
	        _statsChangerFuncAdd = null;
	        _statsChangerFuncRemove = null;
	        _statsChangerFuncClear = null;
	        ThreadLocker = null;
        }
        protected abstract void DisposeInternal();
        public bool AddStatsChanger(iStatsChanger changer, bool updateStats = true)
        {
            lock (ThreadLocker)
            {
	            if (!_statsChangerFuncAdd.Invoke(changer)) return false;
	            if (updateStats)
	            {
		            UpdateByChanger(changer);
	            }
	            return true;
            }
        }
        public bool RemoveStatsChanger(iStatsChanger changer, bool updateStats = true)
        {
            lock (ThreadLocker)
            {
	            if (!_statsChangerFuncRemove.Invoke(changer)) return false;
	            if (updateStats)
	            {
		            UpdateByChanger(changer);
	            }
	            return true;
            }
        }
        public double CalculateCastingTimeMs(double startCastingTimeMs)
        {
            var time = startCastingTimeMs;

            if (FullStats.IncreaseCastingTime > 0)
            {
                time -= (startCastingTimeMs / 100d * FullStats.IncreaseCastingTime);
            }
            return time;
        }
        public void UpdateAll()
        {
            Update_STR();
            Update_END();
            Update_DEX();
            Update_INT();
            Update_SPR();
            Update_Speed();
            Update_Stuff();
        }
        public void UpdateByChanger(iStatsChanger changer)
        {
            if (changer.Stats.Str != 0
                || changer.Stats.WeaponDamage.Min != 0
                || changer.Stats.WeaponDamage.Max != 0
                || changer.Stats.IncreaseStrPercent != 0d)
            {
                Update_STR();
            }
            if (changer.Stats.Dex != 0
                || changer.Stats.Aim != 0
                || changer.Stats.Evasion != 0
                || changer.Stats.IncreaseDexPercent != 0d)
            {
                Update_DEX();
            }
            if (changer.Stats.End != 0
                || changer.Stats.MaxHP != 0
                || changer.Stats.WeaponDefense != 0
                || changer.Stats.IncreaseEndPercent != 0d
                || changer.Stats.IncreaseDefensePercent != 0d
                || changer.Stats.IncreaseHPPercent != 0d)
            {
                Update_END();
            }
            if (changer.Stats.Int != 0
                || changer.Stats.MagicDamage.Min != 0
                || changer.Stats.MagicDamage.Max != 0
                || changer.Stats.IncreaseIntPercent != 0d)
            {
                Update_INT();
            }
            if (changer.Stats.Spr != 0
                || changer.Stats.MaxSP != 0
                || changer.Stats.MagicDefense != 0
                || changer.Stats.IncreaseSprPercent != 0d
                || changer.Stats.IncreaseSPPercent != 0d)
            {
                Update_SPR();
            }
            if (changer.Stats.WalkSpeed != 0
                || changer.Stats.RunSpeed != 0
                || changer.Stats.IncreaseSpeedPercent != 0d)
            {
                Update_Speed();
            }
            if (changer.Stats.IncreaseCastingTime != 0d)
            {
                Update_Stuff();
            }
            if (changer.Stats.MaxLP != 100)
            {
                Update_LP();
            }
        }
        public virtual void Update_STR()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.Str = BaseStats.Str;
                NormalStats.WeaponDamage.Min = (short)(BaseStats.Str + BaseStats.WeaponDamage.Min);
                NormalStats.WeaponDamage.Max = (short)(BaseStats.Str + BaseStats.WeaponDamage.Max);
                // Full stats
                FullStats.Str = NormalStats.Str;
                FullStats.WeaponDamage.Min = NormalStats.WeaponDamage.Min;
                FullStats.WeaponDamage.Max = NormalStats.WeaponDamage.Max;
                StatsChangerAction((sc) =>
                {
                    FullStats.Str += sc.Stats.Str;
                    FullStats.WeaponDamage.Min += (short)(sc.Stats.Str + sc.Stats.WeaponDamage.Min);
                    FullStats.WeaponDamage.Max += (short)(sc.Stats.Str + sc.Stats.WeaponDamage.Max);
                });
                //increase str by percent
                if (FullStats.IncreaseStrPercent > 0)
                    FullStats.Str += (int)(FullStats.Str / 100d * FullStats.IncreaseStrPercent);
            }
        }
        public virtual void Update_DEX()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.Dex = BaseStats.Dex;
                NormalStats.Aim = (short)(BaseStats.Dex + BaseStats.Aim);
                NormalStats.Evasion = (short)(BaseStats.Dex + BaseStats.Evasion);
                // Full stats
                FullStats.Dex = NormalStats.Dex;
                FullStats.Aim = NormalStats.Aim;
                FullStats.Evasion = NormalStats.Evasion;
                StatsChangerAction((sc) =>
                {
                    FullStats.Dex += sc.Stats.Dex;
                    FullStats.Aim += (short)(sc.Stats.Dex + sc.Stats.Aim);
                    FullStats.Evasion += (short)(sc.Stats.Dex + sc.Stats.Evasion);
                });
                //increase dex by percent
                if (FullStats.IncreaseDexPercent > 0)
                    FullStats.Dex += (int)(FullStats.Dex / 100d * FullStats.IncreaseDexPercent);
            }
        }
        public virtual void Update_END()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.End = BaseStats.End;
                NormalStats.MaxHP = (BaseStats.End * 5) + BaseStats.MaxHP;
                NormalStats.WeaponDefense = (short)(BaseStats.End + BaseStats.WeaponDefense);
                // Full stats
                FullStats.End = NormalStats.End;
                FullStats.MaxHP = NormalStats.MaxHP;
                FullStats.WeaponDefense = NormalStats.WeaponDefense;

                StatsChangerAction((sc) =>
                {
                    FullStats.End += sc.Stats.End;
                    FullStats.MaxHP += (sc.Stats.End * 5) + sc.Stats.MaxHP;
                    FullStats.WeaponDefense += (short)(sc.Stats.End + sc.Stats.WeaponDefense);
                });
                //increase end by percent
                if (FullStats.IncreaseEndPercent > 0)
                    FullStats.End += (int)(FullStats.End / 100d * FullStats.IncreaseEndPercent);

                //increase hp by %
                if (FullStats.IncreaseHPPercent > 0)
                    FullStats.MaxHP += (int)(FullStats.MaxHP / 100d * FullStats.IncreaseHPPercent);

                //increase defense by %
                if (FullStats.IncreaseDefensePercent > 0)
                    FullStats.WeaponDefense += (int)(FullStats.WeaponDefense / 100d * FullStats.IncreaseDefensePercent);
            }
        }
        public virtual void Update_INT()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.Int = BaseStats.Int;
                NormalStats.MagicDamage.Min = (short)(BaseStats.Int + BaseStats.MagicDamage.Min);
                NormalStats.MagicDamage.Max = (short)(BaseStats.Int + BaseStats.MagicDamage.Max);
                // Full stats
                FullStats.Int = NormalStats.Int;
                FullStats.MagicDamage.Min = NormalStats.MagicDamage.Min;
                FullStats.MagicDamage.Max = NormalStats.MagicDamage.Max;
                StatsChangerAction((sc) =>
                {
                    FullStats.Int += sc.Stats.Int;
                    FullStats.MagicDamage.Min += (short)(sc.Stats.Int + sc.Stats.MagicDamage.Min);
                    FullStats.MagicDamage.Max += (short)(sc.Stats.Int + sc.Stats.MagicDamage.Max);
                });
                //increase int by percent
                if (FullStats.IncreaseIntPercent > 0)
                    FullStats.Int += (int)(FullStats.Int / 100d * FullStats.IncreaseIntPercent);
            }
        }
        public virtual void Update_LP()
        {
            lock (ThreadLocker)
            {
                //TODO....
            }
        }
        public virtual void Update_SPR()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.Spr = BaseStats.Spr;
                NormalStats.MaxSP = (BaseStats.Spr * 5) + BaseStats.MaxSP;
                NormalStats.MagicDefense = (short)(BaseStats.Spr + BaseStats.MagicDefense);
                // Full stats
                FullStats.Spr = NormalStats.Spr;
                FullStats.MaxSP = NormalStats.MaxSP;
                FullStats.MagicDefense = NormalStats.MagicDefense;
                StatsChangerAction((sc) =>
                {
                    FullStats.Spr += sc.Stats.Spr;
                    FullStats.MaxSP += (sc.Stats.Spr * 5) + sc.Stats.MaxSP;
                    FullStats.MagicDefense += (short)(sc.Stats.Spr + sc.Stats.MagicDefense);
                });
                //increase spr by percent
                if (FullStats.IncreaseSprPercent > 0)
                    FullStats.Spr += (int)(FullStats.Spr / 100d * FullStats.IncreaseSprPercent);
                //increase sp by %
                if (FullStats.IncreaseSPPercent > 0)
                    FullStats.MaxSP += (int)(FullStats.MaxSP / 100d * FullStats.IncreaseSPPercent);
            }
        }

        public virtual void Update_Speed()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.WalkSpeed = BaseStats.WalkSpeed;
                NormalStats.RunSpeed = BaseStats.RunSpeed;
                NormalStats.IncreaseSpeedPercent = BaseStats.IncreaseSpeedPercent;
                // Full Stats
                FullStats.WalkSpeed = NormalStats.WalkSpeed;
                FullStats.RunSpeed = NormalStats.RunSpeed;
                FullStats.IncreaseSpeedPercent = NormalStats.IncreaseSpeedPercent;
                StatsChangerAction((sc) =>
                {
                    FullStats.WalkSpeed += sc.Stats.WalkSpeed;
                    FullStats.RunSpeed += sc.Stats.RunSpeed;
                    FullStats.IncreaseSpeedPercent += sc.Stats.IncreaseSpeedPercent;
                });
                //increase runspeed by %
                if (FullStats.IncreaseSpeedPercent > 0)
                    FullStats.RunSpeed += (ushort)(FullStats.RunSpeed / 100d * FullStats.IncreaseSpeedPercent);
            }
        }

        public virtual void Update_Stuff()
        {
            lock (ThreadLocker)
            {
                // Normal stats
                NormalStats.IncreaseCastingTime = BaseStats.IncreaseCastingTime;
                // Full stats
                FullStats.IncreaseCastingTime = NormalStats.IncreaseCastingTime;
                StatsChangerAction((sc) =>
                {
                    FullStats.IncreaseCastingTime += sc.Stats.IncreaseCastingTime;
                });
            }
        }
        public virtual int GetStatByType(StatsType type)
        {
            switch (type)
            {
                case StatsType.STR:
                    return FullStats.Str;
                case StatsType.END:
                    return FullStats.End;
                case StatsType.DEX:
                    return FullStats.Dex;
                case StatsType.INT:
                    return FullStats.Int;
                case StatsType.SPR:
                    return FullStats.Spr;
                case StatsType.Damage_Min:
                    return FullStats.WeaponDamage.Min;
                case StatsType.Damage_Max:
                    return FullStats.WeaponDamage.Max;
                case StatsType.Defense:
                    return FullStats.WeaponDefense;

                case StatsType.Aim:
                    return FullStats.Aim;
                case StatsType.Evasion:
                    return FullStats.Evasion;

                case StatsType.MagicDamage_Min:
                    return FullStats.MagicDamage.Min;
                case StatsType.MagicDamage_Max:
                    return FullStats.MagicDamage.Max;
                case StatsType.MagicDefense:
                    return FullStats.MagicDefense;

                case StatsType.MaxHP:
                    return FullStats.MaxHP;
                case StatsType.MaxSP:
                    return FullStats.MaxSP;
	            case StatsType.Bonus_Damage:
		            break;
	            case StatsType.Bonus_Defense:
		            break;
	            case StatsType.Bonus_MagicDamage:
		            break;
	            case StatsType.Bonus_MagicDefense:
		            break;
	            case StatsType.Bonus_Aim:
		            break;
	            case StatsType.Bonus_Evasion:
		            break;
	            case StatsType.CriticalRate:
		            break;
	            case StatsType.BlockRate:
		            break;
	            case StatsType.Bonus_MaxHP:
		            break;
	            case StatsType.Bonus_MaxSP:
		            break;
	            case StatsType.MaxLP:
		            break;
	            default:
		            throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return 0;
        }
        protected void StatsChangerAction(Action<iStatsChanger> action)
        {
            lock (ThreadLocker)
            {
                foreach (var stat in StatsChangers)
                {
                    action.Invoke(stat);
                }
            }
        }
    }
}