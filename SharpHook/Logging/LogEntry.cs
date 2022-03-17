namespace SharpHook.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

using SharpHook.Internal;
using SharpHook.Native;

/// <summary>
/// Represents a log entry from libuiohook.
/// </summary>
/// <remarks>
/// <para>
/// For simple handling the <see cref="Level" /> and <see cref="FullText" /> properties are anough. For more advanced
/// handling you can use the entries' formats and arguments.
/// </para>
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
/// <para>
/// You shouldn't create <see cref="LogEntry" /> instances yourself. Instead, use the <see cref="LogEntryParser" />
/// class to create them in the logging callback.
/// </para>
/// </remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntryParser" />
public sealed class LogEntry : IEquatable<LogEntry>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogEntry" /> class.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="fullText">The full text of the log entry.</param>
    /// <param name="format">The format of the log entry which can be used in <c>String.Format</c>.</param>
    /// <param name="nativeFormat">The native format of the log entry.</param>
    /// <param name="arguments">The arguments of the log entry which can be passed to <c>String.Format</c>.</param>
    /// <param name="rawArguments">The arguments as they appear in the entry's full text.</param>
    /// <param name="argumentPlaceholders">The argument placeholders from the native format.</param>
    public LogEntry(
        LogLevel level,
        string fullText,
        string format,
        string nativeFormat,
        IReadOnlyList<object> arguments,
        IReadOnlyList<string> rawArguments,
        IReadOnlyList<string> argumentPlaceholders)
    {
        this.Level = level;
        this.FullText = fullText;
        this.Format = format;
        this.NativeFormat = nativeFormat;
        this.Arguments = arguments;
        this.RawArguments = rawArguments;
        this.ArgumentPlaceholders = argumentPlaceholders;
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
    public string FullText { get; }

    /// <summary>
    /// Gets the format of the current log entry.
    /// </summary>
    /// <value>The format of the current log entry.</value>
    public string Format { get; }

    /// <summary>
    /// Gets the native format of the current log entry, as defined in libuiohook.
    /// </summary>
    /// <value>The native format of the current log entry.</value>
    public string NativeFormat { get; }

    /// <summary>
    /// Gets the arguments of the current log entry.
    /// </summary>
    /// <value>The arguments of the current log entry.</value>
    /// <remarks>The function and line are the first two arguments of the log entry.</remarks>
    public IReadOnlyList<object> Arguments { get; }

    /// <summary>
    /// Gets the arguments of the current log entry as they appear in entry's full text.
    /// </summary>
    /// <value>The arguments of the current log entry as they appear in entry's full text.</value>
    /// <remarks>The function and line are the first two arguments of the log entry.</remarks>
    public IReadOnlyList<string> RawArguments { get; }

    /// <summary>
    /// Gets the argument placeholders from the native format.
    /// </summary>
    /// <value>The argument placeholders from the native format.</value>
    public IReadOnlyList<string> ArgumentPlaceholders { get; }

    /// <summary>
    /// Gets the libuiohook function which called the logger.
    /// </summary>
    /// <value>The function which called the logger.</value>
    /// <remarks>The function is the first argument of the log entry.</remarks>
    public string Function =>
        this.Arguments.Count > 0 && this.Arguments[0] is string func ? func : String.Empty;

    /// <summary>
    /// Gets the source code line in libuiohook where the logger was called.
    /// </summary>
    /// <value>The source code line in libuiohook where the logger was called.</value>
    /// <remarks>The source code line is the second argument of the log entry.</remarks>
    public uint SourceLine =>
        this.Arguments.Count > 1 && this.Arguments[1] is uint line ? line : 0;

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
            this.NativeFormat == entry.NativeFormat &&
            Enumerable.SequenceEqual(this.Arguments, entry.Arguments) &&
            Enumerable.SequenceEqual(this.RawArguments, entry.RawArguments) &&
            Enumerable.SequenceEqual(this.ArgumentPlaceholders, entry.ArgumentPlaceholders);

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public override int GetHashCode() =>
        HashCodeUtil.GetHashCode(
            this.FullText,
            this.Format,
            this.NativeFormat,
            this.Arguments,
            this.RawArguments,
            this.ArgumentPlaceholders);

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
