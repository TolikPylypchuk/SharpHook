namespace SharpHook.Native;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a function which is called when a logging event occurs.
/// </summary>
/// <param name="level">The log level.</param>
/// <param name="userData">
/// The data that was passed to <see cref="UioHook.SetLoggerProc(LoggerProc?, IntPtr)" />. Should not be used.
/// </param>
/// <param name="format">The format of the log message.</param>
/// <param name="args">The arguments of the log message.</param>
/// <seealso cref="UioHook.SetLoggerProc(LoggerProc?, IntPtr)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void LoggerProc(LogLevel level, IntPtr userData, IntPtr format, IntPtr args);
