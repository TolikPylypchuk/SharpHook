namespace SharpHook.Reactive.Logging;

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using SharpHook.Logging;

/// <summary>
/// Adapts an <see cref="ILogSource" /> to the <see cref="IReactiveLogSource" /> interface.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="IReactiveLogSource" />
public sealed class ReactiveLogSourceAdapter : IReactiveLogSource
{
    private readonly ILogSource logSource;
    private readonly Subject<LogEntry> messageLoggedSubject = new();
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveLogSourceAdapter" /> class.
    /// </summary>
    /// <param name="logSource">The log source to adapt.</param>
    public ReactiveLogSourceAdapter(ILogSource logSource)
    {
        this.logSource = logSource;

        Observable.FromEventPattern<LogEventArgs>(
            h => this.logSource.MessageLogged += h, h => this.logSource.MessageLogged -= h)
            .Select(e => e.EventArgs.LogEntry)
            .Subscribe(this.messageLoggedSubject);
    }

    ~ReactiveLogSourceAdapter() =>
        this.Dispose(false);

    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    public IObservable<LogEntry> MessageLogged =>
        this.messageLoggedSubject.AsObservable();

    /// <summary>
    /// Disposes the adapted log source and emits the completion signal for <see cref="MessageLogged" />.
    /// </summary>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.logSource.Dispose();
            this.disposed = true;
        }

        messageLoggedSubject.OnCompleted();
    }
}
