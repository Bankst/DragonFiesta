using System;
using System.Collections.Concurrent;
using System.IO;

namespace DFEngine.Logging
{
    public class FileLog : ConsoleLogger
    {
	    private readonly string BaseDirectory = "Logs";

        /// <summary>
        /// Returns the path where the logs will be saved.
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Returns the ID of the current log session.
        /// </summary>
        public uint SessionId { get; private set; }

        /// <summary>
        /// Returns the time when this log session was started.
        /// </summary>
        public DateTime SessionTime { get; private set; }

        protected internal byte MaxFileLogLevel = byte.MaxValue;

		public FileLog(string directory)
		{
            Directory = Path.Combine(BaseDirectory, directory.ToEscapedString());

            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }

            LoadSession();
            IOLocker = new object();
        }

        private void LoadSession()
        {
            foreach (var file in System.IO.Directory.GetFiles(Directory, "*.txt", SearchOption.TopDirectoryOnly))
            {
                var withoutEx = Path.GetFileNameWithoutExtension(file);

	            if (!withoutEx.Contains("_")) continue;
	            var split = withoutEx.Split('_');

	            if (uint.TryParse(split[0], out var id) && id >= SessionId)
	            {
		            SessionId = (id + 1);
	            }
            }

            SessionTime = DateTime.Now;
        }

        public void SetFileLogLevel(byte logLevel) => MaxFileLogLevel = logLevel;

        public void Write(LogType logType, dynamic logSubType, string message, params object[] args)
        {
            try
            {
	            lock (IOLocker)
	            {
		            var filePath = $"{Directory}{SessionId}_{logType}_{SessionTime:MM_dd_yyyy}.txt";
		            var msg = $"[{DateTime.Now}][{LogTypeName.ToString()}][{logSubType}] {string.Format(message, args)}";

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
            }
            catch (Exception ex)
            {
                ConsoleWriteLine(LogType.EngineLog, EngineLogLevel.Exception, $"Exception while writing log:\n {ex}");
            }
        }

        public void WriteException(Exception exception, dynamic logType, string commend, params object[] args)
        {
            Write(LogTypeName, logType, string.Format("{0}{1}{1}{1}{2}{1}{1}{3}{1}{1}{1}", commend, Environment.NewLine, exception.Message, exception.StackTrace), args);
        }
    }
}
