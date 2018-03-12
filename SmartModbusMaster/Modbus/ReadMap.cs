// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Modbus
{
    public struct ReadMap
    {
        public ushort Range;
        public ushort StartAddress;
        public override string ToString()
        {
            return string.Format("From {0} to {1} Total {2}", StartAddress, (StartAddress + Range - 1), Range);
        }
    }
}