namespace SharpHook.Logging;

using System;
using System.Linq;

using SharpHook.Internal;
using SharpHook.Native;

/// <summary>
/// Represents a log entry from libuiohook.
/// </summary>
/// <remarks>
/// <para>
/// Each log entry from libuiohook has the following format:
/// <code>
/// function [line]: message
/// </code>
/// </para>
/// <para>
/// The function and line reference the source code of libuiohook itself, and can be retrieved using the
/// <see cref="Function" /> and <see cref="SourceLine" /> properties. The message can be retrieved by splitting the text
/// by <c>:</c> and getting the trimmed second part.
/// </para>
/// </remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntryParser" />
public sealed class LogEntry : IEquatable<LogEntry>
{
    public LogEntry(LogLevel level, string rawText, string format, string rawFormat, params object[] args)
    {
        this.Level = level;
        this.FullText = rawText;
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
    /// <remarks>The text is formatted using the <c>vsprintf</c> function from the C runtime.</remarks>
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
    /// <remarks>The function and line are the first two arguments of the log entry.</remarks>
    public object[] Args { get; }

    /// <summary>
    /// Gets the libuiohook function which called the logger.
    /// </summary>
    /// <value>The function which called the logger.</value>
    /// <remarks>The function is the first argument of the log entry.</remarks>
    public string Function =>
        this.Args.Length > 0 && this.Args[0] is string func ? func : String.Empty;

    /// <summary>
    /// Gets the source code line in libuiohook where the logger was called.
    /// </summary>
    /// <value>The source code line in libuiohook where the logger was called.</value>
    /// <remarks>The source code line is the second argument of the log entry.</remarks>
    public uint SourceLine =>
        this.Args.Length > 1 && this.Args[1] is uint line ? line : 0;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is LogEntry entry && this.Equals(entry);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="entry">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(LogEntry? entry) =>
        entry != null &&
            this.FullText == entry.FullText &&
            this.Format == entry.Format &&
            this.RawFormat == entry.RawFormat &&
            Enumerable.SequenceEqual(this.Args, entry.Args);

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public override int GetHashCode() =>
        HashCodeUtil.GetHashCode(this.FullText, this.Format, this.RawFormat, this.Args);

    /// <summary>
    /// Gets the string representation of the log entry i.e. its full text.
    /// </summary>
    /// <returns>The full text of the log entry.</returns>
    public override string ToString() =>
        this.FullText;

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(LogEntry? left, LogEntry? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(LogEntry? left, LogEntry? right) =>
        !(left == right);
}
