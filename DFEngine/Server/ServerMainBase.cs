using System;
using System.Reflection;
using DFEngine.Logging;
using DFEngine.Utils;

namespace DFEngine.Server
{
	public class ServerMainBase
	{
		public static ServerMainBase InternalInstance { get; private set; }

		public ServerType ServerType { get; private set; }
		public string StartDirectory { get; private set; }
		public string StartExecutable { get; private set; }
		public virtual bool ServerIsReady { get; set; } = false;

		public GameTime CurrentTime { get; internal set; }
		public TimeSpan TotalUpTime { get; internal set; }


		public ServerMainBase(ServerType pType)
		{
			if (InternalInstance != null)
			{
				throw new InvalidOperationException("Can only load one instance of this class at once.");
			}

			InternalInstance = this;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			StartDirectory = AppDomain.CurrentDomain.BaseDirectory.ToEscapedString();
			StartExecutable = (Assembly.GetEntryAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\"));
			CurrentTime = (GameTime)DateTime.Now;
			ServerType = pType;
		}

		public void WriteConsoleLogo()
		{
			Logo.PrintLogo(ConsoleColor.DarkYellow);
		}


		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			EngineLog.Write(EngineLogLevel.Exception, e.ExceptionObject.ToString());
		}
	}
}
