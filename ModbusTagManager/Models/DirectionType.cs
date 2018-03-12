namespace ModbusTagManager.Models
{
    public class DirectionType
    {
        public static DirectionType Read = new DirectionType("Read", "read");
        public static DirectionType Write = new DirectionType("Write", "write");

        public static DirectionType[] Types = new DirectionType[]
        {
            Read,Write
        };

        

        private DirectionType(string value, string nick)
        {
            Value = value;
            Nick = nick;
        }

        public string Nick { get; private set; }
        public string Value { get; private set; }
    }
}