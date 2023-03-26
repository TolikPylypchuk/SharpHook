namespace SharpHook.Sample;

using System.Reactive.Linq;

using SharpHook.Logging;
using SharpHook.Native;
using SharpHook.Reactive;
using SharpHook.Reactive.Logging;

public static class Program
{
    private static readonly IEventSimulator Simulator = new EventSimulator();

    private static void Main()
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        Console.WriteLine("---------- SharpHook Sample ----------\n");

        using var logSource = LogSource.Register(minLevel: LogLevel.Debug);
        using var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);

        reactiveLogSource.MessageLogged.Subscribe(OnMessageLogged);

        PrintSystemInfo();

        logSource.MinLevel = LogLevel.Info;

        Console.WriteLine("---------- Press q to quit ----------\n");

        Task.Run(async () =>
        {
            await Task.Delay(500);
            await SimulateInputEvents();
        });

        var hook = CreateHook();
        hook.Run();
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

        hook.HookEnabled.Subscribe(OnHookEvent);
        hook.HookDisabled.Subscribe(OnHookEvent);

        hook.KeyTyped.Subscribe(OnHookEvent);
        hook.KeyPressed.Subscribe(OnHookEvent);
        hook.KeyReleased.Subscribe(OnHookEvent);
        hook.KeyReleased.Subscribe(e => OnKeyReleased(e, hook));

        hook.MouseClicked.Subscribe(OnHookEvent);
        hook.MousePressed.Subscribe(OnHookEvent);
        hook.MouseReleased.Subscribe(OnHookEvent);

        hook.MouseMoved
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(OnHookEvent);

        hook.MouseDragged
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(OnHookEvent);

        hook.MouseWheel.Subscribe(OnHookEvent);

        return hook;
    }

    private static async Task SimulateInputEvents()
    {
        Simulator.SimulateMousePress(MouseButton.Button1);
        await Task.Delay(50);

        Simulator.SimulateMouseRelease(MouseButton.Button1);
        await Task.Delay(50);

        Simulator.SimulateKeyPress(KeyCode.VcA);
        await Task.Delay(50);

        Simulator.SimulateKeyRelease(KeyCode.VcA);
        await Task.Delay(50);

        Simulator.SimulateMouseWheel(10, -1);
    }

    private static void OnHookEvent(HookEventArgs e) =>
        Console.WriteLine($"{e.EventTime.ToLocalTime()}: {e.RawEvent}");

    private static void OnMessageLogged(LogEntry logEntry) =>
        Console.WriteLine($"{Enum.GetName(logEntry.Level)?.ToUpper()}: {logEntry.FullText}");

    private static void OnKeyReleased(KeyboardHookEventArgs e, IReactiveGlobalHook hook)
    {
        if (e.Data.KeyCode == KeyCode.VcQ)
        {
            hook.Dispose();
        }
    }
}
