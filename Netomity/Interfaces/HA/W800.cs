using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Interfaces.Basic;
using Netomity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.HA
{
    public class W800: HAInterface
    {
        Dictionary<int, string> _HouseCodes = new Dictionary<int, string>()
            {
                { 0x00, "m"},
                { 0x01, "e"},
                { 0x02, "c"},
                { 0x03, "k"},
                { 0x04, "o"},
                { 0x05, "g"},
                { 0x06, "a"},
                { 0x07, "i"},
                { 0x08, "n"},
                { 0x09, "f"},
                { 0x0A, "d"},
                { 0x0B, "l"},
                { 0x0c, "p"},
                { 0x0d, "h"},
                { 0x0e, "b"},
                { 0x0f, "j"},
            };


        public W800(BasicInterface iface)
            : base(iface: iface)
        {

        }


        public override async Task<bool> Command(Command command)
        {
            return false;
            
        }
        protected override List<Command> _DataToCommands(string data)
        {
            var dBytes = Conversions.AsciiToBytes(data);
            var rBytes = Conversions.BytesReverse(dBytes);

            // If this is a corrupt packet, discard and exit
            if (rBytes[0] + rBytes[1] != 255 ||
                rBytes[2] + rBytes[3] != 255)
            {
                Log("Corrupt Packet Received");
                return new List<Command>();
            }


            var sBytes = new byte[4]
            {
                rBytes[2],
                rBytes[3],
                rBytes[0],
                rBytes[1],
            };

            string houseCode = _HouseCodes[sBytes[2] & 0x0f];

            int unitCode =
                ((sBytes[0] & 0x18) >> 3) +
                ((sBytes[0] & 0x02) << 1) +
                ((sBytes[2] & 0x20) >> 2) +
                1;

            CommandType commandPrimary = null;

            switch (sBytes[0] & 0x05)
            {
                case 4:
                    commandPrimary = CommandType.Off;
                    break;
                case 0:
                    commandPrimary = CommandType.On;
                    break;
                default:
                    commandPrimary = CommandType.Unknown;
                    break;
            }

            var commands = new List<Command>()
            {
                new Command() {
                    Primary = commandPrimary,
                    Source = String.Format("{0}{1}", houseCode, unitCode),
                    Destination = String.Format("{0}{1}", houseCode, unitCode),
                    SourceObject = this,
                }
            };

            if (commands[0] != null)
                Log(Core.Logger.Level.Info, String.Format("Received Command: {0} -> {1}: {2}",
                    commands[0].Source,
                    commands[0].Destination,
                    commands[0].Primary.ToString()
                    ));

            return commands;
        }

    }

    /*
1/6/2015 5:01:43 PM|Debug|W800Serial:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:43 PM|Debug|W800TCP:Sending Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:43 PM|Debug|W800BC:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800HA:HA Interface Received: >d☼ ß< (F00F20DF)
1/6/2015 5:01:43 PM|Debug|W800Serial:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800TCP:Sending Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800BC:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800HA:HA Interface Received: >d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800Serial:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800TCP:Sending Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800BC:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800HA:HA Interface Received: >d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800Serial:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800TCP:Sending Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800BC:Received Data:>d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800HA:HA Interface Received: >d☼ ß< (F00F20DF)
1/6/2015 5:01:44 PM|Debug|W800HA:Received Command: j1 -> j1: off
1/6/2015 5:01:44 PM|Debug|Test Motion 1:Command Received: off
1/6/2015 5:01:44 PM|Debug|Test Motion 1:State Current: on
1/6/2015 5:01:44 PM|Debug|Test Motion 1:State Changed: off
1/6/2015 5:01:48 PM|Debug|:HeartBeat
     * */
}
