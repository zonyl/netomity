using Netomity.Core;
using Netomity.Interfaces.Basic;
using Netomity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netomity.Interfaces.HA
{
    public class Insteon: HABase
    {
        private BasicInterface _i;

        Dictionary<CommandType, Tuple<byte, byte, byte>> _CommandLookup =
            new Dictionary<CommandType, Tuple<byte, byte, byte>>()
            {
                { CommandType.On, new Tuple<byte, byte, byte>(0x62,0x11,0xff)},
                { CommandType.Off, new Tuple<byte, byte, byte>(0x62,0x13,0x00)},
            };

        public Insteon(BasicInterface basicInterface) : base(basicInterface: basicInterface)
        {

        }

        public void _DataReceived(string data)
        {
            base._DataReceived(data);
        }

        public void Command(Command command)
        {
            var bAddress = Conversions.HexToBytes(command.Destination);
            var commandLookup = _CommandLookup[command.Type];
            var lCommand = 
                new List<byte>() {
                    0x02,
                    commandLookup.Item1,
                    bAddress[0],
                    bAddress[1],
                    bAddress[2],
                    0x0f,
                    commandLookup.Item2,
                    commandLookup.Item3
                };

            byte[] bCommand = lCommand.ToArray();

            var aCommand = Conversions.BytesToAscii(bCommand);
            //_interface.Send(aCommand);
            var succesResponse = lCommand;
            succesResponse.Add(0x06);
            var failResponse = lCommand;
            failResponse.Add(0x15);

            Send(new SendParams(){
                    SendData = bCommand,
                    SuccessResponse = succesResponse.ToArray(),
                    FailureResponse = failResponse.ToArray(),
                    Timeout = 2
                }
            );
            
        }

    //    private 
    }
}
