namespace SharpHook.Testing;

using System.Collections.Concurrent;

/// <summary>
/// A global hook and event simulator which can be used in tests.
/// </summary>
public sealed class TestGlobalHook : IGlobalHook, IEventSimulator
{
    private readonly BlockingCollection<UioHookEvent> hookEvents = new();

    private CancellationTokenSource? cancellationTokenSource;
    private TaskCompletionSource<bool>? runCompletionSource;

    private readonly List<HookEventArgs> simulatedEvents = new();
    private readonly List<string> simulatedText = new();

    private Func<EventType, DateTimeOffset> eventDateTime = t => DateTimeOffset.UtcNow;
    private Func<EventType, ModifierMask> eventMask = t => ModifierMask.None;

    private Func<KeyCode, IEnumerable<char>> keyCodeToChars = c => Enumerable.Empty<char>();
    private Func<KeyCode, ushort> keyCodeToRawCode = c => 1;

    private Func<short> currentMouseX = () => 0;
    private Func<short> currentMouseY = () => 0;

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
    public IReadOnlyList<HookEventArgs> SimulatedEvents =>
        this.simulatedEvents.AsReadOnly();

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
    public Func<EventType, ModifierMask> EventMask
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
    /// Runs the global hook on the current thread, blocking it. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <exception cref="HookException">
    /// <see cref="RunResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public void Run()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var result = this.RunResult;

        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        this.cancellationTokenSource = new();
        this.IsRunning = true;

        var hookEnabled = new UioHookEvent
        {
            Type = EventType.HookEnabled,
            Time = (ulong)this.EventDateTime(EventType.HookEnabled).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.HookEnabled),
            Reserved = EventReservedValueMask.None
        };

        this.DispatchEvent(ref hookEnabled);
        this.SetStarted(true);

        while (this.IsRunning)
        {
            try
            {
                var currentEvent = this.hookEvents.Take(cancellationTokenSource.Token);
                this.DispatchEvent(ref currentEvent);
                this.TestEventHandled?.Invoke(this, new TestEventHandledEventArgs(currentEvent, true));
            } catch (OperationCanceledException) { }
        }
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
    /// <exception cref="HookException">
    /// <see cref="RunResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public Task RunAsync()
    {
        this.ThrowIfDisposed();
        this.ThrowIfRunning();

        return Task.Run(this.Run);
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the hook has been started.
    /// </returns>
    /// <exception cref="HookException">
    /// <see cref="RunResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public Task RunAndWaitForStart(CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();
        this.ThrowIfRunning();

        this.runCompletionSource = new();
        cancellationToken.Register(this.runCompletionSource.SetCanceled);

        var completionSource = this.runCompletionSource;

        new Thread(this.Run).Start();

        return completionSource.Task;
    }

    /// <summary>
    /// Destroys the global hook.
    /// </summary>
    /// <exception cref="HookException">
    /// <see cref="DisposeResult" /> was set to something other than <see cref="UioHookResult.Success" />.
    /// </exception>
    /// <remarks>
    /// After calling this method, the hook cannot be started again.
    /// </remarks>
    public void Dispose()
    {
        this.ThrowIfDisposed();

        var result = this.DisposeResult;

        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        var hookDisabled = new UioHookEvent
        {
            Type = EventType.HookDisabled,
            Time = (ulong)this.EventDateTime(EventType.HookDisabled).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(EventType.HookDisabled),
            Reserved = EventReservedValueMask.None
        };

        this.DispatchEvent(ref hookDisabled);

        this.IsRunning = false;

        if (this.cancellationTokenSource is not null)
        {
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource.Dispose();
            this.cancellationTokenSource = null;
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
    /// This method simulates <see cref="EventType.KeyTyped" /> events as well if <see cref="KeyCodeToChars" /> returns
    /// characters for <paramref name="keyCode" />.
    /// </remarks>
    public UioHookResult SimulateKeyPress(KeyCode keyCode)
    {
        ushort rawCode = this.keyCodeToRawCode(keyCode);

        var result = this.SimulateEvent(
            this.SimulateKeyPressResult,
            new UioHookEvent
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
            });

        if (result == UioHookResult.Success)
        {
            foreach (var ch in this.keyCodeToChars(keyCode) ?? Enumerable.Empty<char>())
            {
                var e = new UioHookEvent
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

                this.hookEvents.Add(e);
            }
        }

        return result;
    }

    /// <summary>
    /// Simulates pressing a key if <see cref="SimulateKeyPressResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method simulates <see cref="EventType.KeyTyped" /> events as well if <see cref="KeyCodeToChars" /> returns
    /// characters for <paramref name="keyCode" />.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateKeyPressAndWaitForHandler(
        KeyCode keyCode,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateKeyPress(keyCode),
            e => e.Type == EventType.KeyPressed && e.Keyboard.KeyCode == keyCode,
            cancellationToken);

    /// <summary>
    /// Simulates pressing a key if <see cref="SimulateKeyPressResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <param name="keyChar">The char of the <see cref="EventType.KeyTyped" /> event which should be awaited.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the <see cref="EventType.KeyTyped" /> event has been handled, and
    /// contains the handler result, or <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method should be used only if <see cref="KeyCodeToChars" /> returns <paramref name="keyChar" /> for
    /// <paramref name="keyCode" />, otherwise the resulting task will never finish.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateKeyPressAndWaitForKeyTypedHandler(
        KeyCode keyCode,
        char keyChar,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateKeyPress(keyCode),
            e => e.Type == EventType.KeyTyped && e.Keyboard.KeyChar == keyChar,
            cancellationToken);

    /// <summary>
    /// Simulates releasing a key if <see cref="SimulateKeyReleaseResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <returns>The value of <see cref="SimulateKeyReleaseResult" />.</returns>
    public UioHookResult SimulateKeyRelease(KeyCode keyCode)
    {
        ushort rawCode = this.keyCodeToRawCode(keyCode);

        return this.SimulateEvent(
            this.SimulateKeyReleaseResult,
            new UioHookEvent
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
            });
    }

    /// <summary>
    /// Simulates releasing a key if <see cref="SimulateKeyReleaseResult" /> is <see cref="UioHookResult.Success" />.
    /// Otherwise, does nothing.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateKeyReleaseAndWaitForHandler(
        KeyCode keyCode,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateKeyRelease(keyCode),
            e => e.Type == EventType.KeyReleased && e.Keyboard.KeyCode == keyCode,
            cancellationToken);

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
    public UioHookResult SimulateMousePress(MouseButton button) =>
        this.SimulateMousePress(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateMousePressAndWaitForHandler(
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateMousePressAndWaitForHandler(this.currentMouseX(), this.currentMouseY(), button, cancellationToken);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button) =>
        this.SimulateEvent(
            this.SimulateMousePressResult,
            new UioHookEvent
            {
                Type = EventType.MousePressed,
                Time = (ulong)this.EventDateTime(EventType.MousePressed).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MousePressed),
                Mouse = new MouseEventData
                {
                    Button = button,
                    X = x,
                    Y = y,
                    Clicks = this.MouseClickCount
                }
            });

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateMousePressAndWaitForHandler(
        short x,
        short y,
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateMousePress(x, y, button),
            e => e.Type == EventType.MousePressed && e.Mouse.X == x && e.Mouse.Y == y && e.Mouse.Button == button,
            cancellationToken);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </remarks>
    public UioHookResult SimulateMouseRelease(MouseButton button) =>
        this.SimulateMouseRelease(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateMouseReleaseAndWaitForHandler(
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateMouseReleaseAndWaitForHandler(
            this.currentMouseX(), this.currentMouseY(), button, cancellationToken);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method should be called only when <see cref="RaiseMouseClicked" /> is <see langword="true" />, otherwise
    /// the resulting task will never finish.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateMouseReleaseAndWaitForMouseClickHandler(
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateMouseReleaseAndWaitForMouseClickHandler(
            this.currentMouseX(), this.currentMouseY(), button, cancellationToken);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </remarks>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button)
    {
        ushort clickCount = this.MouseClickCount;

        var result = this.SimulateEvent(
            this.SimulateMouseReleaseResult,
            new UioHookEvent
            {
                Type = EventType.MouseReleased,
                Time = (ulong)this.EventDateTime(EventType.MouseReleased).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MouseReleased),
                Mouse = new MouseEventData
                {
                    Button = button,
                    X = x,
                    Y = y,
                    Clicks = clickCount
                }
            });

        if (result == UioHookResult.Success && this.RaiseMouseClicked)
        {
            var e = new UioHookEvent
            {
                Type = EventType.MouseClicked,
                Time = (ulong)this.EventDateTime(EventType.MouseClicked).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MouseClicked),
                Mouse = new MouseEventData
                {
                    Button = button,
                    X = x,
                    Y = y,
                    Clicks = clickCount
                }
            };

            this.hookEvents.Add(e);
        }

        return result;
    }

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateMouseReleaseAndWaitForHandler(
        short x,
        short y,
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateMouseRelease(x, y, button),
            e => e.Type == EventType.MouseReleased && e.Mouse.X == x && e.Mouse.Y == y && e.Mouse.Button == button,
            cancellationToken);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    /// <remarks>
    /// This method should be called only when <see cref="RaiseMouseClicked" /> is <see langword="true" />, otherwise
    /// the resulting task will never finish.
    /// </remarks>
    public Task<TestEventHandledEventArgs?> SimulateMouseReleaseAndWaitForMouseClickHandler(
        short x,
        short y,
        MouseButton button,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateMouseRelease(x, y, button),
            e => e.Type == EventType.MouseClicked && e.Mouse.X == x && e.Mouse.Y == y && e.Mouse.Button == button,
            cancellationToken);

    /// <summary>
    /// Simulates moving a mouse pointer if <see cref="SimulateMouseMovementResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    /// <returns>The value of <see cref="SimulateMouseMovementResult" />.</returns>
    public UioHookResult SimulateMouseMovement(short x, short y)
    {
        var eventType = this.DragMouseWhenMoving ? EventType.MouseDragged : EventType.MouseMoved;

        return this.SimulateEvent(
                    this.SimulateMouseMovementResult,
                    new UioHookEvent
                    {
                        Type = eventType,
                        Time = (ulong)this.EventDateTime(eventType).ToUnixTimeMilliseconds(),
                        Mask = this.EventMask(eventType),
                        Mouse = new MouseEventData
                        {
                            X = x,
                            Y = y
                        }
                    });
    }

    /// <summary>
    /// Simulates moving a mouse pointer if <see cref="SimulateMouseMovementResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateMouseMovementAndWaitForHandler(
        short x,
        short y,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateMouseMovement(x, y),
            e => e.Type is EventType.MouseMoved or EventType.MouseDragged && e.Mouse.X == x && e.Mouse.Y == y,
            cancellationToken);

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position if
    /// <see cref="SimulateMouseMovementResult" /> is <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The value of <see cref="SimulateMouseMovementResult" />.</returns>
    public UioHookResult SimulateMouseMovementRelative(short x, short y) =>
        this.SimulateMouseMovement((short)(this.currentMouseX() + x), (short)(this.currentMouseY() + y));

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position if
    /// <see cref="SimulateMouseMovementResult" /> is <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateMouseMovementRelativeAndWaitForHandler(
        short x,
        short y,
        CancellationToken cancellationToken = default) =>
        this.SimulateMouseMovementAndWaitForHandler(
            (short)(this.currentMouseX() + x), (short)(this.currentMouseY() + y), cancellationToken);

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
    public UioHookResult SimulateMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll) =>
        this.SimulateEvent(
            this.SimulateMouseWheelResult,
            new UioHookEvent
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
            });

    /// <summary>
    /// Simulates scrolling the mouse wheel if <see cref="SimulateMouseWheelResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="rotation">
    /// The wheel rotation. A positive value indicates that the wheel will be rotated up or right,
    /// and a negative value indicates that the wheel will be rotated down or left.
    /// </param>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="type">The scroll type.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> which finishes when the event has been handled, and contains the handler result, or
    /// <see langword="null" /> if the event was not simulated.
    /// </returns>
    public Task<TestEventHandledEventArgs?> SimulateMouseWheelAndWaitForHandler(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll,
        CancellationToken cancellationToken = default) =>
        this.SimulateEventAndWaitForHandler(
            () => this.SimulateMouseWheel(rotation, direction, type),
            e => e.Type == EventType.MouseWheel &&
                e.Wheel.Rotation == rotation && e.Wheel.Direction == direction && e.Wheel.Type == type,
            cancellationToken);

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
                e.Reserved |= EventReservedValueMask.SuppressEvent;
            }

            this.simulatedEvents.Add(args);
        }
    }

    private void SetStarted(bool started)
    {
        if (this.runCompletionSource is not null)
        {
            this.runCompletionSource.SetResult(started);
            this.runCompletionSource = null;
        }
    }

    private UioHookResult SimulateEvent(UioHookResult result, UioHookEvent e)
    {
        if (result == UioHookResult.Success)
        {
            this.hookEvents.Add(e);
        }

        return result;
    }

    private Task<TestEventHandledEventArgs?> SimulateEventAndWaitForHandler(
        Func<UioHookResult> simulate,
        Func<UioHookEvent, bool> shouldHandle,
        CancellationToken cancellationToken = default)
    {
        var completionSource = new TaskCompletionSource<TestEventHandledEventArgs?>();
        cancellationToken.Register(completionSource.SetCanceled);

        void Handler(object? sender, TestEventHandledEventArgs args)
        {
            if (shouldHandle(args.Event))
            {
                this.TestEventHandled -= Handler;
                completionSource.SetResult(args);
            }
        }

        this.TestEventHandled += Handler;

        var result = simulate();

        if (result != UioHookResult.Success)
        {
            this.TestEventHandled -= Handler;
            return Task.FromResult<TestEventHandledEventArgs?>(null);
        }

        return completionSource.Task;
    }

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            this.SetStarted(false);
            throw new InvalidOperationException("The global hook is already running");
        }
    }

    private void ThrowIfDisposed()
    {
        if (this.IsDisposed)
        {
            this.SetStarted(false);
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

    /// <summary>
    /// An event which is raised when a simulated input event has been handled. The input event's
    /// <see cref="UioHookEvent.Reserved" /> field can be inspected using this event.
    /// </summary>
    public event EventHandler<TestEventHandledEventArgs>? TestEventHandled;

    /// <summary>
    /// Gets or sets the delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </summary>
    /// <returns>The delay (in nanoseconds) between posting individual characters when posting text on Linux.</returns>
    TimeSpan IEventSimulator.TextSimulationDelayOnX11 { get; set; }
}
