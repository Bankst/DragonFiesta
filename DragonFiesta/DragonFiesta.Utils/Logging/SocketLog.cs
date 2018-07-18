using DragonFiesta.Utils.Logging;
using System;

public sealed class SocketLog : FileLog
{
    protected override string LogTypeName => "SocketLog";

    private SocketLog(string Directory)
  : base(Directory)
    {
    }

    private static SocketLog Instance { get { return (_Instance ?? (_Instance = new SocketLog(@"Socket"))); } }
    private static SocketLog _Instance;

    public static void SetupLevels(byte mConsolenLevel, byte mFileLogLevel)
    {
        Instance.SetConsoleLevel(mConsolenLevel);
        Instance.SetFileLogLevel(mFileLogLevel);
    }

    public static void WriteConsoleLine(SocketLogLevel Type, string Message, params object[] args)
    {
        Instance.ConsoleWriteLine(Instance.ToString(), Type, Message, args);
    }

    public static void Write(SocketLogLevel Type, string Message, params object[] args)
    {
        Instance.Write(Instance.ToString(), Type, Message, args);
    }

    public static void Write(Exception Exception, string Message, params object[] args)
    {
        Instance.WriteException(Exception, SocketLogLevel.Exception, Message, args);
    }
}