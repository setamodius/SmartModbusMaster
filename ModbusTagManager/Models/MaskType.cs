namespace ModbusTagManager.Models
{
    public class MaskType
    {
        public static MaskType AndMask = new MaskType("AND Mask", "andmask");
        public static MaskType None = new MaskType("None", "none");
        public static MaskType OrMask = new MaskType("OR Mask", "ormask");

        public static MaskType[] Types = new MaskType[]
        {
            None,AndMask,OrMask
        };

        private MaskType(string value, string nick)
        {
            Value = value;
            Nick = nick;
        }

        public string Nick { get; private set; }
        public string Value { get; private set; }
    }
}