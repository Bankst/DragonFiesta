using DFEngine;
using DFEngine.Network;

using ZoneServer.Engines;
using ZoneServer.Services;

namespace ZoneServer.Handlers
{
	public static class UserHandlers
	{
		internal static void NC_USER_NORMALLOGOUT_CMD(NetworkMessage message, NetworkConnection client)
		{
			var logoutType = (LogoutType) message.ReadByte();
			NetEngine.RemovePing(client);
			CharacterService.Logout(client.Character, logoutType);
		}
	}
}
