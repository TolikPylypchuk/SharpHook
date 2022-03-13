namespace SharpHook.Logging;

using System;

/// <summary>
/// Represents a source of libuiohook logs.
/// </summary>
/// <seealso cref="LogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public interface ILogSource : IDisposable
{
    /// <summary>
    /// An event which is raised when libuiohook logs a message.
    /// </summary>
    event EventHandler<LogEventArgs> MessageLogged;
}
