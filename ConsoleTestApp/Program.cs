using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using Kr.Communication.SmartModbusMaster.Diagnostic;
using Kr.Communication.SmartModbusMaster.Modbus;

namespace ConsoleTestApp
{
    class Program
    {
        private static Timer myTimer;
        private static ModbusDevices myDevices;
        private static ushort count;
        static void Main()
        {
            myTimer = new Timer();
            myTimer.Elapsed += MyTimer_Elapsed;
            myTimer.Interval = 10000;
            GlobalLogger.LogMessageReceived += GlobalLogger_LogMessageReceived;
            Console.WriteLine("Savronik.Tools.Modbus.Reader started");
            myDevices = Creator.FromFile("address.csv");

            if (myDevices == null)
            {
                return;
            }
            myDevices.TagStatusChanged += MyDevices_TagStatusChanged;
            foreach (var device in myDevices.Values)
            {
                device.ConnectionStatusChanged += Device_ConnectionStatusChanged;
                device.Collection.GetAllWriteTags();
            }
            myDevices.StartDevices();
            myTimer.Start();
            Console.ReadLine();
        }

        private static void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            count++;
            myDevices.WriteTag("ETP/TrafficControlOutside/VtsController/Vts1/Vts1Command/Command", count);
            myTimer.Stop();
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
            //myDevices.WriteTag("ETP/TrafficControlOutside/VtsController/Vts1/Vts1Command/Command", count);
        }
       
    }
}
