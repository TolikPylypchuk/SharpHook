namespace SharpHook;

/// <summary>
/// Event args for keyboard-related hook events.
/// </summary>
/// <param name="rawEvent">The raw event data.</param>
/// <seealso cref="HookEventArgs" />
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseWheelHookEventArgs" />
/// <seealso cref="KeyboardEventData" />
public sealed class KeyboardHookEventArgs(UioHookEvent rawEvent) : HookEventArgs(rawEvent)
{
    /// <summary>
    /// Gets the event data.
    /// </summary>
    /// <value>The event data.</value>
    public KeyboardEventData Data { get; } = rawEvent.Keyboard;
}
