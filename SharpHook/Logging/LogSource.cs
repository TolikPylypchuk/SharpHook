namespace SharpHook.Logging;

using System;

using SharpHook.Native;

/// <summary>
/// Represents a source of libuiohook logs.
/// </summary>
/// <remarks>Only a single <see cref="LogSource" /> instance must be used at a time.</remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public sealed class LogSource : ILogSource
{
    private readonly LogEntryParser parser = new();
    private bool disposed;

    private LogSource(LogLevel minLevel) =>
        this.MinLevel = minLevel;

    /// <summary>
    /// Unregisters the current <see cref="LogSource" /> instance.
    /// </summary>
    ~LogSource() =>
        this.Dispose(false);

    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    /// <value>The minimum log level.</value>
    public LogLevel MinLevel { get; set; }

    /// <summary>
    /// Creates and registers a source of libuiohook logs.
    /// </summary>
    /// <param name="minLevel">The minimum log level.</param>
    /// <returns>A source of libuiohook logs.</returns>
    /// <remarks>
    /// This method must not be called when another <see cref="LogSource" /> instance has already been registerd and
    /// hasn't been disposed.
    /// </remarks>
    public static LogSource Register(LogLevel minLevel = LogLevel.Info)
    {
        var source = new LogSource(minLevel);
        UioHook.SetLoggerProc(source.OnLog);
        return source;
    }

    /// <summary>
    /// Stops the currently registered <see cref="LogSource" /> instance from receiving libuiohook logs.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        UioHook.SetLoggerProc(null);

        if (disposing)
        {
            this.disposed = true;
        }
    }

    private bool OnLog(LogLevel level, IntPtr format, IntPtr args)
    {
        if (level < this.MinLevel)
        {
            return false;
        }

        var logEntry = this.parser.ParseNativeLogEntry(level, format, args);

        this.MessageLogged?.Invoke(this, new LogEventArgs(logEntry));

        return true;
    }

    /// <summary>
    /// An event which is raised when libuiohook logs a message.
    /// </summary>
    public event EventHandler<LogEventArgs>? MessageLogged;
}
