using Netomity.Core;
using Netomity.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{
    public class MapCommand: BehaviorBase
    {

        List<Mapping> _mappings = null;

        public MapCommand(CommandType primaryInput=null,
                    CommandType primaryOutput=null,
                    string secondaryInput=null,
                    string secondaryOutput=null,
                    Dictionary<StringEnum, string> stringParams = null,
                    string sourceAddress = null,
                    NetomityObject sourceObject=null,
                    int delaySecs=0
            )
        {
            if (primaryInput != null)
                Add(primaryInput: primaryInput,
                    primaryOutput: primaryOutput,
                    secondaryInput: secondaryInput,
                    secondaryOutput: secondaryOutput,
                    stringParams: stringParams,
                    sourceAddress: sourceAddress,
                    sourceObject: sourceObject,
                    delaySecs: delaySecs
                    );
        }

        public void Add(CommandType primaryInput,
                    CommandType primaryOutput,
                    string secondaryInput=null,
                    string secondaryOutput=null,
                    Dictionary<StringEnum, string> stringParams=null,
                    string sourceAddress=null,
                    NetomityObject sourceObject=null,
                    int delaySecs=0
            )
        {
            if (_mappings == null)
                _mappings = new List<Mapping>();

            _mappings.Add(new Mapping()
            {
                PrimaryInput = primaryInput,
                PrimaryOutput = primaryOutput,
                SecondaryInput = secondaryInput,
                SecondaryOutput = secondaryOutput,
                StringParams = stringParams,
                SourceAddress = sourceAddress,
                SourceObject = sourceObject,
                DelaySecs = delaySecs,
            });
        }

        public override Command FilterCommand(Command command)
        {
            if (command != null)
            {
                bool IsMapFound = false;
                Command mapped = null;
                var mappedOutputs = new List<Command>();

                foreach (var mapping in _mappings)
                {
                    if ((mapping.PrimaryInput == null || command.Primary == mapping.PrimaryInput) &&
                        (mapping.SecondaryInput == null || command.Secondary == mapping.SecondaryInput) &&
                        (mapping.SourceAddress == null || command.Source == mapping.SourceAddress) &&
                        ((mapping.SourceObject == null && !_mappings.Select(mi => mi.SourceObject).Contains(command.SourceObject)) || command.SourceObject == mapping.SourceObject))
                    {
                        IsMapFound = true;
                        mapped = new Command()
                        {
                            Destination = command.Destination,
                            Primary = command.Primary,
                            Secondary = command.Secondary,
                            StringParams = command.StringParams,
                            Source = command.Source,
                            SourceObject = command.SourceObject,
                        };
                        

                        if (mapping.PrimaryOutput != null)
                            mapped.Primary = mapping.PrimaryOutput;

                        if (mapping.SecondaryOutput != null)
                            mapped.Secondary = mapping.SecondaryOutput;

                        if (mapping.StringParams != null)
                            mapped.StringParams = mapping.StringParams;

                        if (mapping.SourceAddress != null)
                            mapped.Source = mapping.SourceAddress;

                        if (mapping.SourceObject != null)
                            mapped.SourceObject = mapping.SourceObject;

                        if (mapping.DelaySecs != 0)
                            mapped = DelayCommand(mapping: mapping, command: mapped, delay: mapping.DelaySecs);
                        mappedOutputs.Add(mapped);
                    }
                }
                if (!IsMapFound)
                    mappedOutputs.Add(base.FilterCommand(command));

                // We can only return one mapped command. Lets pick one that does something
                return mappedOutputs.Where(m => m.Primary != null).LastOrDefault();
            }
            else
                return command;
        }

        private Command DelayCommand(Mapping mapping, Command command, int delay)
        {
            if (mapping.Timer == null)
                mapping.Timer = new Netomity.Utility.Timer(seconds: delay, 
                   action: () => DelegateCommand(primary: command.Primary, secondary: command.Secondary, sourceObject: this)
            );

            mapping.Timer.Stop();
            Log("Timer Started for {0} secs", delay.ToString());
            mapping.Timer.Start();

            return new Command()
            {
                SourceObject = command.SourceObject,
                Source = command.Source
            };
        }
    }

    internal class Mapping
    {
        internal CommandType PrimaryInput { get; set; }
        internal CommandType PrimaryOutput { get; set; }
        internal string SecondaryInput { get; set; }
        internal string SecondaryOutput { get; set; }
        internal Dictionary<StringEnum, string> StringParams { get; set; }
        internal string SourceAddress { get; set; }
        internal NetomityObject SourceObject { get; set; }
        internal int DelaySecs { get; set; }
        internal Netomity.Utility.Timer Timer { get; set; }
    }
}
