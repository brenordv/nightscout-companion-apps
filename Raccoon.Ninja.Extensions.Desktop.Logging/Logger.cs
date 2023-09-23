using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Raccoon.Ninja.Extensions.Desktop.Logging;


public static class Logger
{
    public static event Action<LogLevel, string, object[]> OnLog;
    
    private static readonly ILogger Log = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger(typeof(Logger));

    public static void LogInfo(string message, params object[] args)
    {
        LogNonError(LogLevel.Information, message, args);
    }
    
    public static void LogTrace(string message, params object[] args)
    {
        LogNonError(LogLevel.Trace, message, args);
    }
    
    public static void LogError(string message, Exception exception, params object[] args)
    {
        const LogLevel logLevel = LogLevel.Error;
        OnLog?.Invoke(logLevel, message, args);
        Log.Log(logLevel, exception, message, args);
    }
    
    private static void LogNonError(LogLevel logLevel, string message, params object[] args)
    {
        OnLog?.Invoke(logLevel, message, args);
        Log.Log(logLevel, message, args);
    }
}