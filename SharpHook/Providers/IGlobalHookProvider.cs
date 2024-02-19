namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level global hook functionality.
/// </summary>
/// <seealso cref="UioHookProvider" />
public interface IGlobalHookProvider
{
    /// <summary>
    /// Sets the hook callback function.
    /// </summary>
    /// <param name="dispatchProc">
    /// The function to call when an event is raised, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback. Should not be used, and <see cref="IntPtr.Zero" /> should always be passed.
    /// </param>
    /// <seealso cref="DispatchProc" />
    void SetDispatchProc(DispatchProc? dispatchProc, IntPtr userData);

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult Run();

    /// <summary>
    /// Runs the global hook only for keyboard events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method makes a difference only on Windows where there are two different global hooks - a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply filters mouse events
    /// out at the libuiohook level on these OSes.
    /// </remarks>
    UioHookResult RunKeyboard();

    /// <summary>
    /// Runs the global hook only for mouse events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method makes a difference only on Windows where there are two different global hooks - a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply filters keyboard
    /// events out at the libuiohook level on these OSes.
    /// </remarks>
    UioHookResult RunMouse();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult Stop();
}
