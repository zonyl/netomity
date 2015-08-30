using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace Netomity.Interfaces.Basic
{
    public class SerialIO : BasicInterface
    {
        System.IO.Ports.SerialPort _sp;
        //string _commandBuffer = "";
        //public delegate void DataReceivedHandler(string command);
        Task IntTask { get; set; }

        public SerialIO(string portName="COM4", int portSpeed = 9600)
        {
            _sp = new SerialPort(portName, portSpeed);
            _sp.DataReceived += _serialPort_DataReceived;
        }

        public override event DataReceivedHandler DataReceived;

        
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var blockLimit = 4000;
            byte[] buffer = new byte[blockLimit];
            Action kickoffRead = delegate
            {
                _sp.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult ar)
                {
                    try
                    {
                        int actualLength = _sp.BaseStream.EndRead(ar);
                        byte[] received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                        _parseData(received);
                    }
                    catch (IOException exc)
                    {
                        //                        handleAppSerialError(exc);
                        Log(exc);
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }, null);
            };
            kickoffRead();
        }

        private void _parseData(byte[] received)
        {
            string result = System.Text.Encoding.UTF8.GetString(received);

            //foreach (char c in result)
            //{
            //    if (c != '\r')
            //        if (c == '\n')
            //        {
            //            DataReceived(_commandBuffer);
            //            _commandBuffer = "";
            //        }
            //        else
            //            _commandBuffer += c;
            //}
            base._DataReceived(result);
            DataReceived(result);

        }

        public override Task Open()
        {
            try
            {

            //await Task.Run(() =>
            //{
            //    try
            //    {
            //    _sp.Open();
            //    }
            //    catch (SystemException ex)
            //    {
            //        Log(Core.Logger.Level.Error, ex.ToString());
            //    }
            //});

            IntTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        _sp.Open();
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                    while (true)
                    { }
                });

            c_tasks.Add(IntTask);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            
            return IntTask;
        }

        public override Boolean IsOpen
        {
            get
            {
                return _sp.IsOpen;
            }
        }

        public override void Send(string text)
        {
            base.Send(text);
            _sp.Write(text);
        }


    }
}
