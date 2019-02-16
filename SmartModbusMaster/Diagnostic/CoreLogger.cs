namespace Kr.Communication.SmartModbusMaster.Diagnostic
{
    using System;

    public class CoreLogger : ICoreLogger
    {
        public void Log(CoreLogLevel loglevel, string message, Exception exception)
        {
            var hasGlobalListeners = GlobalLogger.HasListeners;

            var newlog = new LogMessage(DateTime.UtcNow, Environment.CurrentManagedThreadId, loglevel, message, exception);
            
            if (hasGlobalListeners)
            {
                GlobalLogger.Publish(newlog);
            }
        }

        public void Fatal(Exception exception, string message)
        {
            Log(CoreLogLevel.Fatal, message, exception);
        }

        public void Info(string message)
        {
            Log(CoreLogLevel.Info, message, null);
        }        

        public void Trace(string message)
        {
            Log(CoreLogLevel.Trace, message, null);
        }

        public void Warning(Exception exception, string message)
        {
            Log(CoreLogLevel.Warning, message, exception);
        }
    }
}
