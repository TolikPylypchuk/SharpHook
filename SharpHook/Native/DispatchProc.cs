namespace SharpHook.Native;

/// <summary>
/// Represents a function which is called when an event occurs.
/// </summary>
/// <param name="e">The event that has occured.</param>
/// <param name="userData">
/// The data that was passed to <see cref="UioHook.SetDispatchProc(DispatchProc?, nint)" />.
/// Usually should not be used.
/// </param>
/// <seealso cref="UioHook.SetDispatchProc(DispatchProc?, nint)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void DispatchProc(ref UioHookEvent e, nint userData);
