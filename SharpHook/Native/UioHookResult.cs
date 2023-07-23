namespace SharpHook.Native;

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
    /// A <see langword="null" /> string was passed to <see cref="UioHook.PostText(string)" />.
    /// </summary>
    ErrorPostTextNull = 0x03,

    /// <summary>
    /// XOpenDisplay failed (X11)
    /// </summary>
    ErrorXOpenDisplay = 0x20,

    /// <summary>
    /// XRecord not found (X11)
    /// </summary>
    ErrorXRecordNotFound = 0x21,

    /// <summary>
    /// XRecordAllocRange failed (X11)
    /// </summary>
    ErrorXRecordAllocRange = 0x22,

    /// <summary>
    /// XRecordCreateContext failed (X11)
    /// </summary>
    ErrorXRecordCreateContext = 0x23,

    /// <summary>
    /// XRecordEnableContext failed (X11)
    /// </summary>
    ErrorXRecordEnableContext = 0x24,

    /// <summary>
    /// XRecordGetContext failed (X11)
    /// </summary>
    ErrorXRecordGetContext = 0x25,

    /// <summary>
    /// SetWindowsHookEx failed (Windows)
    /// </summary>
    ErrorSetWindowsHookEx = 0x30,

    /// <summary>
    /// Couldn't get a module handle for SetWindowsHookEx (Windows)
    /// </summary>
    ErrorGetModuleHandle = 0x31,

    /// <summary>
    /// Couldn't create an invisible window which will listen to screen changes (Windows)
    /// </summary>
    ErrorCreateInvisibleWindow = 0x32,

    /// <summary>
    /// Accessibility API is disabled for the process (macOS)
    /// </summary>
    ErrorAxApiDisabled = 0x40,

    /// <summary>
    /// Failed to create an event port (macOS)
    /// </summary>
    ErrorCreateEventPort = 0x41,

    /// <summary>
    /// CFRunLoopSourceCreate or CFMachPortCreateRunLoopSource failed (macOS)
    /// </summary>
    ErrorCreateRunLoopSource = 0x42,

    /// <summary>
    /// CFRunLoopGetCurrent failed (macOS)
    /// </summary>
    ErrorGetRunLoop = 0x43,

    /// <summary>
    /// CFRunLoopObserverCreate failed (macOS)
    /// </summary>
    ErrorCreateObserver = 0x44
}
