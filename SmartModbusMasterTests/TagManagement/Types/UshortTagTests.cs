using Kr.Communication.SmartModbusMaster.Modbus;
using Kr.Communication.SmartModbusMaster.TagManagement.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartModbusMasterTests.TagManagement.Types
{
    [TestClass]
    public class UshortTagTests
    {
        public Dictionary<ushort, ushort> GetModbusUshortValues()
        {
            Dictionary<ushort, ushort> results = new Dictionary<ushort, ushort>
            {
                { 1, 100 },
                { 2, 101 },
                { 3, 102 },
                { 4, 103 },
                { 5, 104 },
                { 6, 12 },
                { 7, 13 },
                { 8, 14 },
                { 9, 15 },
                { 10, 160 },
                { 11, 161 },
                { 12, 162 },
                { 13, 163 },
                { 14, 164 },
                { 15, 165 },
                { 16, 166 },
                { 17, 167 },
                { 18, 168 },
                { 19, 169 },
                { 20, 170 }
            };

            return results;
        }

        public Dictionary<ushort, ushort> GetModbusUshortValues2()
        {
            Dictionary<ushort, ushort> results = new Dictionary<ushort, ushort>
            {
                { 1, 100 },
                { 2, 101 },
                { 3, 102 },
                { 4, 203 },
                { 5, 204 },
                { 6, 12 },
                { 7, 13 },
                { 8, 14 },
                { 9, 15 },
                { 10, 200 },
                { 11, 161 },
                { 12, 162 },
                { 13, 163 },
                { 14, 164 },
                { 15, 165 },
                { 16, 166 },
                { 17, 167 },
                { 18, 168 },
                { 19, 169 },
                { 20, 170 }
            };

            return results;
        }

        [TestMethod()]
        public void UshortTag_AddAddress()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            myTag.SetData(GetModbusUshortValues());
        }

        [TestMethod()]
        public void UshortTag_AndMask1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 15;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(6, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 8;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(0, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 15;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(8, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask4()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 8;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(8, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMerge1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.AddAddress(5); // 104
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(96, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMerge2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(102, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMerge3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(8); // 14
            myTag.AddAddress(9); // 15
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(14, myTag.Value);
        }

        [TestMethod]
        public void UshortTag_Initialize()
        {
            UshortTag myTag1 = new UshortTag(RegisterFunction.HoldingRegister);
            Assert.AreEqual(myTag1.InnerTagType, TagType.UshortTag);
        }

        [TestMethod()]
        public void UshortTag_OrMask1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 15;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 8;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(110, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 15;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask4()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 8;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(104, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask5()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.None;
            myTag.Mask = 1;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(104, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMerge1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.AddAddress(5); // 104
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMerge2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(103, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMerge3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(8); // 14
            myTag.AddAddress(9); // 15
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(15, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            bool isChanged = false;
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                isChanged = true;
            };
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(true, isChanged);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest10()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.Range = 30;
            myTag.SetData(GetModbusUshortValues2());
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(200, values[0]);
            Assert.AreEqual(160, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            bool isChanged = false;
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                isChanged = true;
            };
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            Assert.AreEqual(false, isChanged);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(5);
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(104, values[0]);
            Assert.AreEqual(204, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest4()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(4);
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 7;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(7, values[0]);
            Assert.AreEqual(3, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest5()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(160, values[0]);
            Assert.AreEqual(200, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest6()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.Range = 50;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(160, values[0]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest7()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.Range = 30;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(160, values[0]);
            Assert.AreEqual(200, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest8()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.SetData(GetModbusUshortValues2());
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(200, values[0]);
            Assert.AreEqual(160, values[1]);
        }

        [TestMethod()]
        public void UshortTag_ValueChangedTest9()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            List<ushort> values = new List<ushort>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(10);
            myTag.Range = 50;
            myTag.SetData(GetModbusUshortValues2());
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(200, values[0]);
        }
    }
}