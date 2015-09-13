using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.HA
{
    public class SendParams
    {
        public string SendData { get; set; }
        public string SuccessResponse { get; set; }
        public string FailureResponse { get; set; }
        public int Timeout { get; set; }
    }
}
