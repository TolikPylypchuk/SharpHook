namespace SharpHook.Native;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

/// <summary>
/// Contains native methods of libuiohook.
/// </summary>
#if NET5_0_OR_GREATER
[SuppressMessage(
    "Interoperability",
    "CA1401:P/Invokes should not be visible",
    Justification = "These methods are the whole point of the project")]
#endif
[ExcludeFromCodeCoverage]
public static partial class UioHook
{
    private const string LibUioHook = "uiohook";

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
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    public static partial void SetDispatchProc(DispatchProc? dispatchProc, IntPtr userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    public static extern void SetDispatchProc(DispatchProc? dispatchProc, IntPtr userData);
#endif

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run")]
    public static partial UioHookResult Run();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run")]
    public static extern UioHookResult Run();
#endif

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_stop")]
    public static partial UioHookResult Stop();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_stop")]
    public static extern UioHookResult Stop();
#endif

    /// <summary>
    /// Sets the log callback function.
    /// </summary>
    /// <param name="loggerProc">
    /// The function to call for logging, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback. Should not be used, and <see cref="IntPtr.Zero" /> should always be passed.
    /// </param>
    /// <seealso cref="LoggerProc" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_logger_proc")]
    public static partial void SetLoggerProc(LoggerProc? loggerProc, IntPtr userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_logger_proc")]
    public static extern void SetLoggerProc(LoggerProc? loggerProc, IntPtr userData);
#endif

    /// <summary>
    /// Posts a fake input event.
    /// </summary>
    /// <param name="e">The event to post.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The instance of the event doesn't need all fields to have value. Only <see cref="UioHookEvent.Type" />,
    /// <see cref="UioHookEvent.Keyboard" />/<see cref="UioHookEvent.Mouse" />/<see cref="UioHookEvent.Wheel" /> should
    /// be present.
    /// </para>
    /// <para>
    /// The following table describes the specifics of simulating each event type.
    /// <list type="table">
    /// <listheader>
    /// <term>Event type</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term><see cref="EventType.HookEnabled" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.HookDisabled" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyPressed" /></term>
    /// <term>Only <see cref="KeyboardEventData.KeyCode" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyReleased" /></term>
    /// <term>Only <see cref="KeyboardEventData.KeyCode" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyTyped" /></term>
    /// <term>
    /// Events of this type are ignored. <see cref="PostText(string)" /> should be used to post Unicode characters.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MousePressed" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.X" />, <see cref="MouseWheelEventData.Y" />,
    /// and <see cref="MouseEventData.Button" /> are considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseReleased" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.X" />, <see cref="MouseWheelEventData.Y" />,
    /// and <see cref="MouseEventData.Button" /> are considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseClicked" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseMoved" /></term>
    /// <term>Only <see cref="MouseEventData.X" /> and <see cref="MouseEventData.Y" /> are considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDragged" /></term>
    /// <term>Not recommended to use; same as <see cref="EventType.MouseMoved" />.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseWheel" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.Rotation" /> and <see cref="MouseWheelEventData.Delta" /> are considered.
    /// </term>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_event")]
    public static partial UioHookResult PostEvent(ref UioHookEvent e);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_event")]
    public static extern UioHookResult PostEvent(ref UioHookEvent e);
#endif

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Text entry simulation is not guaranteed to work correctly. View the docs for more info.
    /// </para>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_text")]
    public static partial UioHookResult PostText([MarshalAs(UnmanagedType.LPWStr)] string text);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_text")]
    public static extern UioHookResult PostText([MarshalAs(UnmanagedType.LPWStr)] string text);
#endif

    /// <summary>
    /// Gets the delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </summary>
    /// <returns>The delay (in nanoseconds) between posting individual characters when posting text on Linux.</returns>
    /// <remarks>
    /// <para>
    /// X11 doesn't support posting arbitrary Unicode characters directly. Instead, for each character,
    /// an unused key code is remapped to that character, and then key press/release is simulated. Since the receiving
    /// application must react to the remapping, and may not do so instantaneously, a delay is needed for accurate
    /// simulation.
    /// </para>
    /// <para>
    /// The default delay is 50 milliseconds.
    /// </para>
    /// <para>
    /// On Windows and macOS this method always returns 0.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_x11")]
    public static partial ulong GetPostTextDelayX11();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_x11")]
    public static extern ulong GetPostTextDelayX11();
#endif

    /// <summary>
    /// Sets the delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </summary>
    /// <param name="delayNanoseconds">
    /// The delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </param>
    /// <remarks>
    /// <para>
    /// X11 doesn't support posting arbitrary Unicode characters directly. Instead, for each character,
    /// an unused key code is remapped to that character, and then key press/release is simulated. Since the receiving
    /// application must react to the remapping, and may not do so instantaneously, a delay is needed for accurate
    /// simulation.
    /// </para>
    /// <para>
    /// The default delay is 50 milliseconds.
    /// </para>
    /// <para>
    /// On Windows and macOS this method does nothing.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_x11")]
    public static partial void SetPostTextDelayX11(ulong delayNanoseconds);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_x11")]
    public static extern void SetPostTextDelayX11(ulong delayNanoseconds);
#endif

    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <param name="count">The number of screens.</param>
    /// <returns>
    /// The information about screens as an unmanaged array of <see cref="ScreenData" /> whose length is returned
    /// as <paramref name="count" />. The memory used by the array must be freed manually.
    /// </returns>
    /// <remarks>
    /// You should use <see cref="CreateScreenInfo()" /> instead as it returns a managed array.
    /// </remarks>
    /// <seealso cref="CreateScreenInfo()" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
    public static partial IntPtr CreateScreenInfo(out byte count);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
    public static extern IntPtr CreateScreenInfo(out byte count);
#endif

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
        int size = Marshal.SizeOf<ScreenData>();

        for (int i = 0; i < count; i++)
        {
            result[i] = Marshal.PtrToStructure<ScreenData>(new IntPtr(screens.ToInt64() + i * size));
        }

        Marshal.FreeHGlobal(screens);

        return result;
    }

    /// <summary>
    /// Gets the auto-repeat rate.
    /// </summary>
    /// <returns>The auto-repeat rate.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate")]
    public static partial int GetAutoRepeatRate();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate")]
    public static extern int GetAutoRepeatRate();
#endif

    /// <summary>
    /// Gets the auto-repeat delay.
    /// </summary>
    /// <returns>The auto-repeat delay.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
    public static partial int GetAutoRepeatDelay();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
    public static extern int GetAutoRepeatDelay();
#endif

    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_multiplier")]
    public static partial int GetPointerAccelerationMultiplier();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_multiplier")]
    public static extern int GetPointerAccelerationMultiplier();
#endif

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_threshold")]
    public static partial int GetPointerAccelerationThreshold();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_threshold")]
    public static extern int GetPointerAccelerationThreshold();
#endif

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
    public static partial int GetPointerSensitivity();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
    public static extern int GetPointerSensitivity();
#endif

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
    public static partial int GetMultiClickTime();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
    public static extern int GetMultiClickTime();
#endif
}
