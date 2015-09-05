using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Netomity.Utility;


namespace Netomity.Interfaces.Basic
{
    public class TCPServer : BasicInterface
    {

        public override event DataReceivedHandler DataReceived;

        public int Port { get; set; }
        public IPAddress Address { get; set; }

        TcpListener _server = null;

        public override bool IsOpen
        {
            get { return _stream != null; }
        }
        public Task IntTask { get; set; }

        NetworkStream _stream = null;

        public TCPServer(int port, string address="Any")
        {
            Port = port;
            if (address == "Any")
                Address = IPAddress.Any;
            else
                Address = IPAddress.Parse(address);
        }

        public override async Task Open()
        {

        // Set the TcpListener on port 13000.
            Int32 port = Port;

            try
            {
                // TcpListener server = new TcpListener(port);
                _server = new TcpListener(Address, port);

                // Start listening for client requests.
                _server.Start();

                while (true)
                {
                    try
                    {
                        Log(message: "Waiting for Connections");
                        TcpClient tcpClient = await _server.AcceptTcpClientAsync();
                        Task t = ProcessRequest(tcpClient);
                        await t;
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }   

        private async Task ProcessRequest(TcpClient tcpClient)
        {

            Log(level: Netomity.Core.Logger.Level.Info, 
                message: String.Format(
                "Connection from: {0}",
                tcpClient.Client.RemoteEndPoint.ToString()
                ));

            byte[] buffer = new byte[1024];
            _stream = tcpClient.GetStream();

            while (tcpClient.Connected)
            {
                try
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                        break;
                    var bArray = new Byte[bytesRead];
                    for (int i = 0; i < bytesRead; i++)
                        bArray[i] = buffer[i];
//                    var data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    var data = Conversions.BytesToAscii(bArray);
                    base._DataReceived(data);

                    if (DataReceived != null)
                        DataReceived(data);
                }
                catch (System.IO.IOException ex)
                {
                    Log(Core.Logger.Level.Info, "Client Connection Closed or Fault");
                    break;
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
            _stream = null;
            
        }
    
        public override void Send(string data)
        {
            try
            {
                base.Send(data);
                if (IsOpen && _stream != null)
                {
                    // Process the data sent by the client.
                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    var msg = Conversions.AsciiToBytes(data);

                    // Send back a response.
                    _stream.Write(msg, 0, msg.Length);
                    Log(message:
                        String.Format("Sent: {0}", data));
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public override void Close()
        {
            _server.Stop();
        }
    }
}
