namespace Utils.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Logs a Info / Verbose messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Logs a Normal message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        void LogMessage(string message, params object[] args);

        /// <summary>
        /// Logs a Success message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        void LogSuccess(string message, params object[] args);

        /// <summary>
        /// Logs a Warning message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Logs an Error message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        void LogError(string message, params object[] args);
    }
}