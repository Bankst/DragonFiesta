#region

using System;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Characters;

#endregion

namespace DragonFiesta.Game.Characters.Data
{
    public class CharacterInfo : IDisposable
    {
        public int CharacterID { get; set; }

        public ClassId Class { get; set; }

        public virtual string Name { get; set; }

        public ulong ExpForNextLevel { get; set; }

        public ulong EXP { get; set; }

        public byte Level { get; set; }

        public bool IsMale { get; set; }

        public byte Slot { get; set; }

        public virtual ulong Money { get; set; }

        public const ulong MaxMoney = 99999999999;

        public ushort FriendPoints { get; set; }

        public CharacterInfo()
        {

        }
        ~CharacterInfo()
        {
            Dispose();
        }

	    public virtual bool RefreshFromEntity(CharacterBase character) // TODO: Rewrite to pull from entity, not CharacterBase
	    {
		    try
		    {
			    CharacterID = character.ID;
			    Name = character.Name;
			    Slot = character.Slot;
			    Class = character.Class;
			    Level = character.Level;
			    Money = character.Money;
			    IsMale = character.IsMale;
			    FriendPoints = character.FriendPoints;

			    ExpForNextLevel = CharacterDataProviderBase.GetEXPForNextLevel(Level);

			    return true;
		    }
		    catch (Exception ex)
		    {
			    GameLog.Write(ex, $"Failed Load CharacterInfos ID : {CharacterID}");
			    return false;
			}
	    }

        public virtual bool RefreshFromSQL(SQLResult pRes, int i)
        {
            try
            {
                CharacterID = pRes.Read<int>(i, "ID");
                Name = pRes.Read<string>(i, "Name");
                Slot = pRes.Read<byte>(i, "Slot");
                Class = (ClassId)pRes.Read<byte>(i, "Class");
                Level = pRes.Read<byte>(i, "Level");
                Money = pRes.Read<ulong>(i, "Money");
                IsMale = pRes.Read<bool>(i, "IsMale");

                FriendPoints = pRes.Read<ushort>(i, "FriendPoints");

                ExpForNextLevel = CharacterDataProviderBase.GetEXPForNextLevel(Level);

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, $"Failed Load CharacterInfos ID : {CharacterID}");
                return false;
            }
        }


        public virtual void Dispose()
        {
            Name = null;
        }
    }
}