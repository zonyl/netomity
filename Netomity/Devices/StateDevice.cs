using Netomity.Core;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class StateDevice: NetomityObject
    {
        HAInterface _iface = null;
        string _address = null;

        State _state = null;
        State _previousState = null;

        List<Action<Command>> _commandDelegates = null;
        List<Action<State>> _stateDelegates = null;

        public StateDevice(HAInterface iface, string address)
        {
            _iface = iface;
            _address = address;
            _commandDelegates = new List<Action<Command>>();
            _stateDelegates = new List<Action<State>>();

            _state = new State(){
                Primary = StateType.Unknown
            };

            _iface.OnCommand(source: _address, action: _CommandReceived);
        }

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

        private void _CommandReceived(Command command)
        {
            State newState = null;
            Log(String.Format("Command Received: {0}", command.Primary.ToString()));
            Log(String.Format("State Current: {0}", _state.Primary.ToString()));
            newState = CommandToState(command);
            if (newState.Primary != _state.Primary)
            {
                Log(String.Format("State Changed: {0}", _state.Primary.ToString()));
                _state = newState;
                _delegateState(state: _state);

            }
            _delegateCommand(command: command);

        }

        private void _delegateCommand(Command command)
        {
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
                Primary = st
            };
        }

        public Task<bool> Command(Command command)
        {
            Log(String.Format("Sending command to interface: {0}", command.Primary.ToString()));
            return _iface.Command(command);
        }

        public Task<bool> Command(CommandType commandT, string secondary=null)
        {
            return Command(command: new Command()
            {
                Destination = _address,
                Primary = commandT,
                Secondary = secondary
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
            return Command(commandT: commandType, secondary: secondary);
        }

        public void OnCommand(Action<Command> action)
        {
            _commandDelegates.Add(action);
        }

        public void OnStateChange(Action<State> action)
        {
            _stateDelegates.Add(action);
        }

    }
}
