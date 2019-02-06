namespace ModbusTagManager.Models
{
    public class ValueType
    {
        public static ValueType Bool = new ValueType("Boolean", "bool");
        public static ValueType FloatLSRF = new ValueType("Float (LSRF)", "lsrf");
        public static ValueType FloatMSRF = new ValueType("FLoat (MSRF)", "msrf");
        public static ValueType Ushort = new ValueType("Ushort", "ushort");

        public static ValueType[] Types = new ValueType[]
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