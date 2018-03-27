using DragonFiesta.Utils.Logging;
using System;

public class CommandLog : FileLog
{
    protected override string LogTypeName => "CommandLog";

    private CommandLog(string Directory)
     : base(Directory)
    {
    }

    private static CommandLog Instance { get { return (_Instance ?? (_Instance = new CommandLog(@"CommandLog"))); } }
    private static CommandLog _Instance;

    public static void SetupLevels(byte mConsolenLevel, byte mFileLogLevel)
    {
        Instance.SetConsolenLevel(mConsolenLevel);
        Instance.SetFileLogLevel(mFileLogLevel);
    }

    public static void WriteConsoleLine(CommandLogLevel Type, string Message, params object[] args)
    {
        Instance.ConsoleWriteLine(Instance.ToString(), Type, Message, args);
    }

    public static void Write(CommandLogLevel Type, string Message, params object[] args)
    {
        Instance.Write(Instance.ToString(), Type, Message, args);
    }

    public static void Write(Exception Exception, string Message, params object[] args)
    {
        Instance.WriteException(Exception, CommandLogLevel.Error, Message, args);
    }
}