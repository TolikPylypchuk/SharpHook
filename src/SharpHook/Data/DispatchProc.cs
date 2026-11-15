namespace SharpHook.Data;

/// <summary>
/// Represents a function which is called when an event occurs.
/// </summary>
/// <param name="event">The event that has occurred.</param>
/// <param name="userData">
/// The data that was passed to <see cref="IGlobalHookProvider.SetDispatchProc(DispatchProc?, nint)" />.
/// Usually should not be used.
/// </param>
/// <seealso cref="IGlobalHookProvider.SetDispatchProc(DispatchProc?, nint)" />
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void DispatchProc(ref UioHookEvent @event, nint userData);
