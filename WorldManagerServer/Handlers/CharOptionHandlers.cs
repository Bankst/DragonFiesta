using DFEngine.Network;

namespace WorldManagerServer.Handlers
{
	internal static class CharOptionHandlers
	{
		public static void NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK(connection.Avatar.ShortcutSizeData).Send(connection);
		}

		public static void NC_CHAR_OPTION_GET_WINDOWPOS_REQ(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_CHAR_OPTION_GET_WINDOWPOS_ACK(connection.Avatar.WindowPosData).Send(connection);
		}
	}
}
