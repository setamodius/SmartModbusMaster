namespace ModbusTagManager.Models
{
    public class ValueType
    {
        public static readonly ValueType Bool = new ValueType("Boolean", "bool");
        public static readonly ValueType FloatLSRF = new ValueType("Float (LSRF)", "lsrf");
        public static readonly ValueType FloatMSRF = new ValueType("FLoat (MSRF)", "msrf");
        public static readonly ValueType Ushort = new ValueType("Ushort", "ushort");

        public static readonly ValueType[] Types = new ValueType[]
        {
            Bool,Ushort,FloatLSRF,FloatMSRF
        };

       
        private ValueType(string value, string nick)
        {
            Value = value;
            Nick = nick;
        }

        public string Nick { get; private set; }
        public string Value { get; private set; }
    }
}