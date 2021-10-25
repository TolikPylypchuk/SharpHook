namespace SharpHook.Native;

/// <summary>
/// Represents a result of an operation of libuiohook.
/// </summary>
/// <seealso cref="UioHook" />
public enum UioHookResult
{
    Success = 0x00,
    Failure = 0x01,

    ErrorOutOfMemory = 0x02,

    ErrorXOpenDisplay = 0x20,
    ErrorXRecordNotFound = 0x21,
    ErrorXRecordAllocRange = 0x22,
    ErrorXRecordCreateContext = 0x23,
    ErrorXRecordEnableContext = 0x24,
    ErrorXRecordGetContext = 0x25,

    ErrorSetWindowsHookEx = 0x30,
    ErrorGetModuleHandle = 0x31,

    ErrorAxApiDisabled = 0x40,
    ErrorCreateEventPort = 0x41,
    ErrorCreateRunLoopSource = 0x42,
    ErrorGetRunLoop = 0x43,
    ErrorCreateObserver = 0x44
}
