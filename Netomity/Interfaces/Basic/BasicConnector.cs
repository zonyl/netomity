using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.Basic
{
    public class BasicConnector: BasicInterface
    {
        public override event DataReceivedHandler DataReceived;
        private List<BasicInterface> _interfaces = null;
        Task IntTask { get; set; }
        List<Task> IntTasks { get; set; }

        public BasicConnector(List<BasicInterface> interfaces, Logger logger = null)
        {
            Initialize();
            _interfaces = interfaces;
            interfaces.ForEach(
                original => interfaces.Where(target => target != original)
                    .ToList()
                    .ForEach(target => Connect(original, target)
                    )
                );

            Logger = logger;

        }
        public BasicConnector(BasicInterface interfaceA, BasicInterface interfaceB)
        {
            Initialize();
            if (_interfaces == null)
                _interfaces = new List<BasicInterface>();

            Connect(interfaceA, interfaceB);
            _interfaces.Add(interfaceA);
            _interfaces.Add(interfaceB);
        }

        public void Initialize()
        {
            IntTasks = new List<Task>();
        }

        public void Connect(BasicInterface interfaceA, BasicInterface interfaceB)
        {
            Log(Core.Logger.Level.Debug, String.Format("Connect {0} - {1}",
                interfaceA.Name,
                interfaceB.Name));
            interfaceA.DataReceived += interfaceB.Send;
//            interfaceB.DataReceived += interfaceA.Send;
            interfaceA.DataReceived += _DataReceived;
//            interfaceB.DataReceived += _DataReceived;

        }

        public override Task Open()
        {
            var task = Task.Factory.StartNew(() => 
                {
                    _interfaces.ForEach(i =>
                        {
                            IntTasks.Add(i.Open());
                            Log(Logger.Level.Debug,
                                String.Format("Opening:{0} - {1}",
                                i.Name,
                                i));
                        }
                    );

                    Task.WaitAll(IntTasks.ToArray());
                }
            );

            return task;

        }

        private void _DataReceived(string data)
        {
            Log(Core.Logger.Level.Debug, "data received");
            if (DataReceived != null)
                DataReceived(data);
        }

        public override void Send(string data)
        {
            Log(Core.Logger.Level.Debug, "data to send");
            _interfaces.ForEach(i => i.Send(data));
        }

        public override Boolean IsOpen
        {
            get
            {
                Boolean isOpen = true;
                _interfaces.Where(i => i.IsOpen == false).ToList().ForEach(i => isOpen = false);
                return isOpen;
            }
        }

        public List<BasicInterface> Interfaces
        {
            get
            {
                return _interfaces;
            }
        }
    }
}
