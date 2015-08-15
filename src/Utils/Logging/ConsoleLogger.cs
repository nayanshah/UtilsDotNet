using System;

namespace Utils.Logging
{
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// App name used to prefix logs
        /// </summary>
        private readonly string _app;

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger"/>
        /// </summary>
        /// <param name="app">Optional name of the app</param>
        public ConsoleLogger(string app = null)
        {
            _app = string.Empty;
            if (!string.IsNullOrWhiteSpace(app))
            {
                _app += app + ": ";
            }
        }

        /// <summary>
        /// Logs an Info / Verbose message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public void LogInfo(string message, params object[] args)
        {
            string format = string.Format("{0}Info: {1}", _app, message);
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Logs a Normal message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public void LogMessage(string message, params object[] args)
        {
            string format = string.Format("{0}{1}", _app, message);
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Logs a Success message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public void LogSuccess(string message, params object[] args)
        {
            string format = string.Format("{0}Success: {1}", _app, message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Logs a Warning message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public void LogWarning(string message, params object[] args)
        {
            string format = string.Format("{0}Warning: {1}", _app, message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Logs an Error message
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format argmuents</param>
        public void LogError(string message, params object[] args)
        {
            string format = string.Format("{0}Error: {1}", _app, message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(format, args);
            Console.ResetColor();
        }
    }
}