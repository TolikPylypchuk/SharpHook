namespace SharpHook.Native;

/// <summary>
/// Represents the type of an input event.
/// </summary>
/// <seealso cref="UioHookEvent" />
public enum EventType
{
    /// <summary>
    /// Raised when the global hook is started.
    /// </summary>
    HookEnabled = 1,

    /// <summary>
    /// Raised when the global hook is stopped.
    /// </summary>
    HookDisabled,

    /// <summary>
    /// Raised when a character is typed.
    /// </summary>
    KeyTyped,

    /// <summary>
    /// Raised when a key is pressed.
    /// </summary>
    KeyPressed,

    /// <summary>
    /// Raised when a key is released.
    /// </summary>
    KeyReleased,

    /// <summary>
    /// Raised when a mouse button is clicked.
    /// </summary>
    MouseClicked,

    /// <summary>
    /// Raised when a mouse button is pressed.
    /// </summary>
    MousePressed,

    /// <summary>
    /// Raised when a mouse button is released.
    /// </summary>
    MouseReleased,

    /// <summary>
    /// Raised when the mouse is moved.
    /// </summary>
    MouseMoved,

    /// <summary>
    /// Raised when the mouse is dragged.
    /// </summary>
    MouseDragged,

    /// <summary>
    /// Raised when the mouse wheel is scrolled.
    /// </summary>
    MouseWheel
}
