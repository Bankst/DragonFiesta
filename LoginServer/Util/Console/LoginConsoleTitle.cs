using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Config;
using DFEngine.Utils.ServerConsole;

namespace LoginServer.Util.Console
{
	public class LoginConsoleTitle : ConsoleTitle
	{
		private static readonly string DebuggingString = System.Diagnostics.Debugger.IsAttached ? " (Debugging)" : "";

		private const string Worldstring = "DragonFiesta.LoginServer Worlds [{0}/{1}] Connections [{2}/{3}] -{4}";
		
		private static readonly int WorldCount = ServerMain.WorldServer.Connections.Count;

		private static int MaxWorldCount => NetworkConfiguration.Instance != null ? NetworkConfiguration.Instance.LoginNetConfig.MaxWorldConnections : 0;

		private static readonly int ConnectionCount = ServerMain.ClientServer.Connections.Count;

		private static int MaxConnection => NetworkConfiguration.Instance != null ? NetworkConfiguration.Instance.LoginNetConfig.MaxClientConnections : 0;
		
		public void Update() => Title = string.Format(Worldstring, WorldCount, MaxWorldCount, ConnectionCount, MaxConnection, DebuggingString);
	}
}
