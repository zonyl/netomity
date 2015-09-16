using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Web
{
    [ServiceContract()]
    public class RestAPI: NetomityObject
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        [OperationContract()]
        public long Add(int a, int b)
        {
            return (a + b);
        }
    }
}
