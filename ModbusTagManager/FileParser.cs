using ModbusTagManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Linq;
using System;
using System.Windows;


namespace ModbusTagManager
{
    internal static class FileParser
    {
        public const string separator = ",";

        public static void SaveFile(IEnumerable<DeviceModel> values, string filename)
        {
            StringBuilder fileDeviceString = new StringBuilder();
            StringBuilder fileTagString = new StringBuilder();

            foreach (var item in values)
            {
                DeviceModel deviceModelItem = item as DeviceModel;
                if (deviceModelItem == null)
                {
                    break;
                }
               
                    string deviceInfoline = string.Format("//device{0}Device{0}Ip{0}Port{0}DeviceId{0}RefreshRate{0}IsActive", separator);
                    string deviceline = string.Format("device{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                    separator,
                    deviceModelItem.Name,
                    deviceModelItem.Ip,
                    deviceModelItem.Port,
                    deviceModelItem.DeviceId,
                    deviceModelItem.RefreshRate,
                    deviceModelItem.IsActive ? "1" : "0");

                    if (fileDeviceString.Length==0)
                    {
                       fileDeviceString.AppendLine(deviceInfoline); 
                    }
                    
                    fileDeviceString.AppendLine(deviceline);
                
                
                
                foreach (var tag in deviceModelItem.Tags)
                {
                    TagModel tagModelItem = tag as TagModel;
                    if (tagModelItem == null)
                    {
                        break;
                    }                    								

                    string tagInfoline = string.Format("//tag{0}Tag{0}Device{0}Address{0}ModbusType(cs/is/hr/ir){0}Direction(read/write){0}Type(bool/ushort/lsfr/msrf){0}MaskType(none/andmask/ormask){0}Mask{0}MergeType(andmerge/ormerge){0}Range", separator);
                    string tagline = string.Format("tag{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}",
                    separator,
                    tagModelItem.Name,
                    deviceModelItem.Name,
                    tagModelItem.AddressString,
                    tagModelItem.TagModbusType.Nick,
                    tagModelItem.TagDirectionType.Nick,
                    tagModelItem.TagValueType.Nick,
                    tagModelItem.TagMaskType.Nick,
                    tagModelItem.Mask,
                    tagModelItem.TagMergeType.Nick,
                    tagModelItem.Range);

                    if (fileTagString.Length == 0)
                    {
                        fileTagString.AppendLine(tagInfoline);
                    }

                    fileTagString.AppendLine(tagline);
                }
            }
            
            File.AppendAllText(filename,fileDeviceString.ToString()+fileTagString.ToString());
        }

        public static ObservableCollection<DeviceModel> ReadFile(string filename)
        {            
            string[] allLines = File.ReadAllLines(filename);

            var devices = from line in allLines
                          where line.StartsWith("device")
                          let data = line.Split(',')
                          select new Models.DeviceModel()
                          {
                              Name = data[1],
                              Ip = data[2],
                              Port = Convert.ToInt32(data[3]),
                              DeviceId = Convert.ToByte(data[4]),
                              RefreshRate = Convert.ToInt32(data[5]),
                              IsActive = data[6] == "1"
                          };

            var tags = from line in allLines
                       where line.StartsWith("tag")
                       let data = line.Split(',')
                       select new
                       {
                           Name = data[1],
                           device = data[2],
                           AddressString = data[3],
                           TagModbusType = data[4],
                           Direction = data[5],
                           TagValueType = data[6], 
                           TagMaskType = data[7],
                           Mask = data[8],
                           TagMergeType = data[9],
                           Range = data[10]
                       }; 
           

            Dictionary<string,DeviceModel> allDevices=new Dictionary<string,DeviceModel>();

            foreach (var device in devices)
            {
                if (!allDevices.ContainsKey(device.Name))
                {
                    allDevices.Add(device.Name, device);

                }
            }

            foreach (var tag in tags)
            {
                if (allDevices.ContainsKey(tag.device))
                {
                    TagModel aTag = new TagModel();

                    aTag.Name = tag.Name;
                    aTag.AddressString = tag.AddressString;
                    #region TagModbusType
                    switch (tag.TagModbusType.Trim().ToLower())
                    {
                        case ("cs"):
                            aTag.TagModbusType = ModbusType.CoilStatus;
                            break;
                        case ("is"):
                            aTag.TagModbusType = ModbusType.InputStatus;
                            break;
                        case ("hr"):
                            aTag.TagModbusType = ModbusType.HoldingRegister;
                            break;
                        case ("ir"):
                            aTag.TagModbusType = ModbusType.InputRegister;
                            break;
                        default:
                            break;
                    } 
                    #endregion
                    #region TagValueType
                    switch (tag.TagValueType.Trim().ToLower())
                    {
                        case ("bool"):
                            aTag.TagValueType = ModbusTagManager.Models.ValueType.Bool;
                            break;
                        case ("ushort"):
                            aTag.TagValueType = ModbusTagManager.Models.ValueType.Ushort;
                            break;
                        case ("lsfr"):
                            aTag.TagValueType = ModbusTagManager.Models.ValueType.FloatLSRF;
                            break;
                        case ("msrf"):
                            aTag.TagValueType = ModbusTagManager.Models.ValueType.FloatMSRF;
                            break;

                        default:
                            break;
                    } 
                    #endregion
                    #region Direction
                    switch (tag.Direction.Trim().ToLower())
                    {
                        case ("read"):
                            aTag.TagDirectionType = Models.DirectionType.Read;
                            break;
                        case ("write"):
                            aTag.TagDirectionType = Models.DirectionType.Write;
                            break;
                        default:
                            break;
                    } 
                    #endregion
                    #region TagMaskType
                    switch (tag.TagMaskType.Trim().ToLower())
                    {//none/andmask/ormask
                        case ("andmask"):
                            aTag.TagMaskType = Models.MaskType.AndMask;
                            break;
                        case ("ormask"):
                            aTag.TagMaskType = Models.MaskType.OrMask;
                            break;
                        
                        default:
                            aTag.TagMaskType = Models.MaskType.None;
                            break;
                    }
                    #endregion                    
                    aTag.Mask = tag.Mask;
                    #region TagMergeType
                    switch (tag.TagMergeType.Trim().ToLower())
                    {//andmerge/ormerge
                        case ("andmerge"):
                            aTag.TagMergeType = Models.MergeType.AndMerge;
                            break;

                        default:
                            aTag.TagMergeType = Models.MergeType.OrMerge;
                            break;
                    }
                    #endregion                    
                    aTag.Range = tag.Range;

                    allDevices[tag.device].Tags.Add(aTag); 
                }
            }
            

            ObservableCollection<DeviceModel> result = new ObservableCollection<DeviceModel>();
            foreach (var item in allDevices.Values)
            {
                result.Add(item);
            }

            return result;
            
        }
    }
}