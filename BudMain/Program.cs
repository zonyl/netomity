using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netomity.Core;
using Netomity.Devices;
using Netomity.Interfaces.HA;
using Netomity.Interfaces.Basic;
using Netomity.Web;

namespace BudMain
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BASE_ADDR = "http://localhost:8081/Netomity/API/v1";


            var logger = new Logger(@"C:\temp\netomity-log.txt");

            var ns = NetomitySystem.Factory();

            var tp = new TCPClient(address: "192.168.12.161", port: 3333);
            var plm = new Insteon(iface: tp);
            var test_tamp = new StateDevice(address: "00.5B.5d", iface: plm)
            {
                Name = "TestLamp1"
            };

            var rh = new RestHost(address: BASE_ADDR);

            ns.Run();

        }
    }
}
