namespace SharpHook.Native;

/// <summary>
/// Contains native methods of libuiohook.
/// </summary>
/// <seealso cref="IAccessibilityProvider" />
/// <seealso cref="IEventSimulationProvider" />
/// <seealso cref="IGlobalHookProvider" />
/// <seealso cref="ILoggingProvider" />
/// <seealso cref="IMouseInfoProvider" />
/// <seealso cref="IScreenInfoProvider" />
/// <seealso cref="UioHookProvider" />
#if NET5_0_OR_GREATER
[SuppressMessage(
    "CodeQuality",
    "IDE0079:Remove unnecessary suppression",
    Justification = "Suppressing CA1401 is necessary")]
[SuppressMessage(
    "Interoperability",
    "CA1401:P/Invokes should not be visible",
    Justification = "These methods are the whole point of the library")]
#endif
[ExcludeFromCodeCoverage]
public static partial class UioHook
{
    private const string LibUioHook = "uiohook";

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
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_logger_proc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLoggerProc(LoggerProc? loggerProc, nint userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_logger_proc", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetLoggerProc(LoggerProc? loggerProc, nint userData);
#endif

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
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetDispatchProc(DispatchProc? dispatchProc, nint userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetDispatchProc(DispatchProc? dispatchProc, nint userData);
#endif

    /// <summary>
    /// Runs the global hook and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method must not be called when a global hook is already running since it will corrupt the global state of
    /// libuiohook.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult Run();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult Run();
#endif

    /// <summary>
    /// Runs the global hook only for keyboard events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// This method makes a difference only on Windows where there are two different global hooks – a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply enables filtering
    /// mouse events out on these OSes.
    /// </para>
    /// <para>
    /// When a keyboard-only hook is running, the <see cref="UioHookEvent.Mask" /> field will not contain any mouse
    /// button state.
    /// </para>
    /// <para>
    /// This method must not be called when a global hook is already running since it will corrupt the global state of
    /// libuiohook.
    /// </para>
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run_keyboard")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult RunKeyboard();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run_keyboard", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult RunKeyboard();
#endif

    /// <summary>
    /// Runs the global hook only for mouse events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// This method makes a difference only on Windows where there are two different global hooks – a keyboard hook and
    /// a mouse hook. On macOS and Linux there is one hook for all events, and this method simply enables filtering
    /// keyboard events out on these OSes.
    /// </para>
    /// <para>
    /// When a mouse-only hook is running, the <see cref="UioHookEvent.Mask" /> field will not contain any keyboard
    /// modifier state.
    /// </para>
    /// <para>
    /// This method must not be called when a global hook is already running since it will corrupt the global state of
    /// libuiohook.
    /// </para>
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run_mouse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult RunMouse();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run_mouse", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult RunMouse();
#endif

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <returns>The result of the operation.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_stop")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult Stop();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_stop", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult Stop();
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
    /// Only <see cref="MouseWheelEventData.Rotation" />, <see cref="MouseWheelEventData.Direction" />, and
    /// <see cref="MouseWheelEventData.Type" /> are considered.
    /// </term>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_event")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult PostEvent(ref UioHookEvent e);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_event", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult PostEvent(ref UioHookEvent e);
#endif

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
    /// surrogate pairs, e.g., emojis) is supported.
    /// </para>
    /// <para>
    /// On Windows, text simulation should work correctly and consistently.
    /// </para>
    /// <para>
    /// On macOS, applications are not required to process text simulation, but most of them should handle it correctly.
    /// </para>
    /// <para>
    /// X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to
    /// that character, and then key press/release is simulated. Since the receiving application must react to the
    /// remapping, and may not do so instantaneously, a delay is needed for accurate simulation. This means that text
    /// simulation on Linux works slowly and is not guaranteed to be correct. <see cref="SetPostTextDelayX11" /> can be
    /// used to increase (or decrease) the delay if needed – longer dealys add consistency but may be more jarring to
    /// end users. <see cref="GetPostTextDelayX11" /> can be used to get the currently configured delay – the default is
    /// 50 milliseconds.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_text")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult PostText([MarshalAs(UnmanagedType.LPWStr)] string text);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_text", CallingConvention = CallingConvention.Cdecl)]
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
    /// On Windows and macOS, this method always returns <c>0</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_x11")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetPostTextDelayX11();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_x11", CallingConvention = CallingConvention.Cdecl)]
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
    /// On Windows and macOS, this method does nothing.
    /// </para>
    /// </remarks>
    /// <seealso cref="EventSimulator" />
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_x11")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetPostTextDelayX11(ulong delayNanoseconds);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_x11", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPostTextDelayX11(ulong delayNanoseconds);
#endif

    /// <summary>
    /// Checks whether events of type <see cref="EventType.KeyTyped" /> are enabled.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if events of type <see cref="EventType.KeyTyped" /> are enabled. Otherwise,
    /// <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// If the application doesn't use events of type <see cref="EventType.KeyTyped" />, then they should be disabled
    /// so that there is no performance penalty and no subtle system-wide side effects.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_is_key_typed_enabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool IsKeyTypedEnabled();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_is_key_typed_enabled", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool IsKeyTypedEnabled();
#endif

    /// <summary>
    /// Sets the value which indicates whether events of type <see cref="EventType.KeyTyped" /> are enabled. The default
    /// value is <see langword="true" />.
    /// </summary>
    /// <param name="enabled">
    /// <see langword="true" /> if events of type <see cref="EventType.KeyTyped" /> should be enabled. Otherwise,
    /// <see langword="false" />.
    /// </param>
    /// <remarks>
    /// If the application doesn't use events of type <see cref="EventType.KeyTyped" />, then they should be disabled
    /// so that there is no performance penalty and no subtle system-wide side effects.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_key_typed_enabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetKeyTypedEnabled([MarshalAs(UnmanagedType.I1)] bool enabled);
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_set_key_typed_enabled",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetKeyTypedEnabled([MarshalAs(UnmanagedType.I1)] bool enabled);
#endif

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
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_is_ax_api_enabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool IsAxApiEnabled([MarshalAs(UnmanagedType.I1)] bool promptUserIfDisabled);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_is_ax_api_enabled", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool IsAxApiEnabled([MarshalAs(UnmanagedType.I1)] bool promptUserIfDisabled);
#endif

    /// <summary>
    /// Gets the value which indicates whether global hooks or event simulation should prompt the user when they try
    /// to request access to macOS Accessibility API, and it is disabled. The default value is <see langword="true" />.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if global hooks and event simulation will prompt the user for access to macOS
    /// Accessibility API when it is disabled. Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// On Windows and Linux, this method does nothing and always returns <see langword="false" />.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_prompt_user_if_ax_api_disabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool GetPromptUserIfAxApiDisabled();
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_get_prompt_user_if_ax_api_disabled",
        CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool GetPromptUserIfAxApiDisabled();
#endif

    /// <summary>
    /// Sets the value which indicates whether global hooks or event simulation should prompt the user when they try
    /// to request access to macOS Accessibility API, and it is disabled. The default value is <see langword="true" />.
    /// </summary>
    /// <param name="promptUserIfDisabled">
    /// <see langword="true" /> if global hooks and event simulation should prompt the user for access to macOS
    /// Accessibility API when it is disabled. Otherwise, <see langword="false" />.
    /// </param>
    /// <remarks>
    /// On Windows and Linux, this method does nothing.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_prompt_user_if_ax_api_disabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetPromptUserIfAxApiDisabled([MarshalAs(UnmanagedType.I1)] bool promptUserIfDisabled);
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_set_prompt_user_if_ax_api_disabled",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPromptUserIfAxApiDisabled([MarshalAs(UnmanagedType.I1)] bool promptUserIfDisabled);
#endif

    /// <summary>
    /// Gets the frequency in seconds for polling the access to the macOS Accessibility API when the global hook is
    /// running. The default value is 1 second.
    /// </summary>
    /// <returns>
    /// The frequency in seconds for polling the access to the macOS Accessibility API when the global hook is running.
    /// </returns>
    /// <remarks>
    /// On Windows and Linux, this method does nothing and always returns <c>0</c>.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_ax_poll_frequency")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetAxPollFrequency();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_ax_poll_frequency", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint GetAxPollFrequency();
#endif

    /// <summary>
    /// Sets the frequency in seconds for polling the access to the macOS Accessibility API when the global hook is
    /// running. The default value is 1 second.
    /// </summary>
    /// <param name="frequency">
    /// The frequency in seconds for polling the access to the macOS Accessibility API when the global hook is running.
    /// </param>
    /// <remarks>
    /// On Windows and Linux, this method does nothing.
    /// </remarks>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_ax_poll_frequency")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetAxPollFrequency(uint frequency);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_ax_poll_frequency", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetAxPollFrequency(uint frequency);
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
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateScreenInfo(out byte count);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint CreateScreenInfo(out byte count);
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
        nint screens = CreateScreenInfo(out byte count);

        var result = new ScreenData[count];
        int size = Marshal.SizeOf<ScreenData>();

        for (int i = 0; i < count; i++)
        {
            result[i] = Marshal.PtrToStructure<ScreenData>((nint)((long)screens + i * size));
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
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAutoRepeatRate();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetAutoRepeatRate();
#endif

    /// <summary>
    /// Gets the auto-repeat delay.
    /// </summary>
    /// <returns>The auto-repeat delay.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAutoRepeatDelay();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetAutoRepeatDelay();
#endif

    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_multiplier")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetPointerAccelerationMultiplier();
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_get_pointer_acceleration_multiplier",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPointerAccelerationMultiplier();
#endif

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_threshold")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetPointerAccelerationThreshold();
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_get_pointer_acceleration_threshold",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPointerAccelerationThreshold();
#endif

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetPointerSensitivity();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPointerSensitivity();
#endif

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetMultiClickTime();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_multi_click_time", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetMultiClickTime();
#endif
}
