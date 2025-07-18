namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level global hook functionality.
/// </summary>
public interface IGlobalHookProvider
{
    /// <summary>
    /// Gets or sets the value which indicates whether events of type <see cref="EventType.KeyTyped" /> are enabled. The
    /// default value is <see langword="true" />.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if events of type <see cref="EventType.KeyTyped" /> are enabled. Otherwise,
    /// <see langword="false" />.
    /// </value>
    bool KeyTypedEnabled { get; set; }

    /// <summary>
    /// Sets the hook callback function.
    /// </summary>
    /// <param name="dispatchProc">
    /// The function to call when an event is raised, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback. Should not be used to pass pointers to objects,
    /// and <see cref="IntPtr.Zero" /> should usually be passed.
    /// </param>
    void SetDispatchProc(DispatchProc? dispatchProc, nint userData);

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult Run();

    /// <summary>
    /// Runs the global hook only for keyboard events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult RunKeyboard();

    /// <summary>
    /// Runs the global hook only for mouse events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult RunMouse();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult Stop();
}
