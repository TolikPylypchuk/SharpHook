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
    /// A <see langword="null" /> string was passed to <see cref="UioHook.PostText(string)" />
    /// </summary>
    ErrorPostTextNull = 0x03,

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
    /// Accessibility API is disabled for the process (macOS)
    /// </summary>
    ErrorAxApiDisabled = 0x40,

    /// <summary>
    /// Failed to create an event port (macOS)
    /// </summary>
    ErrorCreateEventPort = 0x41,

    /// <summary>
    /// <c>CFRunLoopSourceCreate</c> or <c>CFMachPortCreateRunLoopSource</c> has failed (macOS)
    /// </summary>
    ErrorCreateRunLoopSource = 0x42,

    /// <summary>
    /// <c>CFRunLoopGetCurrent</c> has failed (macOS)
    /// </summary>
    ErrorGetRunLoop = 0x43,

    /// <summary>
    /// <c>CFRunLoopObserverCreate</c> has failed (macOS)
    /// </summary>
    ErrorCreateObserver = 0x44
}
