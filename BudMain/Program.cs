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
            //const string BASE_ADDR = "http://localhost:8081/Netomity/API/v1";
            const string BASE_ADDR = "http://localhost:8083/";
            const string BASE_FOLDER = @"C:\projects\Netomity\Netomity\Web\Content\";


            var logger = new Logger(@"C:\temp\netomity-log.txt");

            var ns = NetomitySystem.Factory();

            var tp = new TCPClient(address: "192.168.12.161", port: 3333);
            var plm = new Insteon(iface: tp);

            var test_lamp = new StateDevice(address: "00.5B.5d", iface: plm)
            {
                Name = "TestLamp1"
            };

            var master_fan = new StateDevice(address: "1f.ad.76", iface: plm)
            {
                Name = "Master Fan"
            };

            var master_light = new StateDevice(address: "38.2e.a2", iface: plm)
            {
                Name = "Master Light"
            };

            var Bedtime = new Scene(devices: new List<StateDevice>() { test_lamp })
                {
                    Name = "Bedtime"
                };

            //var rh = new RestHost(address: BASE_ADDR);
            var wh = new WebHost(address: BASE_ADDR, filePath: BASE_FOLDER);
            ns.Run();

        }
    }
}
