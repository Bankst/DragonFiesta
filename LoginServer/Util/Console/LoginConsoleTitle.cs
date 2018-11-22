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

		// TODO
		// private static byte WorldCount => InternWorldSessionManager.Instance != null ? (byte)InternWorldSessionManager.Instance.CountOfSessions : (byte)0;
		// TODO
		// private static byte MaxWorldCount => WorldManager.Instance != null ? (byte)WorldManager.Instance.WorldList.Count : (byte)0;
		// TODO
		// private static int ConnectionCount => LoginSessionManager.Instance != null ? LoginSessionManager.Instance.CountOfSessions : (byte)0;

		private static int MaxConnection => NetworkConfiguration.Instance != null ? NetworkConfiguration.Instance.LoginNetConfig.MaxConnections : (byte)0;
		
		public void Update() => Title = string.Format(Worldstring, 0, 1, 0, MaxConnection, DebuggingString);
	}
}
