﻿namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    using Modbus;

    public delegate void TagValueChangedEventHandler(ITagType tag);

    public interface ITagType
    {
        event TagValueChangedEventHandler TagValueChangedEvent;

        ModbusFunction Function { get; set; }

        void AddAddress(ushort address);

        ushort[] GetAddresses();

        bool IsDefault { get; }
    }
}