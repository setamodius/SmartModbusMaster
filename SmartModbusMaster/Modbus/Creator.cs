﻿namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using Converters;
    using TagManagement;
    using TagManagement.Types;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Kr.Communication.SmartModbusMaster.Diagnostic;

    public static class Creator
    {
        public static readonly char _Seperator_ = ',';
        public static int _DefaultPort_ = 502;
        public static int _RefreshRate_ = 2000;
        private static readonly LSRFFloatConverter LSRF = new LSRFFloatConverter();
        private static readonly MSRFFloatConverter MSRF = new MSRFFloatConverter();
        private static readonly LFUintConverter LF = new LFUintConverter();
        private static readonly RFUintConverter RF = new RFUintConverter();
        private static ModbusDevices myDevices;
        private static ICoreLogger logger;

        public static ModbusDevices FromFile(string filename)
        {
            logger = new CoreLogger();
            logger?.Info($"Devices are creating for {filename} file");
            myDevices = new ModbusDevices(logger);
            return myDevices = ParseFile(filename) ? myDevices : ModbusDevices.Empty;
        }        

        public static ushort[] ParseAddressString(string addressstring)
        {
            addressstring = addressstring.Trim();
            HashSet<ushort> addresses = new HashSet<ushort>();
            if (string.IsNullOrWhiteSpace(addressstring))
            {
                return addresses.ToArray();
            }
            string[] spaceSplitted = addressstring.Split(' ');
            if (spaceSplitted.Length > 0)
            {
                foreach (var item in spaceSplitted)
                {
                    string[] rangeSplitted = item.Split('-');
                    if (rangeSplitted.Length == 2)
                    {
                        ushort.TryParse(rangeSplitted[0], out ushort leftValue);
                        ushort.TryParse(rangeSplitted[1], out ushort rightValue);
                        ushort min = Math.Min(leftValue, rightValue);
                        ushort max = Math.Max(leftValue, rightValue);
                        for (ushort i = min; i < max + 1; i++)
                        {
                            addresses.Add(i);
                        }
                    }
                    else if (rangeSplitted.Length < 2)
                    {
                        ushort.TryParse(item, out ushort address);
                        addresses.Add(address);
                    }
                }
            }
            return addresses.ToArray();
        }

        private static TagAddressMaskType ConvertStringToMaskType(string value)
        {
            TagAddressMaskType result;
            switch (value.ToLower())
            {
                case "1" :
                    result = TagAddressMaskType.AndMask;
                    break;

                case "andmask":
                    result = TagAddressMaskType.AndMask;
                    break;

                case "2":
                    result = TagAddressMaskType.OrMask;
                    break;

                case "ormask":
                    result = TagAddressMaskType.OrMask;
                    break;

                default:
                    result = TagAddressMaskType.None;
                    break;
            }
            return result;
        }

        private static TagAddressMergeType ConvertStringToMergeType(string value)
        {
            TagAddressMergeType result;
            switch (value.ToLower())
            {
                case "andmerge":
                    result = TagAddressMergeType.AndMerge;
                    break;
                case "1":
                    result = TagAddressMergeType.AndMerge;
                    break;
                default:
                    result = TagAddressMergeType.OrMerge;
                    break;
            }
            return result;
        }

        private static bool ParseFile(string filename)
        {
            if (!File.Exists(filename))
            {
                logger.Fatal(null, $"File not found - {filename}");
                return false;
            }

            HashSet<string> taglist = new HashSet<string>();
            string[] allLines;
            try
            {
                allLines = File.ReadAllLines(filename);
            }
            catch (IOException ex)
            {
                logger.Fatal(ex,"IO Exception");
                return false;
            }

            foreach (var line in allLines)
            {
                if (line.StartsWith("//"))
                {
                    continue;
                }
                string[] splittedLine = line.Split(_Seperator_);
                if (splittedLine.Length == 0)
                {
                    continue;
                }
                if (splittedLine[0].ToLower() == "device")
                {
                    if (splittedLine.Length < 7)
                    {
                        continue;
                    }
                    string devicename = splittedLine[1];
                    string ip = splittedLine[2];
                    int.TryParse(splittedLine[3], out int port);
                    byte.TryParse(splittedLine[4], out byte deviceid);
                    int.TryParse(splittedLine[5], out int refreshrate);
                    bool isactive = splittedLine[6] == "1" ? true : false;
                    Device aDevice = new Device(
                        devicename,
                        ip,
                        port,
                        deviceid,
                        refreshrate,
                        isactive,
                        logger
                    );
                    myDevices.Add(aDevice.Name, aDevice);
                }
                else if (splittedLine[0].ToLower() == "tag")
                {
                    taglist.Add(line);
                }
            }

            foreach (var tagstring in taglist)
            {
                string[] splittedLine = tagstring.Split(_Seperator_);
                if (splittedLine.Length < 10)
                {
                    continue;
                }
                string tagname = splittedLine[1];
                string devicename = splittedLine[2];
                string addressstring = splittedLine[3];
                string modbustype = splittedLine[4].ToLower();
                string direction = splittedLine[5].ToLower();
                string valuetype = splittedLine[6].ToLower();
                string masktype = splittedLine[7].ToLower();
                string mask = splittedLine[8].ToLower();
                string mergetype = splittedLine[9].ToLower();
                string range = splittedLine[10].ToLower();
                Tag aTag = new Tag(tagname);
                if (modbustype == "cs")
                {
                    bool.TryParse(mask, out bool boolmask);
                    aTag.DefineBoolTag(
                        StatusFunction.CoilStatus,
                        ConvertStringToMaskType(masktype), boolmask,
                        ConvertStringToMergeType(mergetype));
                }
                else if (modbustype == "is")
                {
                    bool.TryParse(mask, out bool boolmask);
                    aTag.DefineBoolTag(
                        StatusFunction.InputStatus,
                        ConvertStringToMaskType(masktype), boolmask,
                        ConvertStringToMergeType(mergetype));
                }
                else if (modbustype == "hr")
                {
                    if (valuetype == "ushort")
                    {
                        ushort.TryParse(mask, out ushort ushortmask);
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUshortTag(
                            RegisterFunction.HoldingRegister,
                            ConvertStringToMaskType(masktype), ushortmask,
                            ConvertStringToMergeType(mergetype),
                            ushortrange);
                    }
                    else if (valuetype == "lsrf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            LSRF,
                            ushortrange);
                    }
                    else if (valuetype == "msrf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            MSRF,
                            ushortrange);
                    }
                    else if(valuetype == "lf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUintTag(
                            RegisterFunction.HoldingRegister,
                            LF,
                            ushortrange);
                    }
                    else if (valuetype == "rf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUintTag(
                            RegisterFunction.HoldingRegister,
                            RF,
                            ushortrange);
                    }
                }
                else if (modbustype == "ir")
                {
                    if (valuetype == "ushort")
                    {
                        ushort.TryParse(mask, out ushort ushortmask);
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUshortTag(
                            RegisterFunction.InputRegister,
                            ConvertStringToMaskType(masktype), ushortmask,
                            ConvertStringToMergeType(mergetype),
                            ushortrange);
                    }
                    else if (valuetype == "lsrf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.InputRegister,
                            LSRF,
                            ushortrange);
                    }
                    else if (valuetype == "msrf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.InputRegister,
                            MSRF,
                            ushortrange);
                    }
                    else if (valuetype == "lf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUintTag(
                            RegisterFunction.InputRegister,
                            LF,
                            ushortrange);
                    }
                    else if (valuetype == "rf")
                    {
                        ushort.TryParse(range, out ushort ushortrange);
                        aTag.DefineUintTag(
                            RegisterFunction.InputRegister,
                            RF,
                            ushortrange);
                    }
                }

                aTag.TagDirection = direction == "write" ? Direction.Write : Direction.Read;
                ushort[] addresses = ParseAddressString(addressstring);
                foreach (var anaddress in addresses)
                {
                    aTag.InnerTag.AddAddress(anaddress);
                }

                if (myDevices.ContainsKey(devicename))
                {
                    myDevices[devicename].Collection.AddTag(aTag);
                }
            }
            return true;
        }
    }
}