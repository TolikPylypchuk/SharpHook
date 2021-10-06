using System;
using System.Threading.Tasks;

using SharpHook.Native;

namespace SharpHook.Sample
{
    public static class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("---------- SharpHook Sample ----------\n");

            PrintSystemInfo();

            Console.WriteLine("---------- Press q to quit ----------\n");

            var hook = CreateHook();
            await hook.Start();
        }

        private static void PrintSystemInfo()
        {
            Console.WriteLine("System info:");
            Console.WriteLine($"Auto-repeat rate: {UioHook.GetAutoRepeatRate()}");
            Console.WriteLine($"Auto-repeat delay: {UioHook.GetAutoRepeatDelay()}");
            Console.WriteLine($"Pointer acceleration threshold: {UioHook.GetPointerAccelerationThreshold()}");
            Console.WriteLine($"Pointer sensitivity: {UioHook.GetPointerSensitivity()}");
            Console.WriteLine($"Multi-click time: {UioHook.GetMultiClickTime()}");
            Console.WriteLine();
        }

        private static IGlobalHook CreateHook()
        {
            var hook = new TaskPoolGlobalHook();

            hook.HookEnabled += OnHookEvent;
            hook.HookDisabled += OnHookEvent;

            hook.KeyTyped += OnHookEvent;
            hook.KeyPressed += OnHookEvent;
            hook.KeyReleased += OnHookEvent;
            hook.KeyReleased += OnKeyReleased;

            hook.MouseClicked += OnHookEvent;
            hook.MousePressed += OnHookEvent;
            hook.MouseReleased += OnHookEvent;
            hook.MouseMoved += OnHookEvent;
            hook.MouseDragged += OnHookEvent;

            hook.MouseWheel += OnHookEvent;

            return hook;
        }

        private static void OnHookEvent(object? sender, HookEventArgs e) =>
            Console.WriteLine(e.RawEvent);

        private static void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
        {
            if (e.Data.KeyCode == KeyCode.VcQ && sender is IGlobalHook hook)
            {
                hook.Dispose();
            }
        }
    }
}
