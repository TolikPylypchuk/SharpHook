namespace SharpHook.Sample;

using System;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;

using SharpHook.Native;
using SharpHook.Reactive;

public static class Program
{
    private static async Task Main()
    {
        Directory.SetCurrentDirectory(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? String.Empty);

        Console.WriteLine("---------- SharpHook Sample ----------\n");

        PrintSystemInfo();

        Console.WriteLine("---------- Press q to quit ----------\n");

        var hook = CreateHook();
        var task = hook.Start();

        await SimulateInputEvents();

        await task;
    }

    private static void PrintSystemInfo()
    {
        Console.WriteLine("System info:");
        Console.WriteLine($"Auto-repeat rate: {UioHook.GetAutoRepeatRate()}");
        Console.WriteLine($"Auto-repeat delay: {UioHook.GetAutoRepeatDelay()}");
        Console.WriteLine($"Pointer acceleration threshold: {UioHook.GetPointerAccelerationThreshold()}");
        Console.WriteLine($"Pointer sensitivity: {UioHook.GetPointerSensitivity()}");
        Console.WriteLine($"Multi-click time: {UioHook.GetMultiClickTime()}");

        var screens = UioHook.CreateScreenInfo();

        foreach (var screen in screens)
        {
            Console.WriteLine($"Screen #{screen.Number}: {screen.Width}x{screen.Height}; {screen.X}, {screen.Y}");
        }

        Console.WriteLine();
    }

    private static IReactiveGlobalHook CreateHook()
    {
        var hook = new SimpleReactiveGlobalHook();

        hook.HookEnabled.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.HookDisabled.Subscribe(e => OnHookEvent(e.Sender, e.Args));

        hook.KeyTyped.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.KeyPressed.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.KeyReleased.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.KeyReleased.Subscribe(e => OnKeyReleased(e.Sender, e.Args));

        hook.MouseClicked.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.MousePressed.Subscribe(e => OnHookEvent(e.Sender, e.Args));
        hook.MouseReleased.Subscribe(e => OnHookEvent(e.Sender, e.Args));

        hook.MouseMoved
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(e => OnHookEvent(e.Sender, e.Args));

        hook.MouseDragged
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(e => OnHookEvent(e.Sender, e.Args));

        hook.MouseWheel.Subscribe(e => OnHookEvent(e.Sender, e.Args));

        return hook;
    }

    private static async Task SimulateInputEvents()
    {
        var simulator = new EventSimulator();

        await FakePressKey(simulator, KeyCode.VcA);
        await FakePressMouseButton(simulator, MouseButton.Button1);

        simulator.SimulateMouseMovement(0, 0);
        simulator.SimulateMouseWheel(0, 0, 10, -1);
    }

    private static void OnHookEvent(object? sender, HookEventArgs e) =>
        Console.WriteLine(e.RawEvent);

    private static void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        if (e.Data.KeyCode == KeyCode.VcQ && sender is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static async Task FakePressKey(IEventSimulator simulator, KeyCode keyCode)
    {
        simulator.SimulateKeyPress(keyCode);
        await Task.Delay(50);

        simulator.SimulateKeyRelease(keyCode);
        await Task.Delay(50);
    }

    private static async Task FakePressMouseButton(IEventSimulator simulator, MouseButton button)
    {
        simulator.SimulateMousePress(button);
        await Task.Delay(50);

        simulator.SimulateMouseRelease(button);
        await Task.Delay(50);
    }
}
