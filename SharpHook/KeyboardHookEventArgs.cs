namespace SharpHook;

/// <summary>
/// Event args for keyboard-related hook events.
/// </summary>
/// <seealso cref="HookEventArgs" />
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseWheelHookEventArgs" />
/// <seealso cref="KeyboardEventData" />
public sealed class KeyboardHookEventArgs : HookEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardHookEventArgs" /> class.
    /// </summary>
    /// <param name="rawEvent">The raw event data.</param>
    public KeyboardHookEventArgs(UioHookEvent rawEvent)
        : base(rawEvent) =>
        this.Data = rawEvent.Keyboard;

    /// <summary>
    /// Gets the event data.
    /// </summary>
    /// <value>The event data.</value>
    public KeyboardEventData Data { get; }
}
