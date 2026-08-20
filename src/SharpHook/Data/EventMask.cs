namespace SharpHook.Data;

/// <summary>
/// Represents the mask of a <see cref="UioHookEvent" /> and contains modifiers and other information.
/// </summary>
/// <seealso cref="UioHookEvent" />
[Flags]
public enum EventMask : uint
{
    /// <summary>No mask.</summary>
    None = 0,

    /// <summary>Left Shift is pressed.</summary>
    LeftShift = 1U << 0,

    /// <summary>Left Control is pressed.</summary>
    LeftCtrl = 1U << 1,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux) is pressed.
    /// </summary>
    LeftMeta = 1U << 2,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS) is pressed.
    /// </summary>
    LeftAlt = 1U << 3,

    /// <summary>Right Shift is pressed.</summary>
    RightShift = 1U << 4,

    /// <summary>Right Control is pressed.</summary>
    RightCtrl = 1U << 5,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux) is pressed.
    /// </summary>
    RightMeta = 1U << 6,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS) is pressed.
    /// </summary>
    RightAlt = 1U << 7,

    /// <summary>Left or right Shift is pressed.</summary>
    Shift = LeftShift | RightShift,

    /// <summary>Left or right Control is pressed.</summary>
    Ctrl = LeftCtrl | RightCtrl,

    /// <summary>
    /// Left or right Win (on Windows), Command (on macOS), or Super/Meta (on Linux) is pressed.
    /// </summary>
    Meta = LeftMeta | RightMeta,

    /// <summary>
    /// Left or right Alt (on Windows and Linux) or Option (on macOS) is pressed.
    /// </summary>
    Alt = LeftAlt | RightAlt,

    /// <summary>
    /// Mouse button 1 (usually the left mouse button) is pressed.
    /// </summary>
    Button1 = 1U << 8,

    /// <summary>
    /// Mouse button 2 (usually the right mouse button) is pressed.
    /// </summary>
    Button2 = 1U << 9,

    /// <summary>Mouse button 3 is pressed.</summary>
    Button3 = 1U << 10,

    /// <summary>Mouse button 4 is pressed.</summary>
    Button4 = 1U << 11,

    /// <summary>Mouse button 5 is pressed.</summary>
    Button5 = 1U << 12,

    /// <summary>Num Lock is locked.</summary>
    NumLock = 1U << 13,

    /// <summary>Caps Lock is locked.</summary>
    CapsLock = 1U << 14,

    /// <summary>Scroll Lock is locked.</summary>
    ScrollLock = 1U << 15,

    /// <summary>The event is simulated.</summary>
    SimulatedEvent = 1U << 30,

    /// <summary>
    /// If this flag is set in the event handler then the event will be suppressed from further propagation.
    /// </summary>
    /// <remarks>Suppressing events works only on Windows and macOS.</remarks>
    SuppressEvent = 1U << 31
}
