using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Converters.Tests
{
    [TestClass()]
    public class LFUintConverterTests
    {
        [TestMethod()]
        public void ConvertToUintTestZero()
        {
            LFUintConverter d = new LFUintConverter();
            ushort value1 = 0;
            ushort value2 = 0;
            uint expected = 0;
            uint actual = d.ConvertToUint(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest1()
        {
            LFUintConverter d = new LFUintConverter();
            ushort value1 = 0;
            ushort value2 = 16560;
            uint expected = 16560;
            uint actual = d.ConvertToUint(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest2()
        {
            LFUintConverter d = new LFUintConverter();
            ushort value1 = 1;
            ushort value2 = 10;
            uint expected = 65546;
            uint actual = d.ConvertToUint(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUintTest3()
        {
            LFUintConverter d = new LFUintConverter();
            ushort value1 = 15;
            ushort value2 = 1;
            uint expected = 983041;
            uint actual = d.ConvertToUint(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertToUshortTestZero()
        {
            LFUintConverter d = new LFUintConverter();
            uint value = 0;
            ushort expected1 = 0;
            ushort expected2 = 0;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest1()
        {
            LFUintConverter d = new LFUintConverter();
            uint value = 5;
            ushort expected1 = 0;
            ushort expected2 = 5;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest2()
        {
            LFUintConverter d = new LFUintConverter();
            uint value = 3256000012;
            ushort expected1 = 49682;
            ushort expected2 = 40460;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void ConvertToUshortTest3()
        {
            LFUintConverter d = new LFUintConverter();
            uint value = 247233247;
            ushort expected1 = 3772;
            ushort expected2 = 31455;
            ushort[] actual = d.ConvertToUshort(value);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }
    }
}