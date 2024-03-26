using System;
using System.IO;
using Microsoft.Extensions.Logging;


public class NotepadLogProvider : ILoggerProvider
{
    private readonly string _filePath;

    public NotepadLogProvider(string filePath)
    {
        _filePath = filePath;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new NotepadLog(_filePath);
    }

    public void Dispose()
    {
    }
}

public class NotepadLog : ILogger
{
    private readonly string _filePath;
    private readonly object _lock = new object();

    public NotepadLog(string filePath)
    {
        _filePath = filePath;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        lock (_lock)
        {
            try
            {
                using (var writer = new StreamWriter(_filePath, append: true))
                {
                    var message = formatter(state, exception);
                    writer.WriteLine($"{DateTime.Now}: [{logLevel}] - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}























//using Microsoft.Extensions.Logging;
//using System;

//public sealed class NotepadLoggerProvider : ILogger
//{
//    private readonly string _name;
//    private readonly Func<NotepadLoggerProviderConfiguration> _getCurrentConfig;

//    public NotepadLoggerProvider(string name, Func<NotepadLoggerProviderConfiguration> getCurrentConfig)
//    {
//        _name = name;
//        _getCurrentConfig = getCurrentConfig;
//    }

//    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

//    public bool IsEnabled(LogLevel logLevel) =>
//        _getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

//    public void Log<TState>(
//        LogLevel logLevel,
//        EventId eventId,
//        TState state,
//        Exception? exception,
//        Func<TState, Exception?, string> formatter)
//    {
//        if (!IsEnabled(logLevel))
//        {
//            return;
//        }

//        NotepadLoggerProviderConfiguration config = _getCurrentConfig();
//        if (config.EventId == 0 || config.EventId == eventId.Id)
//        {
//            ConsoleColor originalColor = Console.ForegroundColor;

//            Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
//            Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

//            Console.ForegroundColor = originalColor;
//            Console.Write($"     {_name} - ");

//            Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
//            Console.Write($"{formatter(state, exception)}");

//            Console.ForegroundColor = originalColor;
//            Console.WriteLine();
//        }
//    }
//}
