# SmartModbusMaster

It converts Modbus polling function to event. It uses NModbus4 


## Usage
```csharp
class Program
{
    static void Main(string[] args)
    {           
        myDevices = Creator.FromFile("addresses.csv");
        foreach (var device in myDevices.Values)
        {
            var readTags = from item in device.Collection.GetAllTags() where item.TagDirection ==
                Kr.Communication.SmartModbusMaster.TagManagement.Types.Direction.Read select item;
            foreach (var tag in device.Collection.GetAllTags())
            {
                tag.TagStatusChanged += Tag_TagStatusChanged;
            }
        }
        myDevices.StartDevices();

        Console.ReadLine();
    }
    
    private static void Tag_TagStatusChanged(Tag sender, object value, bool quality)
    {
        if (quality)
        {
             Console.WriteLine(sender.Name + ": " + sender.Value);
        }
    }
}

```
## CSV File
The CSV file consists of two parts. The first section contains device information. The second section contains address information.
| Type    | Device  | Ip         | Port | Device ID | Refresh Rate | IsActive |
| ------- | ------  | ---------- | ---- | --------- | ------------ | -------- |
| Device  | Device1 | 10.3.4.247 | 502  | 1         | 1000         | 1        |
| Device  | Device2 | 10.3.4.248 | 502  | 2         | 2000         | 1        |



```
//device,Device,Ip,Port,DeviceId,RefreshRate,IsActive
device,Device1,10.3.4.247,502,1,1000,1
//tag,Tag,Device,Address,ModbusType(cs/is/hr/ir),Direction(read/write),Type(bool/ushort/lsfr/msrf),MaskType(none/andmask/ormask),Mask,MergeType(andmerge/ormerge),Range
tag,TagName1,Device1,1,cs,read,bool,none,0,ormerge,0
tag,TagName2,Device1,1,cs,read,bool,none,0,ormerge,0
```
