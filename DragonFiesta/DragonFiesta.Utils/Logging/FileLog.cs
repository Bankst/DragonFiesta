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

        protected internal byte mFileLogLevel = byte.MaxValue;

        private ConcurrentDictionary<string, StreamWriter> Writers;

        public FileLog(string Directory) : base()
        {
            this.Directory = Directory.ToEscapedString();

            if (!System.IO.Directory.Exists(this.Directory))
                System.IO.Directory.CreateDirectory(this.Directory);

            LoadSession();
            Writers = new ConcurrentDictionary<string, StreamWriter>();
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

                    if (uint.TryParse(split[0], out uint ID)
                        && ID >= SessionID)
                    {
                        SessionID = (ID + 1);
                    }
                }
            }

            SessionTime = DateTime.Now;
        }

        public void SetFileLogLevel(byte LogLevel)
        {
            mFileLogLevel = LogLevel;
        }

        public void Write(string LogName, dynamic LogType, string Message, params object[] args)
        {
            try
            {
                lock (IOLocker)
                {
                    if (!Writers.TryGetValue(LogName.ToLower(), out StreamWriter writer))
                    {
                        writer = new StreamWriter(String.Format("{0}{1}_{2}_{3}.txt", Directory, SessionID, LogName, SessionTime.ToString("MM_dd_yyyy"))) { AutoFlush = true };
                        Writers.TryAdd(LogName.ToLower(), writer);
                    }

                    string msg = (String.Format("[{0}][{1}][{2}] {3}", DateTime.Now, LogTypeName, LogType, String.Format(Message, args)));

                    if ((byte)LogType <= mFileLogLevel)
                    {
                        writer.WriteLine(msg);
                    }

                    if ((byte)LogType <= mConsoleLogLevel)
                    {
                        ConsoleWriteLine(LogName, LogType, msg);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void WriteException(Exception Exception, dynamic LogType, string Commend, params object[] args)
        {
            Write(LogTypeName, LogType, String.Format("{0}{1}{1}{1}{2}{1}{1}{3}{1}{1}{1}", Commend, Environment.NewLine, Exception.Message, Exception.StackTrace), args);
        }
    }
}