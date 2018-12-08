using DFEngine;
using DFEngine.Accounts;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using WorldManagerServer.Services;

namespace WorldManagerServer.Handlers
{
	internal static class UserHandlers
	{
		internal static void NC_USER_WILLLOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var userNo = message.ReadInt32();
			var userName = message.ReadString(256);
			var guid = message.ReadString(32);

			var transfer = new NetworkTransfer(new Account(userNo, userName), guid);
			WorldManagerServer.Transfers.AddSafe(transfer);

			// We can check here for dual logins.

			new PROTO_NC_USER_WILLLOGIN_ACK(guid, true).Send(connection);
		}

		public static void NC_USER_LOGINWORLD_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var username = message.ReadString(256);
			var guid = message.ReadString(32);

			var transfer = WorldManagerServer.Transfers.First(t => t.Guid == guid);
			WorldManagerServer.Transfers.RemoveSafe(transfer);

			if (!transfer || transfer.Account.Username != username)
			{
				connection.Disconnect();
				return;
			}

			connection.Account = transfer.Account;
			connection.Account.Avatars = AvatarService.LoadAll(connection.Account.UserNo);

			new PROTO_NC_USER_LOGINWORLD_ACK(connection.Handle, connection.Account.Avatars).Send(connection);
		}

		public static void NC_USER_NORMALLOGOUT_CMD(NetworkMessage message, NetworkConnection connection)
		{
			var logoutType = (LogoutType) message.ReadByte();
			Object.Destroy(connection);
		}

		public static void NC_USER_WILL_WORLD_SELECT_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var md5 = connection.Account.Username.ToMD5();
			//new PROTO_NC_LOCAL_ADDTRANSFER_CMD(md5).Send(WorldManagerServer.LoginServer);
			new PROTO_NC_USER_WILL_WORLD_SELECT_ACK(7768, md5).Send(connection);
		}
	}
}
