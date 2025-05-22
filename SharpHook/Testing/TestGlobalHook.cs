using System.Collections.Concurrent;

namespace SharpHook.Testing;

/// <summary>
/// A global hook and event simulator which can be used in tests.
/// </summary>
public sealed class TestGlobalHook : IGlobalHook, IEventSimulator
{
#if NET9_0_OR_GREATER
    private readonly Lock syncRoot = new();
#else
    private readonly object syncRoot = new();
#endif

    private TaskCompletionSource<object?>? runCompletionSource;
    private BlockingCollection<UioHookEvent>? eventLoop;

    private readonly List<UioHookEvent> simulatedEvents = [];
    private readonly List<UioHookEvent> suppressedEvents = [];
    private readonly List<string> simulatedText = [];

    private Func<EventType, DateTimeOffset> eventDateTime = t => DateTimeOffset.UtcNow;
    private Func<EventType, EventMask> eventMask = t => Data.EventMask.None;

    private Func<KeyCode, IEnumerable<char>> keyCodeToChars = c => [];
    private Func<KeyCode, ushort> keyCodeToRawCode = c => 1;

    private Func<short> currentMouseX = () => 0;
    private Func<short> currentMouseY = () => 0;

    /// <summary>
    /// Initializes a new instance of <see cref="TestGlobalHook" />.
    /// </summary>
    public TestGlobalHook()
        : this(TestThreadingMode.Simple)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="TestGlobalHook" />.
    /// </summary>
    /// <param name="threadingMode">The threading mode to use.</param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public TestGlobalHook(TestThreadingMode threadingMode) =>
        this.ThreadingMode = threadingMode;

    /// <summary>
    /// Gets the threading mode of this global hook.
    /// </summary>
    public TestThreadingMode ThreadingMode { get; }

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets the value which indicates whether the global hook is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>A disposed global hook cannot be started again.</remarks>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Gets the events that have been simulated using this instance.
    /// </summary>
    public IReadOnlyList<UioHookEvent> SimulatedEvents =>
        this.simulatedEvents.AsReadOnly();

    /// <summary>
    /// Gets the events that have been suppressed in the event handlers.
    /// </summary>
    public IReadOnlyList<UioHookEvent> SuppressedEvents =>
        this.suppressedEvents.AsReadOnly();

    /// <summary>
    /// Gets the text that has been simulated using this instance.
    /// </summary>
    public IReadOnlyList<string> SimulatedText =>
        this.simulatedText.AsReadOnly();

    /// <summary>
    /// Gets or sets the function which will be called to set the date/time of events.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<EventType, DateTimeOffset> EventDateTime
    {
        get => this.eventDateTime;
        set => this.eventDateTime = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will be called to set the mask of events.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<EventType, EventMask> EventMask
    {
        get => this.eventMask;
        set => this.eventMask = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will map key codes to characters.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    /// <remarks>By default every key code maps to an empty character sequence.</remarks>
    public Func<KeyCode, IEnumerable<char>> KeyCodeToChars
    {
        get => this.keyCodeToChars;
        set => this.keyCodeToChars = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will map key codes to raw key codes.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<KeyCode, ushort> KeyCodeToRawCode
    {
        get => this.keyCodeToRawCode;
        set => this.keyCodeToRawCode = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will specify the current mouse X-coordinate.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<short> CurrentMouseX
    {
        get => this.currentMouseX;
        set => this.currentMouseX = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the function which will specify the current mouse Y-coordinate.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public Func<short> CurrentMouseY
    {
        get => this.currentMouseY;
        set => this.currentMouseY = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the click count for events of type <see cref="EventType.MousePressed" />,
    /// <see cref="EventType.MouseReleased" />, and <see cref="EventType.MouseClicked" />.
    /// </summary>
    /// <value>
    /// The click count for events of type <see cref="EventType.MousePressed" />,
    /// <see cref="EventType.MouseReleased" />, and <see cref="EventType.MouseClicked" />.
    /// </value>
    public ushort MouseClickCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets the value which specifies whether <see cref="EventType.MouseClicked" /> events should be raised
    /// after <see cref="EventType.MouseReleased" /> events.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if <see cref="EventType.MouseClicked" /> events should be raised.
    /// </value>
    public bool RaiseMouseClicked { get; set; } = true;

    /// <summary>
    /// Gets or sets the value which indicates whether simulating mouse movement will simulate events of type
    /// <see cref="EventType.MouseDragged" />.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if simulating mouse movement should simulate events of type
    /// <see cref="EventType.MouseDragged" />. Otherwise, <see langword="false" />.
    /// </value>
    public bool DragMouseWhenMoving { get; set; } = false;

    /// <summary>
    /// Gets or sets the result of the <see cref="Run()" /> and <see cref="RunAsync" /> methods. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will throw a <see cref="HookException" />.
    /// </summary>
    /// <value>The result of the <see cref="Run()" /> and <see cref="RunAsync" /> methods.</value>
    public UioHookResult RunResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="Stop()" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will throw a <see cref="HookException" />.
    /// </summary>
    /// <value>The result of the <see cref="Stop()" /> method.</value>
    public UioHookResult StopResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="Dispose()" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will throw a <see cref="HookException" />.
    /// </summary>
    /// <value>The result of the <see cref="Dispose()" /> method.</value>
    public UioHookResult DisposeResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateKeyPress(KeyCode)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateKeyPress(KeyCode)" /> method.</value>
    public UioHookResult SimulateKeyPressResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateKeyRelease(KeyCode)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateKeyRelease(KeyCode)" /> method.</value>
    public UioHookResult SimulateKeyReleaseResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateTextEntry(string)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateTextEntry(string)" /> method.</value>
    public UioHookResult SimulateTextEntryResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateMousePress(MouseButton)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateMousePress(MouseButton)" /> method.</value>
    public UioHookResult SimulateMousePressResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateMouseRelease(MouseButton)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateMouseRelease(MouseButton)" /> method.</value>
    public UioHookResult SimulateMouseReleaseResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the <see cref="SimulateMouseMovement(short, short)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateMouseMovement(short, short)" /> method.</value>
    public UioHookResult SimulateMouseMovementResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Gets or sets the result of the
    /// <see cref="SimulateMouseWheel(short, MouseWheelScrollDirection, MouseWheelScrollType)" /> method. If anything
    /// other than <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>
    /// The result of the <see cref="SimulateMouseWheel(short, MouseWheelScrollDirection, MouseWheelScrollType)" />
    /// method.
    /// </value>
    public UioHookResult SimulateMouseWheelResult { get; set; } = UioHookResult.Success;

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be stopped by calling the
    /// <see cref="Stop" /> or <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <exception cref="HookException">
    /// <see cref="RunResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public void Run()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var result = this.RunResult;
        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                this.RunSimple();
                break;
            case TestThreadingMode.EventLoop:
                this.RunWithEventLoop();
                break;
        }
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be stopped by calling the
    /// <see cref="Stop" /> or <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is stopped.</returns>
    /// <exception cref="HookException">
    /// <see cref="RunResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// Depending on the threading mode, this method will either run an event loop or do nothing and wait for the hook
    /// to be stopped.
    /// </remarks>
    public async Task RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var result = this.RunResult;
        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                await this.RunAsyncSimple();
                break;
            case TestThreadingMode.EventLoop:
                await this.RunAsyncWithEventLoop();
                break;
        }
    }

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <exception cref="HookException">
    /// <see cref="StopResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// After calling this method, the hook can be started again.
    /// </remarks>
    public void Stop()
    {
        this.ThrowIfDisposed();

        var result = this.StopResult;

        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        this.StopInternal();
    }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running.
    /// </summary>
    /// <exception cref="HookException">
    /// <see cref="DisposeResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <remarks>
    /// After calling this method, the hook cannot be started again. This method does nothing if the hook has already
    /// been disposed.
    /// </remarks>
    public void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        if (this.IsRunning)
        {
            var result = this.DisposeResult;

            if (result != UioHookResult.Success)
            {
                throw new HookException(result);
            }

            this.StopInternal();
        }

        this.IsDisposed = true;
    }

    /// <summary>
    /// Simulates pressing a key if <see cref="SimulateKeyPressResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The value of <see cref="SimulateKeyPressResult" />.</returns>
    /// <remarks>
    /// <para>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </para>
    /// <para>
    /// This method simulates <see cref="EventType.KeyTyped" /> events as well if <see cref="KeyCodeToChars" /> returns
    /// characters for <paramref name="keyCode" />.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateKeyPress(KeyCode keyCode)
    {
        ushort rawCode = this.keyCodeToRawCode(keyCode);

        var keyPressedEvent = new UioHookEvent
        {
            Type = EventType.KeyPressed,
            Time = (ulong)this.EventDateTime(EventType.KeyPressed).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.KeyPressed),
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = KeyboardEventData.RawUndefinedChar
            }
        };

        var result = this.SimulateEvent(this.SimulateKeyPressResult, ref keyPressedEvent);

        if (result == UioHookResult.Success)
        {
            foreach (var ch in this.keyCodeToChars(keyCode) ?? [])
            {
                var keyTypedEvent = new UioHookEvent
                {
                    Type = EventType.KeyTyped,
                    Time = (ulong)this.EventDateTime(EventType.KeyTyped).ToUnixTimeMilliseconds(),
                    Mask = this.EventMask(EventType.KeyTyped),
                    Keyboard = new KeyboardEventData
                    {
                        KeyCode = keyCode,
                        RawCode = rawCode,
                        RawKeyChar = ch
                    }
                };

                this.SimulateEvent(UioHookResult.Success, ref keyTypedEvent);
            }
        }

        return result;
    }

    /// <summary>
    /// Simulates releasing a key if <see cref="SimulateKeyReleaseResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <returns>The value of <see cref="SimulateKeyReleaseResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateKeyRelease(KeyCode keyCode)
    {
        ushort rawCode = this.keyCodeToRawCode(keyCode);

        var keyReleasedEvent = new UioHookEvent
        {
            Type = EventType.KeyReleased,
            Time = (ulong)this.EventDateTime(EventType.KeyReleased).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.KeyReleased),
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = KeyboardEventData.RawUndefinedChar
            }
        };

        return this.SimulateEvent(this.SimulateKeyReleaseResult, ref keyReleasedEvent);
    }

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters if <see cref="SimulateTextEntryResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The value of <see cref="SimulateTextEntryResult" />.</returns>
    /// <remarks>This method doesn't cause any input events to be created, unlike real text entry simulation.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    public UioHookResult SimulateTextEntry(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        var result = this.SimulateTextEntryResult;

        if (result == UioHookResult.Success)
        {
            this.simulatedText.Add(text);
        }

        return result;
    }

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMousePress(MouseButton button) =>
        this.SimulateMousePress(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMousePress(MouseButton button, ushort clicks) =>
        this.SimulateMousePress(this.currentMouseX(), this.currentMouseY(), button, clicks);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button) =>
        this.SimulateMousePress(x, y, button, this.MouseClickCount);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button, ushort clicks)
    {
        var mousePressedEvent = new UioHookEvent
        {
            Type = EventType.MousePressed,
            Time = (ulong)this.EventDateTime(EventType.MousePressed).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.MousePressed),
            Mouse = new MouseEventData
            {
                Button = button,
                X = x,
                Y = y,
                Clicks = clicks
            }
        };

        return this.SimulateEvent(this.SimulateMousePressResult, ref mousePressedEvent);
    }

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// <para>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </para>
    /// <para>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseRelease(MouseButton button) =>
        this.SimulateMouseRelease(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// <para>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </para>
    /// <para>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseRelease(MouseButton button, ushort clicks) =>
        this.SimulateMouseRelease(this.currentMouseX(), this.currentMouseY(), button, clicks);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// <para>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </para>
    /// <para>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button) =>
        this.SimulateMouseRelease(x, y, button, this.MouseClickCount);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// <para>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </para>
    /// <para>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button, ushort clicks)
    {
        var mouseReleasedEvent = new UioHookEvent
        {
            Type = EventType.MouseReleased,
            Time = (ulong)this.EventDateTime(EventType.MouseReleased).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.MouseReleased),
            Mouse = new MouseEventData
            {
                Button = button,
                X = x,
                Y = y,
                Clicks = clicks
            }
        };

        var result = this.SimulateEvent(this.SimulateMouseReleaseResult, ref mouseReleasedEvent);

        if (result == UioHookResult.Success && this.RaiseMouseClicked)
        {
            var mouseClickedEvent = new UioHookEvent
            {
                Type = EventType.MouseClicked,
                Time = (ulong)this.EventDateTime(EventType.MouseClicked).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MouseClicked),
                Mouse = new MouseEventData
                {
                    Button = button,
                    X = x,
                    Y = y,
                    Clicks = clicks
                }
            };

            this.SimulateEvent(UioHookResult.Success, ref mouseClickedEvent);
        }

        return result;
    }

    /// <summary>
    /// Simulates moving a mouse pointer if <see cref="SimulateMouseMovementResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    /// <returns>The value of <see cref="SimulateMouseMovementResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMouseMovement(short x, short y)
    {
        var eventType = this.DragMouseWhenMoving ? EventType.MouseDragged : EventType.MouseMoved;

        var mouseEvent = new UioHookEvent
        {
            Type = eventType,
            Time = (ulong)this.EventDateTime(eventType).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(eventType),
            Mouse = new MouseEventData
            {
                X = x,
                Y = y
            }
        };

        return this.SimulateEvent(this.SimulateMouseMovementResult, ref mouseEvent);
    }

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position if
    /// <see cref="SimulateMouseMovementResult" /> is <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The value of <see cref="SimulateMouseMovementResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMouseMovementRelative(short x, short y) =>
        this.SimulateMouseMovement((short)(this.currentMouseX() + x), (short)(this.currentMouseY() + y));

    /// <summary>
    /// Simulates scrolling the mouse wheel if <see cref="SimulateMouseWheelResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="rotation">
    /// The wheel rotation. A positive value indicates that the wheel will be rotated up or left,
    /// and a negative value indicates that the wheel will be rotated down or right.
    /// </param>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="type">The scroll type.</param>
    /// <returns>The value of <see cref="SimulateMouseWheelResult" />.</returns>
    /// <remarks>
    /// If the hook's threading mode is <see cref="TestThreadingMode.Simple" /> then this method will immediately
    /// dispatch the event. If the threading mode is <see cref="TestThreadingMode.EventLoop" /> then the event will be
    /// posted to an event loop which runs on the same thread on which the testing hook itself runs, and then dispatched
    /// there.
    /// </remarks>
    public UioHookResult SimulateMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll)
    {
        var mouseWheelEvent = new UioHookEvent
        {
            Type = EventType.MouseWheel,
            Time = (ulong)this.EventDateTime(EventType.MouseWheel).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.MouseWheel),
            Wheel = new MouseWheelEventData
            {
                Rotation = rotation,
                Direction = direction,
                Type = type
            }
        };

        return this.SimulateEvent(this.SimulateMouseWheelResult, ref mouseWheelEvent);
    }

    private void RunSimple()
    {
        try
        {
            this.RunAsync().Wait();
        } catch (AggregateException e)
        {
            var exception = e.InnerExceptions.FirstOrDefault();
            if (exception is not null)
            {
                throw exception;
            } else
            {
                throw;
            }
        }
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

            var eventLoopCompletionSource = new TaskCompletionSource<object?>();

            var thread = new Thread(() =>
            {
                this.RunEventLoop();
                eventLoopCompletionSource.SetResult(null);
            });

            thread.Start();

            await eventLoopCompletionSource.Task;

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
            this.DispatchEvent(ref currentEvent);

            if (currentEvent.Mask.HasFlag(Data.EventMask.SuppressEvent))
            {
                this.suppressedEvents.Add(currentEvent);
            }
        }
    }

    private UioHookResult SimulateEvent(UioHookResult result, ref UioHookEvent e)
    {
        if (result != UioHookResult.Success)
        {
            return result;
        }

        switch (this.ThreadingMode)
        {
            case TestThreadingMode.Simple:
                this.DispatchEvent(ref e);

                if (e.Mask.HasFlag(Data.EventMask.SuppressEvent))
                {
                    this.suppressedEvents.Add(e);
                }
                break;
            case TestThreadingMode.EventLoop:
                this.eventLoop?.Add(e);
                break;
        }

        this.simulatedEvents.Add(e);

        return result;
    }

    private void DispatchHookEvent(EventType eventType)
    {
        var hookEvent = new UioHookEvent
        {
            Type = eventType,
            Time = (ulong)this.EventDateTime(eventType).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(eventType)
        };

        this.DispatchEvent(ref hookEvent);
    }

    private void DispatchEvent(ref UioHookEvent e)
    {
        HookEventArgs? args = null;

        switch (e.Type)
        {
            case EventType.HookEnabled:
                this.HookEnabled?.Invoke(this, args = new HookEventArgs(e));
                break;
            case EventType.HookDisabled:
                this.HookDisabled?.Invoke(this, args = new HookEventArgs(e));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(e);
                args = keyTypedArgs;
                this.KeyTyped?.Invoke(this, keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(e);
                args = keyPressedArgs;
                this.KeyPressed?.Invoke(this, keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(e);
                args = keyReleasedArgs;
                this.KeyReleased?.Invoke(this, keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(e);
                args = mouseClickedArgs;
                this.MouseClicked?.Invoke(this, mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(e);
                args = mousePressedArgs;
                this.MousePressed?.Invoke(this, mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(e);
                args = mouseReleasedArgs;
                this.MouseReleased?.Invoke(this, mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(e);
                args = mouseMovedArgs;
                this.MouseMoved?.Invoke(this, mouseMovedArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(e);
                args = mouseDraggedArgs;
                this.MouseDragged?.Invoke(this, mouseDraggedArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(e);
                args = mouseWheelArgs;
                this.MouseWheel?.Invoke(this, mouseWheelArgs);
                break;
        };

        if (args is not null)
        {
            if (args.SuppressEvent)
            {
                e.Mask |= Data.EventMask.SuppressEvent;
            }
        }
    }

    private void StopInternal()
    {
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
    }

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            throw new InvalidOperationException("The global hook is already running");
        }
    }

    private void ThrowIfDisposed()
    {
        if (this.IsDisposed)
        {
            throw new ObjectDisposedException("The global hook is disposed");
        }
    }

    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="Run" /> or <see cref="RunAsync" /> method is called.</remarks>
    public event EventHandler<HookEventArgs>? HookEnabled;

    /// <summary>
    /// An event which is raised when the global hook is disabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IDisposable.Dispose" /> method is called.</remarks>
    public event EventHandler<HookEventArgs>? HookDisabled;

    /// <summary>
    /// An event which is raised when a key is typed.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyReleased;

    /// <summary>
    /// An event which is raised when a mouse button is clicked.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseClicked;

    /// <summary>
    /// An event which is raised when a mouse button is pressed.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MousePressed;

    /// <summary>
    /// An event which is raised when a mouse button is released.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseMoved;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseDragged;

    /// <summary>
    /// An event which is raised when the mouse wheel is scrolled.
    /// </summary>
    public event EventHandler<MouseWheelHookEventArgs>? MouseWheel;

    TimeSpan IEventSimulator.TextSimulationDelayOnX11 { get; set; }
}
