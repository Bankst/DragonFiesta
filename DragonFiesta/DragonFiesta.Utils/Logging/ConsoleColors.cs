using System;
using System.Collections.Generic;

namespace DragonFiesta.Utils.Logging
{
    public static class ConsoleColors
    {
        private static Dictionary<byte, ConsoleColor> GameColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)GameLogLevel.Debug,ConsoleColor.Magenta },
            { (byte)GameLogLevel.Startup,ConsoleColor.Green },
            { (byte)GameLogLevel.Internal,ConsoleColor.Cyan},
            { (byte)GameLogLevel.Warning,ConsoleColor.Yellow },
            { (byte)GameLogLevel.Exception,ConsoleColor.Red  },
        };

        private static Dictionary<byte, ConsoleColor> DatabaseColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)DatabaseLogLevel.Debug,ConsoleColor.DarkYellow },
            { (byte)DatabaseLogLevel.Startup,ConsoleColor.Green },
            { (byte)DatabaseLogLevel.Warning,ConsoleColor.Yellow },
            { (byte)DatabaseLogLevel.Error,ConsoleColor.DarkRed },
            { (byte)DatabaseLogLevel.DatabaseClientError,ConsoleColor.Red  },
        };

        private static Dictionary<byte, ConsoleColor> SocketColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)SocketLogLevel.Debug,ConsoleColor.Blue },
            { (byte)SocketLogLevel.Startup,ConsoleColor.Blue },
            { (byte)SocketLogLevel.Warning,ConsoleColor.Blue },
            { (byte)SocketLogLevel.Exception,ConsoleColor.DarkBlue },
        };

        private static Dictionary<byte, ConsoleColor> EngineColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)EngineLogLevel.Debug,ConsoleColor.Magenta },
            { (byte)EngineLogLevel.Info,ConsoleColor.DarkGreen },
            { (byte)EngineLogLevel.Startup,ConsoleColor.Green },
            { (byte)EngineLogLevel.Warning,ConsoleColor.Blue },
            { (byte)EngineLogLevel.Exception,ConsoleColor.Red },
        };

        private static Dictionary<byte, ConsoleColor> CommandColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)CommandLogLevel.Error,ConsoleColor.Blue },
            { (byte)CommandLogLevel.Execute,ConsoleColor.DarkBlue },
            { (byte)CommandLogLevel.InvalidAccess,ConsoleColor.Red },
            { (byte)CommandLogLevel.InvalidParameters,ConsoleColor.DarkRed },
        };

        public static bool GetColor(string LogType, byte LogLevel, out ConsoleColor pColor)
        {
            switch (LogType)
            {
                case "CommandLog":
                    return CommandColors.TryGetValue(LogLevel, out pColor);

                case "DatabaseLog":
                    return DatabaseColors.TryGetValue(LogLevel, out pColor);

                case "SocketLog":
                    return SocketColors.TryGetValue(LogLevel, out pColor);

                case "GameLog":
                    return GameColors.TryGetValue(LogLevel, out pColor);

                case "EngineLog":
                    return EngineColors.TryGetValue(LogLevel, out pColor);

                default:
                    pColor = ConsoleColor.White;
                    return true;
            }
        }
    }
}