namespace Kr.Communication.SmartModbusMaster.Diagnostic
{
    using System;

    public class LogReceivedEventArgs : EventArgs
    {
        public LogReceivedEventArgs(LogMessage logmessage)
        {
            Message = logmessage;
        }

        public LogMessage Message { get; }
    }
}
