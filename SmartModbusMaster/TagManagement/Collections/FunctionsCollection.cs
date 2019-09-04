namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections
{
    using Modbus;
    using System.Collections.Generic;
    using System.Linq;
    using Types;

    public abstract class FunctionsCollection : Dictionary<string, ITagType>
    {
        private readonly HashSet<ushort> addresses = new HashSet<ushort>();
        public ReadMap[] Maps { get; private set; }
        public ushort ParseSize { get; set; }

        new public void Add(string name, ITagType tag)
        {
            if (!ContainsKey(name))
            {
                AddAddresses(tag);
                base.Add(name, tag);
            }
        }

        public void CalculteParseAddresses()
        {
            if (addresses.Count == 0)
            {
                return;
            }
            ushort count = (ushort)((addresses.Max() - addresses.Min()) + 1);
            ushort poolCount = (ushort)(((count - 1) / ParseSize) + 1);
            ReadMap[] maps = new ReadMap[poolCount];
            for (int i = 0; i < poolCount; i++)
            {
                maps[i].StartAddress = (ushort)(addresses.Min() + (i * ParseSize));
                maps[i].Range = ParseSize;
            }
            maps[poolCount - 1].Range = (ushort)(count - ((poolCount - 1) * ParseSize));
            Maps = maps;
        }

        private void AddAddresses(ITagType tag)
        {
            foreach (var item in tag.GetAddresses())
            {
                addresses.Add(item);
            }
        }
    }
}