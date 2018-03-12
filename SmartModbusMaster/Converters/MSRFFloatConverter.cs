// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Converters
{
    using System;

    public class MSRFFloatConverter : IFloatConverter
    {
        public float ConvertToFloat(ushort value1, ushort value2)
        {
            ushort[] buffer = { value1, value2 };
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(buffer[1] & 0xFF);
            bytes[1] = (byte)(buffer[1] >> 8);
            bytes[2] = (byte)(buffer[0] & 0xFF);
            bytes[3] = (byte)(buffer[0] >> 8);
            return BitConverter.ToSingle(bytes, 0);
        }

        public ushort[] ConvertToUshort(float value)
        {
            ushort[] ushortData = new ushort[2];
            float[] floatData = new float[] { value };
            Buffer.BlockCopy(floatData, 0, ushortData, 0, 4);
            ushort shift = ushortData[0];
            ushortData[0] = ushortData[1];
            ushortData[1] = shift;
            return ushortData;
        }
    }
}