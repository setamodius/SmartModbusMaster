// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Modbus
{
    public class StatusFunction : ModbusFunction
    {
        public static readonly StatusFunction CoilStatus = new StatusFunction() { Value = 0x01 };
        public static readonly StatusFunction InputStatus = new StatusFunction() { Value = 0x02 };
    }
}