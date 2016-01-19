using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Devices;
using Netomity.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Netomity.Web
{
    [RoutePrefix("net")]
    public class RestController : ApiController
    {
        [Route("values")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("objects/{type}")]
        [HttpGet]
        public IHttpActionResult NetomityObjects(string type)
        {

            NetomityObjectType not = Conversions.ValueToStringEnum<NetomityObjectType>(type);

            var objs = NetomitySystem.Factory().NetomityObjects.Where(o => o.Type == not).ToList();

//            return _JSONResponse<List<NetomityObject>>(true, objs);
            return Json(objs);
        }

        [Route("object/{id}/{property}/{primary}/{secondary?}")]
        [HttpGet]
        public IHttpActionResult SetNetomityObjectProperty(string id, string property, string primary, string secondary=null)
        {

            Int32 iid = 0;
            IQueryable<NetomityObject> nsojbs = null;
            List<NetomityObject> objs = null;
            NetomityObject obj = null;
            bool status = false;

            nsojbs = NetomitySystem.Factory().NetomityObjects;
            //            var objs = c_objects.Where(o =>).ToList();
            if (Int32.TryParse(id, out iid))
                objs = nsojbs.Where(o => o.Id == iid).ToList();

            if (iid == 0 || objs.Count() == 0)
                objs = nsojbs.Where(o => o.Name != null && o.Name.ToLower() == id.ToLower()).ToList();


            if (objs.Count() > 0)
            {
                obj = objs.First();
                var sd = (StateDevice)obj;
                switch (property.ToLower())
                {
                    case "command":
                        sd.Command(Conversions.ValueToStringEnum<CommandType>(primary), secondary);
                        status = true;
                        break;
                    case "state":
                        var state = new State()
                        {
                            Primary = Conversions.ValueToStringEnum<StateType>(primary),
                            Secondary = secondary
                        };
                        sd.State = state;
                        status = true;
                        break;

                }


            }

            //return _JSONResponse<NetomityObject>(status, obj);
            return Json(obj);

        }

        private string _JSONResponse<T>(bool status, T obj)
        {
            var rObj = new RestResponse()
            {
                status = RestResponseStatusType.Success,
                data = obj
            };

            string response = "[ ";
            response = JsonConvert.SerializeObject(rObj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            response = response.Replace("\r\n", "");

            return response;
        }

    }
}
