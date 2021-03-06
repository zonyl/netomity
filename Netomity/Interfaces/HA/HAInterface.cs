﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netomity.Core;
using Netomity.Interfaces.Basic;
using Netomity.Utility;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Netomity.Core.Enum;

namespace Netomity.Interfaces.HA
{

    public class HAInterface: NetomityObject
    {
        protected BasicInterface _interface = null;
        protected ConcurrentQueue<string> _receivedData = null;
        protected string _receivedBuffer = null;
        protected DateTime? _receivedStart = null;
        protected Dictionary<Guid, CommandQueueDetail> _outgoingCommandQueueDetail = null;
        protected Task _taskDispatch = null;
        protected List<Tuple<string, OnCommandCallBack>> _OnCommandList = null;
        protected List<Tuple<string, Action<Command>>> _OnCommandListA = null;

        private const int SEND_RETRIES = 3;

        public ConcurrentQueue<string> ReceiveQueue
        {
            get
            {
                return _receivedData;
            }
        }

        public HAInterface(BasicInterface iface)
        {
            _interface = iface;
            _receivedData = new ConcurrentQueue<string>();
            _outgoingCommandQueueDetail = new Dictionary<Guid, CommandQueueDetail>();
            _OnCommandList = new List<Tuple<string, OnCommandCallBack>>();
            _OnCommandListA = new List<Tuple<string, Action<Command>>>();

            if (_interface != null)
                _interface.DataReceived += _DataReceived;
            Log("Initialized");
            _taskDispatch = Task.Factory.StartNew(() => {
                try
                {
                    Dispatch();
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
            );

        }

        public virtual void _DataReceived(string data)
        {
            Log(String.Format("HA Interface Received: >{0}< ({1})",
                data,
                Conversions.AsciiToHex(data)));
            
            _receivedData.Enqueue(data);
        }

        public async Task<bool> Send(SendParams sp)
        {

            var commandResponseEvent = new AutoResetEvent(false);
            var commandId = Guid.NewGuid();

            var commandQueueDetail = new CommandQueueDetail()
            {
                Id = Guid.NewGuid(),
                Command = sp.SendData,
                Success = sp.SuccessResponse,
                Failure = sp.FailureResponse,
                OriginalSentTime = DateTime.Now,
                Retries = SEND_RETRIES,
                Status = CommandStatus.Pending,
                Timeout = new TimeSpan(0,0,0,0,sp.Timeout),
                CommandResponseEvent = commandResponseEvent,
            };

            Log("Queueing command");
            _outgoingCommandQueueDetail[commandId] = commandQueueDetail;

            if (sp.SuccessResponse == null && sp.FailureResponse == null)
                return true;

            Log("Waiting Results");
            return await Task.Run(() =>
            {
                var success = false;
                Log("Waiting");
                commandResponseEvent.WaitOne();
                if (commandQueueDetail.Status == CommandStatus.Success)
                    success = true;
                _outgoingCommandQueueDetail.Remove(commandId);
                return success;
            });

        }

        private void Dispatch()
        {
            Log("Dispatch Initialized");
            while (true)
            {
                DispatchIncoming();
                DispatchOutgoing();
                DispatchOutgoingTimeouts();
                Thread.Sleep(50);
            }
        }

        private void DispatchIncoming()
        {
            TimeSpan deadTime = new TimeSpan(0,0,0,0,300);

            var currentTime = DateTime.Now;

            string data;
            //if (_receivedData.TryDequeue(out data))
            //{
            //    _receivedBuffer += data;
            //    _receivedStart = currentTime;

            //}
            while (_receivedData.TryDequeue(out data))
            {
                _receivedBuffer += data;
                _receivedStart = currentTime;

            }

            if (_receivedBuffer != null &&
                _receivedStart != null &&
                currentTime - _receivedStart >= deadTime)
            {
                if (!DispatchIncomingExpected(_receivedBuffer))
                    DispatchIncomingUnexpected(_receivedBuffer);
                _receivedStart = null;
                _receivedBuffer = null;
            }                    


        }

        private bool DispatchIncomingExpected(string data)
        {

            foreach (var key in _outgoingCommandQueueDetail.Keys)
            {
                var commandDetail = _outgoingCommandQueueDetail[key];
                Log(String.Format("Received Something in Queue: {0} {1} {2}",
                    Conversions.AsciiToHex(data),
                    Conversions.AsciiToHex(commandDetail.Success),
                    Conversions.AsciiToHex(commandDetail.Failure)
                    ));
                if (commandDetail.Success == data)
                {
                    Log("Success");
                    commandDetail.Status = CommandStatus.Success;
                    commandDetail.CommandResponseEvent.Set();
                    return true;
                }
                if (commandDetail.Failure == data)
                {
                    Log("Failure");
                    commandDetail.Status = CommandStatus.Failure;
                    commandDetail.CommandResponseEvent.Set();
                    return true;
                }

            }
            return false;
        }

        public virtual void DispatchIncomingUnexpected(string data)
        {
            var commands = _DataToCommands(data);

            foreach (var command in commands)
            {
                var delegates = _OnCommandList.Where(x => x.Item1 == null 
                    || command.Source == null
                    || x.Item1.ToLower() == command.Source.ToLower() ).ToList();
                delegates.ForEach(x => x.Item2(command));
                var delegatesA = _OnCommandListA.Where(x => x.Item1 == null 
                    || command.Source == null
                    || x.Item1.ToLower() == command.Source.ToLower()).ToList();
                delegatesA.ForEach(x => x.Item2(command));
            }

        }

        protected virtual List<Command> _DataToCommands(string data)
        {
            var commands = new List<Command>();

            if (data.ToLower() == CommandType.On.ToString().ToLower())
                commands.Add( new Command() { Primary = CommandType.On, Source = null });
            else
                commands.Add( new Command() { Primary = CommandType.Off, Source = null });

            return commands;
        }

        private void DispatchOutgoing()
        {
            var outgoingList = _outgoingCommandQueueDetail.Values
                .Where((x) => x.Status == CommandStatus.Pending);
            foreach (var outgoing in outgoingList)
            {
                Log(String.Format("HA Interface Sending: >{0}< ({1})",
                    outgoing.Command,
                    Conversions.AsciiToHex(outgoing.Command)));
                _interface.Send(outgoing.Command);
                outgoing.OriginalSentTime = DateTime.Now;
                outgoing.Status = CommandStatus.Sent;
            }

        }

        private void DispatchOutgoingTimeouts()
        {
            var now = DateTime.Now;
            var timeoutList = _outgoingCommandQueueDetail.Values
                                .Where((x) => x.Status == CommandStatus.Sent &&
                                now > x.OriginalSentTime + x.Timeout).ToList();
            foreach (var timeout in timeoutList)
            {
                timeout.Status = CommandStatus.Timeout;
                Log(String.Format("Timeout Of Command: {0}",timeout.Command));
                timeout.CommandResponseEvent.Set();
            }
        }

        public delegate void OnCommandCallBack(Command command);

        public virtual void OnCommand(string source, OnCommandCallBack callback )
        {
            _OnCommandList.Add(new Tuple<string, OnCommandCallBack>( source, callback ));

        }

        public virtual void OnCommand(string source, Action<Command> action)
        {
            _OnCommandListA.Add(new Tuple<string, Action<Command>>(source, action));
        }

        public virtual async Task<bool> Command(Command command)
        {
            return await Send(new SendParams()
            {
                SendData = command.Primary.ToString(),
            });
        }
    }

    public class CommandQueueDetail
    {
        public string Command { get; set; }
        public string Success { get; set; }
        public string Failure { get; set; }
        public TimeSpan Timeout { get; set; }
        public DateTime OriginalSentTime { get; set; }
        public CommandStatus Status { get; set; }
        public int Retries { get; set; }
        public AutoResetEvent CommandResponseEvent { get; set; }
        public Guid Id { get; set; }
    }

    public enum CommandStatus
    {
        Pending,
        Sent,
        Success,
        Failure,
        Timeout
    }
}
