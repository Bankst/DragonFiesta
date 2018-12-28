using System;
using DFEngine.Server;

namespace DFEngine.Logging
{
	public sealed class DatabaseLog : FileLog
	{
		protected override string LogTypeName => "DatabaseLog";

		private DatabaseLog(string directory) : base(directory)
		{
		}

		private static DatabaseLog Instance => (_instance ?? (_instance = new DatabaseLog(@"Database")));
		private static DatabaseLog _instance;

		public static void SetupLevels(byte mConsoleLevel, byte mFileLogLevel)
		{
			Instance.SetConsoleLevel(mConsoleLevel);
			Instance.SetFileLogLevel(mFileLogLevel);
		}

		public static void WriteConsoleLine(DatabaseLogLevel type, string message, params object[] args)
		{
			Instance.ConsoleWriteLine(LogType.DatabaseLog, type, message, args);
		}

		public static void Write(DatabaseLogLevel type, string message, params object[] args)
		{
			Instance.Write(LogType.DatabaseLog, type, message, args);
		}

		public static void Write(Exception exception, string message, params object[] args)
		{
			Instance.WriteException(LogType.DatabaseLog, exception, DatabaseLogLevel.DatabaseClientError, message, args);
		}
	}
}
