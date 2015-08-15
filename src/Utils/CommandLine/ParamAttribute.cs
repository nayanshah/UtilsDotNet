using System;

namespace Utils.CommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParamAttribute : Attribute
    {
        /// <summary>
        /// Type of argument, see <see cref="ArgumentType"/>
        /// </summary>
        public ArgumentType Type { get; private set; }

        /// <summary>
        /// Short name for param, e.g. x
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Longer alternative for param, e.g. xyzzy
        /// </summary>
        public string LongKey { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ParamAttribute"/>
        /// </summary>
        /// <param name="type">Type of argument, see <see cref="ArgumentType"/></param>
        /// <param name="key">Short name for param, e.g. x</param>
        /// <param name="longKey">Longer alternative for param, e.g. xyzzy</param>
        public ParamAttribute(ArgumentType type = ArgumentType.Param, string key = null, string longKey = null)
        {
            Key = key;
            LongKey = longKey;
            Type = type;
        }
    }
}