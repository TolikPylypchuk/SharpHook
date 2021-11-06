namespace SharpHook.Native;

/// <summary>
/// Contains native methods of libuiohook.
/// </summary>
#if NET5_0_OR_GREATER
[SuppressMessage(
    "Interoperability",
    "CA1401:P/Invokes should not be visible",
    Justification = "These methods are the whole point of the project")]
#endif
public static class UioHook
{
    /// <summary>
    /// An empty hook callback function.
    /// </summary>
    /// <value>A hook callback function which does nothing.</value>
    /// <remarks>You can use this function to "unset" a hook callback function.</remarks>
    public static readonly DispatchProc EmptyDispatchProc = static (ref UioHookEvent e) => { };

    private const string LibUioHook = "uiohook";

    /// <summary>
    /// Sets the hook callback function.
    /// </summary>
    /// <param name="dispatchProc">The function to call when an event is raised.</param>
    [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    public static extern void SetDispatchProc(DispatchProc dispatchProc);

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_run")]
    public static extern UioHookResult Run();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_stop")]
    public static extern UioHookResult Stop();

    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <param name="count">The number of screens.</param>
    /// <returns>
    /// The information about screens as an unmanaged array of <see cref="ScreenData" /> whose length is returned
    /// as <paramref name="count" />.
    /// </returns>
    /// <remarks>
    /// You should use <see cref="CreateScreenInfo()" /> instead as it returns a managed array.
    /// </remarks>
    /// <seealso cref="CreateScreenInfo()" />
    [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
    public static extern IntPtr CreateScreenInfo(out byte count);

    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <returns>The information about screens.</returns>
    /// <remarks>
    /// This is the safe version of <see cref="CreateScreenInfo(out byte)" /> as it returns a managed array.
    /// </remarks>
    /// <seealso cref="CreateScreenInfo(out byte)" />
    public static ScreenData[] CreateScreenInfo()
    {
        var screens = CreateScreenInfo(out byte count);

        var result = new ScreenData[count];
        var size = Marshal.SizeOf<ScreenData>();

        for (int i = 0; i < count; i++)
        {
            result[i] = Marshal.PtrToStructure<ScreenData>(new IntPtr(screens.ToInt64() + i * size));
        }

        return result;
    }

    /// <summary>
    /// Gets the auto-repeat rate.
    /// </summary>
    /// <returns>The auto-repeat rate.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate")]
    public static extern long GetAutoRepeatRate();

    /// <summary>
    /// Gets the auto-repeat delay.
    /// </summary>
    /// <returns>The auto-repeat delay.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
    public static extern long GetAutoRepeatDelay();

    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_multiplier")]
    public static extern long GetPointerAccelerationMultiplier();

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_threshold")]
    public static extern long GetPointerAccelerationThreshold();

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
    public static extern long GetPointerSensitivity();

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
    [DllImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
    public static extern long GetMultiClickTime();
}
