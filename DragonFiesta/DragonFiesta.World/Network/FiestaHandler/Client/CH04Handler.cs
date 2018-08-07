using DragonFiesta.Game.CommandAccess;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Utils.Config;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Transfer;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Transfer;
using DragonFiesta.World.Network.FiestaHandler.Server;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler04Type._Header)]
    public class CH04Handler
    {

        [PacketHandler(Handler04Type.CMSG_CHAR_TUTORIAL_POPUP_ACK)]
        public static void TUTORIAL_RESPONSE(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.Read<bool>(out bool UseTutorial))
            {
                sender.Dispose();
                return;
            }

            if (!MapManager.GetNormalMapById(UseTutorial ?
                GameConfiguration.Instance.TutorialMap : sender.Character.AreaInfo.MapInfo.ID,
                out NormalMap SpawnMap)) //Spawn to no tutrial map
            {
                _SH04Helpers.SendCharacterError(sender, ConnectionError.MapUnderMaintace);
                return;
            }

            if (sender.Character.AreaInfo.Map != null && (sender.Character.Map.Zone.IsFull))
            {
                _SH04Helpers.SendCharacterError(sender, ConnectionError.MapUnderMaintace);
                return;
            }


            if (UseTutorial)
            {
                sender.Character.AreaInfo.Position = SpawnMap.Info.RegenPosition;
            }

            sender.GameStates.HasPong = true;

            sender.Character.AreaInfo.Map = SpawnMap;

            sender.Character.Session = sender;

            if (!WorldServerTransferManager.AddTransfer(new WorldMapTransfer
            {
                Character = sender.Character,
                Map = SpawnMap,
            }))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.ErrorInStatus);
                return;
            }


            sender.Character.ZoneTransferCallback = () =>
            {
                SH04Handler.SendIngameChunk(sender.Character);
                WorldCharacterManager.Instance.LogCharacterIn(sender.Character);
            };


            TransferMethods.SendZoneTransfer(sender.Character);
        }


        [PacketHandler(Handler04Type.CMSG_CHAR_LOGIN_REQ)]
        public static void SELECT_CHARACTER(WorldSession sender, FiestaPacket packet)
        {
            if (sender.Ingame
                || !sender.AccountIsLoggedIn
                || !packet.Read(out byte Slot))
            {
                _SH04Helpers.SendCharacterError(sender, (CharacterErrors)ConnectionError.ClientManipulation);//Todo Send Manipulitet client
                sender.Dispose();
                return;
            }

            if (!sender.CharacterList.GetCharacterBySlot(Slot, out WorldCharacter Character))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.RequestedCharacterIDNotMatching);
                return;
            }

            if (sender.UserAccount.RoleID > 0)
                Character.LoginInfo.GameRole = GameCommandManager.GetRole(sender.UserAccount.RoleID);

            if (Character.LoginInfo.IsFirstLogin && GameConfiguration.Instance.UseTutorial)
            {
                sender.Character = Character;
                SH04Handler.SendTutorialRequest(sender);
                return;
            }


            if (!MapManager.GetNormalMapById(Character.AreaInfo.MapInfo.Type == MapType.Normal ?
                Character.AreaInfo.MapInfo.ID : GameConfiguration.Instance.DefaultSpawnMapId,
                out NormalMap mMap))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.MapUnderMaintenance);
                return;
            }

            if (mMap.Zone != null && mMap.Zone.IsFull)
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.MapUnderMaintenance);
                return;
            }


            Character.AreaInfo.Map = mMap;

            sender.Character = Character;
            sender.Character.Session = sender;

            if (!WorldServerTransferManager.AddTransfer(new WorldMapTransfer
            { 
                Character = sender.Character,
                Map = mMap,
            }))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.ErrorInStatus);
                return;
            }

            sender.Character.ZoneTransferCallback = () =>
            {
                SH04Handler.SendIngameChunk(sender.Character);

                WorldCharacterManager.Instance.LogCharacterIn(sender.Character);
            };

            TransferMethods.SendZoneTransfer(sender.Character);
        }
    }
}