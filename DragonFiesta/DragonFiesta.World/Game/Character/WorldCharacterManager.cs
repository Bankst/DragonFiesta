using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Data.Characters;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.World.Network;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Utils.Logging;
using Account = DragonFiesta.Game.Accounts.Account;

namespace DragonFiesta.World.Game.Character
{
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    public class WorldCharacterManager : CharacterManagerBase<WorldCharacter>
    {
        public static WorldCharacterManager Instance { get; set; }
		
		public List<WorldCharacter> OnlineCharacterList => LoggedInCharactersByCharacterID.Values.ToList();
        [InitializerMethod]
        public static bool Initial()
        {
            Instance = new WorldCharacterManager();
            SetAllCharacterOfflineEntity();
            return true;
        }

	    public static void SetAllCharacterOfflineEntity()
	    {
		    using (var worldEntity = EDM.GetWorldEntity())
		    {
			    foreach (var character in worldEntity.DBCharacters)
			    {
				    character.IsOnline = false;
			    }
			    worldEntity.SaveChanges();
		    }
	    }

        public static void SetAllCharacterOffline()
        {
            //Clean up when world crashed then all character offline
			// TODO: Is doing this thru SQL faster than thru Entity? Will it break entity if done thru sql?
            DB.RunSQL(DatabaseType.World, "UPDATE Characters SET IsOnline='0'");
        }
        public WorldCharacterManager() : base()
        {

        }

        #region Event




        #endregion
		
        public void UpdateCharacterState(int ID, bool IsOnline, DateTime LastLogin)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {
                        cmd.CreateStoredProcedure("dbo.Character_UpdateState");
                        cmd.SetParameter("@pID", ID);
                        cmd.SetParameter("@pIsOnline", IsOnline);
                        cmd.SetParameter("@pLastLogin", LastLogin);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error updating character state:");
            }
        }

        public override void CharacterChangeClass(WorldCharacter Character, ClassId NewClass)
        {
            //tels zone..
            CharacterMethods.SendCharacterClassChanged(Character, NewClass);
            base.CharacterChangeClass(Character, NewClass);
        }

        protected override bool FinalizeCharacterDeleted(WorldCharacter Character)
        {

            if (!DeleteCharacterFromDatabase(Character))
                return false;

            if (!base.FinalizeCharacterDeleted(Character))
                return false;

            //tells zone ..
            CharacterMethods.SendCharacterDeleted(Character);
            return true;
        }

        public bool DeleteCharacterFromDatabase(WorldCharacter Character)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {
                        cmd.CreateStoredProcedure("Character_Remove");
                        cmd.SetParameter("@pID", Character.Info.CharacterID);
                        int Res = (int)cmd.ExecuteScalar();

                        if (Res != -1)//Error by Prozedure..
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error deleting character:");
                return false;
            }
        }

        public bool CreateCharacter(Account Account,
            string Name,
            byte Slot,
            byte Class,
            bool IsMale,
            HairInfo Hair,
            HairColorInfo HairColor,
            FaceInfo Face,
            out WorldCharacter Character,
            out CharacterCreationError Error)
        {
            Character = null;
            Error = CharacterCreationError.FailedToCreate;

            try

            {
                if (!CharacterClass.IsValidClass(Class))
                {
                    Error = CharacterCreationError.WrongClass;
                    GameLog.Write(GameLogLevel.Warning, "Failed to create character: Can't find class (Class ID: {0})", Class);
                    return false;
                }

                if (!DefaultCharacterDataProvider.GetDefaultCharacterByID(Class, out DefaultCharacterInfo pDefaulInfo))
                {
                    Error = CharacterCreationError.WrongClass;
                    GameLog.Write(GameLogLevel.Warning, "Failed to create character: Can't find DefaultCharacterInfo (Class ID: {0})", Class);
                    return false;
                }

                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {
                        cmd.CreateStoredProcedure("Character_Insert");

                        cmd.SetParameter("@pAccountID", Account.ID);
                        cmd.SetParameter("@pName", Name);
                        cmd.SetParameter("@pSlot", Slot);
                        cmd.SetParameter("@pMap", (short)pDefaulInfo.StartMap.ID);
                        cmd.SetParameter("@pPositionX", (int)pDefaulInfo.StartPosition.X);
                        cmd.SetParameter("@pPositionY", (int)pDefaulInfo.StartPosition.Y);
                        cmd.SetParameter("@pRotation", (byte)pDefaulInfo.StartPosition.Rotation);
                        cmd.SetParameter("@pClass", (byte)Class);
                        cmd.SetParameter("@pHP", (int)pDefaulInfo.HP);
                        cmd.SetParameter("@pSP", (int)pDefaulInfo.SP);
                        cmd.SetParameter("@pLP", (int)pDefaulInfo.LP);
                        cmd.SetParameter("@pHPStones", (short)pDefaulInfo.HPStones);
                        cmd.SetParameter("@pSPStones", (short)pDefaulInfo.HPStones);
                        cmd.SetParameter("@pMoney", (long)pDefaulInfo.Money);
                        cmd.SetParameter("@pIsMale", IsMale);
                        cmd.SetParameter("@pHair", Hair.ID);
                        cmd.SetParameter("@pHairColor", HairColor.ID);
                        cmd.SetParameter("@pFace", Face.ID);


                        var idParam = cmd.SetParameter("@pID", SqlDbType.Int, ParameterDirection.Output);

                        switch ((int)cmd.ExecuteScalar())
                        {
                            //success
                            case 0:

                                var characterID = (int)idParam.Value;
                                if (!CreateCharacterObject(characterID,
                                                           Account,
                                                           Name,
                                                           Slot,
                                                           (ClassId)Class,
                                                           pDefaulInfo.StartLevel,
                                                           pDefaulInfo.Money,
                                                           pDefaulInfo.StartMap,
                                                           pDefaulInfo.StartPosition,
                                                           IsMale,
                                                           Hair,
                                                           HairColor,
                                                           Face,
                                                           out Character))
                                {
                                    return false;
                                }

                                //TODO
                                /*
                                pDefaulInfo.AddDefaultItemsToCharacter(Character);
                                pDefaulInfo.AddDefaultPassoveSkillsToCharacter(Character);
                                pDefaulInfo.AddDefaultSkillsToCharacter(Character);*()
                                Character.IsFirstLogin = true;*/
                                if (!AddCharacter(Character, out CharacterErrors Errors))
                                {
                                    Error = CharacterCreationError.FailedToCreate;
                                }
                                return true;
                            //name in use
                            case -1:
                                Error = CharacterCreationError.NameInUse;
                                return false;
                            //slot already used
                            case -2:
                                Error = CharacterCreationError.ErrorInMaxSlot;
                                return false;
                            //ID overflow
                            case -3:
                            //SQL error
                            case -4:
                            default:
                                Error = CharacterCreationError.FailedToCreate;
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error creating character:");
                return false;
            }
        }

        public bool CreateCharacterObject(int ID,
            Account Account,
            string Name,
            byte Slot, ClassId Class,
            byte level,
            ulong Money,
            MapInfo pMapInfo,
            Position Position,
            bool IsMale,
            HairInfo Hair,
            HairColorInfo HairColor,
            FaceInfo Face,
            out WorldCharacter Character)
        {
            try
            {
                Character = new WorldCharacter
                {
                    AreaInfo = new CharacterAreaInfo(pMapInfo)
                    {
                        Position = Position,
                    },
                    Style = new CharacterStyle
                    {
                        Hair = Hair,
                        HairColor = HairColor,
                        Face = Face,
                    },
                };

                Character.Info.CharacterID = ID;
                Character.Info.Name = Name;
                Character.Info.Slot = Slot;
                Character.Info.Class = Class;
                Character.Info.Level = level;
                Character.Info.Money = Money;
                Character.Info.IsMale = IsMale;
                Character.LoginInfo.AccountID = Account.ID;

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed to Create WorldCharacerObject");
                Character = null;
                return false;
            }
        }

        public bool ChangeCharacterNameById(WorldCharacter pCharacter, string NewName)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {
                        cmd.CreateStoredProcedure("Character_ChangeName");
                        cmd.SetParameter("@pCharID", pCharacter.Info.CharacterID);
                        cmd.SetParameter("@pName", NewName);
                        int Res = (int)cmd.ExecuteScalar();

                        if (Res == -1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error ChangeName character OldName {0} : NewName {1}:", pCharacter.Info.Name, NewName);
                return false;
            }
        }


        public override void CharacterMapRefreshed(WorldCharacter Character)
        {
            //TODO :)
            if (!Character.Map.AddCharacter(Character))
            {
                Character.Session.Dispose();
                return;
            }

            base.CharacterMapRefreshed(Character);
        }

        protected override void FinalizeLogCharacterIn(WorldCharacter Character)
        {
            if (Character.IsConnected)
            {
                Character.Session.OnDispose += (sender, args) => LogCharacterOut(Character, true);
                Character.LoginInfo.IsOnline = true;

                if (Character.LoginInfo.IsFirstLogin)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {
                        cmd.CreateStoredProcedure("dbo.Character_Update_FirstLogin");
                        cmd.SetParameter("@pID", Character.Info.CharacterID);
                        cmd.SetParameter("@pIsFirstLogin", false);
                        cmd.ExecuteNonQuery();
                    }
                    Character.LoginInfo.IsFirstLogin = false;
                }

                Character.LoginInfo.IsOnline = true;
                Character.LoginInfo.LastLogin = DateTime.Now;
                UpdateCharacterState(Character.Info.CharacterID, true, Character.LoginInfo.LastLogin);

                //tells all zone...
                CharacterMethods.BroadcastOnlineCharacter(Character);
            }
            base.FinalizeLogCharacterIn(Character);
        }

        protected override void FinalizeCharacterLogOut(WorldCharacter Character)
        {
            if (Character.IsConnected)
            {
                if (Character.Session.Ingame && !Character.Map.RemoveCharacter(Character))
                {
                    Character.Session.Dispose();
                    return;
                }

                if (WorldPingManager.Instance.RevokeClient(Character.Session))
                {
                    Character.LoginInfo.IsOnline = false;
                    UpdateCharacterState(Character.Info.CharacterID, Character.LoginInfo.IsOnline, Character.LoginInfo.LastLogin);

                    //tells all zone...
                    CharacterMethods.BroadCastOfflineCharacter(Character);
                }
            }
            base.FinalizeCharacterLogOut(Character);
        }

        protected override void FinalizeCharacterLevelChanged(WorldCharacter Character, byte NewLevel, ushort MobId = ushort.MaxValue)
        {
            //Tells zone level changed...
            CharacterMethods.BroadcastLevelChanged(Character.Info.CharacterID, NewLevel, MobId);
            base.FinalizeCharacterLevelChanged(Character, NewLevel, MobId);
        }

        public override void CharacterChangeMap(WorldCharacter Character, IMap NewMap)
        {
            if (!Character.Map.RemoveCharacter(Character))//Remove from old map..
            {
                Character.Session.Dispose();
                return;
            }

            base.CharacterChangeMap(Character, NewMap);
            Character.AreaInfo.Map = NewMap;
        }
    }
}