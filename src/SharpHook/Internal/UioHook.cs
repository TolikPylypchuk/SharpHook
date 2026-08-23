namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal static partial class UioHook
{
    private const string LibUioHook = "uiohook";

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_logger_proc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLoggerProc(LoggerProc? loggerProc, nint userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_logger_proc", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetLoggerProc(LoggerProc? loggerProc, nint userData);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetDispatchProc(DispatchProc? dispatchProc, nint userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetDispatchProc(DispatchProc? dispatchProc, nint userData);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult Run();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult Run();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run_keyboard")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult RunKeyboard();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run_keyboard", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult RunKeyboard();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_run_mouse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult RunMouse();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_run_mouse", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult RunMouse();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_stop")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult Stop();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_stop", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult Stop();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_event")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult PostEvent(ref UioHookEvent @event);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_event", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult PostEvent(ref UioHookEvent @event);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_events")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult PostEvents([In] UioHookEvent[] events, uint size);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_events", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult PostEvents([In] UioHookEvent[] events, uint size);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_post_text")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult PostText([MarshalAs(UnmanagedType.LPWStr)] string text);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_post_text", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult PostText([MarshalAs(UnmanagedType.LPWStr)] string text);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_init_virtual_devices")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult InitializeVirtualDevices(
        [MarshalAs(UnmanagedType.LPStr)] string applicationName);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_init_virtual_devices", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult InitializeVirtualDevices(
        [MarshalAs(UnmanagedType.LPStr)] string applicationName);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_destroy_virtual_devices")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult DestroyVirtualDevices();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_destroy_virtual_devices", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult DestroyVirtualDevices();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_optional_feature_support")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookFeature GetOptionalFeatureSupport();
#else
    [DllImport(
        LibUioHook,
        EntryPoint = "hook_get_optional_feature_support",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookFeature GetOptionalFeatureSupport();
#endif

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

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_ax_poll_frequency")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetAxPollFrequency();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_ax_poll_frequency", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint GetAxPollFrequency();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_ax_poll_frequency")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetAxPollFrequency(uint frequency);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_ax_poll_frequency", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetAxPollFrequency(uint frequency);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_linux")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetPostTextDelayLinux();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_post_text_delay_linux", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong GetPostTextDelayLinux();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_linux")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetPostTextDelayLinux(ulong delayNanoseconds);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_post_text_delay_linux", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPostTextDelayLinux(ulong delayNanoseconds);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_linux_mode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LinuxMode GetLinuxMode();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_linux_mode", CallingConvention = CallingConvention.Cdecl)]
    public static extern LinuxMode GetLinuxMode();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_linux_mode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UioHookResult SetLinuxMode(LinuxMode mode);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_linux_mode", CallingConvention = CallingConvention.Cdecl)]
    public static extern UioHookResult SetLinuxMode(LinuxMode mode);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_loaded_linux_backend")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LinuxBackend GetLoadedLinuxBackend();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_loaded_linux_backend", CallingConvention = CallingConvention.Cdecl)]
    public static extern LinuxBackend GetLoadedLinuxBackend();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_set_device_procs")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetDeviceProcs(OpenDeviceProc? openProc, CloseDeviceProc? closeProc, nint userData);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_set_device_procs", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetDeviceProcs(OpenDeviceProc? openProc, CloseDeviceProc? closeProc, nint userData);
#endif

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

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateScreenInfo(out byte count);
#else
    [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info", CallingConvention = CallingConvention.Cdecl)]
    internal static extern nint CreateScreenInfo(out byte count);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAutoRepeatRate();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetAutoRepeatRate();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAutoRepeatDelay();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetAutoRepeatDelay();
#endif

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

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetPointerSensitivity();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPointerSensitivity();
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetMultiClickTime();
#else
    [DllImport(LibUioHook, EntryPoint = "hook_get_multi_click_time", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetMultiClickTime();
#endif
}
