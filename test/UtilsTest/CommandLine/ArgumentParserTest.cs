using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.CommandLine;
using Utils.Logging;

namespace UtilsTest.CommandLine
{
    [TestClass]
    public class ArgumentParserTest
    {
        private MockLogger Logger { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Logger = new MockLogger();
            LogHelper.Logger = Logger;
        }

        [TestMethod]
        public void TestArgumentParser_Parse_Basic()
        {
            string[] args = { "/s", "svalue", "/a", "a1value", "a2value", "-f" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.IsTrue(testParams.IsFree);
            Assert.AreEqual("svalue", testParams.String);
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.IsTrue(testParams.Array.Contains("a2value"));

            args = new []{ "-s:value", "-a", "a1value", "a2value"};
            testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.AreEqual("value", testParams.String);
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.IsTrue(testParams.Array.Contains("a2value"));

            args = new []{ "/flag", "/a:a1value,a2value", "/s=value" };
            testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.IsTrue(testParams.IsFree);
            Assert.AreEqual("value", testParams.String);
            Assert.AreEqual(2, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.IsTrue(testParams.Array.Contains("a2value"));
        }

        [TestMethod]
        public void TestArgumentParser_Parse_NonString()
        {
            // Int & Enum values
            string[] args = { "/s:svalue", "/a:a1value", "/iv:23", "/ev:Third" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.AreEqual("svalue", testParams.String);
            Assert.AreEqual(1, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.AreEqual(23, testParams.Count);
            Assert.AreEqual(TestOrdinal.Third, testParams.OrdinalVal);

            // Int & Enum arrays
            args = new []{ "-s:value", "-a:a1value", "-ia:4,5", "/ea:First,Second"};
            testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.AreEqual("value", testParams.String);
            Assert.AreEqual(1, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.AreEqual(2, testParams.IntArray.Count());
            Assert.IsTrue(testParams.IntArray.Contains(4));
            Assert.IsTrue(testParams.IntArray.Contains(5));
            Assert.AreEqual(2, testParams.EnumArray.Count());
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.First));
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.Second));
        }

        [TestMethod]
        public void TestArgumentParser_Parse_Defaults()
        {
            // Default values set correctly
            string[] args = { "/s:svalue", "/a:a1value" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNotNull(testParams);
            Assert.AreEqual("svalue", testParams.String);
            Assert.AreEqual(1, testParams.Array.Count());
            Assert.IsTrue(testParams.Array.Contains("a1value"));
            Assert.AreEqual(true, testParams.FreeDefault);
            Assert.AreEqual(42, testParams.Count);
            Assert.AreEqual(2, testParams.EnumArray.Count());
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.Second));
            Assert.IsTrue(testParams.EnumArray.Contains(TestOrdinal.Third));
        }

        [TestMethod]
        public void TestArgumentParser_Parse_DuplicateParams()
        {
            string[] args = { "/s:sval1", "/a:a1value", "/s:sval2" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNull(testParams);

            // Ensure error message is displayed
            Assert.AreEqual(1, Logger.ErrorLog.Count);
            Assert.AreEqual("Error: Argument 's' re-defined with value 'sval2'.", Logger.ErrorLog.First());
        }

        [TestMethod]
        public void TestArgumentParser_Parse_UnrecognizedParam()
        {
            string[] args = { "/xyz", "value" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNull(testParams);

            // Ensure error message is displayed
            Assert.AreEqual(1, Logger.ErrorLog.Count);
            Assert.AreEqual("Error: Unrecogized parameter '/xyz'.", Logger.ErrorLog.First());
        }

        [TestMethod]
        public void TestArgumentParser_Parse_MissingParams()
        {
            string[] args = { "/a", "a1value", "a2value" };
            TestParams testParams = ArgumentParser<TestParams>.Parse(args);
            Assert.IsNull(testParams);

            // Ensure error message is displayed
            Assert.AreEqual(1, Logger.ErrorLog.Count);
            Assert.AreEqual("Error: Argument [-s or --str] is required.", Logger.ErrorLog.First());
        }

        [TestMethod]
        public void TestArgumentParser_Usage()
        {
            // Process name may or may not have x86 appended to it, depending on the platform
            string processName = Process.GetCurrentProcess().ProcessName;
            string expectedUsage = string.Format("{0}{1}{2}", Environment.NewLine, processName, ExpectedUsage);
            string actualUsage = ArgumentParser<TestParams>.Usage();
            Assert.AreEqual(expectedUsage, actualUsage);
        }

        /// <summary>
        /// Expected usage string for <see cref="TestParams"/>
        /// </summary>
        private const string ExpectedUsage =
            @"

    Test params class

  -s             --str                    String value. e.g. xyzzy
  -a                                      Array of strings. e.g. a b c

  -h             --help                   Prints this help text (Optional)
  -f             --flag                   Boolean flag. (Optional)
  -df                                     Boolean flag with default true. (Optional)
  -iv                                     Int value. e.g. 42 (Optional)
  -ev                                     Enum value value. e.g. First (Optional)
  -ia                                     Array of integers. e.g. 1 2 3 (Optional)
  -ea                                     Array of enums. e.g. First Second Third (Optional)
";
    }
}
