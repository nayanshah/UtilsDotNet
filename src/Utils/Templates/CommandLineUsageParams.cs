using System.Collections.Generic;
using System.Linq;
using Utils.CommandLine;

namespace Utils.Templates
{
    public partial class CommandLineUsage
    {
        /// <summary>
        /// Name of the tool
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description for the tool
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of required arguments
        /// </summary>
        internal IEnumerable<ArgumentData> Required { get; set; }

        /// <summary>
        /// List of optional arguments
        /// </summary>
        internal IEnumerable<ArgumentData> Optional { get; set; }

        /// <summary>
        /// Validates the template
        /// </summary>
        /// <returns>True if all required params have been validated.</returns>
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            Description = Description ?? string.Empty;
            Required = Required ?? Enumerable.Empty<ArgumentData>();
            Optional = Optional ?? Enumerable.Empty<ArgumentData>();

            return true;
        }
    }
}