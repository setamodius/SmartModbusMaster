// ********************************************************************
//
// Copyright (c) 2015 - 2016, Kerem Bilgicer
// All rights reserved.
//
// ********************************************************************

namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using Converters;
    using TagManagement;
    using TagManagement.Types;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    public static class Creator
    {
        public static readonly char _Seperator_ = ',';
        public static int _DefaultPort_ = 502;
        public static int _RefreshRate_ = 2000;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static LSRFFloatConverter LSRF = new LSRFFloatConverter();
        private static MSRFFloatConverter MSRF = new MSRFFloatConverter();
        private static ModbusDevices myDevices;

        public static ModbusDevices FromFile(string filename)
        {
            logger.Debug("{0} dosyası için cihaz oluşturuluyor", filename);
            myDevices = new ModbusDevices();            
            return myDevices = parseFile(filename) ? myDevices : ModbusDevices.Empty;
        }

        public static ushort[] parseAddressString(string addressstring)
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
                        ushort leftValue = 0;
                        ushort rightValue = 0;
                        ushort.TryParse(rangeSplitted[0], out leftValue);
                        ushort.TryParse(rangeSplitted[1], out rightValue);
                        ushort min = Math.Min(leftValue, rightValue);
                        ushort max = Math.Max(leftValue, rightValue);
                        for (ushort i = min; i < max + 1; i++)
                        {
                            addresses.Add(i);
                        }
                    }
                    else if (rangeSplitted.Length < 2)
                    {
                        ushort address = 0;
                        ushort.TryParse(item, out address);
                        addresses.Add(address);
                    }
                }
            }
            return addresses.ToArray();
        }

        private static TagAddressMaskType convertStringToMaskType(string value)
        {
            TagAddressMaskType result;
            switch (value.ToLower())
            {
                case "1":
                    result = TagAddressMaskType.AndMask;
                    break;

                case "2":
                    result = TagAddressMaskType.OrMask;
                    break;

                default:
                    result = TagAddressMaskType.None;
                    break;
            }
            return result;
        }

        private static TagAddressMergeType convertStringToMergeType(string value)
        {
            TagAddressMergeType result;
            switch (value.ToLower())
            {
                case "andmerge":
                    result = TagAddressMergeType.AndMerge;
                    break;
                default:
                    result = TagAddressMergeType.OrMerge;
                    break;
            }
            return result;
        }

        private static bool parseFile(string filename)
        {
            if (!File.Exists(filename))
            {
                logger.Error("Dosya bulunamadı - {0}", filename);
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
                logger.Error(ex);
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
                    int port = 502;
                    int.TryParse(splittedLine[3], out port);
                    byte deviceid = 0x01;
                    byte.TryParse(splittedLine[4], out deviceid);
                    int refreshrate = _RefreshRate_;
                    int.TryParse(splittedLine[5], out refreshrate);
                    bool isactive = splittedLine[6] == "1" ? true : false;
                    Device aDevice = new Device(
                        devicename,
                        ip,
                        port,
                        deviceid,
                        refreshrate,
                        isactive
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
                    bool boolmask = false;
                    bool.TryParse(mask, out boolmask);
                    aTag.DefineBoolTag(
                        StatusFunction.CoilStatus,
                        convertStringToMaskType(masktype), boolmask,
                        convertStringToMergeType(mergetype));
                }
                else if (modbustype == "is")
                {
                    bool boolmask = false;
                    bool.TryParse(mask, out boolmask);
                    aTag.DefineBoolTag(
                        StatusFunction.InputStatus,
                        convertStringToMaskType(masktype), boolmask,
                        convertStringToMergeType(mergetype));
                }
                else if (modbustype == "hr")
                {
                    if (valuetype == "ushort")
                    {
                        ushort ushortmask = ushort.MaxValue;
                        ushort.TryParse(mask, out ushortmask);
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineUshortTag(
                            RegisterFunction.HoldingRegister,
                            convertStringToMaskType(masktype), ushortmask,
                            convertStringToMergeType(mergetype),
                            ushortrange);
                    }
                    else if (valuetype == "lsrf")
                    {
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            LSRF,
                            ushortrange);
                    }
                    else if (valuetype == "msrf")
                    {
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            MSRF,
                            ushortrange);
                    }
                }
                else if (modbustype == "ir")
                {
                    if (valuetype == "ushort")
                    {
                        ushort ushortmask = ushort.MaxValue;
                        ushort.TryParse(mask, out ushortmask);
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineUshortTag(
                            RegisterFunction.HoldingRegister,
                            convertStringToMaskType(masktype), ushortmask,
                            convertStringToMergeType(mergetype),
                            ushortrange);
                    }
                    else if (valuetype == "lsrf")
                    {
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            LSRF,
                            ushortrange);
                    }
                    else if (valuetype == "msrf")
                    {
                        ushort ushortrange = ushort.MinValue;
                        ushort.TryParse(range, out ushortrange);
                        aTag.DefineFloatTag(
                            RegisterFunction.HoldingRegister,
                            MSRF,
                            ushortrange);
                    }
                }
                aTag.TagDirection = direction == "write" ? Direction.Write : Direction.Read;
                ushort[] addresses = parseAddressString(addressstring);
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