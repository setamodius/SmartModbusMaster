// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections
{
    using Modbus;
    using Types;
    using System.Collections.Generic;

    public class InputRegisterCollection : FunctionsCollection
    {
        public InputRegisterCollection(ushort parsesize)
        {
            ParseSize = parsesize;
            InputRegisterValues = new Dictionary<ushort, ushort>();
        }

        public Dictionary<ushort, ushort> InputRegisterValues { get; private set; }

        public void RefreshValues(ReadMap map, ushort[] modbusvalues)
        {
            //TODO : May performance better
            for (int i = 0; i < modbusvalues.Length; i++)
            {
                ushort address = (ushort)(i + map.StartAddress);
                if (!InputRegisterValues.ContainsKey(address))
                {
                    InputRegisterValues.Add(address, modbusvalues[i]);
                }
                else
                {
                    InputRegisterValues[address] = modbusvalues[i];
                }
            }
            foreach (var item in Values)
            {
                if (item is UshortTag)
                {
                    (item as UshortTag).SetData(InputRegisterValues);
                }
                if (item is FloatTag)
                {
                    (item as FloatTag).SetData(InputRegisterValues);
                }
            }
        }
    }
}