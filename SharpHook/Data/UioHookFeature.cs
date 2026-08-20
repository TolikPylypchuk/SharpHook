namespace SharpHook.Data;

/// <summary>
/// Represents an optional feature of libuiohook that is not supported on all platforms.
/// </summary>
[Flags]
public enum UioHookFeature : uint
{
    /// <summary>
    /// Used if the current platform supports no optional features
    /// </summary>
    None = 0,

    /// <summary>
    /// Event suppression. Supported on Windows and macOS. Not supported on Linux – neither on X11 nor on Wayland.
    /// </summary>
    EventSuppression = 1 << 0,

    /// <summary>
    /// Events of type <see cref="EventType.KeyTyped" />. Supported on Windows, macOS, and X11. Not supported on
    /// Wayland.
    /// </summary>
    KeyTypedEvents = 1 << 1,

    /// <summary>
    /// Posting text via <see cref="UioHook.PostText(string)" />. Supported on Windows, macOS, and X11. Not supported on
    /// Wayland.
    /// </summary>
    PostText = 1 << 2,

    /// <summary>
    /// If this feature is enabled, then auto-repeated key-down events are raised by global hooks. Supported on Windows,
    /// macOS, and the XRecord-based X11 backend. Not supported on the libinput/uinput Linux backends.
    /// </summary>
    KeyAutoRepeat = 1 << 3,

    /// <summary>
    /// If this feature is enabled, then mouse movement events always report absolute coordinates. Supported on Windows,
    /// macOS, and X11. Not supported on Wayland.
    /// </summary>
    AbsoluteMouseMovement = 1 << 4,

    /// <summary>
    /// If this feature is enabled, then mouse button and wheel events report their absolute coordinates. Supported on
    /// Windows, macOS, and X11. Not supported on Wayland.
    /// </summary>
    AbsoluteMouseButtonCoordinates = 1 << 5,

    /// <summary>
    /// If this feature is enabled, then <see cref="UioHook.GetPointerAccelerationMultiplier" />,
    /// <see cref="UioHook.GetPointerAccelerationThreshold" />, and <see cref="UioHook.GetPointerSensitivity" /> are
    /// supported. Supported on Windows, macOS, and X11. Not supported on Wayland.
    /// </summary>
    PointerProperties = 1 << 6,
}
