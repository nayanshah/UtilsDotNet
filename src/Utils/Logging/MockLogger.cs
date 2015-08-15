using System.Collections.Generic;

namespace Utils.Logging
{
    public class MockLogger : ILogger
    {
        /// <summary>
        /// Stores all logged messages
        /// </summary>
        private IDictionary<LogType, IList<string>> LogData { get; set; }

        /// <summary>
        /// Info messages
        /// </summary>
        public IList<string> InfoLog { get { return LogData[LogType.Info]; } }

        /// <summary>
        /// Normal messages
        /// </summary>
        public IList<string> MessageLog { get { return LogData[LogType.Message]; } }

        /// <summary>
        /// Success messages
        /// </summary>
        public IList<string> SuccessLog { get { return LogData[LogType.Success]; } }

        /// <summary>
        /// Warning messages
        /// </summary>
        public IList<string> WarningLog { get { return LogData[LogType.Warning]; } }

        /// <summary>
        /// Error messages
        /// </summary>
        public IList<string> ErrorLog { get { return LogData[LogType.Error]; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MockLogger()
        {
            LogData = new Dictionary<LogType, IList<string>>();
        }

        /// <summary>
        /// Logs a Info / Verbose messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        public void LogInfo(string message, params object[] args)
        {
            SaveLog(LogType.Info, message, args);
        }

        /// <summary>
        /// Logs normal messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        public void LogMessage(string message, params object[] args)
        {
            SaveLog(LogType.Message, message, args);
        }

        /// <summary>
        /// Logs success messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        public void LogSuccess(string message, params object[] args)
        {
            SaveLog(LogType.Success, message, args);
        }

        /// <summary>
        /// Logs warning messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        public void LogWarning(string message, params object[] args)
        {
            SaveLog(LogType.Warning, message, args);
        }

        /// <summary>
        /// Logs error messages
        /// </summary>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        public void LogError(string message, params object[] args)
        {
            SaveLog(LogType.Error, message, args);
        }

        /// <summary>
        /// Saves the log
        /// </summary>
        /// <param name="type">LogType</param>
        /// <param name="message">Message format</param>
        /// <param name="args">Format arguments</param>
        private void SaveLog(LogType type, string message, params object[] args)
        {
            if (!LogData.ContainsKey(type))
            {
                LogData[type] =  new List<string>();
            }

            string prefix = type != LogType.Success ? type + ": " : string.Empty;
            LogData[type].Add(string.Format(prefix + message, args));
        }
    }
}