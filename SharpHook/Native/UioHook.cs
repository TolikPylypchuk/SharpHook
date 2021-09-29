using System.Runtime.InteropServices;

namespace SharpHook.Native
{
    public static class UioHook
    {
        private const string LibUioHook = "uiohook";

        [DllImport(LibUioHook, EntryPoint = "hook_set_dispatch_proc")]
        public static extern void SetDispatchProc(DispatchProc dispatchProc);

        [DllImport(LibUioHook, EntryPoint = "hook_run")]
        public static extern UioHookResult Run();

        [DllImport(LibUioHook, EntryPoint = "hook_stop")]
        public static extern UioHookResult Stop();

        [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
        public static extern ScreenData[] CreateScreenInfo(byte[] count);

        [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_rate")]
        public static extern long GetAutoRepeatRate();

        [DllImport(LibUioHook, EntryPoint = "hook_get_auto_repeat_delay")]
        public static extern long GetAutoRepeatDelay();

        [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_multiplier")]
        public static extern long GetPointerAccelerationMultiplier();

        [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_acceleration_threshold")]
        public static extern long GetPointerAccelerationThreshold();

        [DllImport(LibUioHook, EntryPoint = "hook_get_pointer_sensitivity")]
        public static extern long GetPointerSensitivity();

        [DllImport(LibUioHook, EntryPoint = "hook_get_multi_click_time")]
        public static extern long GetMultiClickTime();
    }
}
