using System.Diagnostics.CodeAnalysis;

namespace SharpHook.R3.Logging;

/// <summary>
/// Adapts an <see cref="ILogSource" /> to the <see cref="IR3LogSource" /> interface.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="IR3LogSource" />
[ExcludeFromCodeCoverage]
public sealed class R3LogSourceAdapter : IR3LogSource, ILogSource
{
    private readonly ILogSource logSource;
    private readonly Subject<LogEntry> messageLoggedSubject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="R3LogSourceAdapter" /> class.
    /// </summary>
    /// <param name="logSource">The log source to adapt.</param>
    /// <param name="defaultTimeProvider">
    /// The default time provider for the observable, or <see langword="null" /> to use the default one (as defined in
    /// <see cref="ObservableSystem.DefaultTimeProvider" />).
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logSource" /> is <see langword="null" />.
    /// </exception>
    public R3LogSourceAdapter(ILogSource logSource, TimeProvider? defaultTimeProvider = null)
    {
        this.logSource = logSource ?? throw new ArgumentNullException(nameof(logSource));

        defaultTimeProvider ??= ObservableSystem.DefaultTimeProvider;

        Observable.FromEventHandler<LogEventArgs>(
            h => this.logSource.MessageLogged += h, h => this.logSource.MessageLogged -= h)
            .Select(this.SelectEventArgs)
            .Select(e => e.LogEntry)
            .Subscribe(this.messageLoggedSubject.AsObserver());

        this.MessageLogged = this.messageLoggedSubject.ObserveOn(defaultTimeProvider);
    }

    /// <summary>
    /// Completes the <see cref="MessageLogged" /> observable if the log source hasn't been disposed.
    /// </summary>
    ~R3LogSourceAdapter() =>
        this.Dispose(false);

    /// <summary>
    /// An observable which is emitted when libuiohook logs a message.
    /// </summary>
    public Observable<LogEntry> MessageLogged { get; }

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

        this.messageLoggedSubject.Dispose();
    }

    private TArgs SelectEventArgs<TArgs>((object? Sender, TArgs Args) e) =>
        e.Args;

    event EventHandler<LogEventArgs> ILogSource.MessageLogged
    {
        add => this.logSource.MessageLogged += value;
        remove => this.logSource.MessageLogged -= value;
    }
}
