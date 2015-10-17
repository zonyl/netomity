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
using System.Net.Http.Formatting;
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
        const string HTTP_CONFIG = "MS_HttpConfiguration";
        const string BASE_FILE_PATH_KEY = "BaseFilePath";
        string _baseFolder = "";

        [Route("")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            foreach (var kp in Request.GetOwinContext().Environment)
            {
                var a = 0;
            }
            var objs = NetomitySystem.Factory().NetomityObjects;

            return RenderPage<IQueryable<NetomityObject>>(BaseFolder + "index.html", objs);
        }


        [Route("devices")]
        [AcceptVerbs("get","post")]
        public HttpResponseMessage Devices(FormDataCollection formData)
        {
            var allDevices = NetomitySystem.Factory().NetomityObjects.Where(o => o.Type == NetomityObjectType.Device).Cast<StateDevice>();
            if (formData!= null && formData.Count() > 0)
            {
                var fDevice = formData.FirstOrDefault();
                var device = allDevices.Where(d => d.Id.ToString() == formData.First().Key).FirstOrDefault();
                if (device != null)
                    device.Command(fDevice.Value);
            }


//            return _JSONResponse<List<NetomityObject>>(true, objs);
            return RenderPage<IEnumerable<StateDevice>>(BaseFolder + "devicecontrol.html", allDevices);
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

        public string BaseFolder
        {
            get
            {
                if (Request != null)
                {
                    HttpConfiguration http = (HttpConfiguration)Request.Properties[HTTP_CONFIG];
                    _baseFolder = http.Properties[BASE_FILE_PATH_KEY].ToString();
                }
                return _baseFolder;
            }
            set
            {
                _baseFolder = value;
            }
        }
    }
}
