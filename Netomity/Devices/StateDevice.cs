using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Devices.Behaviors;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    [DataContract]
    public class StateDevice: NetomityObject
    {
        HAInterface _iface = null;
        public string Address { get; set; }

        State _state = null;
        State _previousState = null;

        protected List<Action<Command>> _commandDelegates = null;
        protected List<Action<State>> _stateDelegates = null;
        protected List<StateDevice> _devices = null;
        protected List<Command> _commandsAvailable = null;
        protected List<BehaviorBase> _behaviors = null;

        public StateDevice(string address=null, HAInterface iface=null, List<StateDevice> devices=null, List<BehaviorBase> behaviors=null)
        {
            _iface = iface;
            Address = address;
            _commandDelegates = new List<Action<Command>>();
            _stateDelegates = new List<Action<State>>();
            _behaviors = new List<BehaviorBase>();

            _state = new State()
            {
                Primary = StateType.Unknown
            };

            _Initialize();

            RegisterDevices(devices);
            RegisterBehaviors(behaviors);

            if(_iface !=null)
                _iface.OnCommand(source: Address, action: _CommandReceived);
        }

        protected void RegisterBehaviors(List<BehaviorBase> behaviors)
        {
            if (behaviors != null)
                behaviors.ForEach(b => RegisterBehavior(b));
        }

        protected void RegisterBehavior(BehaviorBase behavior)
        {
            if (behavior != null)
            {
                if (_behaviors == null)
                    _behaviors = new List<BehaviorBase>();

                _behaviors.Add(behavior);

                behavior.Register(this);
            }
        }

        internal virtual void _Initialize()
        {
            Type = NetomityObjectType.Device;

            _commandsAvailable = new List<Command>() {
                new Command() {
                    Primary = CommandType.On
                },
                new Command() {
                    Primary = CommandType.Off
                },  
            };

        }

        private void RegisterDevices(List<StateDevice> devices)
        {
            if (devices != null)
                _devices = devices;
            else
                _devices = new List<StateDevice>();

 //           _devices.ForEach(d => d.OnCommand((c) => { _CommandReceived(c); }));
            _devices.ForEach(d => d.OnCommand((c) => { Command(c); }));
        }

        [DataMember]
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        [DataMember]
        public List<Command> CommandsAvailable
        {
            get
            {
                return _commandsAvailable;
            }
        }

        protected virtual void _CommandReceived(Command command)
        {
            State newState = null;
            Log(String.Format("Command Received: {0}", command.Primary.ToString()));
            Log(String.Format("State Current: {0}", _state.Primary.ToString()));
            newState = CommandToState(command);
            if (newState.Primary != _state.Primary)
            {
                _state = newState;
                Log(String.Format("State Changed: {0}", _state.Primary.ToString()));
                _delegateState(state: _state);

            }
            _delegateCommand(command: command);

        }

        private void _delegateCommand(Command command)
        {
            command.SourceObject = this;
            foreach (var action in _commandDelegates)
            {
                action(command);
            }            
        }

        private void _delegateState(State state)
        {
            foreach (var action in _stateDelegates)
            {
                action(state);
            }
        }


        private State CommandToState(Command command)
        {
            var commandS = command.Primary.ToString();
            StateType st = null;


            foreach (var fieldS in typeof(StateType).GetFields())
            {
                if (fieldS.GetValue(null).ToString().ToLower() == command.Primary.ToString().ToLower())
                {
                    st = (StateType)fieldS.GetValue(null);
                    break;
                }
            }

            return new State()
            {
                Primary = st ?? StateType.Unknown,
                Secondary = command.Secondary
            };
        }

        private async Task<bool> Command(Command command)
        {
            bool isSent = false;

            Log(String.Format("Incoming Command: {0}", command.Primary));
            Log("Filtering");
            var c = ApplyBehaviors(command);

            if (c == null)
            {
                Log(String.Format("Command Filtered to nothing"));
                return true;
            }

            Log(String.Format("Sending command to interface: {0}", c.Primary.ToString()));
            try
            {
                if (_iface != null)
                    isSent = await _iface.Command(c);
                else
                {
                    Log("Command issued however no interface");
                    isSent = true;
                }
            }
            catch (System.Exception ex)
            {
                Log(ex);
            }
            _CommandReceived(c);
            return isSent;

        }

        protected virtual Command ApplyBehaviors(Command command)
        {
            Command commandOutput = command;
            _behaviors.OrderBy(b => Convert.ToInt32(b.Priority)).ToList().ForEach(b => {
                commandOutput = b.FilterCommand(commandOutput);
            }
                );
            return commandOutput;
        }

        public Task<bool> Command(CommandType primary, string secondary=null, NetomityObject sourceObject=null, Dictionary<StringEnum, string> stringParams=null)
        {
            return Command(command: new Command()
            {
                Destination = Address,
                Primary = primary,
                Secondary = secondary,
                SourceObject = sourceObject,
                StringParams = stringParams
            });
        }

        public Task<bool> Command(string primary, string secondary=null)
        {
            CommandType commandType = null;
            foreach (var field in typeof(CommandType).GetFields())
            {
                if (field.GetValue(null).ToString().ToLower() == primary.ToLower())
                {
                    commandType = (CommandType)field.GetValue(null);
                    break;
                }
            }
            return Command(primary: commandType, secondary: secondary);
        }

        public void OnCommand(Action<Command> action)
        {
            _commandDelegates.Add(action);
        }

        public void OnStateChange(Action<State> action)
        {
            _stateDelegates.Add(action);
        }

        public Task<bool> On()
        {
            return Command(primary: CommandType.On);
        }

        public Task<bool> Off()
        {
            return Command(primary: CommandType.Off);
        }

    }
}
