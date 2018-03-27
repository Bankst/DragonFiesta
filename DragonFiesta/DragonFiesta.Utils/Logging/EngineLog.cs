using DragonFiesta.Utils.Logging;
using System;

public sealed class EngineLog : FileLog
{
    protected override string LogTypeName => "EngineLog";

    private EngineLog(string Directory)
  : base(Directory)
    {
    }

    private static EngineLog Instance { get { return (_Instance ?? (_Instance = new EngineLog(@"Engine"))); } }
    private static EngineLog _Instance;

    public static void SetupLevels(byte mConsolenLevel, byte mFileLogLevel)
    {
        Instance.SetConsolenLevel(mConsolenLevel);
        Instance.SetFileLogLevel(mFileLogLevel);
    }

    public static void WriteConsoleLine(EngineLogLevel Type, string Message, params object[] args)
    {
        Instance.ConsoleWriteLine(Instance.ToString(), Type, Message, args);
    }

    public static void Write(EngineLogLevel Type, string Message, params object[] args)
    {
        Instance.Write(Instance.ToString(), Type, Message, args);
    }

    public static void Write(Exception Exception, string Message, params object[] args)
    {
        Instance.WriteException(Exception, EngineLogLevel.Exception, Message, args);
    }
}