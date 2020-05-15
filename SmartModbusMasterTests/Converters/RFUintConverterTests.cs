using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Converters.Tests
{
    [TestClass()]
    public class RFUintConverterTests
    {
        [TestMethod()]
        public void ConvertToUintTestZero()
        {
            RFUintConverter d = new RFUintConverter();
            ushort value1 = 0;
            ushort value2 = 0;
            uint expected = 0;
            uint actual = d.ConvertToUint(value2, value1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest1()
        {
            RFUintConverter d = new RFUintConverter();
            ushort value1 = 0;
            ushort value2 = 16560;
            uint expected = 16560;
            uint actual = d.ConvertToUint(value2, value1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest2()
        {
            RFUintConverter d = new RFUintConverter();
            ushort value1 = 1;
            ushort value2 = 10;
            uint expected = 65546;
            uint actual = d.ConvertToUint(value2, value1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest3()
        {
            RFUintConverter d = new RFUintConverter();
            ushort value1 = 15;
            ushort value2 = 1;
            uint expected = 983041;
            uint actual = d.ConvertToUint(value2, value1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUshortTestZero()
        {
            RFUintConverter d = new RFUintConverter();
            uint value = 0;
            ushort expected1 = 0;
            ushort expected2 = 0;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[1]);
            Assert.AreEqual(expected2, actual[0]);
        }

        [TestMethod()]
        public void ConvertToUshortTest1()
        {
            RFUintConverter d = new RFUintConverter();
            uint value = 5;
            ushort expected2 = 0;
            ushort expected1 = 5;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[1]);
            Assert.AreEqual(expected2, actual[0]);
        }

        [TestMethod()]
        public void ConvertToUshortTest2()
        {
            RFUintConverter d = new RFUintConverter();
            uint value = 3256000012;
            ushort expected2 = 49682;
            ushort expected1 = 40460;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[1]);
            Assert.AreEqual(expected2, actual[0]);
        }

        [TestMethod()]
        public void ConvertToUshortTest3()
        {
            RFUintConverter d = new RFUintConverter();
            uint value = 247233247;
            ushort expected2 = 3772;
            ushort expected1 = 31455;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[1]);
            Assert.AreEqual(expected2, actual[0]);
        }
    }
}