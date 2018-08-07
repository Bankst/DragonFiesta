using DragonFiesta.Messages.World.Transfer;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Transfer;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Transfer;
using DragonFiesta.World.Network.FiestaHandler.Server;
using DragonFiesta.World.Network.Helpers;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler03Type._Header)]
    public static class CH03Handler
    {
        [PacketHandler(Handler03Type.CMSG_USER_AVATAR_LIST_REQ)]
        public static void CMSG_CHARACTERLIST_REQUEST(WorldSession sender, FiestaPacket packet)
        {
            EngineLog.Write(EngineLogLevel.Startup, "Test");
        }
        [PacketHandler(Handler03Type.CMSG_USER_NORMALLOGOUT_CMD)]
        public static void CMSG_CONNECTION_CLOSE(WorldSession sender, FiestaPacket packet)
        {

            if (!sender.Ingame ||
                sender.IsDisposed
                || !packet.Read(out bool backToCharacterList))
            {
                sender.Dispose();
                return;
            }


	        if (!backToCharacterList) return;
	        if (sender.CharacterList.Refresh())
	        {
		        SH03Handler.SendCharacterList(sender);
	        }
	        sender.GameStates.IsReady = false;
        }

        [PacketHandler(Handler03Type.CMSG_USER_WILL_WORLD_SELECT_REQ)]
        public static void WORLD_TO_LOGIN(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.AccountIsLoggedIn)
                sender.Dispose();

            TransferMethods.SendLoginTransfer(
                sender.UserAccount.ID,
                sender.GetIP(),
                (msg) =>
                {
	                if (!(msg is AddLoginServerTransfer transfer)) return;
	                SH03Handler.SendLoginToWorldKey(sender, transfer.Id, transfer.Added);

	                if (!transfer.Added)
		                sender.Dispose();
                },
                (msg) => //Time out
                {
                    sender.Dispose();
                });
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.NA)]
        public static void World_Key_NA(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out var accountName, 256)
                || !packet.Read(out int accountID)
                || !packet.ReadBytes(60, out var authbytes)
                || accountID == 0)
            {
                sender.Dispose();
                return;
            }

	        if (VerifyWorldKey(accountID, accountName, sender)) return;
	        SH03Helpers.SendVerifyError(sender, (ushort)ConnectionError.FailedToConnectToWorldServer);
	        sender.Dispose();
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.DE)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.EU)]
        public static void World_Key_EU(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out var accountName, 18)
                || !packet.Read(out int accountID)
                || !packet.ReadBytes(60, out var authbytes)
                || accountID == 0)
            {
                sender.Dispose();
                return;
            }

	        if (VerifyWorldKey(accountID, accountName, sender)) return;
	        SH03Helpers.SendVerifyError(sender, (ushort)ConnectionError.ClientDataError);
	        sender.Dispose();
        }

        private static bool VerifyWorldKey(int accountID, string accountName, WorldSession sender)
        {
            if (!WorldServerTransferManager.FinishTransfer(accountID, out WorldServerTransfer transfer)
                || !transfer.IP.Equals(sender.GetIP())
                || WorldConfiguration.Instance.WorldID != transfer.WorldId
                || !accountName.ToLower().Equals(transfer.Account.Name.ToLower())
                || !WorldSessionManager.Instance.AddAccount(transfer.Account.ID, sender)) //verify...
                return false;

            sender.UserAccount = transfer.Account;

            sender.GameStates.Authenticated = true;
            sender.GameStates.HasPong = true;

            WorldPingManager.Instance.RegisterClient(sender);

            if (!sender.CharacterList.Refresh())
            {
                SH03Helpers.SendVerifyError(sender, (ushort)ConnectionError.ClientManipulation); //TOOD Another..
                return false;
            }

            SH03Handler.SendCharacterList(sender);

            return true;
        }
    }
}