using System;
using DFEngine.Server;

namespace DFEngine
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
		}
	}
}
