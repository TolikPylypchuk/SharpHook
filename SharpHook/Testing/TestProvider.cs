using System.Collections.Concurrent;

namespace SharpHook.Testing;

/// <summary>
/// A provider of low-level functionality which can be used in tests.
/// </summary>
public sealed class TestProvider :
    IGlobalHookProvider,
    ILoggingProvider,
    IEventSimulationProvider,
    IAccessibilityProvider,
    IScreenInfoProvider,
    IMouseInfoProvider
{
#if NET9_0_OR_GREATER
    private readonly Lock syncRoot = new();
#else
    private readonly object syncRoot = new();
#endif

    private TaskCompletionSource<object?>? runCompletionSource;
    private BlockingCollection<UioHookEvent>? eventLoop;

    private readonly List<UioHookEvent> postedEvents = [];
    private readonly List<UioHookEvent> suppressedEvents = [];
    private readonly List<string> postedText = [];

    private DispatchProc? dispatchProc;
    private nint userData;
    private ScreenData[] screenInfo = [];

    private Func<EventType, DateTimeOffset> eventDateTime = t => DateTimeOffset.UtcNow;
    private Func<EventType, EventMask> eventMask = t => Data.EventMask.None;

    /// <summary>
    /// Initializes a new instance of <see cref="TestProvider" />.
    /// </summary>
    public TestProvider()
        : this(TestThreadingMode.Simple)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="TestProvider" />.
    /// </summary>
    /// <param name="threadingMode">The threading mode to use.</param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public TestProvider(TestThreadingMode threadingMode) =>
        this.ThreadingMode = threadingMode;

    /// <summary>
    /// Gets the threading mode of this provider.
    /// </summary>
    public TestThreadingMode ThreadingMode { get; }

    /// <summary>
    /// Gets the events that have been posted using <see cref="PostEvent(ref UioHookEvent)" />.
    /// </summary>
    public IReadOnlyList<UioHookEvent> PostedEvents =>
        this.postedEvents.AsReadOnly();

    /// <summary>
    /// Gets the events that have been suppressed in the event handlers. />.
    /// </summary>
    public IReadOnlyList<UioHookEvent> SuppressedEvents =>
        this.suppressedEvents.AsReadOnly();

    /// <summary>
    /// Gets the text that has been posted using <see cref="PostText(string)" />.
    /// </summary>
    public IReadOnlyList<string> PostedText =>
        this.postedText.AsReadOnly();

    /// <summary>
    /// Gets the value which indicates whether this provider runs the testing hook.
    /// </summary>
    /// <value><see langword="true" /> if the testing hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets the global hook type of this provider. The provider is set when the <see cref="Run()" />,
    /// <see cref="RunKeyboard" />, or <see cref="RunMouse" /> method is called.
    /// </summary>
    public GlobalHookType GlobalHookType { get; private set; }

    /// <summary>
    /// Gets or sets the function which will be called to set the date/time of the <see cref="EventType.HookEnabled" />
    /// and <see cref="EventType.HookDisabled" /> events.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<EventType, DateTimeOffset> EventDateTime
    {
        get => this.eventDateTime;
        set => this.eventDateTime = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will be called to set the mask of the <see cref="EventType.HookEnabled" />
    /// and <see cref="EventType.HookDisabled" /> events.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<EventType, EventMask> EventMask
    {
        get => this.eventMask;
        set => this.eventMask = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the result of the <see cref="Run()" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the testing hook won't be started.
    /// </summary>
    /// <value>The result of the <see cref="Run()" /> method.</value>
    public UioHookResult RunResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="Stop()" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the testing hook won't be stopped.
    /// </summary>
    /// <value>The result of the <see cref="Stop()" /> method.</value>
    public UioHookResult StopResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="PostEvent(ref UioHookEvent)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the event won't be posted.
    /// </summary>
    /// <value>The result of the <see cref="PostEvent(ref UioHookEvent)" /> method.</value>
    public UioHookResult PostEventResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="PostText(string)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the text won't be posted.
    /// </summary>
    /// <value>The result of the <see cref="PostText(string)" /> method.</value>
    public UioHookResult PostTextResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the information about screens for testing.
    /// </summary>
    /// <returns>The information about screens.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public ScreenData[] ScreenInfo
    {
        get => this.screenInfo;
        set => this.screenInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the auto-repeat rate for testing.
    /// </summary>
    /// <returns>The auto-repeat rate.</returns>
    public int AutoRepeatRate { get; set; }

    /// <summary>
    /// Gets or sets the auto-repeat delay for testing.
    /// </summary>
    /// <returns>The auto-repeat rate.</returns>
    public int AutoRepeatDelay { get; set; }

    /// <summary>
    /// Gets or sets the pointer acceleration multiplier for testing.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
    public int PointerAccelerationMultiplier { get; set; }

    /// <summary>
    /// Gets or sets the pointer acceleration threshold for testing.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
    public int PointerAccelerationThreshold { get; set; }

    /// <summary>
    /// Gets or sets the pointer sensitivity for testing.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
    public int PointerSensitivity { get; set; }

    /// <summary>
    /// Gets or sets the multi-click time for testing.
    /// </summary>
    /// <returns>The multi-click time.</returns>
    public int MultiClickTime { get; set; }

    /// <summary>
    /// Sets the hook callback function for testing.
    /// </summary>
    /// <param name="dispatchProc">
    /// The function to call when an event is raised, or <see langword="null" /> to unset the function.
    /// </param>
    /// <param name="userData">
    /// Custom data to pass to the callback.
    /// </param>
    public void SetDispatchProc(DispatchProc? dispatchProc, nint userData)
    {
        this.dispatchProc = dispatchProc;
        this.userData = userData;
    }

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public UioHookResult Run() =>
        this.Run(GlobalHookType.All);

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing. The hook will react only to keyboard events.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public UioHookResult RunKeyboard() =>
        this.Run(GlobalHookType.Keyboard);

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing. The hook will react only to mouse events.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public UioHookResult RunMouse() =>
        this.Run(GlobalHookType.Mouse);

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public Task<UioHookResult> RunAsync() =>
        this.RunAsync(GlobalHookType.All);

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing. The hook will react only to keyboard events.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public Task<UioHookResult> RunKeyboardAsync() =>
        this.RunAsync(GlobalHookType.Keyboard);

    /// <summary>
    /// Runs the testing hook if <see cref="RunResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing. The hook will react only to mouse events.
    /// </summary>
    /// <returns>The value of <see cref="RunResult" />.</returns>
    /// <exception cref="InvalidOperationException">The provider is already running.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public Task<UioHookResult> RunMouseAsync() =>
        this.RunAsync(GlobalHookType.Mouse);

    /// <summary>
    /// Stops the testing hook if <see cref="StopResult" /> is set to <see cref="UioHookResult.Success" />. Otherwise,
    /// does nothing.
    /// </summary>
    /// <returns>The value of <see cref="StopResult" />.</returns>
    public UioHookResult Stop()
    {
        var result = this.StopResult;

        if (result != UioHookResult.Success)
        {
            return result;
        }

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                this.runCompletionSource?.SetResult(null);
                this.runCompletionSource = null;
                break;
            case TestThreadingMode.EventLoop:
                lock (this.syncRoot)
                {
                    this.eventLoop?.CompleteAdding();
                }
                break;
        }

        return result;
    }

    /// <summary>
    /// Posts an input event if <see cref="PostEventResult" /> is set to <see cref="UioHookResult.Success" /> -
    /// this event will be dispatched if the provider is running. Otherwise, does nothing.
    /// </summary>
    /// <param name="e">The event to post.</param>
    /// <returns>The value of <see cref="PostEventResult" />.</returns>
    /// <remarks>
    /// If the provider's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult PostEvent(ref UioHookEvent e)
    {
        var result = this.PostEventResult;

        if (result != UioHookResult.Success)
        {
            return result;
        }

        if (this.IsRunning && this.ShouldDispatchEvent(e.Type))
        {
            switch (this.ThreadingMode)
            {
                case TestThreadingMode.Simple:
                    this.dispatchProc?.Invoke(ref e, this.userData);

                    if (e.Mask.HasFlag(Data.EventMask.SuppressEvent))
                    {
                        this.suppressedEvents.Add(e);
                    }
                    break;
                case TestThreadingMode.EventLoop:
                    this.eventLoop?.Add(e);
                    break;
            }
        }

        this.postedEvents.Add(e);

        return result;
    }

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters if <see cref="PostTextResult" /> is set to
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>
    /// The value of <see cref="PostTextResult" />, or <see cref="UioHookResult.ErrorPostTextNull" /> if
    /// <paramref name="text" /> is <see langword="null" />.
    /// </returns>
    /// <remarks>This method doesn't cause any input events to be created, unlike real text entry simulation.</remarks>
    public UioHookResult PostText(string text)
    {
        if (text is null)
        {
            return UioHookResult.ErrorPostTextNull;
        }

        var result = this.PostTextResult;

        if (result == UioHookResult.Success)
        {
            this.postedText.Add(text);
        }

        return result;
    }

    /// <summary>
    /// Checks whether access to macOS Accessibility API is enabled for the process.
    /// </summary>
    /// <param name="promptUserIfDisabled">Not used.</param>
    /// <returns>
    /// <see langword="false" /> if at least one of <see cref="RunResult "/>, <see cref="PostEventResult" />, or
    /// <see cref="PostTextResult" /> is configured to return <see cref="UioHookResult.ErrorAxApiDisabled" />.
    /// Otherwise, <see langword="true" />.
    /// </returns>
    /// <remarks>
    /// In general, if the test provider should simulate the macOS Accessibility API being disabled, then all three
    /// properties - <see cref="RunResult "/>, <see cref="PostEventResult" />, and<see cref="PostTextResult" /> - should
    /// be configured to return <see cref="UioHookResult.ErrorAxApiDisabled" />, for consistency with real
    /// Accessibility API checks.
    /// </remarks>
    public bool IsAxApiEnabled(bool promptUserIfDisabled) =>
        this.RunResult == UioHookResult.ErrorAxApiDisabled ||
        this.PostEventResult == UioHookResult.ErrorAxApiDisabled ||
        this.PostTextResult == UioHookResult.ErrorAxApiDisabled;

    private UioHookResult Run(GlobalHookType globalHookType)
    {
        this.ThrowIfRunning();

        var result = this.RunResult;
        if (result != UioHookResult.Success)
        {
            return result;
        }

        this.GlobalHookType = globalHookType;

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                this.RunAsyncSimple().Wait();
                break;
            case TestThreadingMode.EventLoop:
                this.RunWithEventLoop();
                break;
        }

        return result;
    }

    private void RunWithEventLoop()
    {
        try
        {
            this.eventLoop = [];
            this.IsRunning = true;

            this.DispatchHookEvent(EventType.HookEnabled);
            this.RunEventLoop();
            this.DispatchHookEvent(EventType.HookDisabled);
        } finally
        {
            this.IsRunning = false;

            lock (this.syncRoot)
            {
                this.eventLoop?.Dispose();
                this.eventLoop = null;
            }
        }
    }

    private async Task<UioHookResult> RunAsync(GlobalHookType globalHookType)
    {
        this.ThrowIfRunning();

        var result = this.RunResult;
        if (result != UioHookResult.Success)
        {
            return result;
        }

        this.GlobalHookType = globalHookType;

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                await this.RunAsyncSimple();
                break;
            case TestThreadingMode.EventLoop:
                await this.RunAsyncWithEventLoop();
                break;
        }

        return result;
    }

    private async Task RunAsyncSimple()
    {
        this.runCompletionSource = new();
        var task = this.runCompletionSource.Task;

        this.IsRunning = true;
        this.DispatchHookEvent(EventType.HookEnabled);

        await task;

        this.DispatchHookEvent(EventType.HookDisabled);
        this.IsRunning = false;
    }

    private async Task RunAsyncWithEventLoop()
    {
        try
        {
            this.eventLoop = [];
            this.IsRunning = true;

            this.DispatchHookEvent(EventType.HookEnabled);

            var runCompletionSource = new TaskCompletionSource<object?>();

            var thread = new Thread(() =>
            {
                this.RunEventLoop();
                runCompletionSource.SetResult(null);
            });

            thread.Start();

            await runCompletionSource.Task;

            this.DispatchHookEvent(EventType.HookDisabled);
        } finally
        {
            this.IsRunning = false;

            lock (this.syncRoot)
            {
                this.eventLoop?.Dispose();
                this.eventLoop = null;
            }
        }
    }

    private void RunEventLoop()
    {
        if (this.eventLoop is null)
        {
            return;
        }

        foreach (var @event in this.eventLoop.GetConsumingEnumerable())
        {
            var currentEvent = @event;
            this.dispatchProc?.Invoke(ref currentEvent, this.userData);

            if (currentEvent.Mask.HasFlag(Data.EventMask.SuppressEvent))
            {
                this.suppressedEvents.Add(currentEvent);
            }
        }
    }

    private bool ShouldDispatchEvent(EventType eventType) =>
        eventType switch
        {
            EventType.HookEnabled or EventType.HookDisabled => true,
            EventType.KeyPressed or EventType.KeyReleased or EventType.KeyTyped =>
                this.GlobalHookType != GlobalHookType.Mouse,
            _ => this.GlobalHookType != GlobalHookType.Keyboard
        };

    private void DispatchHookEvent(EventType eventType)
    {
        var hookEvent = new UioHookEvent
        {
            Type = eventType,
            Time = (ulong)this.EventDateTime(eventType).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(eventType)
        };

        this.dispatchProc?.Invoke(ref hookEvent, this.userData);
    }

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            throw new InvalidOperationException("The provider is already running");
        }
    }

    ulong IEventSimulationProvider.PostTextDelayX11 { get; set; }

    bool IAccessibilityProvider.PromptUserIfAxApiDisabled { get; set; } = true;

    uint IAccessibilityProvider.AxPollFrequency { get; set; } = 1;

    void ILoggingProvider.SetLoggerProc(LoggerProc? loggerProc, nint userData)
    { }

    ScreenData[] IScreenInfoProvider.CreateScreenInfo() =>
        this.ScreenInfo;

    int IMouseInfoProvider.GetAutoRepeatRate() =>
        this.AutoRepeatRate;

    int IMouseInfoProvider.GetAutoRepeatDelay() =>
        this.AutoRepeatDelay;

    int IMouseInfoProvider.GetPointerAccelerationMultiplier() =>
        this.PointerAccelerationMultiplier;

    int IMouseInfoProvider.GetPointerAccelerationThreshold() =>
        this.PointerAccelerationThreshold;

    int IMouseInfoProvider.GetPointerSensitivity() =>
        this.PointerSensitivity;

    int IMouseInfoProvider.GetMultiClickTime() =>
        this.MultiClickTime;
}
