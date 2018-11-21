using System;

namespace DFEngine.Logging
{
    public class ConsoleLogger
    {
        protected internal byte mConsoleLogLevel = byte.MaxValue;

        protected virtual string LogTypeName { get; }

        internal object IOLocker;

        public ConsoleLogger()
        {
            IOLocker = new object();
        }

        public void SetConsoleLevel(byte LogLevel)
        {
            mConsoleLogLevel = LogLevel;
        }

        public void ConsoleWriteLine(string LogTypeName, dynamic LogType, string Message)
        {
            if ((byte)LogType <= mConsoleLogLevel)
            {
                lock (IOLocker)
                {
                    if (ConsoleColors.GetColor(LogTypeName, (byte)LogType, out ConsoleColor pColor))
                    {
                        Console.ForegroundColor = pColor;
                        Console.WriteLine("\r" + Message);
                        Console.ResetColor();
                    }
                }
            }
        }

        public void ConsoleWriteLine(string LogTypeName, dynamic LogType, string Message, params object[] args)
        {
            if ((byte)LogType <= mConsoleLogLevel)
            {
                lock (IOLocker)
                {
                    if (ConsoleColors.GetColor(LogTypeName, (byte)LogType, out ConsoleColor pColor))
                    {
                        string msg = string.Format($"[{LogTypeName}][{LogType}] {string.Format(Message, args)}");

                        Console.ForegroundColor = pColor;
                        Console.WriteLine(msg);
                        Console.ResetColor();
                    }
                }
            }
        }

        public void WriteConsoleProgressBar(string Text, params object[] args)
        {
            lock (IOLocker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string ma = string.Format($"{Text} {args} \n");
                Console.WriteLine(Text, args);
                Console.ResetColor();
            }
        }
    }
}
