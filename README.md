# SmartModbusMaster

It converts Modbus polling function to event. It uses NModbus4 


## Usage
```csharp
class Program
{
    static void Main(string[] args)
    {           
        myDevices = Creator.FromFile("addresses.csv");
	if (myDevices == null)
        {
            return;
        }
        myDevices.TagStatusChanged += MyDevices_TagStatusChanged;      
        myDevices.StartDevices();

        Console.ReadLine();
    }
    
    private static void MyDevices_TagStatusChanged(object sender, TagChangedEventArgs e)
    {
        if (e.quality)
        {
             Console.WriteLine(e.Name + ": " + e.Value);
        }
    }
}

```
## CSV File
The CSV file consists of two parts. The first section contains device information. The second section contains address information.

**Device section:**

| Type    | Device  | Ip         | Port | Device ID | Refresh Rate | IsActive |
| ------- | ------- | ---------- | ---- | --------- | ------------ | -------- |
| device  | Device1 | 10.3.4.247 | 502  | 1         | 1000         | 1        |
| device  | Device2 | 10.3.4.248 | 502  | 2         | 2000         | 1        |

| Column Name | Description |
| --- | --- |
| Type | Must be 'device' for device section |
| Device | Device name |
| Ip | IP address to connect  |
| Port | Port to connect |
| Device ID  | An unique number for device |
| Refresh Rate | Refresh rate in milliseconds |
| IsActive | 1: Active, 0: deactive |

**Tag section:**

| Type | Tag      | Device  | Address | Modbus Type | Direction | Type | MaskType | Mask | Merge Type | Range |
| ---- | -------- | ------- | ------- | ----------- | --------- | ---- | -------- | ---- | ---------- | ----- |
| tag  | TagName1 | Device1 | 1       | cs          | read      | bool | none     | 0    | ormerge    | 0     |
| tag  | TagName2 | Device1 | 2       | cs          | read      | bool | none     | 0    | ormerge    | 0     |
| tag  | TagName3 | Device2 | 2       | cs          | read      | bool | none     | 0    | ormerge    | 0     |

| Column Name | Description |
| --- | --- |
| Tag | Must be 'tag' for tag section |
| Device | Device name written in the device section |
| Address | read address x: only reads x, x y: reads x and y, x-y: reads x to y|
| Modbus Type | cs: Coil Status, is: Input status, hr: holding register, ir: input register |
| Type  | bool: boolean, ushort: ushort, lsrf: lsrf float, msrf: msrf float |
| MaskType | none, andmask, ormask |
| Mask | ushort mask value |
| Merge Type | ormerge, andmerge |
| Range | ushort range value |

_Sample CSV file:_

```
//device,Device,Ip,Port,DeviceId,RefreshRate,IsActive
device,Device1,10.3.4.247,502,1,1000,1
device,Device2,10.3.4.248,502,2,1000,1
//tag,Tag,Device,Address,ModbusType(cs/is/hr/ir),Direction(read/write),Type(bool/ushort/lsrf/msrf),MaskType(none/andmask/ormask),Mask,MergeType(andmerge/ormerge),Range
tag,TagName1,Device1,1,cs,read,bool,none,0,ormerge,0
tag,TagName2,Device1,1,cs,read,bool,none,0,ormerge,0
tag,TagName3,Device2,1,cs,read,bool,none,0,ormerge,0
```
