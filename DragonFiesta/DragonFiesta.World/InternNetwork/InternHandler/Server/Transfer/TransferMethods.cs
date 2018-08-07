using DragonFiesta.Messages.World.Transfer;
using DragonFiesta.Messages.Zone.Transfer;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.InternNetwork.InternHandler.Response.Transfer;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Transfer
{
    public static class TransferMethods
    {
        public static void SendLoginTransfer(
            int AccountId,
            string IP,
            Action<IMessage> Callback,
            Action<IMessage> TimeOut)
        {
            AddLoginServerTransfer Transfer = new AddLoginServerTransfer
            {
                Id = Guid.NewGuid(),
                AccountId = AccountId,
                IP = IP,
                Callback = Callback,
            };
            InternLoginConnector.Instance.SendMessage(Transfer);
        }
        public static void SendZoneTransfer(WorldCharacter pCharacter) =>
            SendZoneTransfer(pCharacter.Map,
                (pCharacter.AreaInfo.IsInInstance) ? (pCharacter.Map as IInstanceMap).InstanceId : (ushort)0,
                pCharacter.AreaInfo.Position,
                pCharacter.Session.BaseStateInfo.SessionId,
                 pCharacter.Info.CharacterID,
                 pCharacter.Session.UserAccount.RoleID);


        public static void SendZoneTransfer(
            WorldServerMap Map,
            ushort InstanceId,
            Position SpawnPosition,
            ushort WorldSessionId,
            int CharacterId,
            byte RoleId)
        {
            var Transfer = new ZoneTransferMessage
            {
                MapId = Map.MapId,
                InstanceId = InstanceId,
                SpawnPosition = SpawnPosition,
                WorldSessionId = WorldSessionId,
                CharacterId = CharacterId,
                 RoleId = RoleId,
                Callback = ZoneTransfer_Response.HandleZoneTransfer_Response
            };
            Map.Zone.Send(Transfer);
        }
    }
}
