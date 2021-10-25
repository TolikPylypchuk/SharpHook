namespace SharpHook.Native;

/// <summary>
/// Represents a function which is called by libuiohook when an event occurs.
/// </summary>
/// <param name="e">The event that occured.</param>
/// <seealso cref="UioHook.SetDispatchProc(DispatchProc)" />
public delegate void DispatchProc(ref UioHookEvent e);
