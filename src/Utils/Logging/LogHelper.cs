using System;

namespace Utils.Logging
{
    public static class LogHelper
    {
        /// <summary>
        /// Instance of <see cref="ILogger"/>
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Property to access the instance of <see cref="ILogger"/>
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                return logger ?? (logger = new ConsoleLogger());
            }

            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Argument logger cannot be null.");
                }

                logger = value;
            }
        }

        /// <summary>
        /// Instance of <see cref="ILogger"/>
        /// </summary>
        public static bool ShouldLogInfo { get; set; }

        /// <summary>
        /// Logs in Info message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public static void LogInfo(string message, params object[] args)
        {
            if (ShouldLogInfo)
            {
                Logger.LogInfo(message, args);
            }
        }

        /// <summary>
        /// Logs a Normal message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public static void LogMessage(string message, params object[] args)
        {
            Logger.LogMessage(message, args);
        }

        /// <summary>
        /// Logs a Success message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public static void LogSuccess(string message, params object[] args)
        {
            Logger.LogSuccess(message, args);
        }

        /// <summary>
        /// Logs a Warning message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public static void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        /// <summary>
        /// Logs an Error message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public static void LogError(string message, params object[] args)
        {
            Logger.LogError(message, args);
        }
    }
}