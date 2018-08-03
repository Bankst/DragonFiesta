using DragonFiesta.Utils.Logging;
using System;

public sealed class DataLog : FileLog
{
    protected override string LogTypeName => "DataLog";

    private DataLog(string Directory)
  : base(Directory)
    {
    }

    private static DataLog Instance { get { return (_Instance ?? (_Instance = new DataLog("Data"))); } }
    private static DataLog _Instance;

    public static void SetupLevels(byte mConsolenLevel, byte mFileLogLevel)
    {
        Instance.SetConsoleLevel(mConsolenLevel);
        Instance.SetFileLogLevel(mFileLogLevel);
    }

    public static void WriteProgressBar(string Message, params object[] args)
    {
        Instance.WriteConsoleProgressBar(Message, args);
    }

    public static void WriteConsoleLine(DataLogLevel Type, string Message, params object[] args)
    {
        Instance.ConsoleWriteLine(Instance.ToString(), Type, Message, args);
    }

    public static void Write(DataLogLevel Type, string Message, params object[] args)
    {
        Instance.Write(Instance.ToString(), Type, Message, args);
    }
}