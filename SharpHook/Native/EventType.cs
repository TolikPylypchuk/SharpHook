namespace SharpHook.Native;

/// <summary>
/// Represents the type of an input event.
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
