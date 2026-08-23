namespace SharpHook;

/// <summary>
/// A base class for event args related to the global hook.
/// </summary>
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseWheelEventData" />
/// <seealso cref="UioHookEvent" />
public class HookEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HookEventArgs" /> class.
    /// </summary>
    /// <param name="rawEvent">The raw event data.</param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public HookEventArgs(UioHookEvent rawEvent)
    {
        this.RawEvent = rawEvent;
        this.EventTime = DateTimeOffset.FromUnixTimeMilliseconds((long)rawEvent.Time);
    }

    /// <summary>
    /// Gets the raw event data.
    /// </summary>
    /// <value>The raw event data.</value>
    public UioHookEvent RawEvent { get; }

    /// <summary>
    /// Gets the date and time of the event (in UTC), derived from the event's UNIX timestamp.
    /// </summary>
    /// <value>The date and time of the event.</value>
    public DateTimeOffset EventTime { get; }

    /// <summary>
    /// Gets the value which indicates whether the event has been simulated.
    /// </summary>
    /// <value><see langword="true" /> is the event has been simulated. Otherwise, <see langword="false" />.</value>
    public bool IsEventSimulated =>
        this.RawEvent.Mask.HasFlag(EventMask.SimulatedEvent);

    /// <summary>
    /// Gets or sets whether to suppress the event from further propagation after running the event handler. 
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the event propagation should be suppressed. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>
    /// <para>
    /// Events must be suppressed synchronously, i.e., in the same thread in which the hook itself runs. This means that
    /// some global hooks, like <see cref="EventLoopGlobalHook" /> or <see cref="TaskPoolGlobalHook" />, do not support
    /// suppressing events.
    /// </para>
    /// <para>
    /// Suppressing events works only on Windows and macOS. You can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" /> to check whether the current platform supports
    /// suppresing events. If that method returns <see cref="UioHookFeature.EventSuppression" /> as one of the supported
    /// features, then it does.
    /// </para>
    /// </remarks>
    public bool SuppressEvent { get; set; }
}
