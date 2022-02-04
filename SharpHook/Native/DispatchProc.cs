namespace SharpHook.Native;

/// <summary>
/// Represents a function which is called when an event occurs.
/// </summary>
/// <param name="e">The event that has occured.</param>
/// <seealso cref="UioHook.SetDispatchProc(DispatchProc)" />
public delegate void DispatchProc(ref UioHookEvent e);
