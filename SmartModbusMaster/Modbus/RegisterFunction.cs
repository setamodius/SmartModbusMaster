// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Modbus
{
    public class RegisterFunction : ModbusFunction
    {
        public static readonly RegisterFunction HoldingRegister = new RegisterFunction() { Value = 0x03 };
        public static readonly RegisterFunction InputRegister = new RegisterFunction() { Value = 0x04 };
    }
}