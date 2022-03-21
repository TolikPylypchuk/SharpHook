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
/// <seealso cref="LogEntryParser" />
public sealed class LogSource : ILogSource
{
    private readonly LogEntryParser parser = new();

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
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>
    /// A disposed log source doesn't raise the <see cref="MessageLogged" /> event and cannot be registered again.
    /// </remarks>
    public bool IsDisposed { get; private set; } = false;

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
        UioHook.SetLoggerProc(source.OnLog, IntPtr.Zero);
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
        if (this.IsDisposed)
        {
            return;
        }

        UioHook.SetLoggerProc(null, IntPtr.Zero);

        if (disposing)
        {
            this.IsDisposed = true;
        }
    }

    private void OnLog(LogLevel level, IntPtr userData, IntPtr format, IntPtr args)
    {
        if (level < this.MinLevel)
        {
            return;
        }

        try
        {
            var logEntry = this.parser.ParseNativeLogEntry(level, format, args);
            this.MessageLogged?.Invoke(this, new LogEventArgs(logEntry));
        } catch { }
    }

    /// <summary>
    /// An event which is raised when libuiohook logs a message.
    /// </summary>
    public event EventHandler<LogEventArgs>? MessageLogged;
}
