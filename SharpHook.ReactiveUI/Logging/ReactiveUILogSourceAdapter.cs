using System.Diagnostics.CodeAnalysis;

namespace SharpHook.ReactiveUI.Logging;

/// <summary>
/// Adapts an <see cref="ILogSource" /> to the <see cref="IReactiveLogSource" /> interface.
/// </summary>
/// <seealso cref="ILogSource" />
/// <seealso cref="IReactiveLogSource" />
[ExcludeFromCodeCoverage]
public sealed class ReactiveLogSourceAdapter : IReactiveLogSource, ILogSource
{
    private readonly ILogSource logSource;
    private readonly Signal<LogEntry> messageLoggedSignal = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveLogSourceAdapter" /> class.
    /// </summary>
    /// <param name="logSource">The log source to adapt.</param>
    /// <param name="defaultSequencer">
    /// The default sequencer for the observable, or <see langword="null" /> to use the default one
    /// (<see cref="Sequencer.Immediate" />).
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logSource" /> is <see langword="null" />.
    /// </exception>
    public ReactiveLogSourceAdapter(ILogSource logSource, ISequencer? defaultSequencer = null)
    {
        this.logSource = logSource ?? throw new ArgumentNullException(nameof(logSource));

        defaultSequencer ??= Sequencer.Immediate;

        Signal.FromEventPattern<LogEventArgs>(
            h => this.logSource.MessageLogged += h, h => this.logSource.MessageLogged -= h)
            .Select(e => e.EventArgs.LogEntry)
            .Subscribe(this.messageLoggedSignal);

        this.MessageLogged = this.messageLoggedSignal.ObserveOn(defaultSequencer);
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

        this.messageLoggedSignal.OnCompleted();
        this.messageLoggedSignal.Dispose();
    }

    event EventHandler<LogEventArgs> ILogSource.MessageLogged
    {
        add => this.logSource.MessageLogged += value;
        remove => this.logSource.MessageLogged -= value;
    }
}
