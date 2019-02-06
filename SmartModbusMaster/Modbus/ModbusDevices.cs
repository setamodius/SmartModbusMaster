namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using System;
    using System.Collections.Generic;
    using TagManagement;

    public class ModbusDevices : Dictionary<string, Device>
    {
        public static ModbusDevices Empty = new ModbusDevices();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        ~ModbusDevices()
        {
            KillAllDevices();
        }

        public event TagStatusChangeEventHandler TagStatusChanged;
        public event EventHandler<DeviceStatusChangedEventArgs> DeviceStatusChanged;

        public void KillAllDevices()
        {
            foreach (var item in Values)
            {
                item.Dispose();
            }
        }

        public void StartDevices()
        {
            foreach (var item in Values)
            {
                if (item.IsActive)
                {
                    item.Collection.CalculateParseAddresses();
                    item.Start();
                }
            }
        }

        public void WriteTag(string tagname, object value)
        {
            foreach (var item in Values)
            {
                item.WriteValueToTag(tagname, value);
            }
        }

        new internal void Add(string devicename, Device device)
        {
            if (ContainsKey(devicename))
            {
                logger.Warn("Device already added - {0}", devicename);
                return;
            }
            base.Add(devicename, device);
            device.ConnectionStatusChanged += Device_ConnectionStatusChanged;
            device.Collection.TagStatusChanged += delegate (Tag sender, object value, bool quality)
            {
                TagStatusChanged?.Invoke(sender, value, quality);
            };
        }

        private void Device_ConnectionStatusChanged(object sender, EventArgs e)
        {
            DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs { ChangedDevice = (Device)sender });
        }
    }

    public class DeviceStatusChangedEventArgs:EventArgs
    {
        public Device ChangedDevice { get; set; }
    }
}