namespace SharpHook.Data;

/// <summary>
/// Represents the mode of loading the Linux backend of libuiohook – the XRecord-based X11 backend, or the
/// libinput/uinput backend for X11 or Wayland.
/// </summary>
/// <seealso cref="LinuxBackend" />
public enum LinuxMode
{
    /// <summary>
    /// If this mode is selected, then libuiohook will load the Wayland backend if the current session is Wayland, and
    /// the XRecord-based X11 backend otherwise. This is the default mode.
    /// </summary>
    AutoXRecord = 0,

    /// <summary>
    /// If this mode is selected, then libuiohook will load the Wayland backend if the current session is Wayland, and
    /// the libinput/uinput-based X11 backend otherwise. In both session types, libinput and uinput will be used.
    /// </summary>
    AutoLowLevel = 1,

    /// <summary>
    /// If this mode is selected, the libuiohook will load the XRecord-based X11 backend, even if the current session is
    /// Wayland. This is the behaviour of SharpHook prior to version 8.
    /// </summary>
    XRecord = 2,

    /// <summary>
    /// If this mode is selected, the libuiohook will load the libinput/uinput-based X11 backend, even if the current
    /// session is Wayland.
    /// </summary>
    X11 = 3,

    /// <summary>
    /// If this mode is selected, the libuiohook will load the Wayland backend, even if the current session is X11.
    /// </summary>
    Wayland = 4
}
