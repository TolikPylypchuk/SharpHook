namespace SharpHook.Logging;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Event args for logging events.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="Logging.LogEntry" />
[ExcludeFromCodeCoverage]
public sealed class LogEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogEventArgs" /> class.
    /// </summary>
    /// <param name="logEntry">The entry of log event.</param>
    public LogEventArgs(LogEntry logEntry) =>
        this.LogEntry = logEntry;

    /// <summary>
    /// Gets the entry of the current log event.
    /// </summary>
    /// <value>The entry of the current log event.</value>
    public LogEntry LogEntry { get; }
}
