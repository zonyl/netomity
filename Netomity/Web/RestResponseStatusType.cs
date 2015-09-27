using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Web
{
    [DataContract]
    public sealed class RestResponseStatusType
    {
        private readonly string _name;

        public static readonly RestResponseStatusType Success = new RestResponseStatusType("success");
        public static readonly RestResponseStatusType Fail = new RestResponseStatusType("fail");
        public static readonly RestResponseStatusType Error = new RestResponseStatusType("error");


        private RestResponseStatusType(String name)
        {
            _name = name;
        }

        public override String ToString()
        {
            return _name;
        }

        public static implicit operator string(RestResponseStatusType rt)
        {
            return rt.ToString();
        }

        [DataMember]
        public string Value
        {
            get
            {
                return _name;
            }
        }
    }
}
