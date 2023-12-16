namespace SharpHook;

/// <summary>
/// Event args for mouse wheel-related hook events.
/// </summary>
/// <param name="rawEvent">The raw event data.</param>
/// <seealso cref="HookEventArgs" />
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseWheelEventData" />
public sealed class MouseWheelHookEventArgs(UioHookEvent rawEvent) : HookEventArgs(rawEvent)
{
    /// <summary>
    /// Gets the event data.
    /// </summary>
    /// <value>The event data.</value>
    public MouseWheelEventData Data { get; } = rawEvent.Wheel;
}
