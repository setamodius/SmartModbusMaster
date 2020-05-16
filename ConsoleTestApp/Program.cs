using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kr.Communication.SmartModbusMaster.Diagnostic;
using Kr.Communication.SmartModbusMaster.Modbus;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main()
        {
            GlobalLogger.LogMessageReceived += GlobalLogger_LogMessageReceived;
            Console.WriteLine("Savronik.Tools.Modbus.Reader started");
            var myDevices = Creator.FromFile("address.csv");

            if (myDevices == null)
            {
                return;
            }
            myDevices.TagStatusChanged += MyDevices_TagStatusChanged;
            foreach (var device in myDevices.Values)
            {
                device.ConnectionStatusChanged += Device_ConnectionStatusChanged;
            }
            myDevices.StartDevices();
            Console.ReadLine();
        }

        private static void GlobalLogger_LogMessageReceived(object sender, LogReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void MyDevices_TagStatusChanged(object sender, TagChangedEventArgs e)
        {
            Console.WriteLine($"{e.Name} -> {e.Value} {e.Quality}");
        }

        private static void Device_ConnectionStatusChanged(object sender, EventArgs e)
        {
            Console.WriteLine((sender as Device).IsActive + " " + (sender as Device).IsConnected);
        }
       
    }
}
