namespace SharpHook.Logging;

using System;

using SharpHook.Native;

/// <summary>
/// Represents a log entry from libuiohook.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="LogEventArgs" />
public sealed class LogEntry
{
    public LogEntry(LogLevel level, string fullText, string format, string rawFormat, params object[] args)
    {
        this.Level = level;
        this.FullText = fullText;
        this.Format = format;
        this.RawFormat = rawFormat;
        this.Args = args;
    }

    /// <summary>
    /// Gets the level of the current log entry.
    /// </summary>
    /// <value>The level of the current log entry.</value>
    public LogLevel Level;

    /// <summary>
    /// Gets the full text of the current log entry.
    /// </summary>
    /// <value>The full text of the current log entry.</value>
    /// <remarks><c>entry.FullText</c> is the same as <c>String.Format(entry.Format, entry.Args)</c></remarks>
    public string FullText { get; }

    /// <summary>
    /// Gets the format of the current log entry.
    /// </summary>
    /// <value>The format of the current log entry.</value>
    public string Format { get; }

    /// <summary>
    /// Gets the raw format of the current log entry, as defined in libuiohook.
    /// </summary>
    /// <value>The raw format of the current log entry.</value>
    public string RawFormat { get; }

    /// <summary>
    /// Gets the arguments of the current log entry.
    /// </summary>
    /// <value>The arguments of the current log entry.</value>
    public object[] Args { get; }
}
