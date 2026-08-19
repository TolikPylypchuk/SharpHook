namespace SharpHook.Native;

/// <summary>
/// Represents a function which closes a privileged device using a provided file descriptor.
/// </summary>
/// <param name="fd">The file descriptor of the device to close.</param>
/// <param name="userData">
/// The data that was passed to <see cref="UioHook.SetDeviceProcs(OpenDeviceProc?, CloseDeviceProc?, nint)" />.
/// </param>
/// <seealso cref="UioHook.SetDeviceProcs(OpenDeviceProc?, CloseDeviceProc?, nint)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void CloseDeviceProc(int fd, IntPtr userData);
