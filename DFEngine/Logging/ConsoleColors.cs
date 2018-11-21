using System;
using System.Collections.Generic;

namespace DFEngine.Logging
{
    public static class ConsoleColors
    {
        private static Dictionary<byte, ConsoleColor> EngineColors = new Dictionary<byte, ConsoleColor>
        {
            { (byte)EngineLogLevel.Debug,ConsoleColor.Magenta },
            { (byte)EngineLogLevel.Info,ConsoleColor.DarkGreen },
            { (byte)EngineLogLevel.Startup,ConsoleColor.Green },
            { (byte)EngineLogLevel.Warning,ConsoleColor.Blue },
            { (byte)EngineLogLevel.Exception,ConsoleColor.Red },
        };

 

        public static bool GetColor(string LogType, byte LogLevel, out ConsoleColor pColor)
        {
            switch (LogType)
            {
                case "EngineLog":
                    return EngineColors.TryGetValue(LogLevel, out pColor);

                default:
                    pColor = ConsoleColor.White;
                    return true;
            }
        }
    }
}
