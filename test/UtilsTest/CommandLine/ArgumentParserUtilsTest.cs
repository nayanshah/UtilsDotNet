using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.CommandLine;
using Utils.Logging;

namespace UtilsTest.CommandLine
{
    [TestClass]
    public class ArgumentParserUtilsTest
    {
        private MockLogger Logger { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Logger = new MockLogger();
            LogHelper.Logger = Logger;
        }

        [TestMethod]
        public void TestArgumentParserUtils_ParseParam()
        {
            // Non params
            Tuple<string, string> param = ArgumentParserUtils.ParseParam("a");
            Assert.IsNull(param);

            // Flags
            param = ArgumentParserUtils.ParseParam("/b");
            Assert.IsNotNull(param);
            Assert.AreEqual("b", param.Item1);
            Assert.IsTrue(string.IsNullOrWhiteSpace(param.Item2));

            param = ArgumentParserUtils.ParseParam("-c");
            Assert.IsNotNull(param);
            Assert.AreEqual("c", param.Item1);
            Assert.IsTrue(string.IsNullOrWhiteSpace(param.Item2));

            // Assignments
            param = ArgumentParserUtils.ParseParam("-d:dval");
            Assert.IsNotNull(param);
            Assert.AreEqual("d", param.Item1);
            Assert.AreEqual("dval", param.Item2);

            param = ArgumentParserUtils.ParseParam("/e=eval");
            Assert.IsNotNull(param);
            Assert.AreEqual("e", param.Item1);
            Assert.AreEqual("eval", param.Item2);

            // Long keys
            param = ArgumentParserUtils.ParseParam("--f-gh");
            Assert.IsNotNull(param);
            Assert.AreEqual("f-gh", param.Item1);
            Assert.IsTrue(string.IsNullOrWhiteSpace(param.Item2));

            param = ArgumentParserUtils.ParseParam("--i-jk=kval");
            Assert.IsNotNull(param);
            Assert.AreEqual("i-jk", param.Item1);
            Assert.AreEqual("kval", param.Item2);

            param = ArgumentParserUtils.ParseParam("--l-mn=mval-with-hyphen");
            Assert.IsNotNull(param);
            Assert.AreEqual("l-mn", param.Item1);
            Assert.AreEqual("mval-with-hyphen", param.Item2);

            param = ArgumentParserUtils.ParseParam("/o:oval-with-hyphen");
            Assert.IsNotNull(param);
            Assert.AreEqual("o", param.Item1);
            Assert.AreEqual("oval-with-hyphen", param.Item2);

            // Paths or spaces
            param = ArgumentParserUtils.ParseParam(@"/path:Z:\Dir\File.Ext");
            Assert.IsNotNull(param);
            Assert.AreEqual("path", param.Item1);
            Assert.AreEqual(@"Z:\Dir\File.Ext", param.Item2);

            param = ArgumentParserUtils.ParseParam("-q=arg with space");
            Assert.IsNotNull(param);
            Assert.AreEqual("q", param.Item1);
            Assert.AreEqual("arg with space", param.Item2);
        }

        [TestMethod]
        public void TestArgumentParserUtils_TrySetPropertyValue_Single()
        {
            IDictionary<string, ArgumentData> data = ArgumentParserUtils.GetArgumentData<TestParams>();
            TestParams testParams = new TestParams();

            Assert.IsTrue(testParams.TrySetPropertyValue(data["s"].Property, "value"));
            Assert.AreEqual("value", testParams.String);

            Assert.IsTrue(testParams.TrySetPropertyValue(data["iv"].Property, "7"));
            Assert.AreEqual(7, testParams.Count);

            Assert.IsTrue(testParams.TrySetPropertyValue(data["ev"].Property, "First"));
            Assert.AreEqual(TestOrdinal.First, testParams.OrdinalVal);

            Assert.IsTrue(testParams.TrySetPropertyValue(data["f"].Property, "true"));
            Assert.AreEqual(true, testParams.IsFree);
        }

        [TestMethod]
        public void TestArgumentParserUtils_TrySetPropertyValue_Array()
        {
            IDictionary<string, ArgumentData> data = ArgumentParserUtils.GetArgumentData<TestParams>();
            TestParams testParams = new TestParams();
            
            // string array
            Assert.IsTrue(testParams.TrySetPropertyValue(data["a"].Property, new[] {"value1", "value2"}));
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("value1"));
            Assert.IsTrue(testParams.Array.Contains("value2"));

            // int array
            Assert.IsTrue(testParams.TrySetPropertyValue(data["ia"].Property, new[] { "2", "3" }));
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.IntArray.Contains(2));
            Assert.IsTrue(testParams.IntArray.Contains(2));

            // Enum array
            Assert.IsTrue(testParams.TrySetPropertyValue(data["ea"].Property, new[] { "First", "Second" }));
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.First));
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.Second));
        }

        [TestMethod]
        public void TestArgumentParserUtils_TrySetPropertyValue_Invalid()
        {
            IDictionary<string, ArgumentData> data = ArgumentParserUtils.GetArgumentData<TestParams>();
            TestParams testParams = new TestParams();

            Assert.IsFalse(testParams.TrySetPropertyValue(data["iv"].Property, "NaN"));
            Assert.IsFalse(testParams.TrySetPropertyValue(data["ev"].Property, "Sec"));
            Assert.IsFalse(testParams.TrySetPropertyValue(data["df"].Property, "NotTrue"));

            Assert.IsFalse(testParams.TrySetPropertyValue(data["ia"].Property, new[] { "NaN", "1"}));
            Assert.IsFalse(testParams.TrySetPropertyValue(data["ea"].Property, new[] { "First", "Sec"}));

            // Ensure error messages are displayed
            IList<string> errorLog = Logger.ErrorLog;
            Assert.AreEqual(5, errorLog.Count);
            Assert.AreEqual("Error: Given value 'NaN' should be of type 'Int32'", errorLog[0]);
            Assert.AreEqual("Error: Unknown value 'Sec' given. Valid values - [First, Second, Third]", errorLog[1]);
            Assert.AreEqual("Error: Given value 'NotTrue' should be of type 'Boolean'", errorLog[2]);

            Assert.AreEqual("Error: Given values 'NaN, 1' should be a list of type 'Int32'", errorLog[3]);
            Assert.AreEqual("Error: Given list 'First, Sec' contains an unknown value. Valid values - [First, Second, Third]", errorLog[4]);
        }

        [TestMethod]
        public void TestArgumentParserUtils_ChangeToType()
        {
            Assert.AreEqual(16, "16".ChangeToType(typeof(int)));
            Assert.AreEqual(1.618f, "1.618".ChangeToType(typeof(float)));
            Assert.AreEqual(3.14, "3.14".ChangeToType(typeof(double)));
            Assert.AreEqual(false, "false".ChangeToType(typeof(bool)));
            Assert.AreEqual(TestOrdinal.Second, "Second".ChangeToType(typeof(TestOrdinal)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestArgumentParserUtils_ChangeToType_InvalidEnum()
        {
            Assert.AreEqual(TestOrdinal.Second, "Sec".ChangeToType(typeof(TestOrdinal)));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestArgumentParserUtils_ChangeToType_InvalidValue()
        {
            Assert.AreEqual(1.618, "Golden".ChangeToType(typeof(float)));
        }

        [TestMethod]
        public void TestArgumentParserUtils_ValidEnumValues()
        {
            Assert.AreEqual("First, Second, Third", typeof(TestOrdinal).ValidEnumValues());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestArgumentParserUtils_ValidEnumValues_NotEnum()
        {
            Assert.AreEqual("First, Second, Third", typeof(TestParams).ValidEnumValues());
        }
    }
}