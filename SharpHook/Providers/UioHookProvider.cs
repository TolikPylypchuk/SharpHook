namespace SharpHook.Providers;

/// <summary>
/// A provider of low-level functionality which delegates it to the <see cref="UioHook" /> class.
/// </summary>
/// <seealso cref="UioHook" />
[ExcludeFromCodeCoverage]
public sealed class UioHookProvider :
    ILoggingProvider,
    IGlobalHookProvider,
    IEventSimulationProvider,
    IAccessibilityProvider,
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
    /// Sets the log callback function.
    /// </summary>
    /// <param name="loggerProc">
    /// The function to call for logging, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback. Should not be used to pass pointers to objects,
    /// and <see cref="IntPtr.Zero" /> should usually be passed.
    /// </param>
    /// <seealso cref="LoggerProc" />
    public void SetLoggerProc(LoggerProc? loggerProc, nint userData) =>
        UioHook.SetLoggerProc(loggerProc, userData);

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
    /// <seealso cref="DispatchProc" />
    public void SetDispatchProc(DispatchProc? dispatchProc, nint userData) =>
        UioHook.SetDispatchProc(dispatchProc, userData);

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Run() =>
        UioHook.Run();

    /// <summary>
    /// Runs the global hook only for keyboard events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method makes a difference only on Windows where there are two different global hooks - a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply filters mouse events
    /// out at the libuiohook level on these OSes.
    /// </remarks>
    public UioHookResult RunKeyboard() =>
        UioHook.RunKeyboard();

    /// <summary>
    /// Runs the global hook only for mouse events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method makes a difference only on Windows where there are two different global hooks - a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply filters keyboard
    /// events out at the libuiohook level on these OSes.
    /// </remarks>
    public UioHookResult RunMouse() =>
        UioHook.RunMouse();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Stop() =>
        UioHook.Stop();

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
    /// Only <see cref="MouseWheelEventData.Rotation" />, <see cref="MouseWheelEventData.Direction" />, and
    /// <see cref="MouseWheelEventData.Type" /> are considered.
    /// </term>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public UioHookResult PostEvent(ref UioHookEvent e) =>
        UioHook.PostEvent(ref e);

    /// <summary>
    /// Simulates input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The Unicode characters to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
    /// surrogate pairs, e.g. emojis) is supported.
    /// </para>
    /// <para>
    /// On Windows text simulation should work correctly and consistently.
    /// </para>
    /// <para>
    /// On macOS applications are not required to process text simulation, but most of them should handle it correctly.
    /// </para>
    /// <para>
    /// X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to
    /// that character, and then key press/release is simulated. Since the receiving application must react to the
    /// remapping, and may not do so instantaneously, a delay is needed for accurate simulation. This means that text
    /// entry on Linux works slowly and is not guaranteed to be correct. <see cref="PostTextDelayX11" /> can be used
    /// to get or set the delay if needed - longer delays add consistency but may be more jarring to end
    /// users - the default is 50 milliseconds.
    /// </para>
    /// </remarks>
    public UioHookResult PostText(string text) =>
        UioHook.PostText(text);

    /// <summary>
    /// Gets or sets the delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </summary>
    /// <value>The delay (in nanoseconds) between posting individual characters when posting text on Linux.</value>
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
    /// On Windows and macOS, this property does nothing and always returns <c>0</c>.
    /// </para>
    /// </remarks>
    public ulong PostTextDelayX11
    {
        get => UioHook.GetPostTextDelayX11();
        set => UioHook.SetPostTextDelayX11(value);
    }

    /// <summary>
    /// Checks whether access to macOS Accessibility API is enabled for the process, optionally prompting the user
    /// if it is disabled.
    /// </summary>
    /// <param name="promptUserIfDisabled">Prompt the user if access to macOS Accessibility API is disabled.</param>
    /// <returns>
    /// <see langword="true" /> if access to macOS Accessibility API is enabled for the process which means that
    /// global hooks and event simulation can be used. Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// On Windows and Linux, this method does nothing and always returns <see langword="true" />.
    /// </remarks>
    public bool IsAxApiEnabled(bool promptUserIfDisabled) =>
        UioHook.IsAxApiEnabled(promptUserIfDisabled);

    /// <summary>
    /// Gets or sets the value which indicates whether global hooks or event simulation should prompt the user when they
    /// try to request access to macOS Accessibility API, and it is disabled. The default value is
    /// <see langword="true" />.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if global hooks and event simulation should prompt the user for access to macOS
    /// Accessibility API when it is disabled. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>
    /// On Windows and Linux, this property does nothing and always returns <see langword="false" />.
    /// </remarks>
    public bool PromptUserIfAxApiDisabled
    {
        get => UioHook.GetPromptUserIfAxApiDisabled();
        set => UioHook.SetPromptUserIfAxApiDisabled(value);
    }

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
