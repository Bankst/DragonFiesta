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
                || !packet.Read(out bool BackToCharacterList))
            {
                sender.Dispose();
                return;
            }


            if (BackToCharacterList)
            {
                if (sender.CharacterList.Refresh())
                {
                    SH03Handler.SendCharacterList(sender);
                }
                sender.GameStates.IsReady = false;
            }
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
                    if (msg is AddLoginServerTransfer Transfer)
                    {
                        SH03Handler.SendLoginToWorldKey(sender, Transfer.Id, Transfer.Added);

                        if (!Transfer.Added)
                            sender.Dispose();
                    }
                },
                (msg) => //Time out
                {
                    sender.Dispose();
                });
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.NA)]
        public static void World_Key_NA(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out string AccountName, 256)
                || !packet.Read(out int AccountID)
                || !packet.ReadBytes(60, out byte[] authbytes)
                || AccountID == 0)
            {
                sender.Dispose();
                return;
            }

            if (!VerfiryWorldKey(AccountID, AccountName, sender))
            {
                SH03Helpers.SendVerfiryError(sender, (ushort)ConnectionError.ClientManipulation);
                sender.Dispose();
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.DE)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_LOGINWORLD_REQ, ClientRegion.EU)]
        public static void World_Key_EU(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out string AccountName, 18)
                || !packet.Read(out int AccountID)
                || !packet.ReadBytes(60, out byte[] authbytes)
                || AccountID == 0)
            {
                sender.Dispose();
                return;
            }

            if (!VerfiryWorldKey(AccountID, AccountName, sender))
            {
                SH03Helpers.SendVerfiryError(sender, (ushort)ConnectionError.ClientDataError);
                sender.Dispose();
            }
        }

        private static bool VerfiryWorldKey(int accountID, string accountName, WorldSession sender)
        {
            if (!WorldServerTransferManager.FinishTransfer(accountID, out WorldServerTransfer Transfer)
                || !Transfer.IP.Equals(sender.GetIP())
                || WorldConfiguration.Instance.WorldID != Transfer.WorldId
                || !accountName.Equals(Transfer.Account.Name)
                || !WorldSessionManager.Instance.AddAccount(Transfer.Account.ID, sender)) //verfiry...
                return false;

            sender.UserAccount = Transfer.Account;

            sender.GameStates.Authenticated = true;
            sender.GameStates.HasPong = true;

            WorldPingManager.Instance.RegisterClient(sender);

            if (!sender.CharacterList.Refresh())
            {
                SH03Helpers.SendVerfiryError(sender, (ushort)ConnectionError.ClientManipulation); //TOOD Another..
                return false;
            }

            SH03Handler.SendCharacterList(sender);

            return true;
        }
    }
}