using log4net;
using System;

namespace TourGuide.Logs
{
    public static class LoggerHelper
    {
        private static readonly ILog log = LogManager.GetLogger("TourGuideLogger");

        public static void Info(string message)
        {
            if (log.IsInfoEnabled)
                log.Info(message);
        }

        public static void Warn(string message)
        {
            if (log.IsWarnEnabled)
                log.Warn(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            if (log.IsErrorEnabled)
            {
                if (ex != null)
                    log.Error(message, ex);
                else
                    log.Error(message);
            }
        }

        public static void Debug(string message)
        {
            if (log.IsDebugEnabled)
                log.Debug(message);
        }

        public static void Fatal(string message, Exception ex = null)
        {
            if (log.IsFatalEnabled)
            {
                if (ex != null)
                    log.Fatal(message, ex);
                else
                    log.Fatal(message);
            }
        }
    }
}