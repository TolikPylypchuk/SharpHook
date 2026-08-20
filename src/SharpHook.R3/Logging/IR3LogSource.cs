namespace SharpHook.R3.Logging;

/// <summary>
/// Represents a reactive R3 source of libuiohook logs.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
public interface IR3LogSource : IStatefulDisposable
{
    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    Observable<LogEntry> MessageLogged { get; }
}
