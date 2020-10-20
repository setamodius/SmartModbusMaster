namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using Kr.Communication.SmartModbusMaster.Diagnostic;
    using System;
    using System.Collections.Generic;
    using TagManagement;
    using TagManagement.Collections;
    using TagManagement.Types;

    public class Device : IDisposable
    {
        readonly ICoreLogger logger;
        private bool isdeviceConnected = false;
        private readonly ModbusMaster myModbusMaster;        
        public event EventHandler ConnectionStatusChanged;

        public Device(string name, string ip, int port, byte deviceid, int refreshrate, bool isactive, ICoreLogger coreLogger)
        {
            logger = coreLogger ?? throw new ArgumentNullException(nameof(coreLogger));
            logger.Info($"{name}({ip}) device is creating.");
            Name = name;
            Ip = ip;
            Port = port;
            Id = deviceid;
            RefreshRate = refreshrate;
            IsActive = isactive;
            Collection = new TagCollection();
            myModbusMaster = new ModbusMaster(this,logger);
            myModbusMaster.ConnectionStateChanged += MyModbusMaster_ConnectionStateChanged;
        }

        ~Device()
        {
            logger.Info($"{Name}({Ip}) device is disposing.");
        }

        public TagCollection Collection { get; private set; }

        public byte Id { get; set; }

        public string Ip { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public int Port { get; set; }

        public int RefreshRate { get; set; }

        public bool IsConnected { get; set; }

        public void Dispose()
        {
            myModbusMaster.Dispose();
        }

        public void Start()
        {
            logger.Info($"{Name}({Ip}) device is starting.");
            foreach (var item in Collection.GetAllTags())
            {
                item.Quality = false;
            }
            myModbusMaster.Start();
        }

        public bool WriteValueToTag(string tagname, object value)
        {
            var currentTag = Collection.GetWriteTagWithName(tagname);

            if (currentTag == null)
            {
                logger.Warning(null, $"{Name}({Ip}) device write error. Tag not found : {tagname}");
                return false;
            }
            logger.Trace($"{Name}({Ip}) device write.  Tag name : {currentTag?.Name} Tag Value : {value}");
            if (isdeviceConnected)
            {
                WriteTagHelper(currentTag, value);
                return true;
            }
            else
            {
                logger.Warning(null, $"{Name}({Ip}) device not connected. Tag not written : {tagname} ");
                return false;
            }
        }

        public IEnumerable<string> GetAllWriteTags()
        {
            return Collection.GetAllWriteTags();
        }

        private void MyModbusMaster_ConnectionStateChanged(ModbusMaster sender, bool status)
        {
            isdeviceConnected = status;
            IsConnected = status;
            ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
            foreach (var item in Collection.GetAllTags())
            {
                item.Quality = status;
            }
        }

        private void WriteTagHelper(Tag tag, object value)
        {
            if (value == null)
            {
                logger.Warning(null, $"Value is NULL for {Name}({Ip}) tag : {tag?.Name}");
                return;
            }

            if (tag.InnerTag == null)
            {
                logger.Warning(null, $"InnerTag is NULL for {Name}({Ip}) tag : {tag.Name}");
                return;
            }

            logger.Trace($"{Name}({Ip}) device - InnerTag={tag.InnerTag.GetType()}, value={value.GetType()}");
            if (tag.InnerTag is BoolTag booltag && value is bool booleanvalue)
            {
                var currentBoolTag = booltag;
                currentBoolTag.Value = booleanvalue;
                myModbusMaster?.WriteBoolTagValue(currentBoolTag);
            }

            if (tag.InnerTag is UshortTag ushorttag && value is ushort ushortvalue)
            {
                var currentUshortTag = ushorttag;
                currentUshortTag.Value = ushortvalue;
                myModbusMaster?.WriteUshortTagValue(currentUshortTag);
            }

            if (tag.InnerTag is FloatTag floattag && value is float floatvalue)
            {
                var currentFloatTag = floattag;
                currentFloatTag.Value = floatvalue;
                myModbusMaster?.WriteFloatTagValue(currentFloatTag);
            }
        }
    }
}