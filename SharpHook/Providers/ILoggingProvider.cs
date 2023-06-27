namespace SharpHook.Providers;

using System;

using SharpHook.Native;

/// <summary>
/// Represents a provider of low-level logging functionality.
/// </summary>
/// <seealso cref="UioHookProvider" />
public interface ILoggingProvider
{
    /// <summary>
    /// Sets the log callback function.
    /// </summary>
    /// <param name="loggerProc">
    /// The function to call for logging, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback. Should not be used, and <see cref="IntPtr.Zero" /> should always be passed.
    /// </param>
    /// <seealso cref="LoggerProc" />
    void SetLoggerProc(LoggerProc? loggerProc, IntPtr userData);
}
