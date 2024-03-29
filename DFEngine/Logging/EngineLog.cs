﻿using System;

namespace DFEngine.Logging
{
    public sealed class EngineLog : FileLog
    {
        protected override LogType LogTypeName => LogType.EngineLog;

        private EngineLog(string directory) : base(directory)
        {
        }

        private static EngineLog Instance => (_instance ?? (_instance = new EngineLog(@"Engine")));
        private static EngineLog _instance;

        public static void SetupLevels(byte mConsoleLevel, byte mFileLogLevel)
        {
            Instance.SetConsoleLevel(mConsoleLevel);
            Instance.SetFileLogLevel(mFileLogLevel);
        }

        public static void WriteConsoleLine(EngineLogLevel type, string message, params object[] args)
        {
            Instance.ConsoleWriteLine(LogType.EngineLog, type, message, args);
        }

        public static void Write(EngineLogLevel type, string message, params object[] args)
        {
            Instance.Write(LogType.EngineLog, type, message, args);
        }

        public static void Write(Exception exception, string message, params object[] args)
        {
            Instance.WriteException(exception, EngineLogLevel.Exception, message, args);
        }
    }
}
