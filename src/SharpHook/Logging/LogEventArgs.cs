namespace SharpHook.Logging;

/// <summary>
/// Event args for logging events.
/// </summary>
/// <param name="logEntry">The entry of log event.</param>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="Logging.LogEntry" />
[ExcludeFromCodeCoverage]
public sealed class LogEventArgs(LogEntry logEntry) : EventArgs
{
    /// <summary>
    /// Gets the entry of the current log event.
    /// </summary>
    /// <value>The entry of the current log event.</value>
    public LogEntry LogEntry { get; } = logEntry;
}
