using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{
    public class Map: BehaviorBase
    {

        List<Mapping> _mappings = null;

        public Map(CommandType primaryInput,
                    CommandType primaryOutput,
                    string secondaryInput=null,
                    string secondaryOutput=null,
                    string sourceAddress=null,
                    NetomityObject sourceObject=null
            )
        {
            Add(primaryInput: primaryInput,
                primaryOutput: primaryOutput,
                secondaryInput: secondaryInput,
                secondaryOutput: secondaryOutput,
                sourceAddress: sourceAddress,
                sourceObject: sourceObject
                );
        }

        public void Add(CommandType primaryInput,
                    CommandType primaryOutput,
                    string secondaryInput=null,
                    string secondaryOutput=null,
                    string sourceAddress=null,
                    NetomityObject sourceObject=null)
        {
            if (_mappings == null)
                _mappings = new List<Mapping>();

            _mappings.Add(new Mapping()
            {
                PrimaryInput = primaryInput,
                PrimaryOutput = primaryOutput,
                SecondaryInput = secondaryInput,
                SecondaryOutput = secondaryOutput,
                SourceAddress = sourceAddress,
                SourceObject = sourceObject,
            });
        }

        public override Command FilterCommand(Command command)
        {
            Command mapped = null;
            foreach (var mapping in _mappings)
            {
                if ((mapping.PrimaryInput == null || command.Primary == mapping.PrimaryInput) &&
                    (mapping.SecondaryInput == null || command.Secondary == mapping.SecondaryInput) &&
                    (mapping.SourceAddress == null || command.Source == mapping.SourceAddress) &&
                    (mapping.SourceObject == null || command.SourceObject == mapping.SourceObject))
                {
                    mapped = new Command()
                    {
                        Primary = mapping.PrimaryOutput,
                        Secondary = mapping.SecondaryOutput,
                        Destination = command.Destination,
                        Source = command.Source,
                        SourceObject = command.SourceObject,
                    };
                    return mapped;
                }
            }
            if (mapped == null)
                mapped = base.FilterCommand(command);

            return mapped;
        }
    }

    internal class Mapping
    {
        internal CommandType PrimaryInput { get; set; }
        internal CommandType PrimaryOutput { get; set; }
        internal string SecondaryInput { get; set; }
        internal string SecondaryOutput { get; set; }
        internal string SourceAddress { get; set; }
        internal NetomityObject SourceObject { get; set; }
    }
}
