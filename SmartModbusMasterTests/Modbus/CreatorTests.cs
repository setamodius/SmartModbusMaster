using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Modbus.Tests
{
    [TestClass()]
    public class CreatorTests
    {
        [TestMethod()]
        public void FromFileTest()
        {
            ModbusDevices d = Creator.FromFile(@"");// path
            d["Device1"].Start();
        }

        [TestMethod()]
        public void parseAddressStringTest0()
        {
            string addressstring = "";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void parseAddressStringTest1()
        {
            string addressstring = "23";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(1, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
        }

        [TestMethod()]
        public void parseAddressStringTest2()
        {
            string addressstring = "23 27 29";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void parseAddressStringTest3()
        {
            string addressstring = "23-27";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest4()
        {
            string addressstring = "23-27-";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void parseAddressStringTest5()
        {
            string addressstring = "-23-27";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void parseAddressStringTest6()
        {
            string addressstring = "-23-27-";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void parseAddressStringTest7()
        {
            string addressstring = "  ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void parseAddressStringTest8()
        {
            string addressstring = "23-27 ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest9()
        {
            string addressstring = " 23-27";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest10()
        {
            string addressstring = " 23-27 ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest11()
        {
            string addressstring = "23 27 29 ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void parseAddressStringTest12()
        {
            string addressstring = " 23 27 29";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void parseAddressStringTest13()
        {
            string addressstring = " 23 27 29 ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void parseAddressStringTest14()
        {
            string addressstring = "23-27 28";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(6, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
            Assert.AreEqual(28, parsedAddress[5]);
        }

        [TestMethod()]
        public void parseAddressStringTest15()
        {
            string addressstring = "23 -3";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(0, parsedAddress[1]);
            Assert.AreEqual(1, parsedAddress[2]);
            Assert.AreEqual(2, parsedAddress[3]);
            Assert.AreEqual(3, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest16()
        {
            string addressstring = "3- 27";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(0, parsedAddress[0]);
            Assert.AreEqual(1, parsedAddress[1]);
            Assert.AreEqual(2, parsedAddress[2]);
            Assert.AreEqual(3, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void parseAddressStringTest17()
        {
            string addressstring = "10 23-27 28";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(7, parsedAddress.Length);
            Assert.AreEqual(10, parsedAddress[0]);
            Assert.AreEqual(23, parsedAddress[1]);
            Assert.AreEqual(24, parsedAddress[2]);
            Assert.AreEqual(25, parsedAddress[3]);
            Assert.AreEqual(26, parsedAddress[4]);
            Assert.AreEqual(27, parsedAddress[5]);
            Assert.AreEqual(28, parsedAddress[6]);
        }

        [TestMethod()]
        public void parseAddressStringTest18()
        {
            string addressstring = "10 23-27 ";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(6, parsedAddress.Length);
            Assert.AreEqual(10, parsedAddress[0]);
            Assert.AreEqual(23, parsedAddress[1]);
            Assert.AreEqual(24, parsedAddress[2]);
            Assert.AreEqual(25, parsedAddress[3]);
            Assert.AreEqual(26, parsedAddress[4]);
            Assert.AreEqual(27, parsedAddress[5]);
        }

        [TestMethod()]
        public void parseAddressStringTest19()
        {
            string addressstring = "10 23-27 25-28";
            ushort[] parsedAddress = Creator.parseAddressString(addressstring);
            Assert.AreEqual(7, parsedAddress.Length);
            Assert.AreEqual(10, parsedAddress[0]);
            Assert.AreEqual(23, parsedAddress[1]);
            Assert.AreEqual(24, parsedAddress[2]);
            Assert.AreEqual(25, parsedAddress[3]);
            Assert.AreEqual(26, parsedAddress[4]);
            Assert.AreEqual(27, parsedAddress[5]);
            Assert.AreEqual(28, parsedAddress[6]);
        }
    }
}
