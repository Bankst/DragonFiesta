using System.Linq;
using DFEngine;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;
using WorldManagerServer.Services;

namespace WorldManagerServer.Handlers
{
	internal static class CharacterHandlers
	{
		internal static void NC_CHAR_LOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var charSlot = message.ReadByte();
			var avatar = connection.Account.Avatars.First(a => a.Slot == charSlot);

			if (avatar == null)
			{
				SocketLog.Write(SocketLogLevel.Warning, $"{connection.Account.Username} has no character in slot {charSlot}");
				new PROTO_NC_CHAR_LOGINFAIL_ACK((ushort) CharLoginError.NOCHAR_INSLOT).Send(connection);
			}

			WorldData.MapZones.TryGetValue(avatar.MapIndx, out var zoneNo);
			var zone = WorldManagerServer.Zones.First(z => z.Number == zoneNo);
			if (zone == null)
			{
				SocketLog.Write(SocketLogLevel.Exception, $"No zone found for {avatar.MapIndx} for char {avatar.Name}!");
				new PROTO_NC_CHAR_LOGINFAIL_ACK((ushort) CharLoginError.MAP_UNDER_MAINT).Send(connection);
			}

			connection.Avatar = avatar;
			connection.Avatar.Login(connection, zone);
		}
	}
}
