using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kr.Communication.SmartModbusMaster.Converters
{
    public class LFUintConverter : IIntConverter
    {
        public uint ConvertToUint(ushort value1, ushort value2)
        {
            return (uint)(value1 * (ushort.MaxValue + 1)) + value2;
        }

        public ushort[] ConvertToUshort(uint value)
        {
            ushort value1 = (ushort)(value / (ushort.MaxValue + 1));
            ushort value2 = (ushort)(value % (ushort.MaxValue + 1));
            return new ushort[]{ value1,value2};
        }
    }
}
