using DragonFiesta.Utils.Logging;
using System;

public class CommandLog : FileLog
{
    protected override string LogTypeName => "CommandLog";

    private CommandLog(string directory)
     : base(directory)
    {
    }

    private static CommandLog Instance => (_instance ?? (_instance = new CommandLog(@"CommandLog")));
	private static CommandLog _instance;

    public static void SetupLevels(byte mConsoleLevel, byte mFileLogLevel)
    {
        Instance.SetConsoleLevel(mConsoleLevel);
        Instance.SetFileLogLevel(mFileLogLevel);
    }

    public static void WriteConsoleLine(CommandLogLevel type, string message, params object[] args)
    {
        Instance.ConsoleWriteLine(Instance.ToString(), type, message, args);
    }

    public static void Write(CommandLogLevel type, string message, params object[] args)
    {
        Instance.Write(Instance.ToString(), type, message, args);
    }

    public static void Write(Exception exception, string message, params object[] args)
    {
        Instance.WriteException(exception, CommandLogLevel.Error, message, args);
    }
}