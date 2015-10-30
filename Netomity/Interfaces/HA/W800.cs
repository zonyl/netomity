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

            return commands;
        }

    }
}
