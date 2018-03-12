using Kr.Communication.SmartModbusMaster.Modbus;
using Kr.Communication.SmartModbusMaster.TagManagement.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartModbusMasterTests.TagManagement.Types
{
    [TestClass]
    public class UshortTagTests
    {
        public Dictionary<ushort, ushort> getModbusUshortValues()
        {
            Dictionary<ushort, ushort> results = new Dictionary<ushort, ushort>();
            results.Add(1, 100);
            results.Add(2, 101);
            results.Add(3, 102);
            results.Add(4, 103);
            results.Add(5, 104);
            results.Add(6, 12);
            results.Add(7, 13);
            results.Add(8, 14);
            results.Add(9, 15);
            results.Add(10, 160);
            results.Add(11, 161);
            results.Add(12, 162);
            results.Add(13, 163);
            results.Add(14, 164);
            results.Add(15, 165);
            results.Add(16, 166);
            results.Add(17, 167);
            results.Add(18, 168);
            results.Add(19, 169);
            results.Add(20, 170);

            return results;
        }

        public Dictionary<ushort, ushort> getModbusUshortValues2()
        {
            Dictionary<ushort, ushort> results = new Dictionary<ushort, ushort>();
            results.Add(1, 100);
            results.Add(2, 101);
            results.Add(3, 102);
            results.Add(4, 203);
            results.Add(5, 204);
            results.Add(6, 12);
            results.Add(7, 13);
            results.Add(8, 14);
            results.Add(9, 15);
            results.Add(10, 200);
            results.Add(11, 161);
            results.Add(12, 162);
            results.Add(13, 163);
            results.Add(14, 164);
            results.Add(15, 165);
            results.Add(16, 166);
            results.Add(17, 167);
            results.Add(18, 168);
            results.Add(19, 169);
            results.Add(20, 170);

            return results;
        }

        [TestMethod()]
        public void UshortTag_AddAddress()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            myTag.SetData(getModbusUshortValues());
        }

        [TestMethod()]
        public void UshortTag_AndMask1()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 15;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(6, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 8;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(0, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 15;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(8, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMask4()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.AndMask;
            myTag.Mask = 8;
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(96, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMerge2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(102, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_AndMerge3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(8); // 14
            myTag.AddAddress(9); // 15
            myTag.MergeType = TagAddressMergeType.AndMerge;
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 8;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(110, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 15;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask4()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.OrMask;
            myTag.Mask = 8;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(104, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMask5()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 104
            myTag.MaskType = TagAddressMaskType.None;
            myTag.Mask = 1;
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(111, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMerge2()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 102
            myTag.AddAddress(4); // 103
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(103, myTag.Value);
        }

        [TestMethod()]
        public void UshortTag_OrMerge3()
        {
            UshortTag myTag = new UshortTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(8); // 14
            myTag.AddAddress(9); // 15
            myTag.MergeType = TagAddressMergeType.OrMerge;
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues2());
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues());
            myTag.SetData(getModbusUshortValues2());
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
            myTag.SetData(getModbusUshortValues());
            myTag.SetData(getModbusUshortValues2());
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
            myTag.SetData(getModbusUshortValues());
            myTag.SetData(getModbusUshortValues2());
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
            myTag.SetData(getModbusUshortValues());
            myTag.SetData(getModbusUshortValues2());
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
            myTag.SetData(getModbusUshortValues());
            myTag.SetData(getModbusUshortValues2());
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
            myTag.SetData(getModbusUshortValues2());
            myTag.SetData(getModbusUshortValues());
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
            myTag.SetData(getModbusUshortValues2());
            myTag.SetData(getModbusUshortValues());
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(200, values[0]);
        }
    }
}