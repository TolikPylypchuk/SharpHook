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
    /// Gets or sets whether to suppress the event from further propagation after running the event handler. Events must
    /// be suppressed synchronously. Suppressing events works only on Windows and macOS.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the event propagation should be suppressed. Otherwise, <see langword="false" />.
    /// </value>
    public bool SuppressEvent { get; set; }
}
