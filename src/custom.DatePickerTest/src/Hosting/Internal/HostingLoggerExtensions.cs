// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    internal static class HostingLoggerExtensions
    {
        public static void ApplicationError(this ILogger logger, EventId eventId, string message, Exception exception)
        {
            var ex = exception as ReflectionTypeLoadException;
            if (ex != null)
            {
                var loaderExceptions = ex.LoaderExceptions;
                if (loaderExceptions != null)
                {
                    foreach (var ex2 in loaderExceptions)
                    {
                        if (ex2 != null)
                        {
                            message = message + Environment.NewLine + ex2.Message;
                        }
                    }
                }
            }

            var message2 = message;
            logger.LogCritical(eventId, exception, message2);
        }

        public static void Started(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(2, "Hosting started");
            }
        }

        public static void Starting(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(1, "Hosting starting");
            }
        }

        public static void Stopped(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(4, "Hosting stopped");
            }
        }

        public static void StoppedWithException(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(5, ex, "Hosting shutdown exception");
            }
        }

        public static void Stopping(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(3, "Hosting stopping");
            }
        }
    }
}
