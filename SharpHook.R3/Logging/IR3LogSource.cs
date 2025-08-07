namespace SharpHook.R3.Logging;

/// <summary>
/// Represents a reactive R3 source of libuiohook logs.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
public interface IR3LogSource : IDisposable
{
    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    Observable<LogEntry> MessageLogged { get; }

    /// <summary>
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>The <see cref="MessageLogged" /> observable doesn't emit any values in a disposed log source.</remarks>
    bool IsDisposed { get; }
}
