namespace SharpHook.Logging;

/// <summary>
/// Represents a source of libuiohook logs.
/// </summary>
/// <seealso cref="LogSource" />
/// <seealso cref="EmptyLogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public interface ILogSource : IStatefulDisposable
{
    /// <summary>
    /// An event which is raised when libuiohook logs a message.
    /// </summary>
    event EventHandler<LogEventArgs> MessageLogged;
}
