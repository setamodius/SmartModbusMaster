using Kr.Communication.SmartModbusMaster.Modbus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections.Tests
{
    [TestClass()]
    public class FunctionCollectionTests
    {
        private readonly TagCollection myCollection = new TagCollection();

        [TestMethod()]
        public void CoilStatusAddressParseTest()
        {
            var coilmaps = myCollection.CoilStatuses.Maps; //30600 / 6 = 5100

            Assert.AreEqual(3, coilmaps.Length); // 5100 -> 2000, 2000, 1100

            // Start Addresses 0, 2000, 4000
            Assert.AreEqual(0, coilmaps[0].StartAddress);
            Assert.AreEqual(2000, coilmaps[1].StartAddress);
            Assert.AreEqual(4000, coilmaps[2].StartAddress);

            // Ranges 2000, 2000 1100
            Assert.AreEqual(2000, coilmaps[0].Range);
            Assert.AreEqual(2000, coilmaps[1].Range);
            Assert.AreEqual(1100, coilmaps[2].Range);
        }

        [TestMethod()]
        public void InputStatusAddressParseTest()
        {
            var coilmaps = myCollection.InputStatuses.Maps; //30600 / 6 = 5100

            Assert.AreEqual(3, coilmaps.Length); // 5100 -> 2000, 2000, 1100

            // Start Addresses 0, 2000, 4000
            Assert.AreEqual(5100, coilmaps[0].StartAddress);
            Assert.AreEqual(7100, coilmaps[1].StartAddress);
            Assert.AreEqual(9100, coilmaps[2].StartAddress);

            // Ranges 2000, 2000 1100
            Assert.AreEqual(2000, coilmaps[0].Range);
            Assert.AreEqual(2000, coilmaps[1].Range);
            Assert.AreEqual(1100, coilmaps[2].Range);
        }

        [TestMethod()]
        public void HoldingRegisterAddressParseTest()
        {
            var coilmaps = myCollection.HoldingRegisters.Maps;

            Assert.AreEqual(123, coilmaps.Length);

            // Start Addresses 0, 2000, 4000
            for (int i = 0; i < 123; i++)
            {
                Assert.AreEqual(10200 + (i * 125), coilmaps[i].StartAddress);
            }
            for (int i = 0; i < 122; i++)
            {
                Assert.AreEqual(125, coilmaps[i].Range);
            }
            Assert.AreEqual(50, coilmaps[122].Range);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            int totalcount = 30600;
            int boolcoilcount = 0;
            int boolinputcount = 0;
            int ushortholdingcount = 0;
            int ushortinputcount = 0;
            int floatholdingcount = 0;
            int floatinputcount = 0;
            for (int i = 0; i < totalcount; i++)
            {
                string name;
                Tag newTag;
                if (i < (totalcount / 6))
                {
                    //bool tag - coil status
                    boolcoilcount++;
                    name = "boolcoil";
                    newTag = new Tag(name + boolcoilcount.ToString());
                    newTag.DefineBoolTag(StatusFunction.CoilStatus);
                }
                else if (i >= (totalcount / 6) && i < (totalcount * 2 / 6))
                {
                    //bool tag - input status
                    boolinputcount++;
                    name = "boolinput";
                    newTag = new Tag(name + boolinputcount.ToString());
                    newTag.DefineBoolTag(StatusFunction.InputStatus);
                }
                else if (i >= (totalcount * 2 / 6) && i < (totalcount * 3 / 6))
                {
                    //ushort tag - holding register
                    ushortholdingcount++;
                    name = "ushortholding";
                    newTag = new Tag(name + ushortholdingcount.ToString());
                    newTag.DefineUshortTag(RegisterFunction.HoldingRegister);
                }
                else if (i >= (totalcount * 3 / 6) && i < (totalcount * 4 / 6))
                {
                    //ushort tag - input register
                    ushortinputcount++;
                    name = "ushortinput";
                    newTag = new Tag(name + ushortinputcount.ToString());
                    newTag.DefineUshortTag(RegisterFunction.InputRegister);
                }
                else if (i >= (totalcount * 4 / 6) && i < (totalcount * 5 / 6))
                {
                    //float tag - holding register
                    floatholdingcount++;
                    name = "floatholding";
                    newTag = new Tag(name + floatholdingcount.ToString());
                    newTag.DefineFloatTag(RegisterFunction.HoldingRegister, new Converters.LSRFFloatConverter());
                }
                else
                {
                    //float tag - input register
                    floatinputcount++;
                    name = "floatinput";
                    newTag = new Tag(name + floatinputcount.ToString());
                    newTag.DefineFloatTag(RegisterFunction.InputRegister, new Converters.LSRFFloatConverter());
                }
                newTag.InnerTag.AddAddress((ushort)i);
                myCollection.AddTag(newTag);
            }
            myCollection.CalculateParseAddresses();
        }
    }
}