namespace SharpHook.Reactive.Logging;

/// <summary>
/// Adapts an <see cref="ILogSource" /> to the <see cref="IReactiveLogSource" /> interface.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="IReactiveLogSource" />
[ExcludeFromCodeCoverage]
public sealed class ReactiveLogSourceAdapter : IReactiveLogSource
{
    private readonly ILogSource logSource;
    private readonly Subject<LogEntry> messageLoggedSubject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveLogSourceAdapter" /> class.
    /// </summary>
    /// <param name="logSource">The log source to adapt.</param>
    /// <exception cref="ArgumentNullException"><paramref name="logSource" /> is <see langword="null" />.</exception>
    public ReactiveLogSourceAdapter(ILogSource logSource)
        : this(logSource, ImmediateScheduler.Instance)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveLogSourceAdapter" /> class.
    /// </summary>
    /// <param name="logSource">The log source to adapt.</param>
    /// <param name="defaultScheduler">The default shceduler for the observable.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logSource" /> or <paramref name="defaultScheduler" /> is <see langword="null" />.
    /// </exception>
    public ReactiveLogSourceAdapter(ILogSource logSource, IScheduler defaultScheduler)
    {
        this.logSource = logSource ?? throw new ArgumentNullException(nameof(logSource));

        if (defaultScheduler is null)
        {
            throw new ArgumentNullException(nameof(defaultScheduler));
        }

        Observable.FromEventPattern<LogEventArgs>(
            h => this.logSource.MessageLogged += h, h => this.logSource.MessageLogged -= h)
            .Select(e => e.EventArgs.LogEntry)
            .Subscribe(this.messageLoggedSubject);

        this.MessageLogged = this.messageLoggedSubject.ObserveOn(defaultScheduler);
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
