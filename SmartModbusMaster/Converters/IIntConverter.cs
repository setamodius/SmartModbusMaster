using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kr.Communication.SmartModbusMaster.Converters
{
    public interface IIntConverter
    {

        uint ConvertToUint(ushort value1, ushort value2);

        ushort[] ConvertToUshort(uint value);
    }
}
