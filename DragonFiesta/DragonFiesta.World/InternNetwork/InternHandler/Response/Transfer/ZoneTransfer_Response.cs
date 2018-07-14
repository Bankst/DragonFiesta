using DragonFiesta.Messages.Zone.Transfer;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Utils.Logging;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;

namespace DragonFiesta.World.InternNetwork.InternHandler.Response.Transfer
{
    public class ZoneTransfer_Response
    {
        public static void HandleZoneTransfer_Response(IMessage msg)
        {
            if(msg is ZoneTransferMessage_Response Response)
            {
                if(!WorldCharacterManager.Instance.GetCharacterByCharacterID(Response.Request.CharacterId,out WorldCharacter Character))
                {
                    GameLog.Write(GameLogLevel.Warning, "Failed Add ZoneTransferResponse Character {0} Not found!", Response.Request.CharacterId);
                    return;
                }

                if(!Character.IsConnected)
                {
                    GameLog.Write(GameLogLevel.Warning, "Failed to ZoneTransferResponse Character {0} is not Connected", Character.Info.CharacterID);
                    return;
                }

                switch(Response.AddResult)
                {
             
                    case ZoneTransferResult.CharacterError
                        when(Character.IsConnected && Character.Session.IsCharacterLoggetIn()):
                        ZoneChat.CharacterNote(Character, $"CharacterError on Target Zone");
                        break;
                    case ZoneTransferResult.MapDataError
                        when(Character.IsConnected && Character.Session.IsCharacterLoggetIn()):
                        ZoneChat.CharacterNote(Character, $"MapDataError on Target Zone");
                        break;
                    case ZoneTransferResult.TransferDataError 
                    when ( Character.IsConnected  && Character.Session.IsCharacterLoggetIn()):
                        ZoneChat.CharacterNote(Character, $"transferDataError on Target Zone");
                        break;
                    case ZoneTransferResult.Success when (Character.IsConnected):
                        Character.ZoneTransferCallback?.Invoke();
                        Character.ZoneTransferCallback = null;
                        break;
                    case ZoneTransferResult.CharacterError when (Character.IsConnected):
                        _SH04Helpers.SendCharacterError(Character.Session, CharacterErrors.ErrorInCharacterInfo);
                        break;
                    case ZoneTransferResult.MapDataError when(Character.IsConnected):
                        _SH04Helpers.SendCharacterError(Character.Session, CharacterErrors.ErrorInArena);
                        break;
                    case ZoneTransferResult.TransferDataError when (Character.IsConnected):
                        _SH04Helpers.SendCharacterError(Character.Session, CharacterErrors.ErrorInMover);
                        break;
                    default:
                        GameLog.Write(GameLogLevel.Warning, "Unknown Zonetransfer Error");
                        break;
               
                }
            }
        }
    }
}