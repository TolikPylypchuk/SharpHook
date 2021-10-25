namespace SharpHook.Native;

/// <summary>
/// The type of the event which can be raised by libuiohook.
/// </summary>
/// <seealso cref="UioHookEvent" />
public enum EventType
{
    HookEnabled = 1,
    HookDisabled,

    KeyTyped,
    KeyPressed,
    KeyReleased,

    MouseClicked,
    MousePressed,
    MouseReleased,
    MouseMoved,
    MouseDragged,

    MouseWheel
}
