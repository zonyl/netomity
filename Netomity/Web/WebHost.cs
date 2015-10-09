using Microsoft.Owin.Hosting;
using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Web
{
    public class WebHost: NetomityObject
    {
        // netsh http add urlacl url=http://+:8082/ user=Everyone
        public WebHost(string address)
        {
            var options = new StartOptions(address);
            //options.Urls.Add("http://localhost:8083/");
            //options.Urls.Add("http://127.0.0.1:8083/");
            //options.Port = 8083;
            Log(address);
            options.Settings.Add("RouteDebugger:Enabled", "true");
//            WebApp.Start<WebConfig>(url: address);
            WebApp.Start<WebConfig>(options);
        }
    }
}
