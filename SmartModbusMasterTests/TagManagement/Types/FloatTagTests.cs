using Kr.Communication.SmartModbusMaster.Converters;
using Kr.Communication.SmartModbusMaster.Modbus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Types.Tests
{
    [TestClass()]
    public class FloatTagTests
    {
        private readonly LSRFFloatConverter LSRFConverter = new LSRFFloatConverter();
        private readonly MSRFFloatConverter MSRFConverter = new MSRFFloatConverter();

        [TestMethod()]
        public void FloatTag_AddAddress()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5);
            myTag.AddAddress(12);
            myTag.FloatConverter = LSRFConverter;
            myTag.SetData(GetModbusUshortValues());
        }

        [TestMethod]
        public void FloatTag_Initialize()
        {
            FloatTag myTag1 = new FloatTag(RegisterFunction.HoldingRegister);
            Assert.AreEqual(myTag1.InnerTagType, TagType.FloatTag);
        }

        [TestMethod()]
        public void FloatTag_LSRF1()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(2); // 17271
            myTag.AddAddress(1); // 15270
            myTag.AddAddress(3); // 49595
            myTag.FloatConverter = LSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(247.233f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_LSRF2()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(4); // 49595
            myTag.AddAddress(3); // 39322
            myTag.AddAddress(5); // 16560
            myTag.FloatConverter = LSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(-23.45f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_LSRF3()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(6); // 16560
            myTag.AddAddress(5); // 0
            myTag.FloatConverter = LSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(5.5f, myTag.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FloatTag_LSRF4()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(1); // 17271
            myTag.FloatConverter = LSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(5.5f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_MSRF1()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(1); // 17271
            myTag.AddAddress(2); // 15270
            myTag.AddAddress(3); // 49595
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(247.233f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_MSRF2()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(3); // 49595
            myTag.AddAddress(4); // 39322
            myTag.AddAddress(5); // 16560
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(-23.45f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_MSRF3()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 16560
            myTag.AddAddress(6); // 0
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(5.5f, myTag.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FloatTag_MSRF4()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(1); // 17271
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(5.5f, myTag.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void FloatTag_WithoutConverter()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(1); // 17271
            myTag.AddAddress(2); // 15270
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(2477.233f, myTag.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void FloatTag_WithoutConverterAndValue()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(1); // 17271
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(2477.233f, myTag.Value);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest1()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            bool isChanged = false;
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                isChanged = true;
            };
            myTag.AddAddress(1);
            myTag.AddAddress(2);
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(true, isChanged);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest2()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            bool isChanged = false;
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                isChanged = true;
            };
            myTag.AddAddress(1);
            myTag.AddAddress(2);
            myTag.FloatConverter = MSRFConverter;
            Assert.AreEqual(false, isChanged);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest3()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            List<float> values = new List<float>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(1);
            myTag.AddAddress(2);
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(247.233f, values[0]);
            Assert.AreEqual(5.5f, values[1]);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest4()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            List<float> values = new List<float>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(3);
            myTag.AddAddress(4);
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(-23.45f, values[0]);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest5()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            List<float> values = new List<float>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(5);
            myTag.AddAddress(6);
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(5.5f, values[0]);
            Assert.AreEqual(-103.656f, values[1]);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest6()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            List<float> values = new List<float>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(1);
            myTag.AddAddress(2);
            myTag.Range = 241.8f;
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(247.233f, values[0]);
        }

        [TestMethod()]
        public void FloatTag_ValueChangedTest7()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            List<float> values = new List<float>();
            myTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                values.Add(myTag.Value);
            };
            myTag.AddAddress(1);
            myTag.AddAddress(2);
            myTag.Range = 241.7f;
            myTag.FloatConverter = MSRFConverter;
            myTag.SetData(GetModbusUshortValues());
            myTag.SetData(GetModbusUshortValues2());
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(247.233f, values[0]);
            Assert.AreEqual(5.5f, values[1]);
        }

        [TestMethod()]
        public void FloatTag_WithoutSetData()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister);
            myTag.AddAddress(5); // 16560
            myTag.AddAddress(6); // 0
            myTag.FloatConverter = MSRFConverter;
            Assert.AreEqual(0f, myTag.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FloatTag_WithoutAddingAddress()
        {
            FloatTag myTag = new FloatTag(RegisterFunction.HoldingRegister)
            {
                FloatConverter = MSRFConverter
            };
            myTag.SetData(GetModbusUshortValues());
            Assert.AreEqual(0f, myTag.Value);
        }

        public Dictionary<ushort, ushort> GetModbusUshortValues()
        {
            Dictionary<ushort, ushort> results = new Dictionary<ushort, ushort>
            {
                { 1, 17271 },
                { 2, 15270 },
                { 3, 49595 },
                { 4, 39322 },
                { 5, 16560 },
                { 6, 0 },
                { 7, 49871 },
                { 8, 20447 },
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
                { 1, 16560 },
                { 2, 0 },
                { 3, 49595 },
                { 4, 39322 },
                { 5, 49871 },
                { 6, 20447 },
                { 7, 49871 },
                { 8, 20447 },
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
    }
}