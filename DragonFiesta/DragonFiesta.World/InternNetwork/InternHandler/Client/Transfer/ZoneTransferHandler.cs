using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.World.Game.Character;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Transfer
{
    public static class ZoneTransferHandler
    {
        [InternMessageHandler(typeof(CharacterChangeMap))]
        public static void HandleCharacterChangeMap(CharacterChangeMap MapChangeMessage, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(MapChangeMessage.CharacterId, out WorldCharacter pChar))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed CharacterChangeMap Request CharacterId {0}", MapChangeMessage.CharacterId);
                return;
            }

            if (pChar.IsConnected)
            {
                pChar.Session.GameStates.IsReady = false;
                pChar.Session.GameStates.IsTransferring = true;

                if(!pChar.ChangeMap(MapChangeMessage.MapId,
                    MapChangeMessage.InstanceId,
                    MapChangeMessage.Position.X,
                    MapChangeMessage.Position.Y))
                {
                    pChar.Session.Dispose();
                    return;
                }
            }
        }
    }
}