using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core.Enum
{
    [DataContract]
    public sealed class NotificationParamType: StringEnum
    {
        public static readonly NotificationParamType ToAddress = new NotificationParamType("toAddress");
        public static readonly NotificationParamType ToAddressName = new NotificationParamType("toAddressName");
        public static readonly NotificationParamType Subject = new NotificationParamType("subject");

        private NotificationParamType(String name): base(name: name) {}
    }
}