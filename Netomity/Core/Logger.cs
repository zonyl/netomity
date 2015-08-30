using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Netomity.Core
{
    public class Logger: NetomityObject
    {
        private Object _lock = new Object();

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

            var message = "Netomity Logger Initialized: " + LogPath;
            Console.WriteLine(message);
            Log(Level.Info, message);
        }

        public Boolean Log(Level level, string message, object obj=null)
        {
            try
            {
                var fmessage = String.Format(
                    "{0}|{1}|{2}",
                    DateTime.Now,
                    level.ToString(),
                    message
                );

                Console.WriteLine(fmessage);
                lock (_lock)
                {
                    using (var file = File.AppendText(LogPath))
                    {
                        file.WriteLine(fmessage);
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                Console.WriteLine("------Logger Error----");
                Console.WriteLine("Logger Error: " + ex.ToString());
            }
            return true;
        }
    }


}
