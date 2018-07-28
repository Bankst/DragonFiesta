#region

using System;
using System.Linq;
using System.Threading;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Config;

#endregion

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterBase : DBCharacter, IDisposable
    {
        public bool IsDisposed => (_isDisposedInt > 0);
	    private int _isDisposedInt;

	    private DBCharacter LazyLoadMe()
	    {
		    return EDM.GetWorldEntity().DBCharacters.First(c => c.ID == ID);
	    }

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
        //AllDatas we can refresh all
	    public virtual bool RefreshCharacter(DBCharacter character, out CharacterErrors result)
	    {
		    result = CharacterErrors.ErrorInCharacterInfo;

		    if (!LoginInfo.RefreshFromEntity(character))
		    {
			    result = CharacterErrors.RequestedCharacterIDNotMatching;
			    return false;
		    }

		    if (!AreaInfo.RefreshFromEntity(character))
		    {
			    result = CharacterErrors.ErrorInArena;
			    return false;
		    }

		    if (!Style.RefreshFromEntity(character))
		    {
			    result = CharacterErrors.ErrorInAppearance;
			    return false;
		    }

		    if (Info.RefreshFromEntity(character))
		    {
			    result = CharacterErrors.ErrorInCharacterInfo;
			    return false;
		    }

		    result = CharacterErrors.LoadOK;
		    return true;
	    }

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

            if (!Style.RefreshFromEntity(this))
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
            if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal()
        {
        }
    }
}