namespace SharpHook.Testing;

/// <summary>
/// A global hook and event simulator which can be used in tests.
/// </summary>
public sealed class TestGlobalHook : IGlobalHook, IEventSimulator
{
    private TaskCompletionSource<object?> runCompletionSource = new();

    private readonly List<UioHookEvent> simulatedEvents = [];
    private readonly List<string> simulatedText = [];

    private Func<EventType, DateTimeOffset> eventDateTime = t => DateTimeOffset.UtcNow;
    private Func<EventType, ModifierMask> eventMask = t => ModifierMask.None;

    private Func<KeyCode, IEnumerable<char>> keyCodeToChars = c => [];
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
    public IReadOnlyList<UioHookEvent> SimulatedEvents =>
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
    public async Task RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var result = this.RunResult;

        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        var source = this.runCompletionSource;

        this.IsRunning = true;
        this.DispatchHookEvent(EventType.HookEnabled);

        await source.Task;

        this.DispatchHookEvent(EventType.HookDisabled);
        this.IsRunning = false;
    }

    /// <summary>
    /// Destroys the global hook.
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

            this.runCompletionSource.SetResult(null);
            this.runCompletionSource = new();
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

                this.DispatchEvent(ref keyTypedEvent);
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
    public UioHookResult SimulateMousePress(MouseButton button) =>
        this.SimulateMousePress(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates if <see cref="SimulateMousePressResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMousePressResult" />.</returns>
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
    /// <param name="clicks">The click count.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    /// <remarks>
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
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
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
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
    /// This method simulates a <see cref="EventType.MouseClicked" /> event as well if <see cref="RaiseMouseClicked" />
    /// is <see langword="true" />.
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

            this.DispatchEvent(ref mouseClickedEvent);
        }

        return result;
    }

    private void DispatchHookEvent(EventType eventType)
    {
        var hookEvent = new UioHookEvent
        {
            Type = eventType,
            Time = (ulong)this.EventDateTime(eventType).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(eventType),
            Reserved = EventReservedValueMask.None
        };

        this.DispatchEvent(ref hookEvent, addToSimulatedEvents: false);
    }

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

    private void DispatchEvent(ref UioHookEvent e, bool addToSimulatedEvents = true)
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

            if (addToSimulatedEvents)
            {
                this.simulatedEvents.Add(e);
            }
        }
    }

    private UioHookResult SimulateEvent(UioHookResult result, ref UioHookEvent e)
    {
        if (result == UioHookResult.Success)
        {
            this.DispatchEvent(ref e);
        }

        return result;
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
