using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Network;

namespace ZoneServer.Handlers
{
	public static class CharacterHandlers
	{
		internal static void NC_CHAR_LOGOUTREADY_CMD(NetworkMessage message, NetworkConnection client)
		{
			client.Character.PrepareLogout();
		}

		internal static void NC_CHAR_LOGOUTCANCEL_CMD(NetworkMessage message, NetworkConnection client)
		{
			client.Character.CancelLogout();
		}
	}
}
