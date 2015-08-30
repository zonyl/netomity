using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netomity.Core;
using Netomity.Interfaces.Basic;
using System.Threading;

namespace Netomity
{
    class Program
    {
        static void Main(string[] args)
        {

            //$logger = New-Logger -FileName "c:\temp\netomity-log.txt"
            var logger = new Logger(@"C:\temp\netomity-log.txt");

            var ns = NetomitySystem.Factory();

//$insteonPort = New-SerialIO -Name "InsteonSerial" -PortName "COM5" -PortSpeed 19200
            var insteonPort = new SerialIO(portName: "COM5", portSpeed: 19200);
//$w800Port = New-SerialIO -Name "W800Serial" -PortName "COM4" -PortSpeed 4800
            var w800Port = new SerialIO(portName: "COM4", portSpeed: 4800);
//$insteonTCP = New-TCPServer -Name "InsteonTCP" -PortNumber 3333
            var insteonTCP = new TCPServer(3333);
//$w800TCP = New-TCPServer -Name "W800TCP" -PortNumber 3334
            var W800TCP = new TCPServer(3334);
//$insteonBC = New-BasicConnector -Name "InsteonBC" -Interfaces $insteonPort, $insteonTCP
            var insteonBC = new BasicConnector(insteonPort, insteonTCP);
//$w800BC = New-BasicConnector -Name "W800BC" -Interfaces $w800Port, $w800TCP
            var w800BC = new BasicConnector(w800Port, W800TCP);
//$insteonTask = $insteonBC.Open()
            var insteonTask = insteonBC.Open();

            //$w800Task = $w800BC.Open()
            var w800Task = w800BC.Open();
//$insteonTask.Wait();
//            insteonTask.Wait();
//            Thread.Sleep(60000);
            ns.Run();
        }
    }
}
