namespace Kr.Communication.SmartModbusMaster.Converters
{
    public interface IFloatConverter
    {
        float ConvertToFloat(ushort value1, ushort value2);

        ushort[] ConvertToUshort(float value);
    }
}