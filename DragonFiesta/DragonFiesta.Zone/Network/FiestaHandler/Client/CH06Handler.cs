using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Zone.Data.Maps;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Transfer;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler06Type._Header)]
    public static class CH06Handler
    {

        [PacketHandler(Handler06Type.CMSG_MAP_TOWNPORTAL_REQ)]
        public static void CMSG_MAP_USE_GATE(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame ||
                !packet.Read(out byte Index))
            {
                sender.Dispose();
                return;
            }

            if (!ZoneMapDataProvider.GetTownPortalInfoByID(Index, out TownPortalInfo Portal))
            {
                ZoneChat.CharacterNote(sender.Character, $"Invalid Database Info for Index {Index}");
                return;
            }

            if (Portal.MinLevel < sender.Character.Level)
            {
                ZoneChat.CharacterNote(sender.Character, "Character Level To Low!");
                return;
            }

            sender.Character.ChangeMap(Portal.MapInfo.ID, 0, Portal.Position.X, Portal.Position.Y);
        }

        [PacketHandler(Handler06Type.CMSG_MAP_LOGIN_REQ)]
        public static void CMSG_MAP_TRANSFER_KEY(ZoneSession sender, FiestaPacket packet)
        {
            if (!packet.Read(out ushort SessionId) || !packet.ReadString(out string CharName, 20))
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientManipulation);
                sender.Dispose();//or send Data error
                return;
            }

            if (!ZoneServerTransferManager.FinishTransfer(SessionId, out ZoneTransfer Transfer))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.RequestedCharacterIDNotMatching);
                sender.Dispose();
                return;
            }

            //verifery Name
            if (!Transfer.Character.Info.Name.Equals(CharName))
            {
                _SH04Helpers.SendCharacterError(sender, CharacterErrors.RequestedCharacterIDNotMatching);
                return;
            }

            //Online Handling by World...
            if (!Transfer.Character.LoginInfo.IsOnline)
            {
                sender.Dispose();
                return;
            }
            //Set User info

            sender.Character = Transfer.Character;
            sender.Character.Session = sender;
            sender.Character.LoginInfo.RoleId = Transfer.Character.LoginInfo.RoleId;

            //Set Session Variable
            sender.Character.WorldSessionId = Transfer.WorldSessionId;
            sender.Character.Session = sender;

            //Set Position
            sender.Character.AreaInfo.Map = Transfer.Map;
            sender.Character.AreaInfo.Position = Transfer.SpawnPosition;

            //Set Authenticate
            sender.GameStates.Authenticated = true;
            sender.GameStates.IsTransferring = false;

            //send chunk
            SH04Handler.SendCharacterInfo(sender);
            SH04Handler.SendCharacterLook(sender);

            using (var mPacket = new FiestaPacket(4, 72))
            {
                mPacket.WriteHexAsBytes("FF FF FF FF");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(4, 58))
            {
                mPacket.WriteHexAsBytes("A7 13 1C 00 01 01 3A 01 06 5E 43 D3 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                sender.SendPacket(mPacket);
            }
            using (var mPacket = new FiestaPacket(4, 59))
            {
                mPacket.WriteHexAsBytes("A7 13 1C 00 03 00 2A 00 03 00 00 00 83 EA 80 79 01 4E 00 00 00 00 84 EA A6 79 01 4E 00 00 00 00 85 EA AF 79 01 4E 00 00 00 00");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(4, 206))
            {
                mPacket.WriteHexAsBytes("A7 13 1C 00 00 00");
                sender.SendPacket(mPacket);
            }
            using (var mPacket = new FiestaPacket(4, 215))
            {
                mPacket.WriteHexAsBytes("A7 13 1C 00 00 00");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(4, 61))
            {
                mPacket.WriteHexAsBytes("01 03 19 00 A7 13 1C 00 19 00 C8 19 00 00 00 00 00 20 00 00 00 00 DC 19 00 00 00 00 00 00 00 00 00 00 F0 19 00 00 00 00 00 00 00 00 00 00 49 71 00 00 00 00 00 00 00 00 00 00 16 72 00 00 00 00 00 00 00 00 00 00 19 72 00 00 00 00 00 00 00 00 00 00 F6 72 00 00 00 00 00 00 00 00 00 00 1C 73 00 00 00 00 00 00 00 00 00 00 1D 73 00 00 00 00 00 00 00 00 00 00 1E 73 00 00 00 00 00 00 00 00 00 00 20 73 00 00 00 00 00 00 00 00 00 00 45 73 00 00 00 00 00 00 00 00 00 00 4A 73 00 00 00 00 00 00 00 00 00 00 4B 73 00 00 00 00 00 00 00 00 00 00 4C 73 00 00 00 00 00 00 00 00 00 00 AD 73 00 00 00 00 00 00 00 00 00 00 AE 73 00 00 00 00 00 00 00 00 00 00 CC 73 00 00 00 00 00 00 00 00 00 00 CD 73 00 00 00 00 00 00 00 00 00 00 31 74 00 00 00 00 00 00 00 00 00 00 32 74 00 00 00 00 00 00 00 00 00 00 54 74 00 00 00 00 00 00 00 00 00 00 55 74 00 00 00 00 00 00 00 00 00 00 56 74 00 00 00 00 00 00 00 00 00 00 8F 74 00 00 00 00 00 00 00 00 00 00");
                sender.SendPacket(mPacket);
            }
            using (var mPacket = new FiestaPacket(4, 62))
            {
                mPacket.WriteHexAsBytes("00 00");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(4, 73))
            {
                mPacket.WriteHexAsBytes("00 00 00 00 00 00");
                sender.SendPacket(mPacket);
            }
            using (var mPacket = new FiestaPacket(4, 74))
            {
                mPacket.WriteHexAsBytes("00 00");
                sender.SendPacket(mPacket);
            }
            using (var mPacket = new FiestaPacket(4, 72))
            {
                mPacket.WriteHexAsBytes("FF FF FF FF");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(4, 222))
            {
                mPacket.WriteHexAsBytes("00 00 00 00 00 00 00 00 10 27 00 00 00 00 00 00");
                sender.SendPacket(mPacket);
            }

            using (var mPacket = new FiestaPacket(17, 30))
            {
                mPacket.WriteHexAsBytes("60 A5 85 56 60 53 C7 57 E0 D3 D5 57 E0 BC DD 57");
                sender.SendPacket(mPacket);
            }

            SH06Handler.SendDetailedInfoExtra(sender);
        }

        [PacketHandler(Handler06Type.CMSG_MAP_LOGINCOMPLETE_CMD)]
        public static void CMSG_CHARACTER_MAP_LOAD_READY(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.IsAuthenticatet)
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientManipulation);
                sender.Dispose();
                return;
            }

            if (!sender.Character.IsOnThisZone)
            {
                sender.Dispose();
                return;
            }

            //First login
            if (sender.Character.LoginInfo.IsFirstLogin)
            {
                sender.Character.LivingStats.HP = (uint)sender.Character.Info.Stats.FullStats.MaxHP;
                sender.Character.LivingStats.SP = (uint)sender.Character.Info.Stats.FullStats.MaxSP;
                sender.Character.LivingStats.LP = (uint)sender.Character.Info.Stats.FullStats.MaxLP;
                sender.Character.LoginInfo.IsFirstLogin = false;
            }

            //update stats in Client
            sender.Character.Info.Stats.UpdateAll();
            SH09Handler.SendHPUpdate(sender.Character);
            SH09Handler.SendSPUpdate(sender.Character);

            if (CharacterClass.ClassUsedLP(sender.Character.Info.Class))
            {
                SH09Handler.SendLPUpdate(sender.Character);
            }

            //FreeStats
            if (sender.Character.Info.FreeStats.FreeStat_Points > 0)
            {
                SH04Handler.SendRemainingStatPoints(sender);
            }

            sender.GameStates.IsReady = true;
            sender.GameStates.IsTransferring = false;
            sender.GameStates.HasPong = true;
            ZoneCharacterManager.Instance.CharacterMapRefreshed(sender.Character);
        }
    }
}