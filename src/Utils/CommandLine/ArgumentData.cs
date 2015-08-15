using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Utils.Logging;

namespace Utils.CommandLine
{
    public class ArgumentData
    {
        internal string Name
        {
            get
            {
                bool hasKey = !string.IsNullOrWhiteSpace(Key);
                bool hasLongKey = !string.IsNullOrWhiteSpace(LongKey);

                if (hasKey && hasLongKey)
                {
                    return string.Format("-{0} or --{1}", Key, LongKey);
                }

                if (hasKey)
                {
                    return "-" + Key;
                }

                return "--" + LongKey;
            }
        }
        /// <summary>
        /// Type of argument, see <see cref="ArgumentType"/>
        /// </summary>
        internal ArgumentType Type { get; set; }

        /// <summary>
        /// Short name for param, e.g. x
        /// </summary>
        internal string Key { get; set; }

        /// <summary>
        /// Longer alternative for param, e.g. xyzzy
        /// </summary>
        internal string LongKey { get; set; }

        /// <summary>
        /// Description about the param
        /// </summary>
        internal string Details { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        internal string DefaultValue { get; set; }

        /// <summary>
        /// Whether the argument is optional
        /// </summary>
        internal bool Optional { get; set; }

        /// <summary>
        /// Reference to the <see cref="PropertyInfo"/>
        /// </summary>
        internal PropertyInfo Property { get; set; }

        /// <summary>
        /// Reference to the <see cref="PropertyInfo"/>
        /// </summary>
        internal bool Seen { get; set; }

        /// <summary>
        /// Internal constructor
        /// </summary>
        internal ArgumentData()
        {
        }

        /// <summary>
        /// string representation of the argument
        /// </summary>
        /// <returns>sting representation</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string temp = string.Empty;
            if (!string.IsNullOrWhiteSpace(Key))
            {
                temp = "-" + Key;
            }

            sb.Append(temp.PadRight(15));
            temp = string.Empty;
            if (!string.IsNullOrWhiteSpace(LongKey))
            {
                temp = "--" + LongKey;
            }

            sb.Append(temp.PadRight(25));
            sb.Append(Details);

            return sb.ToString();
        }

        internal static ArgumentData Parse(PropertyInfo propertyInfo)
        {
            DetailsAttribute detailsAttr = propertyInfo.GetCustomAttribute<DetailsAttribute>();
            OptionalAttribute optionalAttr = propertyInfo.GetCustomAttribute<OptionalAttribute>();
            ParamAttribute paramAttr = propertyInfo.GetCustomAttribute<ParamAttribute>();
            if (paramAttr == null)
            {
                return null;
            }

            if (!IsTypeSupported(propertyInfo.PropertyType, paramAttr.Type))
            {
                throw new InvalidOperationException(
                    string.Format("Incompatible type specified for '{0}'", propertyInfo.Name));
            }

            if (string.IsNullOrWhiteSpace(paramAttr.Key) && string.IsNullOrWhiteSpace(paramAttr.LongKey))
            {
                throw new InvalidOperationException(
                    string.Format("Both Key & LongKey cannot be null for property '{0}'.", propertyInfo.Name));
            }

            string details = detailsAttr != null ? detailsAttr.Details : null;
            string defaultValue = optionalAttr != null ? optionalAttr.DefaultValue : null;
            ArgumentData data = new ArgumentData
            {
                DefaultValue = defaultValue,
                Details = details,
                Key = paramAttr.Key,
                LongKey = paramAttr.LongKey,
                Optional = optionalAttr != null,
                Property = propertyInfo,
                Type = paramAttr.Type,
            };
            return data;
        }

        /// <summary>
        /// Checks whether current type is compatible with given ArgumentType
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="argumentType">Argument type</param>
        /// <returns>True if current type is supported</returns>
        internal static bool IsTypeSupported(Type type, ArgumentType argumentType)
        {
            switch (argumentType)
            {
                case ArgumentType.Flag:
                    return type == typeof(bool);

                case ArgumentType.Param:
                    break;

                case ArgumentType.ParamArray:
                    if (type.IsArray || (type.IsGenericType && type.GenericTypeArguments.Length == 1))
                    {
                        return true;
                    }

                    LogHelper.LogError("ParamArray should be of type Array or collection with single generic type argument.");
                    return false;

                default:
                    throw new NotImplementedException();
            }

            return true;
        }
    }
}