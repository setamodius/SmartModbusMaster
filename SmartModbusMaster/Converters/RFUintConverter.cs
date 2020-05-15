using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kr.Communication.SmartModbusMaster.Converters
{
    public class RFUintConverter : IIntConverter
    {
        public uint ConvertToUint(ushort value1, ushort value2)
        {
            return (uint)(value2 * (ushort.MaxValue + 1)) + value1;
        }

        public ushort[] ConvertToUshort(uint value)
        {
            ushort value2 = (ushort)(value / (ushort.MaxValue + 1));
            ushort value1 = (ushort)(value % (ushort.MaxValue + 1));
            return new ushort[]{ value2,value1};
        }
    }
}
