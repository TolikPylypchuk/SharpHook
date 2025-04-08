namespace SharpHook.Logging;

/// <summary>
/// Represents a source of libuiohook logs.
/// </summary>
/// <remarks>
/// <para>
/// Only a single <see cref="LogSource" /> instance must be used at a time.
/// </para>
/// <para>
/// On Windows this class requires the Visual C++ Redistributable package to be installed as it uses the C runtime.
/// </para>
/// </remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="EmptyLogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
/// <seealso cref="LogEntryParser" />
[ExcludeFromCodeCoverage]
public sealed class LogSource : ILogSource
{
#if NET9_0_OR_GREATER
    private static readonly Lock syncRoot = new();
#else
    private static readonly object syncRoot = new();
#endif

    private static LogSource? registeredLogSource = null;

    private readonly LogEntryParser parser;
    private readonly LoggerProc loggerProc;
    private readonly ILoggingProvider loggingProvider;

    private LogSource(ILoggingProvider loggingProvider, LogEntryParser parser, LogLevel minLevel)
    {
        this.loggerProc = this.OnLog;
        this.loggingProvider = loggingProvider;
        this.parser = parser;
        this.MinLevel = minLevel;
    }

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
    /// Creates and registers a source of libuiohook logs or returns the registered instance.
    /// </summary>
    /// <param name="minLevel">The minimum log level.</param>
    /// <returns>A source of libuiohook logs.</returns>
    public static LogSource RegisterOrGet(LogLevel minLevel = LogLevel.Info) =>
        RegisterOrGet(UioHookProvider.Instance, minLevel);

    /// <summary>
    /// Creates and registers a source of libuiohook logs or returns the registered instance.
    /// </summary>
    /// <param name="loggingProvider">The logging provider used to register the log source.</param>
    /// <param name="minLevel">The minimum log level.</param>
    /// <returns>A source of libuiohook logs.</returns>
    /// <remarks>
    /// A single instance of <see cref="LogSource" /> can be registered using this method, irrespective of the logging
    /// provider used.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="loggingProvider" /> is <see langword="null" />.
    /// </exception>
    public static LogSource RegisterOrGet(ILoggingProvider loggingProvider, LogLevel minLevel = LogLevel.Info)
    {
        if (loggingProvider is null)
        {
            throw new ArgumentNullException(nameof(loggingProvider));
        }

        lock (syncRoot)
        {
            if (registeredLogSource is null)
            {
                registeredLogSource = new LogSource(loggingProvider, LogEntryParser.Instance, minLevel);
                loggingProvider.SetLoggerProc(registeredLogSource.loggerProc, IntPtr.Zero);
            }

            return registeredLogSource;
        }
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

        lock (syncRoot)
        {
            registeredLogSource = null;
            this.loggingProvider.SetLoggerProc(null, IntPtr.Zero);
        }

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
