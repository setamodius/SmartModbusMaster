namespace ModbusTagManager.Models
{
    public class TagModel : ObservableObject
    {
        private string _addressstring;
        private bool _isdevice;
        private string _mask;
        private MergeType _mergetype;
        private string _name;
        private string _range;
        private DirectionType _tagdirectiontype;
        private MaskType _tagmasktype;
        private ModbusType _tagmodbustype;
        private ValueType _tagvaluetype;

        public TagModel()
        {
            Name = "Tag Name";
            TagModbusType = ModbusType.CoilStatus;
            TagValueType = ValueType.Bool;
            TagDirectionType = DirectionType.Read;
            AddressString = "1";
            TagMaskType = MaskType.None;
            Mask = "";
            TagMergeType = MergeType.OrMerge;
            IsDevice = false;
            Range = "0";
        }

        public string AddressString
        {
            get { return _addressstring; }
            set { SetProperty(ref _addressstring, value); }
        }

        public bool IsDevice
        {
            get { return _isdevice; }
            set { SetProperty(ref _isdevice, value); }
        }

        public string Mask
        {
            get { return _mask; }
            set { SetProperty(ref _mask, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Range
        {
            get { return _range; }
            set { SetProperty(ref _range, value); }
        }

        public DirectionType TagDirectionType
        {
            get { return _tagdirectiontype; }
            set { SetProperty(ref _tagdirectiontype, value); }
        }

        public MaskType TagMaskType
        {
            get { return _tagmasktype; }
            set { SetProperty(ref _tagmasktype, value); }
        }

        public MergeType TagMergeType
        {
            get { return _mergetype; }
            set { SetProperty(ref _mergetype, value); }
        }

        public ModbusType TagModbusType
        {
            get { return _tagmodbustype; }
            set
            {
                if (value == ModbusType.CoilStatus || value == ModbusType.InputStatus)
                {
                    TagValueType = ValueType.Bool;
                }
                SetProperty(ref _tagmodbustype, value);
            }
        }

        public ValueType TagValueType
        {
            get { return _tagvaluetype; }
            set { SetProperty(ref _tagvaluetype, value); }
        }
    }
}