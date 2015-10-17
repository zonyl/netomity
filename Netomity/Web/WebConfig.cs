using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Netomity.Web
{
    public class WebConfig
    {
        Type RestControllerType = typeof(Netomity.Web.RestController);
        private const string BASE_FILE_PATH_KEY = "BaseFilePath";
        public static string BaseFilePath;

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            ThreadPool.SetMinThreads(50, 4);

            //config.Routes.MapHttpRoute("css", "css/{name}", new { controller = "Stylesheets" });
            //config.Routes.MapHttpRoute("js", "js/{name}", new { controller = "Javascript" });
            //config.Routes.MapHttpRoute("img", "img/{name}", new { controller = "Images" });
            //config.Routes.MapHttpRoute("defaultext", "{controller}.{ext}", new { ext = RouteParameter.Optional });
            //config.Routes.MapHttpRoute("default", "{controller}", new { controller = "Home" });

            //config.MapHttpAttributeRoutes();
            config.MapHttpAttributeRoutes();
            config.Properties[BASE_FILE_PATH_KEY] = BaseFilePath;
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            

            appBuilder.UseWebApi(config);
        } 
    }
}
