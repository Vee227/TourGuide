using Xunit;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using System.Collections.Generic;
using TourGuide.Logs;

namespace TourGuide.Test
{
    public class LoggerHelperTests
    {
        private readonly MemoryAppender _memoryAppender;

        public LoggerHelperTests()
        {
            _memoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(_memoryAppender);
        }

        [Fact]
        public void Info_ShouldLogInfoMessage()
        {
            string message = "Test Info Message";
            
            LoggerHelper.Info(message);
            LoggingEvent[] events = _memoryAppender.GetEvents();
            
            Assert.Contains(events, e =>
                e.Level == Level.Info && e.RenderedMessage == message);
        }
        
        [Fact]
        public void Warn_ShouldLogWarningMessage()
        {
            string message = "Test Warning";

            LoggerHelper.Warn(message);
            LoggingEvent[] events = _memoryAppender.GetEvents();
            
            Assert.Contains(events, e =>
                e.Level == Level.Warn && e.RenderedMessage == message);
        }
        
        [Fact]
        public void Error_ShouldLogErrorWithException()
        {
            string message = "Something went wrong!";
            var exception = new System.Exception("Boom");

            LoggerHelper.Error(message, exception);
            LoggingEvent[] events = _memoryAppender.GetEvents();

            Assert.Contains(events, e =>
                e.Level == Level.Error &&
                e.RenderedMessage == message &&
                e.ExceptionObject?.Message == "Boom");
        }
    }
}