namespace ModbusTagManager.Models
{
    public class ModbusType
    {
        public static ModbusType CoilStatus = new ModbusType("01 - Coil Status", "cs");
        public static ModbusType HoldingRegister = new ModbusType("03 - Holding Register", "hr");
        public static ModbusType InputRegister = new ModbusType("04 - Input Register", "ir");
        public static ModbusType InputStatus = new ModbusType("02 - Input Status", "is");

        public static ModbusType[] Types = new ModbusType[]
        {
            CoilStatus,InputStatus,HoldingRegister,InputRegister
        };

        private ModbusType(string value, string nick)
        {
            Value = value;
            Nick = nick;
        }

        public string Nick { get; private set; }
        public string Value { get; private set; }
    }
}