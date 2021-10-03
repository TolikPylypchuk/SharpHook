using System.Runtime.InteropServices;

namespace SharpHook.Native
{
    /// <summary>
    /// Contains native methods of libuiohook.
    /// </summary>
    public static class UioHook
    {
        private const string LibUioHook = "uiohook";

        /// <summary>
        /// Sets the hook callback function.
        /// </summary>
        /// <param name="dispatchProc">The callback to call when an event is raised.</param>
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
        /// <param name="count">The count.</param>
        /// <returns>The information about screens.</returns>
        [DllImport(LibUioHook, EntryPoint = "hook_create_screen_info")]
        public static extern ScreenData[] CreateScreenInfo(byte[] count);

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
}
