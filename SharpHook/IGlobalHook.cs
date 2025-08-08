namespace SharpHook;

/// <summary>
/// Represents a global keyboard and mouse hook.
/// </summary>
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
/// <remarks>
/// It is highly recommended not to implement this interface directly. If you want to create a custom global hook, you
/// should instead extend the <see cref="GlobalHookBase" /> class as it correctly implements the invariants required for
/// advanced scenarios.
/// </remarks>
public interface IGlobalHook : IBasicGlobalHook
{
    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>
    /// This event is raised when the <see cref="IBasicGlobalHook.Run" /> or <see cref="IBasicGlobalHook.RunAsync" />
    /// method is called.
    /// </remarks>
    event EventHandler<HookEventArgs> HookEnabled;

    /// <summary>
    /// An event which is raised when the global hook is disabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IDisposable.Dispose" /> method is called.</remarks>
    event EventHandler<HookEventArgs> HookDisabled;

    /// <summary>
    /// An event which is raised when a key is typed.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyReleased;

    /// <summary>
    /// An event which is raised when a mouse button is clicked.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseClicked;

    /// <summary>
    /// An event which is raised when a mouse button is pressed.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MousePressed;

    /// <summary>
    /// An event which is raised when a mouse button is released.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseMoved;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseDragged;

    /// <summary>
    /// An event which is raised when the mouse wheel is scrolled.
    /// </summary>
    event EventHandler<MouseWheelHookEventArgs> MouseWheel;
}
