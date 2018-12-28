using DFEngine.Server;
using DFEngine.Utils.ServerConsole;

namespace WorldManagerServer.Util.Console
{
	public class WorldConsoleTitle : ConsoleTitle
	{
		private const string TitleString = "DragonFiesta.WorldManagerServer ID [{0}] Zones [{1}/{2}] Players [{3}/{4}]";

		private static int PlayerCount => WorldManagerServer.ClientServer.Connections.Count;

		private static int WorldId => BaseApplication.NetConfig != null ? BaseApplication.NetConfig.WorldNetConfig.WorldID : -1;

		private static int MaxPlayers => BaseApplication.NetConfig != null ? BaseApplication.NetConfig.WorldNetConfig.MaxClientConnections : 0;

		private static int ActiveZone => WorldManagerServer.ZoneServer.Connections.Count;

		private static int MaxZones => BaseApplication.NetConfig != null ? BaseApplication.NetConfig.WorldNetConfig.MaxZoneConnections : 0;

		public void Update() => base.Update(TitleString, WorldId, ActiveZone, MaxZones, PlayerCount, MaxPlayers);
	}
}
