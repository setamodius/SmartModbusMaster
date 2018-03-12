// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    using System;

    public class UshortTag : TagTypeController<ushort, ushort>
    {
        internal ushort oldValue;

        public UshortTag(Modbus.RegisterFunction function)
        {
            Function = function;
        }

        public ushort GetWriteValue()
        {
            return WriteValue;
        }

        public override string ToString()
        {
            return InnerValue.ToString();
        }

        internal override void CalculateReturnValue()
        {
            ushort result = (MergeType == TagAddressMergeType.AndMerge) ? ushort.MaxValue : ushort.MinValue;
            foreach (var item in modbusValueData.Values)
            {
                if (MergeType == TagAddressMergeType.AndMerge)
                {
                    result = (ushort)(result & item);
                }
                else
                {
                    result = (ushort)(result | item);
                }
            }
            if (MaskType != TagAddressMaskType.None)
            {
                if (MaskType == TagAddressMaskType.AndMask)
                {
                    result = (ushort)(result & Mask);
                }
                else
                {
                    result = (ushort)(result | Mask);
                }
            }
            InnerValue = result;
        }

        internal override void TagValueChanged()
        {
            ushort newValue = Value;
            if (Math.Abs(newValue - oldValue) > Range)
            {
                oldValue = newValue;
                RaiseEvent();
            }
        }
    }
}