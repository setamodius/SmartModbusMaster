namespace Kr.Communication.SmartModbusMaster.TagManagement
{
    using System;
    using Converters;
    using Modbus;
    using Types;   

    public class Tag
    {
        private bool _quality = false;        

        public Tag(string tagname)
        {
            Name = tagname;
        }

        public event EventHandler TagStatusChanged; 

        public ITagType InnerTag { get; private set; }
        public string Name { get; private set; }

        public bool Quality
        {
            get { return _quality; }
            set
            {               
                _quality = value;                
                if (InnerTag != null && !InnerTag.IsDefault)
                {
                    TagStatusChanged?.Invoke(this, EventArgs.Empty);
                }
                                            
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
                if (InnerTag is UintTag)
                {
                    return ((UintTag)InnerTag).Value;
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
            BoolTag innertag = new BoolTag(function)
            {
                MaskType = masktype,
                Mask = mask,
                MergeType = mergetype
            };
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                TagStatusChanged?.Invoke(this, EventArgs.Empty);
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
            FloatTag innertag = new FloatTag(function)
            {
                Range = range,
                FloatConverter = converter
            };
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                TagStatusChanged?.Invoke(this, EventArgs.Empty);                
            };
        }

        public void DefineUintTag(
            RegisterFunction function,
            IIntConverter converter,
            uint range = 0)
        {
            if (InnerTag != null)
            {
                return;
            }
            UintTag innertag = new UintTag(function)
            {
                Range = range,
                IntConverter = converter
            };
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                TagStatusChanged?.Invoke(this, EventArgs.Empty);
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
            UshortTag innertag = new UshortTag(function)
            {
                MaskType = masktype,
                Mask = mask,
                MergeType = mergetype,
                Range = range
            };
            InnerTag = innertag;
            InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                TagStatusChanged?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}