using Netomity.Core;
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
        string _address = null;

        State _state = null;
        State _previousState = null;

        List<Action<Command>> _commandDelegates = null;
        List<Action<State>> _stateDelegates = null;
        List<StateDevice> _devices = null;
        List<Command> _commandsAvailable = null;
        List<BehaviorBase> _behaviors = null;

        public StateDevice(string address=null, HAInterface iface=null, List<StateDevice> devices=null, List<BehaviorBase> behaviors=null)
        {
            _iface = iface;
            _address = address;
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
                _iface.OnCommand(source: _address, action: _CommandReceived);
        }

        private void RegisterBehaviors(List<BehaviorBase> behaviors)
        {
            if (behaviors != null)
                _behaviors = behaviors;

            _behaviors.ForEach(b => b.Register(this));

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

        private void _CommandReceived(Command command)
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
                Primary = st,
                Secondary = command.Secondary
            };
        }

        private async Task<bool> Command(Command command)
        {
            bool isSent = false;

            Log(String.Format("Incoming Command: {0}", command.Primary));
            Log("Filtering");
            var c = ApplyBehaviors(command);

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
                _CommandReceived(c);
            }
            catch (System.Exception ex)
            {
                Log(ex);
            }
            return isSent;

        }

        private Command ApplyBehaviors(Command command)
        {
            Command commandOutput = command;
            _behaviors.OrderByDescending(b => b.Priority).ToList().ForEach(b => {
                commandOutput = b.FilterCommand(commandOutput);
            }
                );
            return commandOutput;
        }

        public Task<bool> Command(CommandType primary, string secondary=null, NetomityObject sourceObject=null)
        {
            return Command(command: new Command()
            {
                Destination = _address,
                Primary = primary,
                Secondary = secondary,
                SourceObject = sourceObject
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
