using System;
using System.Collections.Concurrent;
using System.IO;

namespace DragonFiesta.Utils.Logging
{
    public class FileLog : ConsoleLogger
    {
        /// <summary>
        /// Returns the path where the logs will be saved.
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Returns the ID of the current log session.
        /// </summary>
        public uint SessionID { get; private set; }

        /// <summary>
        /// Returns the time when this log session was started.
        /// </summary>
        public DateTime SessionTime { get; private set; }

        protected internal byte MFileLogLevel = byte.MaxValue;

        private ConcurrentDictionary<string, StreamWriter> _writers;

        public FileLog(string directory) : base()
        {
            this.Directory = directory.ToEscapedString();

            if (!System.IO.Directory.Exists(this.Directory))
                System.IO.Directory.CreateDirectory(this.Directory);

            LoadSession();
            _writers = new ConcurrentDictionary<string, StreamWriter>();
            IOLocker = new object();
        }

        private void LoadSession()
        {
            foreach (var file in System.IO.Directory.GetFiles(Directory, "*.txt", SearchOption.TopDirectoryOnly))
            {
                var withoutEx = Path.GetFileNameWithoutExtension(file);

                if (withoutEx.Contains("_"))
                {
                    var split = withoutEx.Split('_');

                    if (uint.TryParse(split[0], out uint id)
                        && id >= SessionID)
                    {
                        SessionID = (id + 1);
                    }
                }
            }

            SessionTime = DateTime.Now;
        }

		public void SetFileLogLevel(byte logLevel) => MFileLogLevel = logLevel;

		public void Write(string logName, dynamic logType, string message, params object[] args)
        {
            try
            {
                lock (IOLocker)
                {
                    if (!_writers.TryGetValue(logName.ToLower(), out StreamWriter writer))
                    {
                        writer = new StreamWriter(
	                        $"{Directory}{SessionID}_{logName}_{SessionTime:MM_dd_yyyy}.txt") { AutoFlush = true };
                        _writers.TryAdd(logName.ToLower(), writer);
                    }

                    var msg = ($"[{DateTime.Now}][{LogTypeName}][{logType}] {string.Format(message, args)}");

                    if ((byte)logType <= MFileLogLevel)
                    {
                        writer.WriteLine(msg);
                    }

                    if ((byte)logType <= mConsoleLogLevel)
                    {
                        ConsoleWriteLine(logName, logType, msg);
                    }
                }
            }
	        catch (Exception)
	        {
		        // ignored
	        }
        }

        public void WriteException(Exception exception, dynamic logType, string commend, params object[] args)
        {
            Write(LogTypeName, logType, String.Format("{0}{1}{1}{1}{2}{1}{1}{3}{1}{1}{1}", commend, Environment.NewLine, exception.Message, exception.StackTrace), args);
        }
    }
}