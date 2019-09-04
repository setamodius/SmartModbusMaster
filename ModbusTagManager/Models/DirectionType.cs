namespace ModbusTagManager.Models
{
    public class DirectionType
    {
        public static readonly DirectionType Read = new DirectionType("Read", "read");
        public static readonly DirectionType Write = new DirectionType("Write", "write");

        public static readonly DirectionType[] Types = new DirectionType[]
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