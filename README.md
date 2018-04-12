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
