// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Converters
{
    using System;

    public class LSRFFloatConverter : IFloatConverter
    {
        public float ConvertToFloat(ushort value1, ushort value2)
        {
            ushort[] buffer = { value1, value2 };
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(buffer[0] & 0xFF);
            bytes[1] = (byte)(buffer[0] >> 8);
            bytes[2] = (byte)(buffer[1] & 0xFF);
            bytes[3] = (byte)(buffer[1] >> 8);
            return BitConverter.ToSingle(bytes, 0);
        }

        /* kodun arasında acıklama satırlarında dolaşırken kim bilir kaçıncı satırın
        kaçıncı sütununda yazılmış anlamsız bir harf dizisi, beyninde bir kaç küçük
        elektriksel impuls üretimine neden oluyorsa ve bu sende anlamsız bir şekilde
        anlamlandırma oluşturuyorsa anlamsızlık nedir. */

        public ushort[] ConvertToUshort(float value)
        {
            ushort[] ushortData = new ushort[2];
            float[] floatData = new float[] { value };
            Buffer.BlockCopy(floatData, 0, ushortData, 0, 4);
            return ushortData;
        }
    }
}