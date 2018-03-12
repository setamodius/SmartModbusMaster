using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Converters.Tests
{
    [TestClass()]
    public class LSRFFloatConverterTests
    {
        [TestMethod()]
        public void ConvertToFloatTestZero()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            ushort value1 = 0;
            ushort value2 = 0;
            float expected = 0;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest1()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            ushort value1 = 0;
            ushort value2 = 16560;
            float expected = 5.5f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest2()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            ushort value1 = 15270;
            ushort value2 = 17271;
            float expected = 247.233f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToFloatTest3()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            ushort value1 = 20447;
            ushort value2 = 49871;
            float expected = -103.656f;
            float actual = d.ConvertToFloat(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUshortTestZero()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
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
            LSRFFloatConverter d = new LSRFFloatConverter();
            float value = 5.5f;
            ushort expected1 = 0;
            ushort expected2 = 16560;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest2()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            float value = 247.233f;
            ushort expected1 = 15270;
            ushort expected2 = 17271;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest3()
        {
            LSRFFloatConverter d = new LSRFFloatConverter();
            float value = -103.656f;
            ushort expected1 = 20447;
            ushort expected2 = 49871;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }
    }
}