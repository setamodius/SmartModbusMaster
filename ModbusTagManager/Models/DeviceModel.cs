using System.Collections.ObjectModel;

namespace ModbusTagManager.Models
{
    public class DeviceModel : ObservableObject
    {
        private byte _deviceid;

        private string _ip;

        private bool _isactive;

        private bool _isdevice;

        private string _name;

        private int _port;

        private int _refreshrate;

        public DeviceModel()
        {
            Tags = new ObservableCollection<TagModel>();
            IsDevice = true;
            IsActive = true;
        }

        public byte DeviceId
        {
            get { return _deviceid; }
            set { SetProperty(ref _deviceid, value); }
        }

        public string Ip
        {
            get { return _ip; }
            set { SetProperty(ref _ip, value); }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set { SetProperty(ref _isactive, value); }
        }

        public bool IsDevice
        {
            get { return _isdevice; }
            set { SetProperty(ref _isdevice, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        public int RefreshRate
        {
            get { return _refreshrate; }
            set { SetProperty(ref _refreshrate, value); }
        }

        public ObservableCollection<TagModel> Tags { get; private set; }
    }
}