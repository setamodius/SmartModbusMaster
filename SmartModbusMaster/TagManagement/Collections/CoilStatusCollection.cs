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

    public class CoilStatusCollection : FunctionsCollection
    {
        public CoilStatusCollection(ushort parsesize)
        {
            ParseSize = parsesize;
            CoilValues = new Dictionary<ushort, bool>();
        }

        public Dictionary<ushort, bool> CoilValues { get; private set; }

        public void RefreshValues(ReadMap map, bool[] modbusvalues)
        {
            //TODO : May performance better
            for (int i = 0; i < modbusvalues.Length; i++)
            {
                ushort address = (ushort)(i + map.StartAddress);
                if (!CoilValues.ContainsKey(address))
                {
                    CoilValues.Add(address, modbusvalues[i]);
                }
                else
                {
                    CoilValues[address] = modbusvalues[i];
                }
            }
            foreach (var item in Values)
            {
                BoolTag tag = item as BoolTag;
                tag.SetData(CoilValues);
            }
        }
    }
}