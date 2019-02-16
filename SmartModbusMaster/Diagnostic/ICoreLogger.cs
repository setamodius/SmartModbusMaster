namespace Kr.Communication.SmartModbusMaster.Diagnostic
{
    using System;

    public interface ICoreLogger
    {
        void Fatal(Exception exception, string message);

        void Info(string message);

        void Log(CoreLogLevel loglevel, string message, Exception exception);

        void Trace(string message);

        void Warning(Exception exception, string message);
    }
}
