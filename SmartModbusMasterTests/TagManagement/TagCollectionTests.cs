using Kr.Communication.SmartModbusMaster.Modbus;
using Kr.Communication.SmartModbusMaster.TagManagement.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Kr.Communication.SmartModbusMaster.TagManagement.Tests
{
    [TestClass()]
    public class TagCollectionTests
    {
        private readonly Dictionary<string, Tag> testTags = new Dictionary<string, Tag>();

        [TestMethod()]
        public void AddingTagTest()
        {
            TagCollection myCollection = new TagCollection();
            bool firstAddingResult = true;
            foreach (var item in testTags.Values)
            {
                firstAddingResult = firstAddingResult && myCollection.AddTag(item);
            }
            bool secondtAddingResult = true;
            foreach (var item in testTags.Values)
            {
                secondtAddingResult = secondtAddingResult && myCollection.AddTag(item);
            }

            Assert.AreEqual(true, firstAddingResult);
            Assert.AreEqual(false, secondtAddingResult);
        }

        [TestMethod()]
        public void GetTagWithNameTest()
        {
            TagCollection myCollection = new TagCollection();
            foreach (var item in testTags.Values)
            {
                myCollection.AddTag(item);
            }

            int boolcount = 0;
            foreach (var item in testTags.Keys)
            {
                if (item.StartsWith("bool"))
                {
                    if (myCollection.GetBoolTagWithName(item) != null)
                    {
                        boolcount++;
                    }
                }
            }

            int ushortcount = 0;
            foreach (var item in testTags.Keys)
            {
                if (item.StartsWith("ushort"))
                {
                    if (myCollection.GetUshortTagWithName(item) != null)
                    {
                        ushortcount++;
                    }
                }
            }

            int floatcount = 0;
            foreach (var item in testTags.Keys)
            {
                if (item.StartsWith("float"))
                {
                    if (myCollection.GetFloatTagWithName(item) != null)
                    {
                        floatcount++;
                    }
                }
            }

            int totalcount = 0;
            foreach (var item in testTags.Keys)
            {
                if (myCollection.GetTagWithName(item) != null)
                {
                    totalcount++;
                }
            }

            Assert.AreEqual(20, boolcount);
            Assert.AreEqual(20, ushortcount);
            Assert.AreEqual(20, floatcount);
            Assert.AreEqual(60, totalcount);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            int totalcount = 60;
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
                testTags.Add(newTag.Name, newTag);
            }
        }
    }
}