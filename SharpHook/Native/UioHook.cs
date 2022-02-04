using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

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
    private const string LibUioHook = "uiohook";

    /// <summary>
    /// Sets the hook callback function.
    /// </summary>
    /// <param name="dispatchProc">The function to call when an event is raised.</param>
    /// <remarks>Pass <see langword="null" /> to this function to unset a hook callback function.</remarks>
    [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    public static extern void SetDispatchProc(DispatchProc? dispatchProc);

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
    /// Posts a fake input event.
    /// </summary>
    /// <param name="e">The event to post.</param>
    /// <remarks>
    /// <para>
    /// The instance of the event doesn't need all fields to have value. Only <see cref="UioHookEvent.Type" />,
    /// <see cref="UioHookEvent.Keyboard" />/<see cref="UioHookEvent.Mouse" />/<see cref="UioHookEvent.Wheel" /> and
    /// <see cref="UioHookEvent.Mask" /> should be present. <see cref="UioHookEvent.Mask" /> is ignored on Windows.
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
    /// Not recommended to use since on some platforms events of this type are ignored, while on others they are not.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MousePressed" /></term>
    /// <term>Only <see cref="MouseEventData.Button" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseReleased" /></term>
    /// <term>Only <see cref="MouseEventData.Button" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseClicked" /></term>
    /// <term>
    /// Not recommended to use since on some platforms events of this type are ignored, while on others they are not.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseMoved" /></term>
    /// <term>Only <see cref="MouseEventData.X" /> and <see cref="MouseEventData.Y" /> are considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDragged" /></term>
    /// <term>Not recommended to use; instead, use <see cref="EventType.MouseMoved" /> with a modifier mask.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseWheel" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.X" />, <see cref="MouseWheelEventData.Y" />,
    /// <see cref="MouseWheelEventData.Amount" />, and <see cref="MouseWheelEventData.Rotation" /> are considered.
    /// </term>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of
    /// libuiohook for more details.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
    [DllImport(LibUioHook, EntryPoint = "hook_post_event")]
    public static extern void PostEvent(ref UioHookEvent e);

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
