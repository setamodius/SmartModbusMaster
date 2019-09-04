namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using System;

    public class TagChangedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool Quality { get; set; }
    }
}