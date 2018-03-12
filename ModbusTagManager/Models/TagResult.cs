using Kr.Communication.SmartModbusMaster.TagManagement;
using Kr.Communication.SmartModbusMaster.TagManagement.Types;
using System;

namespace ModbusTagManager.Models
{
    public class TagResult : ObservableObject
    {
        private string _function;
        private string _name;

        private bool _quality;

        private object _value;

        public TagResult(Tag tag)
        {
            SetTag(tag);
        }

        public string Function
        {
            get { return _function; }
            set { SetProperty(ref _function, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public bool Quality
        {
            get { return _quality; }
            set { SetProperty(ref _quality, value); }
        }

        public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private bool _isWrite;

        public bool IsWrite
        {
            get { return _isWrite; }
            set { SetProperty(ref _isWrite, value); }
        }

        private string _writeValue;

        public string WriteValue
        {
            get { return _writeValue; }
            set { SetProperty(ref _writeValue, value); }
        }
        private Type _tagType;

        public Type TagType
        {
            get { return _tagType; }
            set { _tagType = value; }
        }

        public void SetTag(Tag tag)
        {
            Name = tag.Name;
            if (tag.TagDirection == Direction.Write)
            {
                _isWrite = true;
            }
            else
            {
                _isWrite = false;
                Quality = tag.Quality;
            }
            Value = tag.Value;
            Function = tag.InnerTag.Function.Value.ToString();
            if (tag.InnerTag is BoolTag)
            {
                TagType = typeof(bool);
            }
            if (tag.InnerTag is UshortTag)
            {
                TagType = typeof(ushort);
            }
            if (tag.InnerTag is FloatTag)
            {
                TagType = typeof(float);
            }
        }
    }
}