using System;

namespace DFEngine.Logging
{
    public class ConsoleLogger
    {
        protected internal byte ConsoleLogLevel = byte.MaxValue;

		protected virtual string LogTypeName { get; }

		internal object IOLocker;

        public ConsoleLogger()
        {
            IOLocker = new object();
        }

        public void SetConsoleLevel(byte logLevel)
        {
            ConsoleLogLevel = logLevel;
        }

		public void ConsoleWriteLine(LogType logTypeName, dynamic logSubtype, string message)
	    {
		    if ((byte)logSubtype > ConsoleLogLevel) return;
		    lock (IOLocker)
		    {
			    if (!ConsoleColors.GetColor(logTypeName, (byte)logSubtype, out var pColor)) return;
			    Console.ForegroundColor = pColor;
			    Console.WriteLine("\r" + message);
			    Console.ResetColor();
		    }
	    }

	    public void ConsoleWriteLine(LogType logTypeName, dynamic logSubtype, string message, params object[] args)
	    {
		    if ((byte)logSubtype > ConsoleLogLevel) return;
		    lock (IOLocker)
		    {
			    if (!ConsoleColors.GetColor(logTypeName, (byte)logSubtype, out var pColor)) return;
			    var msg = string.Format($"[{logTypeName}][{logSubtype}] {string.Format(message, args)}");

			    Console.ForegroundColor = pColor;
			    Console.WriteLine(msg);
			    Console.ResetColor();
		    }
	    }

		public void WriteConsoleProgressBar(string text, params object[] args)
        {
            lock (IOLocker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text, args);
                Console.ResetColor();
            }
        }
    }
}
