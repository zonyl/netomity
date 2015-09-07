using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.HA
{
    public class SendParams
    {
        public byte[] SendData { get; set; }
        public byte[] SuccessResponse { get; set; }
        public byte[] FailureResponse { get; set; }
        public int Timeout { get; set; }
    }
}
