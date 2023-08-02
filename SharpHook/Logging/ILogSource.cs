namespace SharpHook.Logging;

/// <summary>
/// Represents a source of libuiohook logs.
/// </summary>
/// <seealso cref="LogSource" />
/// <seealso cref="EmptyLogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public interface ILogSource : IDisposable
{
    /// <summary>
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>A disposed log source doesn't raise the <see cref="MessageLogged" /> event.</remarks>
    bool IsDisposed { get; }

    /// <summary>
    /// An event which is raised when libuiohook logs a message.
    /// </summary>
    event EventHandler<LogEventArgs> MessageLogged;
}
