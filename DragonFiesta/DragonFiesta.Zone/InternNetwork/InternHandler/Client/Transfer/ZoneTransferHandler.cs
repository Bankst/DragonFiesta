using DragonFiesta.Messages.Zone.Transfer;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Transfer;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Game.Maps.Object;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Client.Transfer
{
    public class ZoneTransferHandler
    {


        [InternMessageHandler(typeof(ZoneTransferMessage))]
        public static void HandleZoneTransferMessage(ZoneTransferMessage TransferMessage, InternWorldConnector pSession)
        {
            ZoneTransferMessage_Response Response = new ZoneTransferMessage_Response
            {
                Id = TransferMessage.Id,
                Request = TransferMessage,
            };

            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(TransferMessage.CharacterId, out ZoneCharacter Character, true))
            {
                Response.AddResult = ZoneTransferResult.CharacterError;
                pSession.SendMessage(Response);
                return;
            }

            if (!MapManager.GetMap(TransferMessage.MapId, TransferMessage.InstanceId, out ZoneServerMap Map)
                && Map is RemoteMap)
            {
                Response.AddResult = ZoneTransferResult.MapDataError;
                pSession.SendMessage(Response);
                return;
            }

            ZoneTransfer Transfer = new ZoneTransfer
            {
                Character = Character,
                Map = Map,
                SpawnPosition = TransferMessage.SpawnPosition,
                WorldSessionId = TransferMessage.WorldSessionId
            };

            if (!ZoneServerTransferManager.AddTransfer(Transfer))
            {
                Response.AddResult = ZoneTransferResult.TransferDataError;
                pSession.SendMessage(Response);
                return;
            }

            Character.Position = TransferMessage.SpawnPosition;
            Character.LoginInfo.RoleId = TransferMessage.RoleId;
            Character.Map = Map;

            pSession.SendMessage(Response);
        }
    }
}
