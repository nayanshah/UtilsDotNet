using System.Collections.Generic;

namespace Utils.Templates
{
    public interface ITemplate
    {
        /// <summary>
        /// Validates the template
        /// </summary>
        /// <returns>True if all required params have been initialized.</returns>
        bool Validate();

        /// <summary>
        /// Compares file contents with that of the template
        /// </summary>
        /// <param name="filePath">Path of local file</param>
        /// <param name="comparer">Optional comparer</param>
        /// <returns>True if the file contents matches that of the template</returns>
        bool Compare(string filePath, IEqualityComparer<string> comparer = null);

        /// <summary>
        /// Validates and then performs the variable replacements in the given template
        /// </summary>
        /// <returns>Evaluated template</returns>
        string Print();

        /// <summary>
        /// Validates & saves the template to given file
        /// </summary>
        /// <param name="filePath">File path for saving the template</param>
        /// <returns>True if the template was successfully saved</returns>
        bool Save(string filePath);
    }
}