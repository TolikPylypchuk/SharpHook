namespace SharpHook.Sample;

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using SharpHook.Logging;
using SharpHook.Native;
using SharpHook.Providers;
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
        var provider = UioHookProvider.Instance;

        Console.WriteLine("System info:");
        Console.WriteLine($"Auto-repeat rate: {provider.GetAutoRepeatRate()}");
        Console.WriteLine($"Auto-repeat delay: {provider.GetAutoRepeatDelay()}");
        Console.WriteLine($"Pointer acceleration multiplier: {provider.GetPointerAccelerationMultiplier()}");
        Console.WriteLine($"Pointer acceleration threshold: {provider.GetPointerAccelerationThreshold()}");
        Console.WriteLine($"Pointer sensitivity: {provider.GetPointerSensitivity()}");
        Console.WriteLine($"Multi-click time: {provider.GetMultiClickTime()}");

        var screens = provider.CreateScreenInfo();

        foreach (var screen in screens)
        {
            Console.WriteLine($"Screen #{screen.Number}: {screen.Width}x{screen.Height}; ({screen.X}, {screen.Y})");
        }

        Console.WriteLine();
    }

    private static IReactiveGlobalHook CreateHook()
    {
        var hook = new SimpleReactiveGlobalHook(TaskPoolScheduler.Default);

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
        Simulator.SimulateMouseMovementRelative(50, 50);
        await Task.Delay(50);

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
