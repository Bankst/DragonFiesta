using System;

namespace DFEngine.Logging
{
	public sealed class GameLog : FileLog
	{
		protected override LogType LogTypeName => LogType.GameLog;

		private GameLog(string directory) : base(directory)
		{
		}

		private static GameLog Instance => (_instance ?? (_instance = new GameLog(@"Game")));
		private static GameLog _instance;

		public static void SetupLevels(byte mConsoleLevel, byte mFileLogLevel)
		{
			Instance.SetConsoleLevel(mConsoleLevel);
			Instance.SetFileLogLevel(mFileLogLevel);
		}

		public static void WriteConsoleLine(GameLogLevel type, string message, params object[] args)
		{
			Instance.ConsoleWriteLine(LogType.GameLog, type, message, args);
		}

		public static void Write(GameLogLevel type, string message, params object[] args)
		{
			Instance.Write(LogType.GameLog, type, message, args);
		}

		public static void Write(Exception exception, string message, params object[] args)
		{
			Instance.WriteException(exception, GameLogLevel.Exception, message, args);
		}
	}
}
