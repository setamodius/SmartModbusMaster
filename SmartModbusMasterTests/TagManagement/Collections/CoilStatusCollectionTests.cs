using Kr.Communication.SmartModbusMaster.Modbus;
using Kr.Communication.SmartModbusMaster.TagManagement.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Collections.Tests
{
    [TestClass()]
    public class CoilStatusCollectionTests
    {
        private TagCollection myCollection = new TagCollection();

        [TestMethod()]
        public void RefreshValuesTest1()
        {
            int a = 0;
            bool[] test1 = new bool[] { true, true };
            bool[] test2 = new bool[] { true, true };
            bool[] test3 = new bool[] { false, true };
            bool[] test4 = new bool[] { false, false };
            bool[] test5 = new bool[] { true, false };
            bool[] test6 = new bool[] { true, false };
            Tag d = myCollection.GetTagWithName("boolcoil1");

            d.InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                a++;
            };
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test1);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test2);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test3);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test4);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test5);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[0], test6);

            Assert.AreEqual(3, a);
        }

        [TestMethod()]
        public void RefreshValuesTest2()
        {
            int a = 0;
            bool[] test1 = new bool[] { true, true };
            bool[] test2 = new bool[] { true, true };
            bool[] test3 = new bool[] { false, true };
            bool[] test4 = new bool[] { false, false };
            bool[] test5 = new bool[] { true, false };
            bool[] test6 = new bool[] { true, false };
            Tag d = myCollection.GetTagWithName("boolcoil1");

            d.InnerTag.TagValueChangedEvent += delegate (ITagType tag)
            {
                a++;
            };
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test1);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test2);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test3);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test4);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test5);
            myCollection.CoilStatuses.RefreshValues(myCollection.CoilStatuses.Maps[1], test6);

            Assert.AreEqual(2, a);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            const int totalcount = 12;
            int boolcoilcount = 0;
            int boolinputcount = 0;
            int ushortholdingcount = 0;
            int ushortinputcount = 0;
            int floatholdingcount = 0;
            int floatinputcount = 0;
            for (int i = 0; i < totalcount; i++)
            {
                string name = "";
                Tag newTag;
                if (i < (totalcount / 6))
                {
                    //bool tag - coil status
                    boolcoilcount++;
                    name = "boolcoil";
                    newTag = new Tag(name + boolcoilcount.ToString());
                    newTag.DefineBoolTag(StatusFunction.CoilStatus);
                    newTag.InnerTag.AddAddress(0);
                    newTag.InnerTag.AddAddress(2001);
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
                myCollection.AddTag(newTag);
                myCollection.CalculateParseAddresses();
            }
        }
    }
}