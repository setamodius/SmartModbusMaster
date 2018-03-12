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

    public class TagCollection
    {
        private Dictionary<string, BoolTag> allBoolTags = new Dictionary<string, BoolTag>();
        private Dictionary<string, FloatTag> allFloatTags = new Dictionary<string, FloatTag>();
        private Dictionary<string, Tag> allTags = new Dictionary<string, Tag>();
        private Dictionary<string, UshortTag> allUshortTags = new Dictionary<string, UshortTag>();
        private Dictionary<string, Tag> allWriteTags = new Dictionary<string, Tag>();

        public TagCollection()
        {
            CoilStatuses = new CoilStatusCollection(2000);
            InputStatuses = new InputStatusCollection(2000);
            HoldingRegisters = new HoldingRegisterCollection(125);
            InputRegisters = new InputRegisterCollection(125);
        }

        public event TagStatusChangeEventHandler TagStatusChanged;

        public CoilStatusCollection CoilStatuses { get; private set; }

        public HoldingRegisterCollection HoldingRegisters { get; private set; }

        public InputRegisterCollection InputRegisters { get; private set; }

        public InputStatusCollection InputStatuses { get; private set; }

        public bool AddTag(Tag addingTag)
        {
            if (!allTags.ContainsKey(addingTag.Name)
                && addingTag.InnerTag != null)
            {
                allTags.Add(addingTag.Name, addingTag);
                classificator(addingTag);
                return true;
            }
            return false;
        }

        public void CalculateParseAddresses()
        {
            CoilStatuses.CalculteParseAddresses();
            InputStatuses.CalculteParseAddresses();
            HoldingRegisters.CalculteParseAddresses();
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return allTags.Values;
        }

        public BoolTag GetBoolTagWithName(string name)
        {
            if (allBoolTags.ContainsKey(name))
            {
                return allBoolTags[name];
            }
            return null;
        }

        public FloatTag GetFloatTagWithName(string name)
        {
            if (allFloatTags.ContainsKey(name))
            {
                return allFloatTags[name];
            }
            return null;
        }

        public Tag GetReadTagWithName(string name)
        {
            if (allWriteTags.ContainsKey(name))
            {
                return allWriteTags[name];
            }
            return null;
        }

        public Tag GetTagWithName(string name)
        {
            if (allTags.ContainsKey(name))
            {
                return allTags[name];
            }
            return null;
        }

        public UshortTag GetUshortTagWithName(string name)
        {
            if (allUshortTags.ContainsKey(name))
            {
                return allUshortTags[name];
            }
            return null;
        }

        private void classificator(Tag addingTag)
        {
            if (addingTag.TagDirection == Direction.Write)
            {
                if (!allWriteTags.ContainsKey(addingTag.Name))
                {
                    allWriteTags.Add(addingTag.Name, addingTag);
                }
                return;
            }
            if (addingTag.InnerTag is BoolTag)
            {
                if (!allBoolTags.ContainsKey(addingTag.Name))
                {
                    allBoolTags.Add(addingTag.Name, (addingTag.InnerTag as BoolTag));
                }
            }
            else if (addingTag.InnerTag is UshortTag)
            {
                allUshortTags.Add(addingTag.Name, (addingTag.InnerTag as UshortTag));
            }
            else if (addingTag.InnerTag is FloatTag)
            {
                allFloatTags.Add(addingTag.Name, (addingTag.InnerTag as FloatTag));
            }

            if (addingTag.InnerTag.Function == StatusFunction.CoilStatus)
            {
                if (!CoilStatuses.ContainsKey(addingTag.Name))
                {
                    CoilStatuses.Add(addingTag.Name, (addingTag.InnerTag));
                }
            }
            else if (addingTag.InnerTag.Function == StatusFunction.InputStatus)
            {
                if (!InputStatuses.ContainsKey(addingTag.Name))
                {
                    InputStatuses.Add(addingTag.Name, (addingTag.InnerTag));
                }
            }
            else if (addingTag.InnerTag.Function == RegisterFunction.HoldingRegister)
            {
                if (!InputStatuses.ContainsKey(addingTag.Name))
                {
                    HoldingRegisters.Add(addingTag.Name, (addingTag.InnerTag));
                }
            }
            else if (addingTag.InnerTag.Function == RegisterFunction.InputRegister)
            {
                if (!InputStatuses.ContainsKey(addingTag.Name))
                {
                    InputRegisters.Add(addingTag.Name, (addingTag.InnerTag));
                }
            }

            addingTag.TagStatusChanged += delegate (Tag sender, object value, bool quality)
            {
                TagStatusChanged?.Invoke(sender, value, quality);
            };
        }
    }
}