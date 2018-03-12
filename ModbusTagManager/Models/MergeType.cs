using System;

namespace ModbusTagManager.Models
{
    [Serializable()]
    public class MergeType
    {
        public static MergeType AndMerge = new MergeType("AND Merge", "andmerge");
        public static MergeType OrMerge = new MergeType("OR Merge", "ormerge");

        public static MergeType[] Types = new MergeType[]
        {
            OrMerge,AndMerge
        };

        private MergeType(string value, string nick)
        {
            Value = value;
            Nick = nick;
        }

        public string Nick { get; private set; }
        public string Value { get; private set; }
    }
}