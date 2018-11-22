using DFEngine;
using DFEngine.Config;
using DFEngine.Logging;
using DFEngine.Server;

namespace LoginServer
{
	public class ServerMain : ServerMainBase
	{
		public new static ServerMain InternalInstance { get; private set; }

		public ServerMain() : base(ServerType.Login)
		{
		}

		public static void Initialize()
		{
			InternalInstance = new ServerMain();
			InternalInstance.WriteConsoleLogo();
			EngineLog.Write(EngineLogLevel.Startup, "Starting LoginServer");

			if (!NetworkConfiguration.Initialize(out var netConfigMsg))
			{
				throw new StartupException(netConfigMsg);
			}

			if (!LoginConfiguration.Initialize(out var loginConfigMsg))
			{
				throw new StartupException(loginConfigMsg);
			}
		}
	}
}
