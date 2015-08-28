using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netomity.Core
{
    public abstract class NetomityObject : INetomityObject
    {
        static internal Logger c_logger = null;
        internal Logger _logger = null;
        public Logger Logger
        {
            get
            {
                return _logger != null ? _logger : c_logger;
            }
            set
            {
                if (value == null && c_logger != null)
                    _logger = c_logger;
                else if (value != null)
                    if (c_logger == null)
                        c_logger = value;
                    _logger = value;
            }
        }

        public string Name { get; set; }

        public NetomityObject(Logger logger=null)
        {
            if (logger != null)
                Logger = logger;
            else
                Logger = c_logger;
        }

        public void Log(Logger.Level level, string message)
        {
            try
            {
                var fMessage = String.Format(
                    "{0}:{1}",
                    Name,
                    message);
                if (Logger != null)
                    Logger.Log(level, fMessage);
                else
                {
                    Console.WriteLine(level.ToString() + fMessage);
                }
            }
            catch (System.Exception ex)
            {
                var a = 0;
            }
        }

    }
}
