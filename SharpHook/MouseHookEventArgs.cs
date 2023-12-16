namespace SharpHook;

/// <summary>
/// Event args for mouse-related hook events.
/// </summary>
/// <param name="rawEvent">The raw event data.</param>
/// <seealso cref="HookEventArgs" />
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="MouseWheelHookEventArgs" />
/// <seealso cref="MouseEventData" />
public sealed class MouseHookEventArgs(UioHookEvent rawEvent) : HookEventArgs(rawEvent)
{
    /// <summary>
    /// Gets the event data.
    /// </summary>
    /// <value>The event data.</value>
    public MouseEventData Data { get; } = rawEvent.Mouse;
}
