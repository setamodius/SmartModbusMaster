namespace Kr.Communication.SmartModbusMaster.Diagnostic
{
    using System;

    public class GlobalLogger
    {
        public static event EventHandler<LogReceivedEventArgs> LogMessageReceived;

        public static bool HasListeners => LogMessageReceived != null;

        public static void Publish(LogMessage logMessage)
        {
            if (logMessage == null) throw new ArgumentNullException(nameof(logMessage));

            LogMessageReceived?.Invoke(null, new LogReceivedEventArgs(logMessage));
        }
    }
}
