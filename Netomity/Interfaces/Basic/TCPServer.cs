using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Netomity.Interfaces.Basic
{
    public class TCPServer : BasicInterface
    {

        public override event DataReceivedHandler DataReceived;

        public int Port { get; set; }
        public IPAddress Address { get; set; }
        public override bool IsOpen
        {
            get { return true; }
        }
        public Task IntTask { get; set; }

        NetworkStream _stream = null;

        public TCPServer(int port, string address="127.0.0.1")
        {
            Port = port;
            Address = IPAddress.Parse(address);
        }

        public override async Task Open()
        {
            TcpListener server = null;

        // Set the TcpListener on port 13000.
            Int32 port = Port;
            IPAddress localAddr = Address;

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            while (true)
            {
                try
                {
                    Log(Core.Logger.Level.Debug, "Waiting for Connections"); 
                    TcpClient tcpClient = await server.AcceptTcpClientAsync();
                    Log(Core.Logger.Level.Debug, "Process");
                    Task t = ProcessRequest(tcpClient);
                    Log(Core.Logger.Level.Debug, "Out");
                    await t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }   

        private async Task ProcessRequest(TcpClient tcpClient)
        {

            Log(Netomity.Core.Logger.Level.Info, String.Format(
                "Connection from: {0}",
                tcpClient.Client.RemoteEndPoint.ToString()
                ));

            byte[] buffer = new byte[1024];
            _stream = tcpClient.GetStream();

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
            Log(Core.Logger.Level.Debug,
               String.Format("Sending: {0}", data));
            if (IsOpen && _stream != null)
            {
                // Process the data sent by the client.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                _stream.Write(msg, 0, msg.Length);
                Log(Core.Logger.Level.Debug,
                    String.Format("Sent: {0}", data));
            }

        }
    }
}
