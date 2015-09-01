using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.Basic
{
    public class TCPClient: BasicInterface
    {

        TcpClient _client = null;
        string _hostName = null;
        int _portNumber = 0;
        NetworkStream _stream = null;
        Object connectingLock = new System.Object();

        public override event DataReceivedHandler DataReceived;
        public TCPClient(string hostName = null, int port = 0)
        {
            _hostName = hostName;
            _portNumber = port;

        }

        public override async Task Open()
        {
            lock (connectingLock)
            {
                _client = new TcpClient();

                Log(Core.Logger.Level.Debug, "Opening Connection");

                //await _client.ConnectAsync(_hostName, _portNumber);
                _client.Connect(_hostName, _portNumber);
                _stream = _client.GetStream();
            }

            Log(Core.Logger.Level.Debug, "Established Connection");

            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead < 0)
                    break;
                var data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Log(Core.Logger.Level.Debug,
                    String.Format("Received: {0}", data));
                DataReceived(data);
            } 

        }

        public override void Send(string data)
        {
            lock (connectingLock) ;
            if (IsOpen && _stream != null)
            {
                // Process the data sent by the client.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                _stream.Write(msg, 0, msg.Length);
                Log(Core.Logger.Level.Debug,
                        String.Format("Sent: {0}", data));
            } else
            {
                Log(String.Format(
                    "Client not connected to send: {0}",
                    data));
            }

        }

        public override Boolean IsOpen
        {
            get
            {
                return _client.Connected;
            }
        }

        public override void Close()
        {
            _client.Close();
        }

    }
}
