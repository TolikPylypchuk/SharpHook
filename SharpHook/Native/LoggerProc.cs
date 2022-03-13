namespace SharpHook.Native;

using System;

/// <summary>
/// Represents a function which is called when a logging event occurs.
/// </summary>
/// <param name="level">The log level.</param>
/// <param name="format">The format of the log message.</param>
/// <param name="args">The arguments of the log message.</param>
/// <returns><see langword="true" /> if the message has been logged. Otherwise, <see langword="false" />.</returns>
/// <seealso cref="UioHook.SetLoggerProc(LoggerProc?)" />
public delegate bool LoggerProc(LogLevel level, IntPtr format, IntPtr args);
