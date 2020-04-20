namespace Kr.Communication.SmartModbusMaster.TagManagement.Types
{
    public class BoolTag : TagTypeController<bool, bool>
    {
        private bool? oldvalue;

        public BoolTag(Modbus.StatusFunction function)
        {
            Function = function;
        }

        public bool GetWriteValue()
        {
            return WriteValue;
        }

        internal override void CalculateReturnValue()
        {
            bool result = (MergeType == TagAddressMergeType.AndMerge);
            foreach (var item in modbusValueData.Values)
            {
                if (MergeType == TagAddressMergeType.AndMerge)
                {
                    result = result && item;
                }
                else
                {
                    result = result || item;
                }
            }
            if (MaskType != TagAddressMaskType.None)
            {
                if (MaskType == TagAddressMaskType.AndMask)
                {
                    result = result && Mask;
                }
                else
                {
                    result = result || Mask;
                }
            }

            InnerValue = result;
        }

        internal override void TagValueChanged()
        {
            if (!oldvalue.HasValue || oldvalue != Value)
            {
                oldvalue = Value;
                RaiseEvent();
            }
        }
    }
}