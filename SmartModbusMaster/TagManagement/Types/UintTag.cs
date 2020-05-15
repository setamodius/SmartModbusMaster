using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    public class UintTag : TagTypeController<uint, ushort>
    {
        private float? oldValue;

        public UintTag(Modbus.RegisterFunction function)
        {
            Function = function;
        }

        public ushort[] GetWriteValue()
        {
            if (IntConverter == null)
            {
                throw new NullReferenceException();
            }
            return IntConverter.ConvertToUshort(WriteValue);
        }

        internal override void CalculateReturnValue()
        {
            if (IntConverter == null)
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
                    InnerValue = IntConverter.ConvertToUint(values[0], values[1]);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        internal override void TagValueChanged()
        {
            uint newValue = Value;
            if (!oldValue.HasValue || Math.Abs(newValue - oldValue.Value) > Range)
            {
                oldValue = newValue;
                RaiseEvent();
            }
        }
    }
}
