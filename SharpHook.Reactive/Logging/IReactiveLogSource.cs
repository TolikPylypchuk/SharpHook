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
}
