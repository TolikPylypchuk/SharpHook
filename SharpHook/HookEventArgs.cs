namespace SharpHook;

/// <summary>
/// A base class for event args related to the global hook.
/// </summary>
/// <param name="rawEvent">The raw event data.</param>
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseWheelEventData" />
/// <seealso cref="UioHookEvent" />
public class HookEventArgs(UioHookEvent rawEvent) : EventArgs
{
    /// <summary>
    /// Gets the raw event data.
    /// </summary>
    /// <value>The raw event data.</value>
    public UioHookEvent RawEvent { get; } = rawEvent;

    /// <summary>
    /// Gets the date and time of the event (in UTC), derived from the event's UNIX timestamp.
    /// </summary>
    /// <value>The date and time of the event.</value>
    public DateTimeOffset EventTime { get; } = DateTimeOffset.FromUnixTimeMilliseconds((long)rawEvent.Time);

    /// <summary>
    /// Gets the value which indicates whether the event has been simulated.
    /// </summary>
    /// <value><see langword="true" /> is the event has been simulated. Otherwise, <see langword="false" />.</value>
    public bool IsEventSimulated =>
        (this.RawEvent.Reserved & EventReservedValueMask.SimulatedEvent) != EventReservedValueMask.None;

    /// <summary>
    /// Gets or sets whether to suppress the event from further propagation after running the event handler. Events must
    /// be suppressed synchronously. Suppressing events works only on Windows and macOS.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the event propagation should be suppressed. Otherwise, <see langword="false" />.
    /// </value>
    public bool SuppressEvent { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="HookEventArgs" /> class or a derived class, depending on the
    /// event type.
    /// </summary>
    /// <param name="rawEvent">The raw event data.</param>
    public static HookEventArgs FromEvent(UioHookEvent rawEvent) =>
        rawEvent.Type switch
        {
            EventType.KeyPressed or EventType.KeyReleased or EventType.KeyTyped => new KeyboardHookEventArgs(rawEvent),
            EventType.MousePressed or EventType.MouseReleased or EventType.MouseClicked or
                EventType.MousePressedIgnoreCoordinates or EventType.MouseReleasedIgnoreCoordinates or
                EventType.MouseMoved or EventType.MouseDragged or
                EventType.MouseMovedRelativeToCursor => new MouseHookEventArgs(rawEvent),
            EventType.MouseWheel => new MouseWheelHookEventArgs(rawEvent),
            _ => new HookEventArgs(rawEvent)
        };
}
