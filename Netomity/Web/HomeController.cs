using Netomity.Core;
using Netomity.Devices;
using Netomity.Utility;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Netomity.Web
{
    [RoutePrefix("home")]
    public class HomeController : ApiController
    {
        const string BASE_FOLDER = @"C:\projects\Netomity\Netomity\Web\Content\";
        [Route("")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var objs = NetomitySystem.Factory().NetomityObjects;

            return RenderPage<IQueryable<NetomityObject>>(BASE_FOLDER + "index.html", objs);
        }


        [Route("devices")]
        [HttpGet]
        public HttpResponseMessage NetomityObjects2()
        {

            var objs = NetomitySystem.Factory().NetomityObjects.Where(o => o.Type == NetomityObjectType.Device).Cast<StateDevice>();

//            return _JSONResponse<List<NetomityObject>>(true, objs);
            return RenderPage<IEnumerable<StateDevice>>(BASE_FOLDER + "devicecontrol.html", objs);
        }

        private HttpResponseMessage StringToHTMLResponse(string message)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(message);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        private string RazorToString<T>(string templateText, T model)
        {
            string templateKeyName = Guid.NewGuid().ToString();
            string myParsedTemplate = Engine.Razor.RunCompile(templateText, templateKeyName, typeof(T), model);

            return myParsedTemplate;
        }

        private HttpResponseMessage RenderPage<T>(string filePath, T model)
        {
            string text = System.IO.File.ReadAllText(filePath);
            return StringToHTMLResponse(RazorToString<T>(text, model));
        }
    }
}
