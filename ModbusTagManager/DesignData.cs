using ModbusTagManager.Models;
using System.Collections.ObjectModel;

namespace ModbusTagManager
{
    public class DesignData:ObservableObject
    {
        public static DesignData DesignDataReference;
        private ObservableCollection<DeviceModel> myDevices;

        public DesignData()
        {
            //fillDemoData();
            
            DesignDataReference = this;
        }

        private void fillDemoData()
        {
            myDevices = new ObservableCollection<DeviceModel>();
            DeviceModel device1 = new DeviceModel();
            device1.Name = "Device 1";
            device1.Ip = "10.0.0.247";
            device1.Port = 247;
            device1.RefreshRate = 1000;
            device1.DeviceId = 0x12;

            TagModel tag1 = new TagModel();
            TagModel tag2 = new TagModel();
            device1.Tags.Add(tag1);
            device1.Tags.Add(tag2);
            myDevices.Add(device1);
        }

        public ObservableCollection<DeviceModel> GetAllData
        {
            get
            {
                return myDevices;
            }
            private set { SetProperty(ref myDevices, value); }

        }

        public void ReadDataFromFile(string file)
        {
            GetAllData = FileParser.ReadFile(file);
        }
    }
}