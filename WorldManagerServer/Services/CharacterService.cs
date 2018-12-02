using System.Linq;
using System.Net;

using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;

namespace WorldManagerServer.Services
{
	public class CharacterService
	{
		internal static bool CharLogin(NetworkConnection connection, int charSlot, out IPEndPoint zoneEndPoint, out int error)
		{
			zoneEndPoint = null;
			error = 0;

			var charBySlot = connection.Account.Avatars.FirstOrDefault(a => a.Slot == charSlot);
			if (charBySlot == null)
			{
				SocketLog.Write(SocketLogLevel.Warning, $"{connection.Account.Username} has no character in slot {charSlot}");
				error = (int) ConnectionError.ThereIsNoCharacterInTheSlot;
				return false;
			}
			var charMapIndx = charBySlot.MapIndx;

			// we need to find the ZoneID by the current character map, and use that. For now, set 0 for first zone.
			var zoneId = 0;

			var zoneConfig = ServerMain.NetConfig.ZoneNetworkConfigs.FirstOrDefault(z => z.ZoneID == zoneId);
			if (zoneConfig == null)
			{
				SocketLog.Write(SocketLogLevel.Exception, $"Zone ID {zoneId} config not found!");
				error = (int) ConnectionError.MapUnderMaintenace;
				return false;
			}

			// If the External IP is not the same as Internal IP, then use External IP
			var zoneIp = zoneConfig.ExternalIP == zoneConfig.ListenIP ? zoneConfig.ListenIP : zoneConfig.ExternalIP;

			zoneEndPoint = new IPEndPoint(IPAddress.Parse(zoneIp), zoneConfig.ListenPort);
			return true;
		}
	}
}
