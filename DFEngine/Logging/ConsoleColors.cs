using System;
using System.Collections.Generic;

namespace DFEngine.Logging
{
	public static class ConsoleColors
	{
		private static readonly Dictionary<byte, ConsoleColor> GameColors = new Dictionary<byte, ConsoleColor>
		{
			{(byte) GameLogLevel.Debug, ConsoleColor.Magenta},
			{(byte) GameLogLevel.Startup, ConsoleColor.Green},
			{(byte) GameLogLevel.Internal, ConsoleColor.Cyan},
			{(byte) GameLogLevel.Warning, ConsoleColor.Yellow},
			{(byte) GameLogLevel.Exception, ConsoleColor.Red},
		};

		private static readonly Dictionary<byte, ConsoleColor> DatabaseColors = new Dictionary<byte, ConsoleColor>
		{
			{(byte) DatabaseLogLevel.Debug, ConsoleColor.DarkYellow},
			{(byte) DatabaseLogLevel.Startup, ConsoleColor.Green},
			{(byte) DatabaseLogLevel.Warning, ConsoleColor.Yellow},
			{(byte) DatabaseLogLevel.Error, ConsoleColor.DarkRed},
			{(byte) DatabaseLogLevel.DatabaseClientError, ConsoleColor.Red},
		};

		private static readonly Dictionary<byte, ConsoleColor> SocketColors = new Dictionary<byte, ConsoleColor>
		{
			{(byte) SocketLogLevel.Debug, ConsoleColor.Blue},
			{(byte) SocketLogLevel.Startup, ConsoleColor.Blue},
			{(byte) SocketLogLevel.Warning, ConsoleColor.Blue},
			{(byte) SocketLogLevel.Exception, ConsoleColor.DarkBlue},
		};

		private static readonly Dictionary<byte, ConsoleColor> EngineColors = new Dictionary<byte, ConsoleColor>
		{
			{(byte) EngineLogLevel.Debug, ConsoleColor.Magenta},
			{(byte) EngineLogLevel.Info, ConsoleColor.DarkGreen},
			{(byte) EngineLogLevel.Startup, ConsoleColor.Green},
			{(byte) EngineLogLevel.Warning, ConsoleColor.Blue},
			{(byte) EngineLogLevel.Exception, ConsoleColor.Red},
		};

		private static readonly Dictionary<byte, ConsoleColor> CommandColors = new Dictionary<byte, ConsoleColor>
		{
			{(byte) CommandLogLevel.Error, ConsoleColor.Blue},
			{(byte) CommandLogLevel.Execute, ConsoleColor.DarkBlue},
			{(byte) CommandLogLevel.InvalidAccess, ConsoleColor.Red},
			{(byte) CommandLogLevel.InvalidParameters, ConsoleColor.DarkRed},
		};

		public static bool GetColor(LogType logType, byte logLevel, out ConsoleColor pColor)
		{
			switch (logType)
			{
				case LogType.EngineLog:
					return EngineColors.TryGetValue(logLevel, out pColor);
				case LogType.CommandLog:
					return CommandColors.TryGetValue(logLevel, out pColor);
				case LogType.DatabaseLog:
					return DatabaseColors.TryGetValue(logLevel, out pColor);
				case LogType.GameLog:
					return GameColors.TryGetValue(logLevel, out pColor);
				case LogType.SocketLog:
					return SocketColors.TryGetValue(logLevel, out pColor);
				default:
					pColor = ConsoleColor.White;
					return true;
			}
		}
	}
}
