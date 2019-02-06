namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using System;
    using System.Collections.Generic;
    using TagManagement;
    using TagManagement.Collections;
    using TagManagement.Types;

    public class Device : IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool isdeviceConnected = false;
        private ModbusMaster myModbusMaster;
        private readonly Queue<Tag> writequeue = new Queue<Tag>();

        public Device(string name, string ip, int port, byte deviceid, int refreshrate, bool isactive)
        {
            logger.Trace("{0}({1}) device is creating.", name, ip);
            Name = name;
            Ip = ip;
            Port = port;
            Id = deviceid;
            RefreshRate = refreshrate;
            IsActive = isactive;
            Collection = new TagCollection();
            myModbusMaster = new ModbusMaster(this);
            myModbusMaster.ConnectionStateChanged += MyModbusMaster_ConnectionStateChanged;
        }

        ~Device()
        {
            logger.Trace("{0}({1}) device is disposing.", Name, Ip);
        }

        public TagCollection Collection { get; private set; }

        public byte Id { get; set; }

        public string Ip { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public int Port { get; set; }

        public int RefreshRate { get; set; }

        public void Dispose()
        {
            myModbusMaster.Dispose();
        }

        public void Start()
        {
            logger.Trace("{0}({1}) device is starting.", Name, Ip);
            foreach (var item in Collection.GetAllTags())
            {
                item.Quality = false;
            }
            myModbusMaster.Start();
        }

        public void WriteValueToTag(string tagname, object value)
        {
            var currentTag = Collection.GetReadTagWithName(tagname);

            if (currentTag == null)
            {
                logger.Error("{0}({1}) device write error. Tag not found : {2} ", Name, Ip, tagname);
                return;
            }
            logger.Trace("{0}({1}) device write.  Tag name : {2} Tag Value : {3}", Name, Ip, currentTag?.Name, value);
            if (isdeviceConnected)
            {
                WriteTagHelper(currentTag, value);
            }
            else
            {
                logger.Warn("{0}({1}) device not connected. Tag not written : {2} ", Name, Ip, tagname);
            }
        }

        private void MyModbusMaster_ConnectionStateChanged(ModbusMaster sender, bool status)
        {
            isdeviceConnected = status;
            foreach (var item in Collection.GetAllTags())
            {
                item.Quality = status;
            }
        }

        private void WriteTagHelper(Tag tag, object value)
        {
            if (value == null)
            {
                logger.Error("Value is NULL for {0}({1}) tag : {2}", Name, Ip, tag?.Name);
                return;
            }
            if (tag.InnerTag == null)
            {
                logger.Error("InnerTag is NULL for {0}({1}) tag : {2}", Name, Ip, tag.Name);
                return;
            }
            logger.Trace("{0}({1}) device - InnerTag={2}, value={3}", Name, Ip, tag.InnerTag.GetType().ToString(), value.GetType().ToString());
            if (tag.InnerTag is BoolTag && value is bool)
            {
                var currentBoolTag = (BoolTag)tag.InnerTag;
                currentBoolTag.Value = (bool)value;
                myModbusMaster?.WriteBoolTagValue(currentBoolTag);
            }
            if (tag.InnerTag is UshortTag && value is ushort)
            {
                var currentUshortTag = (UshortTag)tag.InnerTag;
                currentUshortTag.Value = (ushort)value;
                myModbusMaster?.WriteUshortTagValue(currentUshortTag);
            }
            if (tag.InnerTag is FloatTag && value is float)
            {
                var currentFloatTag = (FloatTag)tag.InnerTag;
                currentFloatTag.Value = (ushort)value;
                myModbusMaster?.WriteFloatTagValue(currentFloatTag);
            }
        }
    }
}