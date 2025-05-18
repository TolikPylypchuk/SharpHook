Directory.SetCurrentDirectory(AppContext.BaseDirectory);

Console.WriteLine("---------- SharpHook Sample ----------\n");

using var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Debug);
using var reactiveLogSource = new ReactiveLogSourceAdapter(logSource, TaskPoolScheduler.Default);

reactiveLogSource.MessageLogged.Subscribe(OnMessageLogged);

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

logSource.MinLevel = LogLevel.Info;

Console.WriteLine("---------- Press q to quit and w to stop the hook for 5 seconds ----------\n");

var hook = new SimpleReactiveGlobalHook(defaultScheduler: TaskPoolScheduler.Default);

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

while (!hook.IsDisposed)
{
    await hook.RunAsync();

    if (!hook.IsDisposed)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}

static void OnHookEvent(HookEventArgs e) =>
    Console.WriteLine($"{e.EventTime.ToLocalTime()}: {e.RawEvent}");

static void OnMessageLogged(LogEntry logEntry) =>
    Console.WriteLine($"{Enum.GetName(logEntry.Level)?.ToUpper()}: {logEntry.FullText}");

static void OnKeyReleased(KeyboardHookEventArgs e, IReactiveGlobalHook hook)
{
    if (e.Data.KeyCode == KeyCode.VcQ)
    {
        hook.Dispose();
    } else if (e.Data.KeyCode == KeyCode.VcW)
    {
        hook.Stop();
    }
}
