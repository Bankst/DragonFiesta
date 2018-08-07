using DragonFiesta.Game.Worlds;
using DragonFiesta.Login.Game.Authentication;
using DragonFiesta.Login.Game.Transfer;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Login.InternNetwork.InternHandler.Server;
using DragonFiesta.Login.Network.FiestaHandler.Server;
using DragonFiesta.Login.Network.Helpers;
using DragonFiesta.Messages.Login.Transfer;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Utils.Core;
using System;
using System.Globalization;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Login.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler03Type._Header)]
    public sealed class CH03Handler
    {
		[PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.NA)]
		public static void Version_NA(LoginSession sender, FiestaPacket packet)
		{
			if (!packet.ReadString(out string version, 32))
			{
				sender.Dispose();
				return;
			}

			if (ServerMainDebug.TriggerVersion &&
				!VersionsManager.GetVersionByHash(version, out Version v) &&
				VersionsManager.AddVersion(version, DateTime.Now))
			{
				GameLog.Write(GameLogLevel.Debug, "Triggered NA Version {0}", version);
				SH3Handler.BinVersionAllowed(sender, false);
				return;
			}

			if (!VersionsManager.GetVersionByHash(version, out Version pVersion))
			{
				SH3Handler.BinVersionAllowed(sender, false);
				return;
			}

			if (LoginManager.Instance.Add(sender))
			{
				SH3Handler.BinVersionAllowed(sender, true);
			}
		}


		[PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.EU)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.DE)]
        public static void Version_EU(LoginSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out string version, 32))
            {
                sender.Dispose();
                return;
            }

            if (ServerMainDebug.TriggerVersion &&
                !VersionsManager.GetVersionByHash(version, out Version v) &&
                VersionsManager.AddVersion(version, DateTime.Now))
            {
                GameLog.Write(GameLogLevel.Debug, "Triggered NA Version {0}", version);
                SH3Handler.BinVersionAllowed(sender, false);
                return;
            }

            if (!VersionsManager.GetVersionByHash(version, out Version pVersion))
            {
                SH3Handler.BinVersionAllowed(sender, false);
                return;
            }

            if (LoginManager.Instance.Add(sender))
            {
                SH3Handler.BinVersionAllowed(sender, true);
            }
        }

        [PacketHandler(Handler03Type.CMSG_LOGIN_REQUEST_NA, ClientRegion.NA)]
        public static void AuthLogin_NA(LoginSession pSession, FiestaPacket packet)
        {
			if (!packet.ReadEncodeString(out string AccountName, 260) ||
					!packet.ReadEncodeString(out string Md5Password, 36) ||
					!packet.ReadEncodeString(out string Orginal, 20))
			{
				return;
            }

            if (!LoginManager.Instance.TryGetLogin(pSession.BaseStateInfo.SessionId, out AuthLogin Login))
            {
                SH03Helpers.SendLoginError(pSession, LoginGameError.TIMEOUT);
                return;
            }

            LoginGameError AuthResult = Login.AuthAccount(pSession, AccountName, Md5Password.ToUpper());

            if (AuthResult == LoginGameError.None)
            {
                pSession.GameStates.Authenticated = true;
                SH3Handler.SendWorldList(pSession, false);
            }
            else
            {
                SH03Helpers.SendLoginError(pSession, AuthResult);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.EU)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.DE)]
        public static void AuthLogin_EU(LoginSession pSession, FiestaPacket packet)
        {
            if (!packet.ReadString(out string username, 18)
            || !packet.ReadString(out string password, 16))
            {
                return;
            }

            if (!LoginManager.Instance.TryGetLogin(pSession.BaseStateInfo.SessionId, out AuthLogin Login))
            {
                pSession.Dispose();
                return;
            }

            LoginGameError AuthResult = Login.AuthAccount(pSession, username, MD5Password.CalculateMD5Hash(password));

            if (AuthResult == LoginGameError.None)
            {
                pSession.GameStates.Authenticated = true;
                SH3Handler.SendWorldList(pSession, false);
            }
            else
            {
                SH03Helpers.SendLoginError(pSession, AuthResult);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_WORLDSELECT_REQ)]
        public static void CMG_WORLD_SELECT(LoginSession Session, FiestaPacket packet)
        {
            if (!packet.Read(out byte WorldID))
            {
                Session.Dispose();
                return;
            }

            if (!WorldManager.Instance.GetWorldByID(WorldID, out World MyWorld))
            {
                SH3Handler.WorldServerIP(Session, null);
                return;
            }

            if (Session.GameStates.Region != MyWorld.Info.Region)
            {
                SH03Helpers.SendLoginError(Session, LoginGameError.WRONG_REGION);
                return;
            }

            if (MyWorld.Status != WorldStatus.Full && MyWorld.Status != WorldStatus.Offline &&
                MyWorld.Status != WorldStatus.Maintenance && MyWorld.Status != WorldStatus.Reserved
               || Session.UserAccount.RoleID >= 1 && MyWorld.Status == WorldStatus.Reserved
               || Session.UserAccount.RoleID >= 1 && MyWorld.Status == WorldStatus.Full) // TestServer Exlusive Login For GM :D
            {

                ServerTransferMethods.SendAddWorldTransfer(MyWorld, Session.UserAccount, Session.GetIP(), (Msg) =>
                {

                    if (Msg is AddWorldServerTransfer transfer)
                    {
                        if (transfer.Added)
                        {
                            SH3Handler.WorldServerIP(Session, MyWorld, WorldStatus.OK);
                            Session.GameStates.IsTransferring = true;

                        }
                        else
                        {
                            SH3Handler.WorldServerIP(Session, null, MyWorld.Status);
                        }
                    }
                });
            }
            else
            {
                SH3Handler.WorldServerIP(Session, null, MyWorld.Status);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGIN_WITH_OTP_REQ)]
        public static void Get_Login_Transfer(LoginSession pSession, FiestaPacket packet)
        {
            if (!packet.ReadString(out string guidString, 32) || !Guid.TryParseExact(guidString, "N", out Guid gui))
            {
                pSession.Dispose();
                return;
            }

            if (!LoginTransferManager.FinishTransfer(gui, out LoginServerTransfer mTransfer))
            {
                //TODO Send Error
                pSession.Dispose();
                return;
            }

            if (!pSession.GetIP().Equals(mTransfer.IP))
            {
                //TODO Send Error
                pSession.Dispose();
                return;
            }

            if (LoginSessionManager.Instance.AddAccount(mTransfer.pAccount.ID, pSession))
            {
                pSession.GameStates.Authenticated = true;
                pSession.UserAccount = mTransfer.pAccount;
                SH3Handler.SendWorldList(pSession, false);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_WORLD_STATUS_REQ)]
        public static void World_List(LoginSession pSession, FiestaPacket packet)
        {
            if (!pSession.GameStates.Authenticated
                || !pSession.AccountIsLoggedIn)
            {
                pSession.Dispose();
                return;
            }
            SH3Handler.SendWorldList(pSession, true);
        }

        [PacketHandler(Handler03Type.CMSG_USER_XTRAP_REQ)]
        public static void SER_XTRAP_REQ(LoginSession sender, FiestaPacket packet)
        {
            if (!packet.Read(out byte XtrapHashLenght) ||
                !packet.ReadString(out string XtrapVersionsHash, XtrapHashLenght))
            {
                return;
            }

            //SH3Handler.BinVersionAllowed(sender, false);

            //SH3Handler.VersionAllowed(sender, false);
        }
    }
}