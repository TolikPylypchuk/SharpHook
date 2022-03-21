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

        this.MessageLogged = this.messageLoggedSubject.AsObservable();
    }

    /// <summary>
    /// Completes the <see cref="MessageLogged" /> observable if the log source hasn't been disposed.
    /// </summary>
    ~ReactiveLogSourceAdapter() =>
        this.Dispose(false);

    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    public IObservable<LogEntry> MessageLogged { get; }

    /// <summary>
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>The <see cref="MessageLogged" /> observable doesn't emit any values in a disposed log source.</remarks>
    public bool IsDisposed => this.logSource.IsDisposed;

    /// <summary>
    /// Disposes the adapted log source and emits the completion signal for <see cref="MessageLogged" />.
    /// </summary>
    public void Dispose()
    {
        if (!this.IsDisposed)
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.logSource.Dispose();
        }

        messageLoggedSubject.OnCompleted();
    }
}
