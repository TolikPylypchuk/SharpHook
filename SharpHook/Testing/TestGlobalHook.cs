namespace SharpHook.Testing;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SharpHook.Native;

/// <summary>
/// A global hook and event simulator which can be used in tests.
/// </summary>
public sealed class TestGlobalHook : IGlobalHook, IEventSimulator
{
    private static readonly ushort UndefinedChar = 0xFF;

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
    /// Gets or sets the result of the <see cref="SimulateMouseWheel(ushort, short)" /> method. If anything other than
    /// <see cref="UioHookResult.Success" /> is set, then the method will do nothing.
    /// </summary>
    /// <value>The result of the <see cref="SimulateMouseWheel(ushort, short)" /> method.</value>
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

        var type = EventType.HookEnabled;
        var hookEnabled = new UioHookEvent
        {
            Type = type,
            Time = (ulong)this.EventDateTime(type).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(type),
            Reserved = EventReservedValueMask.None
        };

        this.DispatchEvent(new HookEventArgs(hookEnabled));
        this.SetStarted(true);

        while (this.IsRunning)
        {
            try
            {
                var currentEvent = this.hookEvents.Take(cancellationTokenSource.Token);

                var eventArgs = HookEventArgs.FromEvent(currentEvent);
                this.DispatchEvent(eventArgs);

                if (eventArgs.SuppressEvent)
                {
                    currentEvent.Reserved |= EventReservedValueMask.SuppressEvent;
                }

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
        var result = this.DisposeResult;

        if (result != UioHookResult.Success)
        {
            throw new HookException(result);
        }

        var type = EventType.HookDisabled;
        var hookDisabled = new UioHookEvent
        {
            Type = type,
            Time = (ulong)this.EventDateTime(type).ToUnixTimeMilliseconds(),
            Mask = this.EventMask(type),
            Reserved = EventReservedValueMask.None
        };

        this.DispatchEvent(new HookEventArgs(hookDisabled));

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
                    RawKeyChar = UndefinedChar
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

                this.DispatchEvent(HookEventArgs.FromEvent(e));
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
    public UioHookResult SimulateKeyRelease(KeyCode keyCode) =>
        this.SimulateEvent(
            this.SimulateKeyReleaseResult,
            new UioHookEvent
            {
                Type = EventType.KeyReleased,
                Time = (ulong)this.EventDateTime(EventType.KeyReleased).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.KeyReleased),
                Keyboard = new KeyboardEventData
                {
                    KeyCode = keyCode,
                    RawCode = 1,
                    RawKeyChar = UndefinedChar
                }
            });

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters if <see cref="SimulateTextEntryResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The value of <see cref="SimulateTextEntryResult" />.</returns>
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
                    Y = y
                }
            });

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    public UioHookResult SimulateMouseRelease(MouseButton button) =>
        this.SimulateMouseRelease(this.currentMouseX(), this.currentMouseY(), button);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates if <see cref="SimulateMouseReleaseResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The value of <see cref="SimulateMouseReleaseResult" />.</returns>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button) =>
        this.SimulateEvent(
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
                    Y = y
                }
            });

    /// <summary>
    /// Simulates moving a mouse pointer if <see cref="SimulateMouseMovementResult" /> is
    /// <see cref="UioHookResult.Success" />. Otherwise, does nothing.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    /// <returns>The value of <see cref="SimulateMouseMovementResult" />.</returns>
    public UioHookResult SimulateMouseMovement(short x, short y) =>
        this.SimulateEvent(
            this.SimulateMouseMovementResult,
            new UioHookEvent
            {
                Type = EventType.MouseMoved,
                Time = (ulong)this.EventDateTime(EventType.MouseMoved).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MouseMoved),
                Mouse = new MouseEventData
                {
                    X = x,
                    Y = y
                }
            });

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
    /// <param name="delta">The scroll delta.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <returns>The value of <see cref="SimulateMouseWheelResult" />.</returns>
    /// <remarks>
    /// A positive <paramref name="rotation" /> value indicates that the wheel will be rotated down and a negative value
    /// indicates that the wheel will be rotated up.
    /// </remarks>
    public UioHookResult SimulateMouseWheel(ushort delta, short rotation) =>
        this.SimulateEvent(
            this.SimulateMouseWheelResult,
            new UioHookEvent
            {
                Type = EventType.MouseWheel,
                Time = (ulong)this.EventDateTime(EventType.MouseWheel).ToUnixTimeMilliseconds(),
                Mask = this.EventMask(EventType.MouseWheel),
                Wheel = new MouseWheelEventData
                {
                    X = this.currentMouseX(),
                    Y = this.currentMouseY(),
                    Delta = delta,
                    Rotation = rotation
                }
            });

    private void DispatchEvent(HookEventArgs eventArgs)
    {
        switch (eventArgs)
        {
            case { RawEvent.Type: EventType.HookEnabled }:
                this.HookEnabled?.Invoke(this, eventArgs);
                break;
            case { RawEvent.Type: EventType.HookDisabled }:
                this.HookDisabled?.Invoke(this, eventArgs);
                break;
            case KeyboardHookEventArgs e and { RawEvent.Type: EventType.KeyPressed }:
                this.KeyPressed?.Invoke(this, e);
                break;
            case KeyboardHookEventArgs e and { RawEvent.Type: EventType.KeyReleased }:
                this.KeyReleased?.Invoke(this, e);
                break;
            case KeyboardHookEventArgs e and { RawEvent.Type: EventType.KeyTyped }:
                this.KeyTyped?.Invoke(this, e);
                break;
            case MouseHookEventArgs e and { RawEvent.Type: EventType.MousePressed }:
                this.MousePressed?.Invoke(this, e);
                break;
            case MouseHookEventArgs e and { RawEvent.Type: EventType.MouseReleased }:
                this.MouseReleased?.Invoke(this, e);
                break;
            case MouseHookEventArgs e and { RawEvent.Type: EventType.MouseClicked }:
                this.MouseClicked?.Invoke(this, e);
                break;
            case MouseHookEventArgs e and { RawEvent.Type: EventType.MouseMoved }:
                this.MouseMoved?.Invoke(this, e);
                break;
            case MouseHookEventArgs e and { RawEvent.Type: EventType.MouseDragged }:
                this.MouseDragged?.Invoke(this, e);
                break;
            case MouseWheelHookEventArgs e and { RawEvent.Type: EventType.MouseWheel }:
                this.MouseWheel?.Invoke(this, e);
                break;
        }

        this.simulatedEvents.Add(eventArgs);
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
            this.DispatchEvent(HookEventArgs.FromEvent(e));
        }

        return result;
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
