
using DFEngine;
using DFEngine.Config;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;

using LoginServer.Services;

namespace LoginServer.Handlers
{
	internal static class UserHandlers
	{
		internal static byte[] XTrapKey = { 0x33, 0x33, 0x42, 0x35, 0x34, 0x33, 0x42, 0x30, 0x43, 0x41, 0x36, 0x45, 0x37, 0x43, 0x34, 0x31, 0x45, 0x35, 0x44, 0x31, 0x44, 0x30, 0x36, 0x35, 0x31, 0x33, 0x30, 0x37, 0x0 };

		internal static void NC_USER_CLIENT_VERSION_CHECK_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var versionKey = message.ReadString(64);

			if (versionKey != LoginConfiguration.Instance.ClientVersion && ServerMain.LoginConfig.CheckVersion)
			{
				SocketLog.Write(SocketLogLevel.Exception, $"Wrong version key used - {versionKey}");

				new PROTO_NC_USER_CLIENT_WRONGVERSION_CHECK_ACK().Send(connection);
				return;
			}

			new PROTO_NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK((byte)XTrapKey.Length, XTrapKey).Send(connection);
		}

		internal static void NC_USER_US_LOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var userName = message.ReadString(260);
			var password = message.ReadString(36);

			AccountService.Login(userName, password, out var userNo, out var blocked, out var canLogin);

			if (userNo == -1)
			{
				SocketLog.Write(SocketLogLevel.Warning, $"[{connection}] Invalid credentials");

				new PROTO_NC_USER_LOGINFAIL_ACK(0x45).Send(connection);
				new PROTO_NC_LOG_USER_LOGINFAIL(userName, password, connection.GetRemoteIP, 0x45).Send(ServerMain.GameLogServer);
				return;
			}

			if (blocked)
			{
				SocketLog.Write(SocketLogLevel.Warning, $"[{connection}] Logging in while banned");

				new PROTO_NC_USER_LOGINFAIL_ACK(0x47).Send(connection);
				new PROTO_NC_LOG_USER_LOGINFAIL(userName, password, connection.GetRemoteIP, 0x47).Send(ServerMain.GameLogServer);
				return;
			}

			if (!canLogin)
			{
				SocketLog.Write(SocketLogLevel.Warning, $"[{connection}] Logging in during maintenance");

				new PROTO_NC_USER_LOGINFAIL_ACK(0x48).Send(connection);
				new PROTO_NC_LOG_USER_LOGINFAIL(userName, password, connection.GetRemoteIP, 0x48).Send(ServerMain.GameLogServer);
				return;
			}

			connection.Account = new Account(userNo, userName);
			new PROTO_NC_USER_LOGIN_ACK(ServerMain.Worlds).Send(connection);
		}

		internal static void NC_USER_XTRAP_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var xtrapClientKeyLength = message.ReadByte();
			var xtrapClientKey = message.ReadBytes(xtrapClientKeyLength);

			new PROTO_NC_USER_XTRAP_ACK(xtrapClientKey.Compare(XTrapKey)).Send(connection);
		}

		internal static void NC_USER_WORLD_STATUS_REQ(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_USER_WORLD_STATUS_ACK(ServerMain.Worlds).Send(connection);
		}

		internal static void NC_USER_WORLDSELECT_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var worldNo = message.ReadByte();
			var world = ServerMain.Worlds.First(w => w.Number == worldNo);

			if (world == null)
			{
				new PROTO_NC_USER_LOGINFAIL_ACK(0x42).Send(connection);
				new PROTO_NC_LOG_USER_LOGINFAIL(connection.Account.Username, null, connection.GetRemoteIP, 0x42).Send(ServerMain.GameLogServer);
				return;
			}

			var transfer = new NetworkTransfer(connection.Account, connection.Guid)
			{
				Connection = connection,
				World = world
			};

			ServerMain.Transfers.Add(transfer);
			new PROTO_NC_USER_WILLLOGIN_REQ(connection.Account, connection.Guid).Send(world.Connection);
			new PROTO_NC_LOG_USER_LOGIN(connection.Account.UserNo, worldNo, connection.GetRemoteIP).Send(ServerMain.GameLogServer);
		}

		internal static void NC_USER_NORMALLOGOUT_CMD(NetworkMessage message, NetworkConnection connection)
		{
			Object.Destroy(connection);
		}

		internal static void NC_USER_LOGIN_WITH_OTP_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var guid = message.ReadString(32);

			if (!ServerMain.CachedAccounts.TryGetValue(guid, out var account))
			{
				new PROTO_NC_USER_LOGINFAIL_ACK(0x49).Send(connection);
				return;
			}

			ServerMain.CachedAccounts.Remove(guid);

			connection.Account = account;
			new PROTO_NC_USER_LOGIN_ACK(ServerMain.Worlds).Send(connection);
		}

		internal static void NC_USER_WILLLOGIN_ACK(NetworkMessage message, NetworkConnection connection)
		{
			var guid = message.ReadString(32);
			var ok = message.ReadBoolean();

			var transfer = ServerMain.Transfers.First(t => t.Guid == guid);
			ServerMain.Transfers.RemoveSafe(transfer);

			if (!ok || !transfer)
			{
				SocketLog.Write(SocketLogLevel.Exception, $"Process transfer failed for - {transfer}");
				new PROTO_NC_USER_LOGINFAIL_ACK(0x49).Send(transfer?.Connection);

				return;
			}

			ServerMain.CachedAccounts.AddSafe(guid, transfer.Account);
			new PROTO_NC_USER_WORLDSELECT_ACK(transfer.World, guid).Send(transfer.Connection);

			Object.Destroy(transfer);
		}
	}
}
