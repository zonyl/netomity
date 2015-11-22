using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{
    public class Filter: BehaviorBase
    {

        List<FilterItem> _filters = null;

        public Filter(CommandType primaryInput,
                    StateDevice filterDevice,
                    StateType filterState
            )
        {
            Add(primaryInput: primaryInput,
                filterDevice: filterDevice,
                filterState: filterState
                );
        }

        public void Add(CommandType primaryInput,
                    StateDevice filterDevice,
                    StateType filterState
            )
        {
            if (_filters == null)
                _filters = new List<FilterItem>();

            _filters.Add(new FilterItem()
            {
                PrimaryInput = primaryInput,
                filterDevice = filterDevice,
                filterState = filterState,
            });
        }

        public override Command FilterCommand(Command command)
        {
            if (_filters.Any(f => f.PrimaryInput == command.Primary &&
                f.filterDevice.State.Primary == f.filterState))
                return null;
            else
                return base.FilterCommand(command: command);
        }
    }

    internal class FilterItem
    {
        internal CommandType PrimaryInput { get; set; }
        internal StateDevice filterDevice { get; set; }
        internal StateType filterState { get; set; }
    }
}
