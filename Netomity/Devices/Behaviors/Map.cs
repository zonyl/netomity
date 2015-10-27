﻿using Netomity.Core;
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
                    NetomityObject sourceObject=null,
                    int delay=0
            )
        {
            Add(primaryInput: primaryInput,
                primaryOutput: primaryOutput,
                secondaryInput: secondaryInput,
                secondaryOutput: secondaryOutput,
                sourceAddress: sourceAddress,
                sourceObject: sourceObject,
                delay: delay
                );
        }

        public void Add(CommandType primaryInput,
                    CommandType primaryOutput,
                    string secondaryInput=null,
                    string secondaryOutput=null,
                    string sourceAddress=null,
                    NetomityObject sourceObject=null,
                    int delay=0
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
                SourceAddress = sourceAddress,
                SourceObject = sourceObject,
                Delay = delay,
            });
        }

        public override Command FilterCommand(Command command)
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
                        Primary = mapping.PrimaryOutput,
                        Secondary = mapping.SecondaryOutput,
                        Destination = command.Destination,
                        Source = command.Source,
                        SourceObject = command.SourceObject,
                    };
                    if (mapping.Delay != 0)
                        mapped = DelayCommand(mapping: mapping, command: mapped, delay: mapping.Delay);
                    mappedOutputs.Add(mapped);
                }
            }
            if (!IsMapFound)
                mappedOutputs.Add(base.FilterCommand(command));

            // We can only return one mapped command. Lets pick one that does something
            return mappedOutputs.Where(m=> m.Primary != null).LastOrDefault();
        }

        private Command DelayCommand(Mapping mapping, Command command, int delay)
        {
            if (mapping.Timer == null)
                mapping.Timer = new Netomity.Utility.Timer(delay, 
                    () => DelegateCommand(primary: command.Primary, secondary: command.Secondary, sourceObject: this)
            );

            mapping.Timer.Stop();
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
        internal string SourceAddress { get; set; }
        internal NetomityObject SourceObject { get; set; }
        internal int Delay { get; set; }
        internal Netomity.Utility.Timer Timer { get; set; }
    }
}
