using MovieWebApp.Integration.Contracts;
using NLog;

namespace MovieWebApp.Integration
{
    public class LoggerManager : ILoggerManager
    {
		private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

		public LoggerManager()
		{
		}

		public void LogDebug(string message)
		{
			logger.Debug(message);
		}

		public void LogError(object obj)
		{
			logger.Error(obj);
		}

		public void LogInfo(string message)
		{
			logger.Info(message);
		}

		public void LogWarn(string message)
		{
			logger.Warn(message);
		}

	}
}
