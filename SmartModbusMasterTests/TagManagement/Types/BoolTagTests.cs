using Kr.Communication.SmartModbusMaster.Modbus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Types.Tests
{
    [TestClass()]
    public class BoolTagTests
    {
        [TestMethod]
        public void BoolTag_Initialize()
        {
            BoolTag myTag1 = new BoolTag(StatusFunction.InputStatus);
            Assert.AreEqual(myTag1.InnerTagType, TagType.BoolTag);
        }

        [TestMethod()]
        public void BoolTag_AddAddress()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            myTag.SetData(GetModbusBoolValues());
        }

        [TestMethod()]
        public void BoolTag_AndMerge1()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.AddAddress(4); // true
            myTag.AddAddress(5); // false
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMerge2()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.AddAddress(4); // true
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMerge3()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(8); // false
            myTag.AddAddress(9); // false
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMerge1()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.AddAddress(4); // true
            myTag.AddAddress(5); // false
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMerge2()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.AddAddress(4); // true
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMerge3()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(8); // false
            myTag.AddAddress(9); // false
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMask1()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = true;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMask2()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = false;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMask3()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5); // false
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = true;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_AndMask4()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5); // false
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = false;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMask1()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = true;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMask2()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(3); // true
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = false;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMask3()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5); // false
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = true;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(true, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMask4()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5); // false
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = false;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        [TestMethod()]
        public void BoolTag_OrMask5()
        {
            BoolTag myTag = new BoolTag(StatusFunction.InputStatus);
            myTag.AddAddress(5); // false
            myTag.MaskType = TagAddressMaskType.None;
            myTag.Mask = true;
            myTag.SetData(GetModbusBoolValues());
            Assert.AreEqual(false, myTag.Value);
        }

        public Dictionary<ushort, bool> GetModbusBoolValues()
        {
            Dictionary<ushort, bool> results = new Dictionary<ushort, bool>
            {
                { 1, true },
                { 2, true },
                { 3, true },
                { 4, true },
                { 5, false },
                { 6, true },
                { 7, true },
                { 8, false },
                { 9, false },
                { 10, true },
                { 11, false },
                { 12, false },
                { 13, true },
                { 14, false },
                { 15, true },
                { 16, false },
                { 17, true },
                { 18, false },
                { 19, true },
                { 20, true }
            };

            return results;
        }
    }
}