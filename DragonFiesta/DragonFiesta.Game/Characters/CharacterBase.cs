#region

using System;
using System.Linq;
using System.Threading;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.Logging;

#endregion

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterBase : IDisposable
    {
        public bool IsDisposed => (_isDisposedInt > 0);
	    private int _isDisposedInt;

		public DBCharacter DBCharacter { get; private set; }

	    private DBCharacter LazyLoadMe()
	    {
		    using (var worldEntity = EDM.GetWorldEntity())
		    {
			    return worldEntity.DBCharacters.First(c => c.ID == ID);
			}
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

		#region DBCharacter-Base

	    public int ID => DBCharacter.ID;
	    public int AccountID => DBCharacter.AccountID;
	    public string Name
	    {
		    get => DBCharacter.Name;
		    set => DBCharacter.Name = value;
	    }
	    public byte Slot => DBCharacter.Slot;
		public short Map
	    {
		    get => DBCharacter.Map;
		    set => DBCharacter.Map = value;
	    }
	    public int PositionX => DBCharacter.PositionX;
	    public int PositionY => DBCharacter.PositionY;
	    public byte Rotation => DBCharacter.Rotation;
	    public ClassId Class
	    {
		    get => (ClassId)DBCharacter.Class;
		    set => DBCharacter.Level = (byte)value;
		}
	    public byte Level
	    {
		    get => DBCharacter.Level;
		    set => DBCharacter.Level = value;
	    }
	    public int HP => DBCharacter.HP;
	    public int SP => DBCharacter.SP;
	    public int LP => DBCharacter.LP;
	    public ushort HPStones
	    {
		    get => (ushort) DBCharacter.HPStones;
		    set => DBCharacter.HPStones = (short) value;
	    }
		public ushort SPStones
		{
			get => (ushort)DBCharacter.SPStones;
			set => DBCharacter.SPStones = (short)value;
		}
		public ulong EXP
		{
			get => (ulong) DBCharacter.EXP;
			set => DBCharacter.EXP = (long) value;
		}
	    public ulong Money
	    {
		    get => (ulong) DBCharacter.Money;
		    set => DBCharacter.Money = (long) value;
	    }
		public uint Fame
		{
		    get => (uint) DBCharacter.Fame;
			set => DBCharacter.Fame = (int) value;
		}
	    public uint KillPoints
	    {
		    get => (uint) DBCharacter.KillPoints;
		    set => DBCharacter.KillPoints = (int) value;
	    }
	    public bool IsMale => DBCharacter.IsMale;
		public byte Hair
	    {
		    get => DBCharacter.Hair;
		    set => DBCharacter.Hair = value;
	    }
	    public byte HairColor
	    {
		    get => DBCharacter.HairColor;
		    set => DBCharacter.HairColor = value;
	    }
	    public byte Face
	    {
		    get => DBCharacter.Face;
		    set => DBCharacter.Face = value;
	    }
	    public byte FreeStat_Points => DBCharacter.FreeStat_Points;
	    public byte FreeStat_Str => DBCharacter.FreeStat_Str;
	    public byte FreeStat_End => DBCharacter.FreeStat_End;
	    public byte FreeStat_Dex => DBCharacter.FreeStat_Dex;
	    public byte FreeStat_Int => DBCharacter.FreeStat_Int;
	    public byte FreeStat_Spr => DBCharacter.FreeStat_Spr;
	    public byte SkillPoints => DBCharacter.SkillPoints;
	    public DateTime LastLogin => DBCharacter.LastLogin;
	    public bool IsOnline => DBCharacter.IsOnline;
	    public bool IsFirstLogin => DBCharacter.IsFirstLogin;
	    public ushort FriendPoints
	    {
		    get => (ushort) DBCharacter.FriendPoints;
		    set => DBCharacter.FriendPoints = value;
	    }

	    #endregion
		public virtual bool LoadBaseWithID(int id, out CharacterErrors result)
	    {
		    result = CharacterErrors.ErrorInCharacterInfo;
		    try
		    {
			    using (var worldEntity = EDM.GetWorldEntity())
			    {
				    DBCharacter = worldEntity.DBCharacters.First(ch => ch.ID == id);
			    }

			    result = CharacterErrors.LoadOK;
			    return true;
		    }
		    catch (Exception ex)
		    {
				GameLog.Write(GameLogLevel.Exception, $"EntityLoad for character ID: {id} failed: \n{ex.Message} \n{ex.StackTrace}");
				return false;
		    }
	    }

		//Style by Errors Code from Client...
		//Only Load one when Added to CharacterManager
		public virtual bool FinalizeLoad(out CharacterErrors result)
        {
            result = CharacterErrors.LoadOK;
            return true;
        }

        //AllDatas we can refresh all
	    public virtual bool RefreshCharacter(out CharacterErrors result)
	    {
		    var character = this;
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

		    if (!Info.RefreshFromEntity(character))
		    {
			    result = CharacterErrors.ErrorInCharacterInfo;
			    return false;
		    }

		    result = CharacterErrors.LoadOK;
		    return true;
	    }

        public virtual bool RefreshCharacter(SQLResult pRes, int i, out CharacterErrors result)
        {
            result = CharacterErrors.ErrorInCharacterInfo;

            if (!LoginInfo.RefreshFromSQL(pRes, i))
            {
                result = CharacterErrors.RequestedCharacterIDNotMatching;
                return false;
            }

            if (!AreaInfo.RefreshFromSQL(pRes, i))
            {
                result = CharacterErrors.ErrorInArena;
                return false;
            }

            if (!Style.RefreshFromEntity(this))
            {
                result = CharacterErrors.ErrorInAppearance;
                return false;
            }

            if (!Info.RefreshFromSQL(pRes, i))
            {
                result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }

            result = CharacterErrors.LoadOK;
            return true;
        }

        public virtual bool GiveEXP(uint amount, ushort mobID = 0xFFFF)
        {
            if (Info.Level >= GameConfiguration.Instance.LimitSetting.LevelLimit)
                return false;
            Info.EXP += amount;
			
            while (Info.EXP > CharacterDataProviderBase.GetEXPForNextLevel(Info.Level))
            {
                LevelUp(mobID);
            }
            return true;
        }
        public virtual bool LevelUp(ushort mobID = 0xFFFF, bool finalizeLevelUp = true)
        {
            if (Info.Level >= GameConfiguration.Instance.LimitSetting.LevelLimit) return false;

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