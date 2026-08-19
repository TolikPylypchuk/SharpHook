namespace SharpHook.Data;

/// <summary>
/// Represents a result of an operation of libuiohook.
/// </summary>
/// <seealso cref="UioHook" />
public enum UioHookResult
{
    /// <summary>
    /// Success
    /// </summary>
    Success = 0x00,

    /// <summary>
    /// Failure
    /// </summary>
    Failure = 0x01,

    /// <summary>
    /// Out of memory
    /// </summary>
    ErrorOutOfMemory = 0x02,

    /// <summary>
    /// <see langword="null" /> was passed to a function that does not accept it
    /// </summary>
    ErrorNull = 0x03,

    /// <summary>
    /// The feature is not supported by the current platform
    /// </summary>
    ErrorUnsupportedFeature = 0x04,

    /// <summary>
    /// Failed to load the Linux backend, or the Linux backend has already been loaded and cannot be changed (Linux)
    /// </summary>
    ErrorLinuxLoadBackend = 0x10,

    /// <summary>
    /// Cannot initialize the udev context (Linux)
    /// </summary>
    ErrorLinuxInitUdev = 0x11,

    /// <summary>
    /// Cannot initialize libinput (Linux)
    /// </summary>
    ErrorLinuxInitLibinput = 0x12,

    /// <summary>
    /// Cannot assign a seat to libinput (Linux)
    /// </summary>
    ErrorLinuxAssignSeat = 0x13,

    /// <summary>
    /// Cannot initialize a stop notification (Linux)
    /// </summary>
    ErrorLinuxInitStopNotification = 0x14,

    /// <summary>
    /// Cannot execute a stop notification (Linux)
    /// </summary>
    ErrorLinuxExecStopNotification = 0x15,

    /// <summary>
    /// No input devices are available to listen to (Linux)
    /// </summary>
    ErrorLinuxNoInputDevices = 0x16,

    /// <summary>
    /// Cannot open /dev/uinput (Linux)
    /// </summary>
    ErrorLinuxOpenUinput = 0x17,

    /// <summary>
    /// Cannot create a uinput device (Linux)
    /// </summary>
    ErrorLinuxCreateUinputDevice = 0x18,

    /// <summary>
    /// Cannot write to uinput (Linux)
    /// </summary>
    ErrorLinuxWriteUinput = 0x19,

    /// <summary>
    /// Cannot open a Wayland display connection (Linux)
    /// </summary>
    ErrorLinuxOpenWaylandDisplay = 0x1A,

    /// <summary>
    /// Cannot open a Wayland display connection (Linux)
    /// </summary>
    ErrorLinuxVirtualDevicesNoInitialized = 0x1B,

    /// <summary>
    /// <c>XOpenDisplay</c> has failed (X11)
    /// </summary>
    ErrorXOpenDisplay = 0x20,

    /// <summary>
    /// <c>XRecord</c> not found (X11)
    /// </summary>
    ErrorXRecordNotFound = 0x21,

    /// <summary>
    /// <c>XRecordAllocRange</c> has failed (X11)
    /// </summary>
    ErrorXRecordAllocRange = 0x22,

    /// <summary>
    /// <c>XRecordCreateContext</c> has failed (X11)
    /// </summary>
    ErrorXRecordCreateContext = 0x23,

    /// <summary>
    /// <c>XRecordEnableContext</c> has failed (X11)
    /// </summary>
    ErrorXRecordEnableContext = 0x24,

    /// <summary>
    /// <c>XRecordGetContext</c> has failed (X11)
    /// </summary>
    ErrorXRecordGetContext = 0x25,

    /// <summary>
    /// <c>SetWindowsHookEx</c> has failed (Windows)
    /// </summary>
    ErrorSetWindowsHookEx = 0x30,

    /// <summary>
    /// Couldn't get a module handle for <c>SetWindowsHookEx</c> (Windows)
    /// </summary>
    ErrorGetModuleHandle = 0x31,

    /// <summary>
    /// Couldn't create an invisible window which will listen to screen changes (Windows)
    /// </summary>
    ErrorCreateInvisibleWindow = 0x32,

    /// <summary>
    /// Accessibility API access is disabled for the process (macOS)
    /// </summary>
    ErrorAxApiDisabled = 0x40,

    /// <summary>
    /// Accessibility API access has been revoked for the process (macOS)
    /// </summary>
    ErrorAxApiRevoked = 0x41,

    /// <summary>
    /// Failed to create an event port (macOS)
    /// </summary>
    ErrorCreateEventPort = 0x42,

    /// <summary>
    /// <c>CFRunLoopSourceCreate</c> or <c>CFMachPortCreateRunLoopSource</c> has failed (macOS)
    /// </summary>
    ErrorCreateRunLoopSource = 0x43,

    /// <summary>
    /// <c>CFRunLoopGetCurrent</c> has failed (macOS)
    /// </summary>
    ErrorGetRunLoop = 0x44,

    /// <summary>
    /// <c>CFRunLoopObserverCreate</c> has failed (macOS)
    /// </summary>
    ErrorCreateObserver = 0x45
}
