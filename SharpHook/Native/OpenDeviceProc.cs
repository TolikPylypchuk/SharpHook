namespace SharpHook.Native;

/// <summary>
/// Represents a function which opens a privileged device and provides a file descriptor.
/// </summary>
/// <param name="path">The path to the device to open.</param>
/// <param name="flags">The flags to use when opening the device.</param>
/// <param name="userData">
/// The data that was passed to <see cref="UioHook.SetDeviceProcs(OpenDeviceProc?, CloseDeviceProc?, nint)" />.
/// </param>
/// <seealso cref="UioHook.SetDeviceProcs(OpenDeviceProc?, CloseDeviceProc?, nint)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int OpenDeviceProc(string path, int flags, nint userData);
