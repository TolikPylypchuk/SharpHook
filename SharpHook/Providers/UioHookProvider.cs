namespace SharpHook.Providers;

using System;

using SharpHook.Native;

/// <summary>
/// A provider of low-level functionality which delegates it to the <see cref="UioHook" /> class.
/// </summary>
/// <seealso cref="UioHook" />
public sealed class UioHookProvider :
    IGlobalHookProvider,
    ILoggingProvider,
    IEventSimulationProvider,
    IScreenInfoProvider,
    IMouseInfoProvider
{
    private UioHookProvider()
    { }

    /// <summary>
    /// Gets the single instance of <see cref="UioHookProvider" />.
    /// </summary>
    public static UioHookProvider Instance { get; } = new();

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
    public void SetDispatchProc(DispatchProc? dispatchProc, IntPtr userData) =>
        UioHook.SetDispatchProc(dispatchProc, userData);

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Run() =>
        UioHook.Run();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Stop() =>
        UioHook.Stop();

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
    public void SetLoggerProc(LoggerProc? loggerProc, IntPtr userData) =>
        UioHook.SetLoggerProc(loggerProc, userData);

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
    /// <term>Events of this type are ignored.</term>
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
    public UioHookResult PostEvent(ref UioHookEvent e) =>
        UioHook.PostEvent(ref e);

    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <returns>The information about screens.</returns>
    public ScreenData[] CreateScreenInfo() =>
        UioHook.CreateScreenInfo();

    /// <summary>
    /// Gets the auto-repeat rate.
    /// </summary>
    /// <returns>The auto-repeat rate.</returns>
    public int GetAutoRepeatRate() =>
        UioHook.GetAutoRepeatRate();

    /// <summary>
    /// Gets the auto-repeat delay.
    /// </summary>
    /// <returns>The auto-repeat delay.</returns>
    public int GetAutoRepeatDelay() =>
        UioHook.GetAutoRepeatDelay();

    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
    public int GetPointerAccelerationMultiplier() =>
        UioHook.GetPointerAccelerationMultiplier();

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
    public int GetPointerAccelerationThreshold() =>
        UioHook.GetPointerAccelerationThreshold();

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
    public int GetPointerSensitivity() =>
        UioHook.GetPointerSensitivity();

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
    public int GetMultiClickTime() =>
        UioHook.GetMultiClickTime();
}
