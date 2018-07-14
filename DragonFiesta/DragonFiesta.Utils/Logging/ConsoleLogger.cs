using System;

namespace DragonFiesta.Utils.Logging
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
                        string msg = (String.Format("[{0}][{1}] {2}", LogTypeName, LogType, String.Format(Message, args)));

                        Console.ForegroundColor = pColor;
                        Console.WriteLine(msg);

                        Console.ResetColor();
                    }
                }
            }
        }

        //Use as Header for bar :)
        public void WriteConsoleProgressBar(string Text, params object[] args)
        {
            lock (IOLocker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                String ma = string.Format("{0} {1} \n", Text, args);
                Console.WriteLine(Text, args);
                Console.ResetColor();
            }
        }
    }
}