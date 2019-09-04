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