namespace SharpHook.Data;

/// <summary>
/// Represents the type of an input event which can be raised from a global hook or posted as a fake event.
/// </summary>
/// <seealso cref="UioHookEvent" />
public enum EventType : ushort
{
    /// <summary>
    /// Raised when the global hook is started. Ignored when posted.
    /// </summary>
    HookEnabled = 1,

    /// <summary>
    /// Raised when the global hook is stopped. Ignored when posted.
    /// </summary>
    HookDisabled,

    /// <summary>
    /// Raised when a key press may have caused a character to be typed regardless of whether a character was actually
    /// typed. Cannot and should not be posted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Events of this type are raised after events of type <see cref="KeyPressed" /> if the pressed key may have caused
    /// characters to be typed regardless of whether they were actually typed. Since a single key press can cause
    /// multiple characters to be typed, a single event of type <see cref="KeyPressed" /> can cause multiple events of
    /// type <see cref="KeyTyped" />.
    /// </para>
    /// <para>
    /// Since events of this type are not raised by the OS, suppressing them has no effect.
    /// </para>
    /// <para>
    /// Events of this type are not supported on Wayland.
    /// </para>
    /// </remarks>
    KeyTyped,

    /// <summary>
    /// Raised when a key is pressed (its state changed from up to down) or posted to press a key.
    /// </summary>
    KeyPressed,

    /// <summary>
    /// Raised when a key is released (its state changed from down to up) or posted to release a key.
    /// </summary>
    KeyReleased,

    /// <summary>
    /// Raised when a mouse button is clicked. Cannot and should not be posted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Events of this type are raised after events of type <see cref="MouseReleased" /> if the mouse was not dragged.
    /// </para>
    /// <para>
    /// Since events of this type are not raised by the OS, suppressing them has no effect.
    /// </para>
    /// </remarks>
    MouseClicked,

    /// <summary>
    /// Raised when a mouse button is pressed (its state changed from up to down) or posted to press a mouse button.
    /// </summary>
    MousePressed,

    /// <summary>
    /// Posted to press a mouse button at the current coordinates. Never raised.
    /// </summary>
    MousePressedIgnoreCoordinates,

    /// <summary>
    /// Raised when a mouse button is released (its state changed from down to up) or posted to release a mouse button.
    /// </summary>
    MouseReleased,

    /// <summary>
    /// Posted to release a mouse button at the current coordinates. Never raised.
    /// </summary>
    MouseReleasedIgnoreCoordinates,

    /// <summary>
    /// Raised when the mouse is moved or posted to move the mouse.
    /// </summary>
    MouseMoved,

    /// <summary>
    /// Raised on Wayland when the mouse is moved, but its absolute position is not available. Posted to move the mouse
    /// relative to the current cursor position.
    /// </summary>
    MouseMovedRelative,

    /// <summary>
    /// Raised when the mouse is dragged. Not recommended to post as it will be the same as <see cref="MouseMoved" />.
    /// </summary>
    MouseDragged,

    /// <summary>
    /// Raised on Wayland when the mouse is dragged, but its absolute position is not available. Not recommended to post
    /// as it will be the same as <see cref="MouseMovedRelative" />.
    /// </summary>
    MouseDraggedRelative,

    /// <summary>
    /// Raised when the mouse wheel is scrolled or posted to scroll the mouse wheel.
    /// </summary>
    MouseWheel
}
