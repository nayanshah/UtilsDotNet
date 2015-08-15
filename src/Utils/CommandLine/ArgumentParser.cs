using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Utils.Logging;
using Utils.Templates;

namespace Utils.CommandLine
{
    public class ArgumentParser<T> where T : new()
    {
        /// <summary>
        /// Result object
        /// </summary>
        private readonly char[] SplitChars = { ',', ';' };

        /// <summary>
        /// List of arguments
        /// </summary>
        private string[] Arguments { get; set; }

        /// <summary>
        /// Data about the arguments
        /// </summary>
        private IDictionary<string, ArgumentData> Data { get; set; }

        /// <summary>
        /// Whether user requested help
        /// </summary>
        private bool NeedHelp { get; set; }

        /// <summary>
        /// Current position of the parser
        /// </summary>
        private int Position { get; set; }

        /// <summary>
        /// Result object
        /// </summary>
        private T Result { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentParser{T}"/>
        /// </summary>
        /// <param name="arguments">List of command line arguments</param>
        /// <param name="data"><see cref="ArgumentData"/></param>
        internal ArgumentParser(string[] arguments, IDictionary<string, ArgumentData> data)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Arguments = arguments;
            Data = data;
            Result = new T();
            Position = -1;
        }

        /// <summary>
        /// Returns an instance of custom parameters class
        /// </summary>
        /// <param name="arg">List of arguments to parse</param>
        /// <returns>Parsed arguments</returns>
        public static T Parse(string[] arg)
        {
            var parser = new ArgumentParser<T>(arg, ArgumentParserUtils.GetArgumentData<T>());
            if (!parser.Parse())
            {
                // TODO: Remove calls to PrintUsage from consumers and uncomment below
                //if(parser.NeedHelp)
                //{
                //    PrintUsage();
                //}

                return default(T);
            }


            return parser.Result;
        }

        /// <summary>
        /// Prints help text based on custom arguments class
        /// </summary>
        public static void PrintUsage()
        {
            Console.Write(Usage());
        }

        /// <summary>
        /// Generates help text based on custom arguments class
        /// </summary>
        internal static string Usage()
        {
            IDictionary<string, ArgumentData> data = ArgumentParserUtils.GetArgumentData<T>();
            DetailsAttribute detailsAttr = typeof(T).GetCustomAttribute<DetailsAttribute>();
            string details = detailsAttr != null ? detailsAttr.Details : string.Empty;
            ITemplate template = new CommandLineUsage
            {
                Name = Process.GetCurrentProcess().ProcessName,
                Description = details,
                Required = data.Values.Where(x => !x.Optional).Distinct(),
                Optional = data.Values.Where(x => x.Optional).Distinct(),
            };

            return template.Print();
        }

        /// <summary>
        /// Internal helper method for parsing arguments
        /// </summary>
        /// <returns>True if all arguments were successfully parsed</returns>
        internal bool Parse()
        {
            while (++Position < Arguments.Length)
            {
                Tuple<string, string> param = ArgumentParserUtils.ParseParam(Arguments[Position]);
                if (param == null || !Data.ContainsKey(param.Item1))
                {
                    if (Arguments[Position].Equals("/?"))
                    {
                        NeedHelp = true;
                        return false;
                    }

                    LogHelper.LogError("Unrecogized parameter '{0}'.", Arguments[Position]);
                    return false;
                }

                if (param.Item1.Equals("h", StringComparison.OrdinalIgnoreCase) 
                    || param.Item1.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    NeedHelp = true;
                    return false;
                }

                ArgumentData argumentData = Data[param.Item1];
                if (argumentData.Seen)
                {
                    LogHelper.LogError("Argument '{0}' re-defined with value '{1}'.", param.Item1, param.Item2);
                    return false;
                }

                argumentData.Seen = true;
                if (!HandleArgument(argumentData, param.Item2))
                {
                    return false;
                }
            }

            foreach (ArgumentData argumentData in Data.Values.Where(d => !d.Seen))
            {
                if (!argumentData.Optional)
                {
                    LogHelper.LogError("Argument [{0}] is required.", argumentData.Name);
                    return false;
                }

                if (!HandleDefaultValue(argumentData))
                {
                    return false;
                }
            }

            return true;
        }

        #region Private methods

        /// <summary>
        /// Mapping between argument type and it's default value handler
        /// </summary>
        /// <param name="argumentData">Argument data</param>
        /// <returns>True if it was successfully handled</returns>
        private bool HandleDefaultValue(ArgumentData argumentData)
        {
            string value = argumentData.DefaultValue;
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            switch (argumentData.Type)
            {
                case ArgumentType.Flag:
                case ArgumentType.Param:
                    return Result.TrySetPropertyValue(argumentData.Property, value);

                case ArgumentType.ParamArray:
                    string[] values = value.Split(SplitChars);
                    return Result.TrySetPropertyValue(argumentData.Property, values);

                default:
                    throw new NotImplementedException(
                        string.Format("No handler found for ArgumentType '{0}'", argumentData.Type));
            }
        }

        /// <summary>
        /// Mapping between argument type and it's handler
        /// </summary>
        /// <param name="argumentData">Argument data</param>
        /// <param name="value">Value for the argument</param>
        /// <returns>True if it was successfully handled</returns>
        private bool HandleArgument(ArgumentData argumentData, string value)
        {
            switch (argumentData.Type)
            {
                case ArgumentType.Flag:
                    return HandleFlag(argumentData, value);

                case ArgumentType.Param:
                    return HandleParam(argumentData, value);

                case ArgumentType.ParamArray:
                    return HandleParamArray(argumentData, value);

                default:
                    throw new NotImplementedException(
                        string.Format("No handler found for ArgumentType '{0}'", argumentData.Type));
            }
        }

        /// <summary>
        /// Handler for boolean flag arguments
        /// </summary>
        /// <param name="argument"><see cref="ArgumentData"/></param>
        /// <param name="value">Value of argument</param>
        /// <returns>True if it was successfully handled</returns>
        private bool HandleFlag(ArgumentData argument, string value)
        {
            bool boolValue = true;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!bool.TryParse(value, out boolValue))
                {
                    LogHelper.LogError("Invalid value '{0}' passed for flag parameter '{1}'", value, argument.Key);
                    return false;
                }
            }

            argument.Property.SetValue(Result, boolValue);
            return true;
        }

        /// <summary>
        /// Handler for single param arguments
        /// </summary>
        /// <param name="argument"><see cref="ArgumentData"/></param>
        /// <param name="value">Value of argument</param>
        /// <returns>True if it was successfully handled</returns>
        private bool HandleParam(ArgumentData argument, string value)
        {
            // Param is specified as key:value or key=value
            if (!string.IsNullOrWhiteSpace(value))
            {
                return Result.TrySetPropertyValue(argument.Property, value);
            }

            // Value should be the next argumentKey
            if (++Position < Arguments.Length)
            {
                Tuple<string, string> nextParam = ArgumentParserUtils.ParseParam(Arguments[Position]);
                if (nextParam == null)
                {
                    return Result.TrySetPropertyValue(argument.Property, Arguments[Position]);
                }
            }

            Position--;
            LogHelper.LogError("Value for parameter '{0}' not specified", argument.Key);
            return false;
        }

        /// <summary>
        /// Handler for param array arguments
        /// </summary>
        /// <param name="argument"><see cref="ArgumentData"/></param>
        /// <param name="value">Value of argument</param>
        /// <returns>True if it was successfully handled</returns>
        private bool HandleParamArray(ArgumentData argument, string value)
        {
            // Param is specified as key:value or key=value
            if (!string.IsNullOrWhiteSpace(value))
            {
                return Result.TrySetPropertyValue(argument.Property, value.Split(SplitChars));
            }

            // Value should be the next argumentKey
            IList<string> array = new List<string>();
            while (++Position < Arguments.Length)
            {
                Tuple<string, string> nextParam = ArgumentParserUtils.ParseParam(Arguments[Position]);
                if (nextParam != null)
                {
                    Position--;
                    break;
                }

                array.Add(Arguments[Position]);
            }

            if (array.Count > 0)
            {
                return Result.TrySetPropertyValue(argument.Property, array);
            }

            LogHelper.LogError("No values specified for array '{0}'", argument.Key);
            return false;
        }

        #endregion
    }
}