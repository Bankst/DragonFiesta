using DFEngine;
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
			ServerMain.Transfers.AddSafe(transfer);

			// We can check here for dual logins.

			new PROTO_NC_USER_WILLLOGIN_ACK(guid, true).Send(connection);
		}

		public static void NC_USER_LOGINWORLD_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var username = message.ReadString(256);
			var guid = message.ReadString(32);

			var transfer = ServerMain.Transfers.First(t => t.Guid == guid);
			ServerMain.Transfers.RemoveSafe(transfer);

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
			Object.Destroy(connection);
		}
	}
}
