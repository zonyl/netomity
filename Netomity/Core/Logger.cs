using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Netomity.Core
{
    public class Logger: NetomityObject
    {
        private const string DEFAULT_LOG_PATHNAME = "NetomityLog.txt";

        public enum Level
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public string LogPath { get; set; }

        public Logger(string logFilePath = DEFAULT_LOG_PATHNAME)
        {
            CreateLogStream(logFilePath);
        }
        public Logger()
        {
            CreateLogStream(DEFAULT_LOG_PATHNAME);
        }
        private void CreateLogStream(string pathname)
        {
            if (pathname != null)
                LogPath = pathname;
            else
                LogPath = DEFAULT_LOG_PATHNAME;

            //_logger = this;
            Logger = this;
        }

        public Boolean Log(Level level, string message, object obj=null)
        {
            var lockObj = new Object();
            lock (lockObj)
            {
                using (var file = File.AppendText(LogPath))
                {
                    file.WriteLine(String.Format("{0}|{1}|{2}",
                        DateTime.Now,
                        level.ToString(),
                        message)
                    );
                    file.Close();
                }
            }
            return true;
        }
    }


}
