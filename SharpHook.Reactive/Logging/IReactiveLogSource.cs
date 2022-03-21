namespace SharpHook.Reactive.Logging;

using System;

using SharpHook.Logging;

/// <summary>
/// Represents a reactive source of libuiohook logs.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="ReactiveLogSourceAdapter" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public interface IReactiveLogSource : IDisposable
{
    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    IObservable<LogEntry> MessageLogged { get; }

    /// <summary>
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>The <see cref="MessageLogged" /> observable doesn't emit any values in a disposed log source.</remarks>
    bool IsDisposed { get; }
}
