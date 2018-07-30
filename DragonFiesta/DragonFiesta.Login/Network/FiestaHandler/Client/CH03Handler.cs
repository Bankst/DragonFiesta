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
			var allowed = false;

			if (!packet.ReadString(out var version, 32))
			{
				sender.Dispose();
				return;
			}

			if (!VersionsManager.GetVersionByHash(version, out _))
			{
				if (ServerMainDebug.TriggerVersion)
				{
					GameLog.Write(GameLogLevel.Debug, "Triggered NA Version {0}", version);
					allowed = VersionsManager.AddVersion(version, DateTime.Now);
				}
				else
				{
					SH3Handler.BinVersionAllowed(sender, false);
				}
			}
			else
			{
				allowed = LoginManager.Instance.Add(sender);
			}

			SH3Handler.BinVersionAllowed(sender, allowed);
		}


		[PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.EU)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_CLIENT_VERSION_CHECK_REQ, ClientRegion.DE)]
        public static void Version_EU(LoginSession sender, FiestaPacket packet)
        {
            if (!packet.ReadString(out var version, 32))
            {
                sender.Dispose();
                return;
            }

            if (ServerMainDebug.TriggerVersion &&
                !VersionsManager.GetVersionByHash(version, out var v) &&
                VersionsManager.AddVersion(version, DateTime.Now))
            {
                GameLog.Write(GameLogLevel.Debug, "Triggered NA Version {0}", version);
                SH3Handler.BinVersionAllowed(sender, false);
                return;
            }

            if (!VersionsManager.GetVersionByHash(version, out var pVersion))
            {
                SH3Handler.BinVersionAllowed(sender, false);
                return;
            }

            if (LoginManager.Instance.Add(sender))
            {
                SH3Handler.BinVersionAllowed(sender, true);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_US_LOGIN_REQ, ClientRegion.NA)]
        public static void AuthLogin_NA(LoginSession pSession, FiestaPacket packet)
        {
			if (!packet.ReadEncodeString(out var accountName, 260) ||
					!packet.ReadEncodeString(out var md5Password, 36) ||
					!packet.ReadEncodeString(out var orginal, 20))
			{
				return;
            }

            if (!LoginManager.Instance.TryGetLogin(pSession.BaseStateInfo.SessionId, out var login))
            {
                SH03Helpers.SendLoginError(pSession, LoginGameError.TIMEOUT);
                return;
            }

            var authResult = login.AuthAccount(pSession, accountName, md5Password.ToUpper());

            if (authResult == LoginGameError.None)
            {
                pSession.GameStates.Authenticated = true;
                SH3Handler.SendWorldList(pSession, false);
            }
            else
            {
                SH03Helpers.SendLoginError(pSession, authResult);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.EU)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.ES)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.FR)]
        [PacketHandler(Handler03Type.CMSG_USER_GER_LOGIN_REQ, ClientRegion.DE)]
        public static void AuthLogin_EU(LoginSession pSession, FiestaPacket packet)
        {
            if (!packet.ReadString(out var username, 18)
            || !packet.ReadString(out var password, 16))
            {
                return;
            }

            if (!LoginManager.Instance.TryGetLogin(pSession.BaseStateInfo.SessionId, out var login))
            {
                pSession.Dispose();
                return;
            }

            var authResult = login.AuthAccount(pSession, username, MD5Password.CalculateMD5Hash(password));

            if (authResult == LoginGameError.None)
            {
                pSession.GameStates.Authenticated = true;
                SH3Handler.SendWorldList(pSession, false);
            }
            else
            {
                SH03Helpers.SendLoginError(pSession, authResult);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_WORLDSELECT_REQ)]
        public static void CMG_WORLD_SELECT(LoginSession session, FiestaPacket packet)
        {
            if (!packet.Read(out byte worldID))
            {
                session.Dispose();
                return;
            }

            if (!WorldManager.Instance.GetWorldByID(worldID, out var myWorld))
            {
                SH3Handler.WorldServerIP(session, null);
                return;
            }

            if (session.GameStates.Region != myWorld.Info.Region)
            {
                SH03Helpers.SendLoginError(session, LoginGameError.WRONG_REGION);
                return;
            }

            if (myWorld.Status != WorldStatus.Full && myWorld.Status != WorldStatus.Offline &&
                myWorld.Status != WorldStatus.Maintenance && myWorld.Status != WorldStatus.Reserved
               || session.UserAccount.RoleID >= 1 && myWorld.Status == WorldStatus.Reserved
               || session.UserAccount.RoleID >= 1 && myWorld.Status == WorldStatus.Full) // TestServer Exlusive Login For GM :D
            {

                ServerTransferMethods.SendAddWorldTransfer(myWorld, session.UserAccount, session.GetIP(), (msg) =>
                {

                    if (msg is AddWorldServerTransfer transfer)
                    {
                        if (transfer.Added)
                        {
                            SH3Handler.WorldServerIP(session, myWorld, WorldStatus.OK);
                            session.GameStates.IsTransferring = true;

                        }
                        else
                        {
                            SH3Handler.WorldServerIP(session, null, myWorld.Status);
                        }
                    }
                });
            }
            else
            {
                SH3Handler.WorldServerIP(session, null, myWorld.Status);
            }
        }

        [PacketHandler(Handler03Type.CMSG_USER_LOGIN_WITH_OTP_REQ)]
        public static void Get_Login_Transfer(LoginSession pSession, FiestaPacket packet)
        {
            if (!packet.ReadString(out var guidString, 32) || !Guid.TryParseExact(guidString, "N", out var gui))
            {
                pSession.Dispose();
                return;
            }

            if (!LoginTransferManager.FinishTransfer(gui, out var mTransfer))
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
	        if (packet.Read(out byte xtrapHashLength) &&
	            packet.ReadString(out var xtrapVersionsHash, xtrapHashLength)) return;
	        using (var smsg = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_XTRAP_ACK))
	        {
		        smsg.Write<byte>(true);
				sender.SendPacket(smsg);
	        }

	        //SH3Handler.BinVersionAllowed(sender, false);

            //SH3Handler.VersionAllowed(sender, false);
        }
    }
}