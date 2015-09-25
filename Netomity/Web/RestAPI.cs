using Netomity.Core;
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

            var r = JsonConvert.SerializeObject(objs, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            r = r.Replace("\r\n", "");
            return r;

        }

        [WebGet(UriTemplate = "/object/{id}/{property}/{primary}/{secondary=default}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        public string SetNetomityObjectProperty(string id, string property, string primary, string secondary)
        {

//            var objs = c_objects.Where(o =>).ToList();

            var r = JsonConvert.SerializeObject(id, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            r = r.Replace("\r\n", "");
            return r;

        }

    
    }
}
