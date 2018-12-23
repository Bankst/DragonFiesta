using DFEngine.Server;
using DFEngine.Utils.ServerConsole;

namespace ZoneServer.Util.Console
{
	public class ZoneConsoleTitle : ConsoleTitle
	{
		private const string ZoneConstString = "DragonFiesta.ZoneServer [{0}] Players [{1}/{2}] Remotes [{3}]";

		private static int ZoneId => ZoneServer.ZoneId;

		private static int PlayerCount => ZoneServer.ClientServer.Connections.Count;

		private static int MaxConnection => ZoneServer.ZoneNetConfig != null ? ZoneServer.ZoneNetConfig.MaxClientConnections : 0;

		private static int RemoteZoneCount => BaseApplication.NetConfig != null ? BaseApplication.NetConfig.ZoneNetworkConfigs.Length - 1 : 0;

		public void Update()=> base.Update(ZoneConstString, ZoneId, PlayerCount, MaxConnection, RemoteZoneCount);
	}
}
