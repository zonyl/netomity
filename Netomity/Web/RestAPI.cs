using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Devices;
using Netomity.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Web
{
    [ServiceContract()]
    public class RestAPI: NetomityObject
    {
        [WebGet(UriTemplate="/objects/{type}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        public string NetomityObjects(string type)
        {
            NetomityObjectType not = Conversions.ValueToStringEnum<NetomityObjectType>(type);
           
            var objs = c_objects.Where(o => o.Type == not).ToList();

            return _JSONResponse<List<NetomityObject>>(true, objs);
        }

        [WebGet(UriTemplate = "/object/{id}/{property}/{primary}/{secondary=default}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        public string SetNetomityObjectProperty(string id, string property, string primary, string secondary)
        {
            Int32 iid = 0;
            List<NetomityObject> objs = null;
            NetomityObject obj = null;
            bool status = false;

            //            var objs = c_objects.Where(o =>).ToList();
            if (Int32.TryParse(id, out iid))
                objs = c_objects.Where(o => o.Id == iid).ToList();

            if (iid == 0 || objs.Count() == 0)
                objs = c_objects.Where(o => o.Name != null && o.Name.ToLower() == id.ToLower()).ToList();


            if (objs.Count() > 0)
            {
                obj = objs.First();
                var sd = (StateDevice) obj;
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

            return _JSONResponse<NetomityObject>(status, obj);

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



    public enum RestResponseStatus
    {
        Success,
        Fail,
        Error
    }
}
