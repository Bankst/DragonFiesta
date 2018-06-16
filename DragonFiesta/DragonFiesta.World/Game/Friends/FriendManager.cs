using DragonFiesta.Game.Characters.Event;
using DragonFiesta.Utils.Config;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Network.FiestaHandler.Server;
using System;

namespace DragonFiesta.World.Game.Friends
{
    [GameServerModule(ServerType.World, GameInitalStage.Friend)]
    public class FriendManager
    {

        private static object ThreadLocker;

        [InitializerMethod]
        public static bool InitialFrendManager()
        {

            if (GameConfiguration.Instance.FriendSettings.MaxFriend > 50 ||
                GameConfiguration.Instance.FriendSettings.MaxSearch > 50)//50 ist client limit...
            {
                throw new StartupException("Invalid ConfigParameter  for  FriendSection");
            }

            ThreadLocker = new object();

            WorldCharacterManager.Instance.OnCharacterLogin += Instance_OnCharacterLogin;
            WorldCharacterManager.Instance.OnCharacterLogout += Instance_OnCharacterLogout;
            WorldCharacterManager.Instance.OnCharacterMapChanged += Instance_OnCharacterMapChanged;
            WorldCharacterManager.Instance.OnCharacterLevelChanged += Instance_OnCharacterLevelChanged;
            WorldCharacterManager.Instance.OnCharacterMapRefresh += Instance_OnCharacterMapRefresh;
            WorldCharacterManager.Instance.OnCharacterClassChange += Instance_OnCharacterClassChange;
            WorldCharacterManager.Instance.OnCharacterDelete += Instance_OnCharacterDelete;
            return true;
        }

        private static void Instance_OnCharacterDelete(object sender, CharacterDeleteEventArgs<WorldCharacter> args)
        {
            if (args.Character.Friends.Count > 0)
            {
                args.Character.Friends.FriendAction((frend) =>
                {
                    if(!RemoveFriendFromDatabase(args.Character,frend.MyFriend))
                    {
                        GameLog.Write(GameLogLevel.Warning, "Character Friend Deletion fail...");
                        return;
                    }

                    //check if friend is online, if so send 'deleted you' packet
                    if (frend.MyFriend.LoginInfo.IsOnline
                        && frend.MyFriend.Session.Ingame)
                    {
                        SH21Handler.SendFriendDeletedYou(frend.MyFriend.Session, args.Character.Info.Name);
                    }

                }, false);
            }
        }

        private static void Instance_OnCharacterClassChange(object sender, CharacterClassChangeEventArgs<WorldCharacter> args)
        {
            if (args.Character.Friends.Count > 0)
            {
                SH21Handler.SendFrendChangeClass(args.Character, args.NewClass);
            }
        }

        private static void Instance_OnCharacterMapRefresh(object sender, CharacterMapRefreshEventArgs<WorldCharacter> args)
        {
            if (args.Character.Friends.Count > 0)
            {
                //Send friend list to client
                SH21Handler.SendFriendList(args.Character);

            }
        }

        private static void Instance_OnCharacterLevelChanged(object sender, CharacterLevelChangedEventArgs<WorldCharacter> e)
        {

            if (e.Character.Friends.Count > 0)
            {
                SH21Handler.SendFriendUpdateLevel(e.Character, e.NewLevel);
            }
        }

        private static void Instance_OnCharacterMapChanged(object sender, CharacterMapEventArgs<WorldCharacter, IMap> e)
        {

            if (e.Character.Friends.Count > 0)
            {
                SH21Handler.SendFriendUpdateMap(e.Character, e.Map.MapInfo);
            }
        }

        private static void Instance_OnCharacterLogout(object sender, DragonFiesta.Game.Characters.Event.CharacterEventArgs<WorldCharacter> e)
        {
            if (e.Character.Friends.Count > 0)
            {
                //send loggedout...
                SH21Handler.BroadcastFriendLoggedOut(e.Character);
            }
        }

        private static void Instance_OnCharacterLogin(object sender, CharacterEventArgs<WorldCharacter> arg)
        {

            if (arg.Character.Friends.Count > 0)
            {

                //Tell another character online..
                SH21Handler.BroadcastFriendLoggedIn(arg.Character);
            }
        }

        public static void AddFriend(WorldCharacter Sender, WorldCharacter Receiver, DateTime RegisterDate)
        {
            try
            {
                //try to add to database
                if (!AddFriendToDatabase(Sender, Receiver, RegisterDate))
                {
                    //send database error to both clients
                    SH21Handler.SendFriendInviteResponse(Sender.Session, Receiver.Info.Name, FriendInviteResponse.DatabaseError);
                    SH21Handler.SendFriendInviteResponse(Receiver.Session, Sender.Info.Name, FriendInviteResponse.DatabaseError);
                    return;
                }


                //add friend to sender
                var senderFriend = new Friend(Sender, Receiver, RegisterDate);
                Sender.Friends.Add(senderFriend);

                //Hmm need it so?
                SH21Handler.SendFriendInviteResponse(Sender.Session, Receiver.Info.Name, FriendInviteResponse.Success);
                SH21Handler.SendFriendExtraInfo(Sender.Session, senderFriend.MyFriend);
                SH21Handler.SendFriendLoggedIn(Sender.Session, Receiver);


                //add friend to receiver
                var receiverFriend = new Friend(Receiver, Sender, RegisterDate);
                Receiver.Friends.Add(receiverFriend);

                SH21Handler.SendFriendInviteResponse(Receiver.Session, Sender.Info.Name, FriendInviteResponse.Success);
                SH21Handler.SendFriendExtraInfo(Receiver.Session, receiverFriend.MyFriend);
                SH21Handler.SendFriendLoggedIn(Receiver.Session, Sender);

            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error adding friend:");
            }
        }

  
        public static void RemoveFriend(WorldCharacter Sender, WorldCharacter Reciever)
        {

            //remove from db and friend list
            if (!RemoveFriendFromDatabase(Sender, Reciever))
            {
                SH21Handler.SendFriendDeleteResponse(Sender.Session, Reciever.Info.Name, FriendDeleteResponse.DatabaseError);
                return;
            }

            //remove from friend lists
            Sender.Friends.RemoveByCharacterID(Reciever.Info.CharacterID);
            Reciever.Friends.RemoveByCharacterID(Sender.Info.CharacterID);



            //send response to client
            SH21Handler.SendFriendDeleteResponse(Sender.Session, Reciever.Info.Name, FriendDeleteResponse.Success);


            //check if friend is online, if so send 'deleted you' packet
            if (Reciever.LoginInfo.IsOnline
                && Reciever.Session.Ingame)
            {
                SH21Handler.SendFriendDeletedYou(Reciever.Session, Sender.Info.Name);
            }
        }

        private static bool AddFriendToDatabase(WorldCharacter Sender, WorldCharacter Receiver, DateTime RegisterDate)
        {
            try
            {
                lock (ThreadLocker)
                {

                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {

                        cmd.CreateStoredProcedure("dbo.Friend_Insert");

                        cmd.SetParameter("@pOwnerID", Sender.Info.CharacterID);
                        cmd.SetParameter("@pFriendID", Receiver.Info.CharacterID);
                        cmd.SetParameter("@pRegisterDate", RegisterDate);



                        switch ((int)cmd.ExecuteScalar())
                        {
                            case 0:
                                return true;

                            case -1: // friend exists in db (from sender)
                            case -2: // friend exists in db (from receiver)
                            case -3: // database insert error 1
                            case -4: // database insert error 2
                            default:
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error adding friend to database:");

                return false;
            }
        }
        private static bool RemoveFriendFromDatabase(WorldCharacter Sender, WorldCharacter Receiver)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                    {

                        cmd.CreateStoredProcedure("dbo.Friend_Remove");

                        cmd.SetParameter("@pOwnerID", Sender.Info.CharacterID);
                        cmd.SetParameter("@pFriendID", Receiver.Info.CharacterID);



                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error removing friend from database:");

                return false;
            }
        }
    }
}
