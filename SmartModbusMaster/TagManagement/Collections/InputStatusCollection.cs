// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections
{
    using Modbus;
    using System.Collections.Generic;
    using Types;

    public class InputStatusCollection : FunctionsCollection
    {
        public InputStatusCollection(ushort parsesize)
        {
            ParseSize = parsesize;
            InputValues = new Dictionary<ushort, bool>();
        }

        public Dictionary<ushort, bool> InputValues { get; private set; }

        public void RefreshValues(ReadMap map, bool[] modbusvalues)
        {
            //TODO : May performance better
            for (int i = 0; i < modbusvalues.Length; i++)
            {
                ushort address = (ushort)(i + map.StartAddress);
                if (!InputValues.ContainsKey(address))
                {
                    InputValues.Add(address, modbusvalues[i]);
                }
                else
                {
                    InputValues[address] = modbusvalues[i];
                }
            }
            foreach (var item in Values)
            {
                BoolTag tag = item as BoolTag;
                tag.SetData(InputValues);
            }
        }
    }
}