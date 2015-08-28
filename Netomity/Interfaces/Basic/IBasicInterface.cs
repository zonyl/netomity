using Netomity.Core;
using System;
using System.Threading.Tasks;

namespace Netomity.Interfaces.Basic
{
    public delegate void DataReceivedHandler(string command);

    public interface IBasicInterface2: INetomityObject
    {
        //event SerialText.DataReceivedHandler DataReceived;
        event DataReceivedHandler DataReceived;
        bool IsOpen { get; }
        Task Open();
        void Send(string text);

    }
}
