namespace SharpHook.Reactive.Logging;

/// <summary>
/// Represents a reactive source of libuiohook logs.
/// </summary>
/// <remarks>
/// Implementations of this interface are defined in the SharpHook.Reactive and SharpHook.ReactiveUI packages.
/// </remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
/// <seealso cref="LogEventArgs" />
/// <seealso cref="LogEntry" />
public interface IReactiveLogSource : IStatefulDisposable
{
    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    IObservable<LogEntry> MessageLogged { get; }
}
