namespace Kr.Communication.SmartModbusMaster.Diagnostic
{
    using System;

    public class LogMessage
    {
        public LogMessage(DateTime timestamp, int threadId, CoreLogLevel level, string message, Exception exception)
        {
            Timestamp = timestamp;
            ThreadId = threadId;
            Level = level;
            Message = message;
            Exception = exception;
        }

        public Exception Exception { get; }

        public CoreLogLevel Level { get; }

        public string Message { get; }

        public int ThreadId { get; }

        public DateTime Timestamp { get; }

        public override string ToString()
        {
            var result = $"[{Timestamp:O}] ({ThreadId}) [{Level}]: {Message}";
            if (Exception != null)
            {
                result += Environment.NewLine + Exception;
            }

            return result;
        }
    }
}