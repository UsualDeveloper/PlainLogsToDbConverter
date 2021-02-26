using System;

namespace PlainLogsToDbConverter.SqlLogger.Interfaces
{
    public interface ISqlLogger: IDisposable
    {
        void AddToLog(string message, object[] fieldValues);
    }
}
