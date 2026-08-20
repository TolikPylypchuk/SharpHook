namespace SharpHook.Data;

/// <summary>
/// Represents a libuiohook backend that can be loaded on Linux.
/// </summary>
/// <seealso cref="LinuxMode" />
public enum LinuxBackend
{
    /// <summary>
    /// No backend has been loaded yet, and changing the backend mode is possible.
    /// </summary>
    None = 0,

    /// <summary>
    /// The X11 backend which uses XRecord to listen to events and XTest to post events. This backend represents the
    /// behaviour of SharpHook prior to version 8, and is the default backend on X11.
    /// </summary>
    XRecord = 1,

    /// <summary>
    /// The X11 backend which uses libinput to listen to events and uinput to post events. This backend requires
    /// elevated privileges to work.
    /// </summary>
    X11 = 2,

    /// <summary>
    /// The Wayland backend which uses libinput to listen to events and uinput to post events. This backend requires
    /// elevated privileges to work.
    /// </summary>
    Wayland = 3
}
