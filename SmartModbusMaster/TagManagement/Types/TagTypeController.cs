namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    using Converters;
    using Modbus;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum Direction
    {
        Read,
        Write
    }

    public enum TagAddressMaskType
    {
        OrMask,
        AndMask,
        None
    }

    public enum TagAddressMergeType
    {
        OrMerge,
        AndMerge
    }

    public enum TagType
    {
        BoolTag,
        UshortTag,
        FloatTag
    }

    public abstract class TagTypeController<TType, TModbusType> : ITagType
    {
        protected TType InnerValue;
        protected Dictionary<ushort, TModbusType> modbusValueData = new Dictionary<ushort, TModbusType>();
        protected TType WriteValue;

        public TagTypeController(TagAddressMergeType mergetype = TagAddressMergeType.OrMerge, TagAddressMaskType masktype = TagAddressMaskType.OrMask)
        {
            if (typeof(TType) == typeof(float) && typeof(TModbusType) == typeof(ushort))
            {
                InnerTagType = TagType.FloatTag;
            }
            else if (typeof(TType) == typeof(ushort) && typeof(TModbusType) == typeof(ushort))
            {
                InnerTagType = TagType.UshortTag;
            }
            else if (typeof(TType) == typeof(bool) && typeof(TModbusType) == typeof(bool))
            {
                InnerTagType = TagType.BoolTag;
            }
            else
            {
                throw new ArgumentException();
            }

            MergeType = mergetype;
            MaskType = masktype;
            Mask = default;
        }

        public event TagValueChangedEventHandler TagValueChangedEvent;

        public IFloatConverter FloatConverter { get; set; }

        public ModbusFunction Function { get; set; }

        public TagType InnerTagType { get; private set; }

        public TType Mask { get; set; }

        public TagAddressMaskType MaskType { get; set; }

        public TagAddressMergeType MergeType { get; set; }

        public TType Range { get; set; }

        public bool IsDefault { get; private set; } = true;

        public TType Value
        {
            get
            {
                IsDefault = false;
                CalculateReturnValue();
                return InnerValue;
            }
            set
            {
                WriteValue = value;
            }
        }

        public void AddAddress(ushort address)
        {
            if (!modbusValueData.ContainsKey(address))
            {
                modbusValueData.Add(address, default);
            }
        }

        public ushort[] GetAddresses()
        {
            return modbusValueData.Count > 0 ? modbusValueData.Keys.ToArray() : new ushort[1];
        }

        public void SetData(Dictionary<ushort, TModbusType> values)
        {
            //bool isChanged = false;
            foreach (var item in modbusValueData.Keys.ToArray())
            {
                if (values.ContainsKey(item) && !EqualityComparer<TModbusType>.Default.Equals(values[item], modbusValueData[item]))
                {
                    //isChanged = true;
                    modbusValueData[item] = values[item];
                }
            }
            TagValueChanged();
            //if (isChanged)
            //{
            //    TagValueChanged();
            //}
        }

        internal abstract void CalculateReturnValue();

        internal abstract void TagValueChanged();

        protected void RaiseEvent()
        {
            TagValueChangedEvent?.Invoke(this);
        }
    }
}