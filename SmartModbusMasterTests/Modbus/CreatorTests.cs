using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.Modbus.Tests
{
    [TestClass()]
    public class CreatorTests
    {
        [TestMethod()]
        public void ParseAddressStringTest0()
        {
            string addressstring = "";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void ParseAddressStringTest1()
        {
            string addressstring = "23";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(1, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
        }

        [TestMethod()]
        public void ParseAddressStringTest2()
        {
            string addressstring = "23 27 29";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void ParseAddressStringTest3()
        {
            string addressstring = "23-27";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest4()
        {
            string addressstring = "23-27-";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void ParseAddressStringTest5()
        {
            string addressstring = "-23-27";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void ParseAddressStringTest6()
        {
            string addressstring = "-23-27-";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void ParseAddressStringTest7()
        {
            string addressstring = "  ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(0, parsedAddress.Length);
        }

        [TestMethod()]
        public void ParseAddressStringTest8()
        {
            string addressstring = "23-27 ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest9()
        {
            string addressstring = " 23-27";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest10()
        {
            string addressstring = " 23-27 ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest11()
        {
            string addressstring = "23 27 29 ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void ParseAddressStringTest12()
        {
            string addressstring = " 23 27 29";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void ParseAddressStringTest13()
        {
            string addressstring = " 23 27 29 ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(3, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(27, parsedAddress[1]);
            Assert.AreEqual(29, parsedAddress[2]);
        }

        [TestMethod()]
        public void ParseAddressStringTest14()
        {
            string addressstring = "23-27 28";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(6, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(24, parsedAddress[1]);
            Assert.AreEqual(25, parsedAddress[2]);
            Assert.AreEqual(26, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
            Assert.AreEqual(28, parsedAddress[5]);
        }

        [TestMethod()]
        public void ParseAddressStringTest15()
        {
            string addressstring = "23 -3";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(23, parsedAddress[0]);
            Assert.AreEqual(0, parsedAddress[1]);
            Assert.AreEqual(1, parsedAddress[2]);
            Assert.AreEqual(2, parsedAddress[3]);
            Assert.AreEqual(3, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest16()
        {
            string addressstring = "3- 27";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(5, parsedAddress.Length);
            Assert.AreEqual(0, parsedAddress[0]);
            Assert.AreEqual(1, parsedAddress[1]);
            Assert.AreEqual(2, parsedAddress[2]);
            Assert.AreEqual(3, parsedAddress[3]);
            Assert.AreEqual(27, parsedAddress[4]);
        }

        [TestMethod()]
        public void ParseAddressStringTest17()
        {
            string addressstring = "10 23-27 28";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
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
        public void ParseAddressStringTest18()
        {
            string addressstring = "10 23-27 ";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
            Assert.AreEqual(6, parsedAddress.Length);
            Assert.AreEqual(10, parsedAddress[0]);
            Assert.AreEqual(23, parsedAddress[1]);
            Assert.AreEqual(24, parsedAddress[2]);
            Assert.AreEqual(25, parsedAddress[3]);
            Assert.AreEqual(26, parsedAddress[4]);
            Assert.AreEqual(27, parsedAddress[5]);
        }

        [TestMethod()]
        public void ParseAddressStringTest19()
        {
            string addressstring = "10 23-27 25-28";
            ushort[] parsedAddress = Creator.ParseAddressString(addressstring);
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