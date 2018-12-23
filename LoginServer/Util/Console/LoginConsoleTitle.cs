using DFEngine.Config;
using DFEngine.Server;
using DFEngine.Utils.ServerConsole;

namespace LoginServer.Util.Console
{
	public class LoginConsoleTitle : ConsoleTitle
	{
		private const string Worldstring = "DragonFiesta.LoginServer Worlds [{0}/{1}] Connections [{2}/{3}]";
		
		private static int WorldCount => LoginServer.WorldServer.Connections.Count;

		private static int MaxWorldCount => BaseApplication.NetConfig != null ? BaseApplication.NetConfig.LoginNetConfig.MaxWorldConnections : 0;

		private static int ConnectionCount => LoginServer.ClientServer.Connections.Count;

		private static int MaxConnection => NetworkConfiguration.Instance != null ? NetworkConfiguration.Instance.LoginNetConfig.MaxClientConnections : 0;

		public void Update() => base.Update(Worldstring, WorldCount, MaxWorldCount, ConnectionCount, MaxConnection);
	}
}
