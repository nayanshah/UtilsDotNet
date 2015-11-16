using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Utils.Logging;

namespace Utils.CommandLine
{
    public static class ArgumentParserUtils
    {
        public static Tuple<string, string> ParseParam(string arg)
        {
            Regex r = new Regex(@"^([/\-]|\-\-)(?<Key>\w[\w\-]*)(?<Value>[:=].+)?");
            Match m = r.Match(arg);
            if (!m.Success)
            {
                return null;
            }

            // strip out the separator [:=]
            string value = m.Groups["Value"].Value;
            if (!String.IsNullOrWhiteSpace(value))
            {
                value = value.Substring(1);
            }

            return Tuple.Create(m.Groups["Key"].Value, value);
        }

        /// <summary>
        /// Constructs a mapping between argument keys & other related data
        /// </summary>
        /// <typeparam name="T">Custom argument class</typeparam>
        /// <returns>Mapping between arguments & corresponding data</returns>
        internal static IDictionary<string, ArgumentData> GetArgumentData<T>() 
        {
            ArgumentData helpData = new ArgumentData
            {
                Details = "Prints this help text",
                Key = "h",
                LongKey = "help",
                Optional = true,
                Type = ArgumentType.Flag,
            };

            IDictionary<string, ArgumentData> mapping = new Dictionary<string, ArgumentData>(StringComparer.OrdinalIgnoreCase)
            {
                { "h", helpData },
                { "help", helpData },
            };

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                ParamAttribute paramAttr = propertyInfo.GetCustomAttribute<ParamAttribute>();
                if (paramAttr == null)
                {
                    continue;
                }

                ArgumentData data = ArgumentData.Parse(propertyInfo);
                mapping.SafeAdd(paramAttr.Key, data);
                mapping.SafeAdd(paramAttr.LongKey, data);
            }

            return mapping;
        }

        /// <summary>
        /// Initializes given object's property with correct argumentType of value
        /// </summary>
        /// <param name="obj">Given object</param>
        /// <param name="propertyInfo">PropertyInfo for property to initialize</param>
        /// <param name="value">Value for given property</param>
        internal static bool TrySetPropertyValue(this object obj, PropertyInfo propertyInfo, string value)
        {
            try
            {
                propertyInfo.SetValue(obj, value.ChangeToType(propertyInfo.PropertyType));
                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogInfo("Exception while initializing '{0}'. Message: '{1}'", propertyInfo.Name, e.Message);
                if (propertyInfo.PropertyType.IsEnum)
                {
                    string validValues = propertyInfo.PropertyType.ValidEnumValues();
                    LogHelper.LogError("Unknown value '{0}' given. Valid values - [{1}]", value, validValues);
                    return false;
                }

                LogHelper.LogError("Given value '{0}' should be of type '{1}'", value, propertyInfo.PropertyType.Name);
                return false;
            }
        }

        /// <summary>
        /// Initializes given object's property with correct argumentType of values
        /// </summary>
        /// <param name="obj">Given object</param>
        /// <param name="propertyInfo">PropertyInfo for property to initialize</param>
        /// <param name="values">List of values for given property</param>
        internal static bool TrySetPropertyValue(this object obj, PropertyInfo propertyInfo, ICollection<string> values)
        {
            if (propertyInfo.PropertyType.GenericTypeArguments.Length != 1)
            {
                throw new InvalidOperationException("Multiple generic arguments not supported.");
            }

            Type genericType = propertyInfo.PropertyType.GenericTypeArguments[0];
            try
            {
                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));
                foreach (string value in values)
                {
                    list.Add(value.ChangeToType(genericType));
                }

                propertyInfo.SetValue(obj, list);
                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogInfo("Exception while initializing '{0}'. Message: '{1}'", propertyInfo.Name, e.Message);
                string valuesStr = string.Join(", ", values);
                if (genericType.IsEnum)
                {
                    string validValues = genericType.ValidEnumValues();
                    LogHelper.LogError(
                        "Given list '{0}' contains an unknown value. Valid values - [{1}]", valuesStr, validValues);
                    return false;
                }

                LogHelper.LogError("Given values '{0}' should be a list of type '{1}'", valuesStr, genericType.Name);
                return false;
            }
        }

        /// <summary>
        /// Returns string representation of all valid Enum values
        /// </summary>
        /// <param name="type">Type of Enum</param>
        /// <returns>csv list of all Enum values</returns>
        internal static string ValidEnumValues(this Type type)
        {
            if (!type.IsEnum)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' is not of Enum argumentType.", type.Name));
            }

            return string.Join(
                    ", ",
                    Enum.GetValues(type)
                        .OfType<object>()
                        .Select(o => o.ToString()));
        }

        /// <summary>
        /// Changes string value to given Type
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="type">Type</param>
        /// <returns>value in given Type</returns>
        internal static object ChangeToType(this string value, Type type)
        {
            return type.IsEnum
                    ? Enum.Parse(type, value, true)
                    : Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Adds a key & value to dictionary if key isn't null
        /// </summary>
        /// <param name="dictionary">Dictionary to add to</param>
        /// <param name="key">Key</param>
        /// <param name="data">Value</param>
        /// <returns>True if the key was added, else false</returns>
        private static bool SafeAdd(this IDictionary<string, ArgumentData> dictionary, string key, ArgumentData data)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (dictionary.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Multiple params cannot have the same key: {0}",
                        key));
            }

            dictionary.Add(key, data);
            return true;
        }
    }
}