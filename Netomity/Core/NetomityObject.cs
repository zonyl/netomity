﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    [DataContract]
    public abstract class NetomityObject : INetomityObject
    {
        public static List<NetomityObject> c_objects = null;
        public static List<Task> c_tasks = new List<Task>();
        static internal Logger c_logger = null;
        internal Logger _logger = null;
        private int _id = 0;

        public int Id
        {
            get
            {
                if (_id == 0)
                    _id = (Type.ToString() + Name + c_objects.Count().ToString()).GetHashCode();
                return _id;
            }
        }
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
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public NetomityObjectType Type { get; set; }

        public List<Task> Tasks
        {
            get
            {
                return c_tasks;
            }
        }

        public NetomityObject(Logger logger=null)
        {

            if (c_objects == null)
                c_objects = new List<NetomityObject>();

            if (c_tasks == null)
                c_tasks = new List<Task>();

            Type = NetomityObjectType.Unknown;
            c_objects.Add(this);

            if (logger != null)
                Logger = logger;
        }

        public void Log(Logger.Level level, string message)
        {
            LogString(level: level, message: message);
        }

        public void Log(Exception ex)
        {
            var message = String.Format("Exception Occured: {0}",
                    ex.ToString());

            if (ex.InnerException != null)
                message+= "==INNER==" + ex.InnerException.ToString();

            LogString(
                level: Core.Logger.Level.Error,
                message: message
                );
        }
       
        public void Log(string message)
        {
            LogString(
                level: Core.Logger.Level.Debug,
                message: message
                );
        }
        public void Log(string fmessage, params string[] args)
        {
            Log(String.Format(fmessage, args));
        }

        private void LogString(Core.Logger.Level level, string message)
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
                Logger.Log(Core.Logger.Level.Error, ex.ToString());
            }

        }

    }
}
