// ********************************************************************
//
// Copyright (c) 2015, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.TagManagement
{
    using Converters;
    using Modbus;
    using Types;

    public delegate void TagStatusChangeEventHandler(Tag sender, object value, bool quality);

    public class Tag
    {
        private bool _quality = false;

        private object _value;

        public Tag(string tagname)
        {
            Name = tagname;
        }

        public event TagStatusChangeEventHandler TagStatusChanged;

        public ITagType InnerTag { get; private set; }
        public string Name { get; private set; }

        public bool Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                TagStatusChanged?.Invoke(this, _value, value);
            }
        }

        public Direction TagDirection { get; set; }

        public object Value
        {
            get
            {
                if (InnerTag is BoolTag)
                {
                    return ((BoolTag)InnerTag).Value;
                }
                if (InnerTag is FloatTag)
                {
                    return ((FloatTag)InnerTag).Value;
                }
                if (InnerTag is UshortTag)
                {
                    return ((UshortTag)InnerTag).Value;
                }
                return null;
            }
        }
        public void DefineBoolTag(
            StatusFunction function,
            TagAddressMaskType masktype = TagAddressMaskType.None,
            bool mask = false,
            TagAddressMergeType mergetype = TagAddressMergeType.OrMerge)
        {
            if (InnerTag != null)
            {
                return;
            }
            BoolTag innertag = new BoolTag(function);
            innertag.MaskType = masktype;
            innertag.Mask = mask;
            innertag.MergeType = mergetype;
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                _value = innertag.Value;
                TagStatusChanged?.Invoke(this, _value, Quality);
            };
        }

        public void DefineFloatTag(
            RegisterFunction function,
            IFloatConverter converter,
            float range = 0)
        {
            if (InnerTag != null)
            {
                return;
            }
            FloatTag innertag = new FloatTag(function);
            innertag.Range = range;
            innertag.FloatConverter = converter;
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                _value = innertag.Value;
                TagStatusChanged?.Invoke(this, _value, Quality);
            };
        }

        public void DefineUshortTag(
                    RegisterFunction function,
            TagAddressMaskType masktype = TagAddressMaskType.None,
            ushort mask = ushort.MaxValue,
            TagAddressMergeType mergetype = TagAddressMergeType.OrMerge,
            ushort range = ushort.MinValue)
        {
            if (InnerTag != null)
            {
                return;
            }
            UshortTag innertag = new UshortTag(function);
            innertag.MaskType = masktype;
            innertag.Mask = mask;
            innertag.MergeType = mergetype;
            innertag.Range = range;
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                _value = innertag.Value;
                TagStatusChanged?.Invoke(this, _value, Quality);
            };
        }
    }
}