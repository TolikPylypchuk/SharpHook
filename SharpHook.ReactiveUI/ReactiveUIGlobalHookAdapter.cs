using System.Runtime.CompilerServices;

namespace SharpHook.ReactiveUI;

/// <summary>
/// Adapts an <see cref="IGlobalHook" /> to the <see cref="IReactiveGlobalHook" /> interface.
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="IReactiveGlobalHook" />
public sealed class ReactiveUIGlobalHookAdapter : IGlobalHook, IReactiveGlobalHook
{
    private readonly IGlobalHook hook;

    private readonly Signal<HookEventArgs> hookEnabledSignal = new();
    private readonly Signal<HookEventArgs> hookDisabledSignal = new();

    private readonly Signal<KeyboardHookEventArgs> keyTypedSignal = new();
    private readonly Signal<KeyboardHookEventArgs> keyPressedSignal = new();
    private readonly Signal<KeyboardHookEventArgs> keyReleasedSignal = new();

    private readonly Signal<MouseHookEventArgs> mouseClickedSignal = new();
    private readonly Signal<MouseHookEventArgs> mousePressedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseReleasedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseMovedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseDraggedSignal = new();

    private readonly Signal<MouseWheelHookEventArgs> mouseWheelSignal = new();

    private readonly MultipleDisposable subscriptions = [];

    /// <summary>
    /// Initializes a new instance of <see cref="ReactiveUIGlobalHookAdapter" />.
    /// </summary>
    /// <param name="hook">The hook to adapt.</param>
    /// <param name="defaultSequencer">
    /// The default sequencer for observables, or <see langword="null" /> to use the default one
    /// (<see cref="Sequencer.Immediate" />).
    /// </param>
    public ReactiveUIGlobalHookAdapter(IGlobalHook hook, ISequencer? defaultSequencer = null)
    {
        this.hook = hook ?? throw new ArgumentNullException(nameof(hook));

        defaultSequencer ??= Sequencer.Immediate;

        Signal.FromEventPattern<HookEventArgs>(
            h => this.hook.HookEnabled += h, h => this.hook.HookEnabled -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.hookEnabledSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<HookEventArgs>(
            h => this.hook.HookDisabled += h, h => this.hook.HookDisabled -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.hookDisabledSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyTyped += h, h => this.hook.KeyTyped -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyTypedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyPressed += h, h => this.hook.KeyPressed -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyPressedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyReleased += h, h => this.hook.KeyReleased -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyReleasedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseClicked += h, h => this.hook.MouseClicked -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseClickedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MousePressed += h, h => this.hook.MousePressed -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mousePressedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseReleased += h, h => this.hook.MouseReleased -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseReleasedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseMoved += h, h => this.hook.MouseMoved -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseMovedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseDragged += h, h => this.hook.MouseDragged -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseDraggedSignal)
            .DisposeWith(this.subscriptions);

        Signal.FromEventPattern<MouseWheelHookEventArgs>(
            h => this.hook.MouseWheel += h, h => this.hook.MouseWheel -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseWheelSignal)
            .DisposeWith(this.subscriptions);

        this.HookEnabled = this.hookEnabledSignal.ObserveOn(defaultSequencer);
        this.HookDisabled = this.hookDisabledSignal.ObserveOn(defaultSequencer);

        this.KeyTyped = this.keyTypedSignal.ObserveOn(defaultSequencer);
        this.KeyPressed = this.keyPressedSignal.ObserveOn(defaultSequencer);
        this.KeyReleased = this.keyReleasedSignal.ObserveOn(defaultSequencer);

        this.MouseClicked = this.mouseClickedSignal.ObserveOn(defaultSequencer);
        this.MousePressed = this.mousePressedSignal.ObserveOn(defaultSequencer);
        this.MouseReleased = this.mouseReleasedSignal.ObserveOn(defaultSequencer);
        this.MouseMoved = this.mouseMovedSignal.ObserveOn(defaultSequencer);
        this.MouseDragged = this.mouseDraggedSignal.ObserveOn(defaultSequencer);

        this.MouseWheel = this.mouseWheelSignal.ObserveOn(defaultSequencer);
    }

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning => this.hook.IsRunning;

    /// <summary>
    /// Gets the value which indicates whether the global hook is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>A disposed global hook cannot be started again.</remarks>
    public bool IsDisposed => this.hook.IsDisposed;

    /// <summary>
    /// Gets an observable which emits a value when the global hook is enabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is enabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IBasicGlobalHook.Run" /> or
    /// <see cref="IBasicGlobalHook.RunAsync" /> method is called.
    /// </remarks>
    public IObservable<HookEventArgs> HookEnabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is disabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is disabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IBasicGlobalHook.Stop" /> or <see cref="IDisposable.Dispose" />
    /// method is called.
    /// </remarks>
    public IObservable<HookEventArgs> HookDisabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is typed.
    /// </summary>
    /// <value>An observable which emits a value when a key is typed.</value>
    public IObservable<KeyboardHookEventArgs> KeyTyped { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a key is pressed.</value>
    public IObservable<KeyboardHookEventArgs> KeyPressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is released.
    /// </summary>
    /// <value>An observable which emits a value when a key is released.</value>
    public IObservable<KeyboardHookEventArgs> KeyReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is clicked.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is clicked.</value>
    public IObservable<MouseHookEventArgs> MouseClicked { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is pressed.</value>
    public IObservable<MouseHookEventArgs> MousePressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is released.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is released.</value>
    public IObservable<MouseHookEventArgs> MouseReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is moved.</value>
    public IObservable<MouseHookEventArgs> MouseMoved { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is dragged.</value>
    public IObservable<MouseHookEventArgs> MouseDragged { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse wheel is scrolled.
    /// </summary>
    /// <value>An observable which emits a value when the mouse wheel is scrolled.</value>
    public IObservable<MouseWheelHookEventArgs> MouseWheel { get; }

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be stopped by calling the
    /// <see cref="Stop" /> or the <see cref="IDisposable.Dispose" /> methods.
    /// </summary>
    /// <param name="globalHookType">The type of the global hook to run.</param>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public void Run(GlobalHookType globalHookType = GlobalHookType.All)
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        this.hook.Run(globalHookType);
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be stopped by calling the
    /// <see cref="Stop" /> or the <see cref="IDisposable.Dispose" /> methods.
    /// </summary>
    /// <param name="globalHookType">The type of the global hook to run.</param>
    /// <param name="useBackgroundThread">A value which indicates whether to use a background thread.</param>
    /// <returns>A task which is completed when the hook is stopped.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// The hook is started on a separate thread.
    /// </remarks>
    public Task RunAsync(GlobalHookType globalHookType = GlobalHookType.All, bool useBackgroundThread = false)
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        return this.hook.RunAsync(globalHookType, useBackgroundThread);
    }

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// After stopping, the global hook can run again.
    /// </remarks>
    public void Stop()
    {
        this.ThrowIfDisposed();
        this.hook.Stop();
    }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running.
    /// </summary>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    /// <remarks>
    /// After calling this method, the hook cannot run again. If you want to stop the global hook with the ability to
    /// run it again, call the <see cref="Stop" /> method instead.
    /// </remarks>
    public void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        bool isRunning = this.IsRunning;

        if (isRunning)
        {
            this.hookDisabledSignal.Subscribe(_ => this.CompleteAllSignalsAndDisposeSubscriptions());
        }

        this.hook.Dispose();

        if (!isRunning)
        {
            this.CompleteAllSignalsAndDisposeSubscriptions();
        }
    }

    private void CompleteAllSignalsAndDisposeSubscriptions()
    {
        this.CompleteAllSignals();
        this.subscriptions.Dispose();
    }

    private void CompleteAllSignals()
    {
        this.hookEnabledSignal.OnCompleted();
        this.hookDisabledSignal.OnCompleted();

        this.keyTypedSignal.OnCompleted();
        this.keyPressedSignal.OnCompleted();
        this.keyReleasedSignal.OnCompleted();

        this.mouseClickedSignal.OnCompleted();
        this.mousePressedSignal.OnCompleted();
        this.mouseReleasedSignal.OnCompleted();
        this.mouseMovedSignal.OnCompleted();
        this.mouseDraggedSignal.OnCompleted();

        this.mouseWheelSignal.OnCompleted();
    }

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            throw new InvalidOperationException("The global hook is already running");
        }
    }

    private void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.IsDisposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} – the object is disposed");
        }
    }

    event EventHandler<HookEventArgs> IGlobalHook.HookEnabled
    {
        add => this.hook.HookEnabled += value;
        remove => this.hook.HookEnabled -= value;
    }

    event EventHandler<HookEventArgs> IGlobalHook.HookDisabled
    {
        add => this.hook.HookDisabled += value;
        remove => this.hook.HookDisabled -= value;
    }

    event EventHandler<KeyboardHookEventArgs> IGlobalHook.KeyTyped
    {
        add => this.hook.KeyTyped += value;
        remove => this.hook.KeyTyped -= value;
    }

    event EventHandler<KeyboardHookEventArgs> IGlobalHook.KeyPressed
    {
        add => this.hook.KeyPressed += value;
        remove => this.hook.KeyPressed -= value;
    }

    event EventHandler<KeyboardHookEventArgs> IGlobalHook.KeyReleased
    {
        add => this.hook.KeyReleased += value;
        remove => this.hook.KeyReleased -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseClicked
    {
        add => this.hook.MouseClicked += value;
        remove => this.hook.MouseClicked -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MousePressed
    {
        add => this.hook.MousePressed += value;
        remove => this.hook.MousePressed -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseReleased
    {
        add => this.hook.MouseReleased += value;
        remove => this.hook.MouseReleased -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseMoved
    {
        add => this.hook.MouseMoved += value;
        remove => this.hook.MouseMoved -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseDragged
    {
        add => this.hook.MouseDragged += value;
        remove => this.hook.MouseDragged -= value;
    }

    event EventHandler<MouseWheelHookEventArgs> IGlobalHook.MouseWheel
    {
        add => this.hook.MouseWheel += value;
        remove => this.hook.MouseWheel -= value;
    }
}
