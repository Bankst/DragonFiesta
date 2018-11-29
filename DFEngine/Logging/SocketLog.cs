using System;
using DFEngine.Server;

namespace DFEngine.Logging
{
	public sealed class SocketLog : FileLog
	{
		protected override string LogTypeName => "SocketLog";

		private SocketLog(string directory) : base(directory)
		{
		}

		private static SocketLog Instance => (_instance ?? (_instance = new SocketLog(@"Socket")));
		private static SocketLog _instance;

		public static void SetupLevels(byte mConsoleLevel, byte mFileLogLevel)
		{
			Instance.SetConsoleLevel(mConsoleLevel);
			Instance.SetFileLogLevel(mFileLogLevel);
		}

		public static void WriteConsoleLine(SocketLogLevel type, string message, params object[] args)
		{
			Instance.ConsoleWriteLine(LogType.SocketLog, type, message, args);
		}

		public static void Write(SocketLogLevel type, string message, params object[] args)
		{
			Instance.Write(LogType.SocketLog, type, message, args);
		}

		public static void Write(Exception exception, string message, params object[] args)
		{
			Instance.WriteException(LogType.SocketLog, exception, SocketLogLevel.Exception, message, args);
		}
	}
}
