using System.Collections.Generic;
using Utils.CommandLine;

namespace UtilsTest.CommandLine
{
    [Details("Test params class")]
    public class TestParams
    {
        [Param(key:"s", longKey:"str")]
        [Details("String value. e.g. xyzzy")]
        public string String { get; set; }

        [Param(ArgumentType.Flag, "f", "flag")]
        [Details("Boolean flag.")]
        [Optional]
        public bool IsFree { get; set; }

        [Param(ArgumentType.Flag, "df")]
        [Details("Boolean flag with default true.")]
        [Optional(defaultValue: true)]
        public bool FreeDefault { get; set; }

        [Param(key: "iv")]
        [Details("Int value. e.g. 42")]
        [Optional(defaultValue: 42)]
        public int Count { get; set; }

        [Param(key: "ev")]
        [Details("Enum value value. e.g. First")]
        [Optional]
        public TestOrdinal OrdinalVal { get; set; }

        [Param(ArgumentType.ParamArray, "a")]
        [Details("Array of strings. e.g. a b c")]
        public IEnumerable<string> Array { get; set; }

        [Param(ArgumentType.ParamArray, "ia")]
        [Details("Array of integers. e.g. 1 2 3")]
        [Optional]
        public IEnumerable<int> IntArray { get; set; }

        [Param(ArgumentType.ParamArray, "ea")]
        [Details("Array of enums. e.g. First Second Third")]
        [Optional(defaultValue: "Second,Third")]
        public IEnumerable<TestOrdinal> EnumArray { get; set; }

        public string ExtraNonParamProperty { get; set; }
    }

    public enum TestOrdinal
    {
        First,
        Second,
        Third,
    }
}