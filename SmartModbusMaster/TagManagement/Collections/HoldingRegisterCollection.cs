namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections
{
    using Modbus;
    using System.Collections.Generic;
    using Types;

    public class HoldingRegisterCollection : FunctionsCollection
    {
        public HoldingRegisterCollection(ushort parsesize)
        {
            ParseSize = parsesize;
            HoldingValues = new Dictionary<ushort, ushort>();
        }

        public Dictionary<ushort, ushort> HoldingValues { get; private set; }

        public void RefreshValues(ReadMap map, ushort[] modbusvalues)
        {
            //TODO : May performance better
            for (int i = 0; i < modbusvalues.Length; i++)
            {
                ushort address = (ushort)(i + map.StartAddress);
                if (!HoldingValues.ContainsKey(address))
                {
                    HoldingValues.Add(address, modbusvalues[i]);
                }
                else
                {
                    HoldingValues[address] = modbusvalues[i];
                }
            }
            foreach (var item in Values)
            {
                if (item is UshortTag)
                {
                    (item as UshortTag).SetData(HoldingValues);
                }
                if (item is FloatTag)
                {
                    (item as FloatTag).SetData(HoldingValues);
                }
            }
        }
    }
}