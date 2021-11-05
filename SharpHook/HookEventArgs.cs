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
    public HookEventArgs(UioHookEvent rawEvent) =>
        this.RawEvent = rawEvent;

    /// <summary>
    /// Gets the raw event data.
    /// </summary>
    /// <value>The raw event data.</value>
    public UioHookEvent RawEvent { get; }
}
