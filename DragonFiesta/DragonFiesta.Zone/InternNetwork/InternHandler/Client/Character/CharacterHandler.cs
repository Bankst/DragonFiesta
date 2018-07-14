using DragonFiesta.Messages.World.Character;
using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Client.Character
{
    public class CharacterHandler
    {
        [InternMessageHandler(typeof(SetFriendPoints))]
        public static void HandleSetFriendPoints(SetFriendPoints Message, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(Message.CharacterId, out ZoneCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "failed to Get Character {0} for Set Friend Request", Message.CharacterId);
                return;
            }

            Character.Info.FriendPoints = Message.FriendPoint;

            //Set Message..
            Message.FriendPoint = Character.Info.FriendPoints;

            pSession.SendMessage(Message, false);
        }
        [InternMessageHandler(typeof(CharacterClassChanged))]
        public static void HandleCharacterClassChanged(CharacterClassChanged Message, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(Message.CharacterId, out ZoneCharacter Character, true))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Find Character for CharacterClassChange {0}", Message.CharacterId);
                return;
            }

            ZoneCharacterManager.Instance.CharacterChangeClass(Character, Message.NewClass);
        }


        [InternMessageHandler(typeof(CharacterDeleted))]
        public static void HandleCharacterDelete(CharacterDeleted Message, InternWorldConnector pSession)
        {
         
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(Message.CharacterId, out ZoneCharacter Character, true))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Find Character for CharacterDelete {0}", Message.CharacterId);
                return;
            }

            ZoneCharacterManager.Instance.DeleteCharacter(Character);
        }

        [InternMessageHandler(typeof(CharacterChangeMap))]
        public static void HandleCharacterChangeMap(CharacterChangeMap TransferMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(TransferMessage.CharacterId, out ZoneCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Find Character {0} to Mapchange", TransferMessage.CharacterId);
                return;
            }


            if (!MapManager.GetMap(TransferMessage.MapId, TransferMessage.InstanceId, out ZoneServerMap Map))
            {
                ZoneChat.CharacterNote(Character, $"Failed to Find Target Map");
                return;
            }


            if (Character.IsOnThisZone)
            {
                SH06Handler.SendChangeMap(
                    Character.Session,
                    Map.Info.MapInfo,
                    TransferMessage.Position.X,
                    TransferMessage.Position.Y,
                    (Map is RemoteMap) ? (Map as RemoteMap).Zone : null);


                Character.Session.GameStates.IsReady = false;
                Character.Session.GameStates.IsTransferring = true;
            }

            //Event...
            ZoneCharacterManager.Instance.CharacterChangeMap(Character, Map);

            //Update on zone
            Character.Map = Map;
            Character.Position = TransferMessage.Position;



        }


        [InternMessageHandler(typeof(CharacterPosition))]
        public static void HandleCharacterPosition(CharacterPosition PositionRequest, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(PositionRequest.CharacterId, out ZoneCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Character {0} for PositionsRequest", PositionRequest.CharacterId);
                return;
            }


            PositionRequest.Position = Character.Position;
            PositionRequest.InstanceId = Character.AreaInfo.InstanceId;
            PositionRequest.MapId = Character.AreaInfo.Map.MapId;
            pSession.SendMessage(PositionRequest, false);
        }

        [InternMessageHandler(typeof(CharacterLevelChanged))]
        public static void HandleCharacterLevelChanged(CharacterLevelChanged LevelChangMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(LevelChangMessage.CharacterId, out ZoneCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Character {0} for LevelUP", LevelChangMessage.CharacterId);
                return;
            }

            ZoneCharacterManager.Instance.CharacterLevelChanged(Character, LevelChangMessage.NewLevel);
        }

        [InternMessageHandler(typeof(CharacterLoggedOut))]
        public static void HandleCharacterCharacterLoggedOut(CharacterLoggedOut LoggedInMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(LoggedInMessage.CharacterId, out ZoneCharacter Character))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get CharacterLogged in {0}", LoggedInMessage.CharacterId);
                return;
            }

            ZoneCharacterManager.Instance.LogCharacterOut(Character);
        }

        [InternMessageHandler(typeof(CharacterLoggedIn))]
        public static void HandleCharacterCharacterLoggedIn(CharacterLoggedIn LoggedInMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(LoggedInMessage.CharacterId, out ZoneCharacter Character, true))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get CharacterLoggedout in {0}", LoggedInMessage.CharacterId);
                return;
            }

            if (!MapManager.GetMap(LoggedInMessage.MapId, LoggedInMessage.Instance, out ZoneServerMap Map))
            {
                GameLog.Write(GameLogLevel.Exception, "Invalid MapLogin From Transfer... map {0}", LoggedInMessage.MapId);
                return;
            }

            //Set map on zone..
            Character.Map = Map;

            ZoneCharacterManager.Instance.LogCharacterIn(Character);
        }
    }
}
