using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Game.CommandAccess;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Config;
using System;
using System.Threading;

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterBase : IDisposable
    {
        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;


        public object ThreadLocker { get; private set; }

        public abstract bool IsConnected { get; }

        public ClientLoginInfo LoginInfo { get; set; }

        public CharacterInfo Info { get; set; }

        public CharacterAreaInfo AreaInfo { get; set; }

        public CharacterStyle Style { get; set; }


        public abstract bool IsGM { get; }
      
        protected CharacterBase()
        {
            ThreadLocker = new object();

            AreaInfo = new CharacterAreaInfo();
            Style = new CharacterStyle();
        }

        ~CharacterBase()
        {
            Dispose();
        }
        //Style by Errors Code from Client...
        //Only Load one when Added to CharacterManager
        public virtual bool FinalizeLoad(out CharacterErrors Result)
        {
            Result = CharacterErrors.LoadOK;
            return true;
        }
        //AllDatas we can refreashes all
        public virtual bool RefreshCharacter(SQLResult pRes, int i, out CharacterErrors Result)
        {
            Result = CharacterErrors.ErrorInCharacterInfo;

            if (!LoginInfo.RefreshFromSQL(pRes, i))
            {
                Result = CharacterErrors.RequestedCharacterIDNotMatching;
                return false;
            }

            if (!AreaInfo.RefreshFromSQL(pRes, i))
            {
                Result = CharacterErrors.ErrorInArena;
                return false;
            }

            if (!Style.RefreshFromSQL(pRes, i))
            {
                Result = CharacterErrors.ErrorInAppearance;
                return false;
            }

            if (!Info.RefreshFromSQL(pRes, i))
            {
                Result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
            Result = CharacterErrors.LoadOK;

            return true;
        }

        public virtual bool GiveEXP(uint Amount, ushort MobID = 0xFFFF)
        {
            if (Info.Level >= GameConfiguration.Instance.LimitSetting.LevelLimit)
                return false;
            Info.EXP += Amount;

            ulong nextEXP = 0;
            while (Info.EXP > (nextEXP = CharacterDataProviderBase.GetEXPForNextLevel(Info.Level)))
            {
                LevelUP(MobID);
            }
            return true;
        }
        public virtual bool LevelUP(ushort MobID = 0xFFFF, bool FinalizeLevelUp = true)
        {
            if (Info.Level >= GameConfiguration.Instance.LimitSetting.LevelLimit)
                return false;

            Info.Level++;


            return true;
        }
        public abstract bool Save();

        public virtual void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal()
        {
        }
    }
}