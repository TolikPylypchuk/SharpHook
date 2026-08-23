namespace SharpHook.Providers;

/// <summary>
/// A provider of low-level functionality which delegates it to libuiohook.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class UioHookProvider :
    ILoggingProvider,
    IGlobalHookProvider,
    IEventSimulationProvider,
    IFeatureProvider,
    IAccessibilityProvider,
    ILinuxBackendProvider,
    IDeviceProcsProvider,
    IScreenInfoProvider,
    IKeyboardInfoProvider,
    IMouseInfoProvider
{
    private UioHookProvider()
    { }

    /// <summary>
    /// Gets the single instance of <see cref="UioHookProvider" />.
    /// </summary>
    public static UioHookProvider Instance { get; } = new();

    /// <summary>
    /// Gets or sets the value which indicates whether events of type <see cref="EventType.KeyTyped" /> are enabled. The
    /// default value is <see langword="false" />.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if events of type <see cref="EventType.KeyTyped" /> are enabled. Otherwise,
    /// <see langword="false" />.
    /// </value>
    /// <remarks>
    /// <para>
    /// If the application doesn't use events of type <see cref="EventType.KeyTyped" />, then they should be disabled
    /// so that there is no performance penalty and no subtle system-wide side effects.
    /// </para>
    /// <para>
    /// Support for events of type <see cref="EventType.KeyTyped" /> can be queried using
    /// <see cref="GetOptionalFeatureSupport" />. If that method returns <see cref="UioHookFeature.KeyTypedEvents" /> as
    /// one of its flags, then <see cref="EventType.KeyTyped" /> events are supported and can be enabled or disabled. On
    /// Wayland, this property does nothing since this feature is not supported.
    /// </para>
    /// </remarks>
    public bool KeyTypedEnabled
    {
        get => UioHook.IsKeyTypedEnabled();
        set => UioHook.SetKeyTypedEnabled(value);
    }

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
    /// On Windows and macOS, as well as Wayland, this property does nothing and always returns <c>0</c>.
    /// </para>
    /// </remarks>
    public ulong PostTextDelayLinux
    {
        get => UioHook.GetPostTextDelayLinux();
        set => UioHook.SetPostTextDelayLinux(value);
    }

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
    /// Gets or sets the frequency in seconds for polling the access to the macOS Accessibility API when the global hook
    /// is running. The default value is 1 second.
    /// </summary>
    /// <value>
    /// The frequency in seconds for polling the access to the macOS Accessibility API when the global hook is running.
    /// </value>
    /// <remarks>
    /// On Windows and Linux, this property does nothing and always returns <c>0</c>.
    /// </remarks>
    public uint AxPollFrequency
    {
        get => UioHook.GetAxPollFrequency();
        set => UioHook.SetAxPollFrequency(value);
    }

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
    /// This method makes a difference only on Windows where there are two different global hooks – a keyboard hook and
    /// a mouse hook. On macOS and Linux, there is one hook for all events, and this method simply filters mouse events
    /// out at the libuiohook level on these OSes.
    /// </remarks>
    public UioHookResult RunKeyboard() =>
        UioHook.RunKeyboard();

    /// <summary>
    /// Runs the global hook only for mouse events and blocks the thread until it's stopped.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// This method makes a difference only on Windows where there are two different global hooks – a keyboard hook and
    /// a mouse hook. On macOS and Linux, there is one hook for all events, and this method simply filters keyboard
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
    /// <param name="event">The event to post.</param>
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
    /// <term><see cref="EventType.MousePressedIgnoreCoordinates" /></term>
    /// <term>
    /// Only <see cref="MouseEventData.Button" /> is considered.
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
    /// <term><see cref="EventType.MouseReleasedIgnoreCoordinates" /></term>
    /// <term>
    /// Only <see cref="MouseEventData.Button" /> is considered.
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
    /// <term><see cref="EventType.MouseMovedRelative" /></term>
    /// <term>Only <see cref="MouseEventData.X" /> and <see cref="MouseEventData.Y" /> are considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDragged" /></term>
    /// <term>Not recommended to use; same as <see cref="EventType.MouseMoved" />.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDraggedRelative" /></term>
    /// <term>Not recommended to use; same as <see cref="EventType.MouseMovedRelative" />.</term>
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
    /// <seealso cref="PostEvents" />
    public UioHookResult PostEvent(ref UioHookEvent @event) =>
        UioHook.PostEvent(ref @event);

    /// <summary>
    /// Posts a sequence of fake input events.
    /// </summary>
    /// <param name="events">The events to post.</param>
    /// <param name="size">The number of events to post.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// All the same rules apply as to <see cref="PostEvent" />. The sequence of events must not contain events of types
    /// <see cref="EventType.KeyTyped" /> or <see cref="EventType.MouseClicked" />, or the method will fail, potentially
    /// in the middle of the simulation sequence.
    /// </remarks>
    /// <seealso cref="PostEvent" />
    public UioHookResult PostEvents(UioHookEvent[] events, uint size) =>
        UioHook.PostEvents(events, size);

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
    /// On Windows, text simulation should work correctly and consistently.
    /// </para>
    /// <para>
    /// On macOS, applications are not required to process text simulation, but most of them should handle it correctly.
    /// </para>
    /// <para>
    /// X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to
    /// that character, and then key press/release is simulated. Since the receiving application must react to the
    /// remapping, and may not do so instantaneously, a delay is needed for accurate simulation. This means that text
    /// entry on Linux works slowly and is not guaranteed to be correct. <see cref="PostTextDelayLinux" /> can be used
    /// to get or set the delay if needed – longer delays add consistency but may be more jarring to end
    /// users – the default is 50 milliseconds.
    /// </para>
    /// </remarks>
    public UioHookResult PostText(string text) =>
        UioHook.PostText(text);

    /// <summary>
    /// Initializes virtual input devices used for event simulation.
    /// </summary>
    /// <param name="applicationName">The application name which is used to identify the virtual devices.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Virtual input devices are required on Linux when using a uinput-based backend. On Windows, macOS, and the
    /// XRecord-based X11 backend, this method does nothing and always returns <see cref="UioHookResult.Success" />.
    /// </para>
    /// <para>
    /// Initializing virtual input devices is expensive, so it should generally be done once early in the application's
    /// lifetime. If virtual devices are initialzed, then calling this method again will increase the reference counter
    /// – the devices will be destroyed only when the reference counter reaches zero, i.e., when the same number of
    /// calls to <see cref="DestroyVirtualDevices" /> are made.
    /// </para>
    /// </remarks>
    /// <seealso cref="DestroyVirtualDevices" />
    public UioHookResult InitializeVirtualDevices(string applicationName) =>
        UioHook.InitializeVirtualDevices(applicationName);

    /// <summary>
    /// Destroys virtual input devices used for event simulation.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Virtual input devices are required on Linux when using a uinput-based backend. On Windows, macOS, and the
    /// XRecord-based X11 backend, this method does nothing and always returns <see cref="UioHookResult.Success" />.
    /// </para>
    /// <para>
    /// If multiple calls to <see cref="InitializeVirtualDevices" /> were made, then this method must be called the same
    /// number of times, and will actually destroy the virtual devices only when the reference counter reaches zero.
    /// </para>
    /// <para>
    /// When virtual devices are not initialized, this method does nothing.
    /// </para>
    /// </remarks>
    /// <seealso cref="DestroyVirtualDevices" />
    public UioHookResult DestroyVirtualDevices() =>
        UioHook.DestroyVirtualDevices();

    /// <summary>
    /// Returns optional features of libuiohook that are supported on the current platform.
    /// </summary>
    /// <returns>
    /// Flags which indicate which optional features are supported on the current platform.
    /// </returns>
    public UioHookFeature GetOptionalFeatureSupport() =>
        UioHook.GetOptionalFeatureSupport();

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
    /// Gets the mode for selecting which Linux backend to load.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns only the selection mode. If you need to get the Linux backend that was actually loaded, then
    /// use <see cref="GetLoadedLinuxBackend()" />.
    /// </para>
    /// <para>
    /// On Windows and macOS, this method always returns <see cref="LinuxMode.AutoXRecord" /> which is the default
    /// value.
    /// </para>
    /// </remarks>
    public LinuxMode GetLinuxMode() =>
        UioHook.GetLinuxMode();

    /// <summary>
    /// Sets the mode for selecting which Linux backend to load.
    /// </summary>
    /// <remarks>
    /// This method returns one of three possible values:
    /// <list type="bullet">
    /// <item><see cref="UioHookResult.Success" /> if the Linux mode was successfully set.</item>
    /// <item><see cref="UioHookResult.Failure" /> if the provided mode is an invalid enum value.</item>
    /// <item>
    /// <see cref="UioHookResult.ErrorLinuxLoadBackend" /> if a Linux backend has already been loaded and changing it is
    /// not allowed.
    /// </item>
    /// </list>
    /// </remarks>
    public UioHookResult SetLinuxMode(LinuxMode mode) =>
        UioHook.SetLinuxMode(mode);

    /// <summary>
    /// Gets the loaded Linux backend.
    /// </summary>
    /// <remarks>
    /// On Windows and macOS, this method always returns <see cref="LinuxBackend.None" />.
    /// </remarks>
    public LinuxBackend GetLoadedLinuxBackend() =>
        UioHook.GetLoadedLinuxBackend();

    /// <summary>
    /// Sets the functions to open and close privieged devices. This can be used to provide a custom way of opening and
    /// closing devices on Linux without requiring direct file access to them. This is an advanced scenario and should
    /// generally be avoided.
    /// </summary>
    /// <param name="openProc">
    /// The function to use when opening a privileged device, or <see langword="null" /> to use the default method.
    /// </param>
    /// <param name="closeProc">
    /// The function to use when closing a privileged device, or <see langword="null" /> to use the default method.
    /// </param>
    /// <param name="userData">
    /// The data to pass to the device functions.
    /// </param>
    /// <remarks>
    /// These functions are used only on Linux, and only when the loaded backend uses libinput and uinput.
    /// </remarks>
    public void SetDeviceProcs(OpenDeviceProc? openProc, CloseDeviceProc? closeProc, nint userData) =>
        UioHook.SetDeviceProcs(openProc, closeProc, userData);

    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <returns>The information about screens.</returns>
    public ScreenData[] CreateScreenInfo()
    {
        nint screens = UioHook.CreateScreenInfo(out byte count);

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
    /// Gets the key auto-repeat rate.
    /// </summary>
    /// <returns>The key auto-repeat rate.</returns>
    public int GetAutoRepeatRate() =>
        UioHook.GetAutoRepeatRate();

    /// <summary>
    /// Gets the key auto-repeat delay.
    /// </summary>
    /// <returns>The key auto-repeat delay.</returns>
    public int GetAutoRepeatDelay() =>
        UioHook.GetAutoRepeatDelay();

    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is supported. Otherwise,
    /// this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    public int GetPointerAccelerationMultiplier() =>
        UioHook.GetPointerAccelerationMultiplier();

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is supported. Otherwise,
    /// this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    public int GetPointerAccelerationThreshold() =>
        UioHook.GetPointerAccelerationThreshold();

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is supported. Otherwise,
    /// this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    public int GetPointerSensitivity() =>
        UioHook.GetPointerSensitivity();

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
    public int GetMultiClickTime() =>
        UioHook.GetMultiClickTime();
}
