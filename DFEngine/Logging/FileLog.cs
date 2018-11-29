using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using DFEngine.Server;

namespace DFEngine.Logging
{
    public class FileLog : ConsoleLogger
    {
	    private readonly string BaseDirectory = "Logs";

        /// <summary>
        /// Returns the path where the logs will be saved.
        /// </summary>
        public string Directory { get; private set; }

		protected internal byte MaxFileLogLevel = byte.MaxValue;

        public FileLog(string directory)
        {
            Directory = Path.Combine(BaseDirectory, directory.ToEscapedString());

            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
        }

        public void SetFileLogLevel(byte logLevel) => MaxFileLogLevel = logLevel;

		public void Write(LogType logType, dynamic logSubType, string message, params object[] args)
		{
			try
            {
	            var callingProcess = Assembly.GetEntryAssembly().GetName().Name;
				var filePath = $"{Directory}{callingProcess}_{logType}_{DateTime.Now:MM_dd_yyyy_HH}.txt";
	            var msg = ($"[{DateTime.Now}][{LogTypeName}][{logSubType}] {string.Format(message, args)}");

	            if ((byte)logSubType <= ConsoleLogLevel)
	            {
		            ConsoleWriteLine(logType, logSubType, msg);
	            }


				using (var tw = TextWriter.Synchronized(File.AppendText(filePath)))
				{
					if ((byte)logSubType <= MaxFileLogLevel)
					{
						tw.WriteLine(msg);
					}
				}
			}
            catch (Exception ex)
            {
                ConsoleWriteLine(LogType.EngineLog, EngineLogLevel.Exception, $"Exception while writing log:\n {ex}");
            }
        }

        public void WriteException(LogType logType, Exception exception, dynamic logSubType, string commend, params object[] args)
        {
            Write(logType, logSubType, string.Format("{0}{1}{1}{1}{2}{1}{1}{3}{1}{1}{1}", commend, Environment.NewLine, exception.Message, exception.StackTrace), args);
        }
    }
}
