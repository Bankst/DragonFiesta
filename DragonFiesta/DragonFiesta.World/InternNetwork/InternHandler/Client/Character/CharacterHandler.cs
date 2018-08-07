using DragonFiesta.Messages.World.Character;
using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.Utils.Logging;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Character;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Character
{
    public static class CharacterHandler
    {
        [InternMessageHandler(typeof(SetFriendPoints))]
        public static void HandleUseFrendPoints(SetFriendPoints Message, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(Message.CharacterId, out WorldCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Character {0} for UseFriendPoints Request ", Message.CharacterId);
                return;
            }

            Character.Info.FriendPoints = Message.FriendPoint;

            Message.FriendPoint = Character.Info.FriendPoints;


            pSession.SendMessage(Message, false);
        }
        [InternMessageHandler(typeof(CharacterClassChanged))]
        public static void HandleCharacterCharacterClassChanged(CharacterClassChanged Message, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(Message.CharacterId, out WorldCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to get Character [0} for ClassChangeRequest ", Message.CharacterId);
                return;
            }

            WorldCharacterManager.Instance.CharacterChangeClass(Character, Message.NewClass);
        }

        [InternMessageHandler(typeof(CharacterPosition))]
        public static void HandleCharacterCharacterPosition(CharacterPosition PosMessage, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(PosMessage.CharacterId, out WorldCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Character for Position Request");
                return;
            }

            if (!Character.IsConnected)
            {
                GameLog.Write(GameLogLevel.Warning, "Character {0} is not online for Position request", PosMessage.CharacterId);
                return;
            }


            CharacterMethods.SendCharacterPositionRequest(Character.Map.Zone, Character.Info.CharacterID, (msg) =>
              {
                  if (msg is CharacterPosition ePosition)//response from zone
                  {
                   
                      msg.Id = PosMessage.Id;
                      pSession.SendMessage(msg);
                  }
              });
        }
            
       
        [InternMessageHandler(typeof(CharacterLevelChanged))]
        public static void HandleCharacterCharacterLevelChanged(CharacterLevelChanged LevelMessage, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(LevelMessage.CharacterId, out WorldCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Character {0} for LevelChanged ", LevelMessage.CharacterId);
                return;
            }


            WorldCharacterManager.Instance.CharacterLevelChanged(Character, LevelMessage.NewLevel);
        }

        [InternMessageHandler(typeof(CharacterLoggedOut))]
        public static void HandleCharacterCharacterLoggedOut(CharacterLoggedOut LoggedInMessage, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(LoggedInMessage.CharacterId, out WorldCharacter Character, true))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get CharacterLogged in {0}", LoggedInMessage.CharacterId);
                return;
            }

            Character.LoginInfo.IsOnline = false;

            WorldCharacterManager.Instance.LogCharacterOut(Character);
        }

        [InternMessageHandler(typeof(SetCharacterMoney))]
        public static void HandleSetCharacterMoney(SetCharacterMoney MoneyMessage, InternZoneSession pSession)
        {
            if (WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(MoneyMessage.CharacterId, out WorldCharacter Character))
            {
                Character.Info.Money = MoneyMessage.NewMoney;
            }
        }
    }
}
