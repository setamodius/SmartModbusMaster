namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    using System;

    public class FloatTag : TagTypeController<float, ushort>
    {
        private float? oldValue;

        public FloatTag(Modbus.RegisterFunction function)
        {
            Function = function;
        }

        public ushort[] GetWriteValue()
        {
            if (FloatConverter == null)
            {
                throw new NullReferenceException();
            }
            return FloatConverter.ConvertToUshort(WriteValue);
        }

        internal override void CalculateReturnValue()
        {
            if (FloatConverter == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                if (modbusValueData.Values.Count > 1)
                {
                    int indexno = 0;
                    ushort[] values = new ushort[2];
                    foreach (var item in base.modbusValueData.Values)
                    {
                        values[indexno] = item;
                        indexno++;
                        if (indexno == 2)
                        {
                            break;
                        }
                    }
                    InnerValue = FloatConverter.ConvertToFloat(values[0], values[1]);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        internal override void TagValueChanged()
        {
            float newValue = Value;
            if (!oldValue.HasValue || Math.Abs(newValue - oldValue.Value) > Range)
            {
                oldValue = newValue;
                RaiseEvent();
            }
        }
    }
}