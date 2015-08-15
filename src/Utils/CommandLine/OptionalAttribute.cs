using System;

namespace Utils.CommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionalAttribute : Attribute
    {
        /// <summary>
        /// Default value of parameter
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OptionalAttribute()
        {
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="defaultValue">Default value</param>
        public OptionalAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="defaultValue">Default value</param>
        public OptionalAttribute(object defaultValue)
        {
            if (defaultValue == null)
            {
                throw new InvalidOperationException("DefaultValue if specified should not be null.");
            }

            DefaultValue = defaultValue.ToString();
        }
    }
}