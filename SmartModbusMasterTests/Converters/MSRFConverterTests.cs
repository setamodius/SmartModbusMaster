using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Converters.Tests
{
    [TestClass()]
    public class MSRFConverterTests
    {
        [TestMethod()]
        public void ConvertToFloatTestZero()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            ushort value1 = 0;
            ushort value2 = 0;
            float expected = 0;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest1()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            ushort value1 = 16560;
            ushort value2 = 0;
            float expected = 5.5f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest2()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            ushort value1 = 17271;
            ushort value2 = 15270;
            float expected = 247.233f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest3()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            ushort value1 = 49871;
            ushort value2 = 20447;
            float expected = -103.656f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUshortTestZero()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            float value = 0;
            ushort expected1 = 0;
            ushort expected2 = 0;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[0]);
        }

        [TestMethod()]
        public void ConvertToUshortTest1()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            float value = 5.5f;
            ushort expected1 = 16560;
            ushort expected2 = 0;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest2()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            float value = 247.233f;
            ushort expected1 = 17271;
            ushort expected2 = 15270;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest3()
        {
            MSRFFloatConverter d = new MSRFFloatConverter();
            float value = -103.656f;
            ushort expected1 = 49871;
            ushort expected2 = 20447;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }
    }
}