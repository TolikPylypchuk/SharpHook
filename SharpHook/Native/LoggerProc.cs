namespace SharpHook.Native;

/// <summary>
/// Represents a function which is called when a logging event occurs.
/// </summary>
/// <param name="level">The log level.</param>
/// <param name="userData">
/// The data that was passed to <see cref="UioHook.SetLoggerProc(LoggerProc?, nint)" />. Usually should not be used.
/// </param>
/// <param name="format">The format of the log message.</param>
/// <param name="args">The arguments of the log message.</param>
/// <seealso cref="UioHook.SetLoggerProc(LoggerProc?, nint)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void LoggerProc(LogLevel level, nint userData, nint format, nint args);
