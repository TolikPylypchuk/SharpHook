namespace SharpHook;

using System;

using SharpHook.Native;

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

    /// <summary>
    /// Gets or sets the value which will be set to the <see cref="UioHookEvent.Reserved" /> field after running
    /// the event handler (unless it's set to <see langword="null" />).
    /// </summary>
    /// <value>
    /// The value which will be set to the <see cref="UioHookEvent.Reserved" /> field after running the event handler.
    /// </value>
    public EventReservedValueMask? Reserved { get; set; }
}
