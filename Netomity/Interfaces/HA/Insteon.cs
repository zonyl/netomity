using Netomity.Core;
using Netomity.Interfaces.Basic;
using Netomity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.HA
{
    public class Insteon: HAInterface
    {
        Dictionary<CommandType, Tuple<byte, byte, byte>> _CommandLookup =
            new Dictionary<CommandType, Tuple<byte, byte, byte>>()
            {
                { CommandType.On, new Tuple<byte, byte, byte>(0x62,0x11,0xff)},
                { CommandType.Off, new Tuple<byte, byte, byte>(0x62,0x13,0x00)},
            };

        public Insteon(BasicInterface iface)
            : base(iface: iface)
        {

        }


        public override async Task<bool> Command(Command command)
        {
            var bAddress = Conversions.HexToBytes(command.Destination);
            var commandLookup = _CommandLookup[command.Primary];
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

            var response = await Send(new SendParams(){
                    SendData = aCommand,
                    SuccessResponse = aCommand + Conversions.HexToAscii("06"),
                    FailureResponse = aCommand + Conversions.HexToAscii("15"),
                    Timeout = 2000
                }
            );

            Log(String.Format("Command Status: {0}", response));
            return response;
            
        }
        protected override List<Command> _DataToCommands(string data)
        {
            var commands = new List<Command>();
            var dataB = Conversions.AsciiToBytes(data);

            var command = new Command();
            if (dataB[0] == 0x02)
                //Incoming Standard Message
                if (dataB[1] == 0x50)
                {
                    command.Source = Conversions.BytesToHex(dataB[2])
                        + "." + Conversions.BytesToHex(dataB[3])
                        + "." + Conversions.BytesToHex(dataB[4]);
                    command.Destination = Conversions.BytesToHex(dataB[5])
                        + "." + Conversions.BytesToHex(dataB[6])
                        + "." + Conversions.BytesToHex(dataB[7]);
                    var messageType = _MessageType(dataB[8]);
                    command.Primary = _CommandType(dataB[9]);
                    command.Secondary = Conversions.BytesToInt(dataB[10]).ToString();
                    if (command.Primary != null 
                            && ( messageType == InsteonMessageType.Broadcast
                            || messageType == InsteonMessageType.Direct
                            || messageType == InsteonMessageType.BroadCastLinkAll
                            ))
                        commands.Add(command);
                }

            return commands;
        }

        private CommandType _CommandType(byte b1)
        {
            var commandType = _CommandLookup.Keys
                .Where(k => _CommandLookup[k].Item2 == b1);
            return commandType.FirstOrDefault();
        }

        private InsteonMessageType _MessageType(byte b)
        {
            return (InsteonMessageType)(b & 0xE0);
        }

        public enum InsteonMessageType
        {
            Broadcast = 0x80,
            Direct = 0x00,
            DirectAck = 0x20,
            DirectNak = 0xA0,
            BroadCastLinkAll = 0xC0,
            BroadCastLinkClean = 0x40,
            BroadCastLinkCleanAck = 0x60,
            BroadCastLinkcleanNak = 0xE0
        }
    //    private 
    }
}
