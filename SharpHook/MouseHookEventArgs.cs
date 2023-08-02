namespace SharpHook;

/// <summary>
/// Event args for mouse-related hook events.
/// </summary>
/// <seealso cref="HookEventArgs" />
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="MouseWheelHookEventArgs" />
/// <seealso cref="MouseEventData" />
public sealed class MouseHookEventArgs : HookEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MouseHookEventArgs" /> class.
    /// </summary>
    /// <param name="rawEvent">The raw event data.</param>
    public MouseHookEventArgs(UioHookEvent rawEvent)
        : base(rawEvent) =>
        this.Data = rawEvent.Mouse;

    /// <summary>
    /// Gets the event data.
    /// </summary>
    /// <value>The event data.</value>
    public MouseEventData Data { get; }
}
