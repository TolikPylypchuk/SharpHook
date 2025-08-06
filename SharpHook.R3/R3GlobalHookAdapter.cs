using System.Runtime.CompilerServices;

namespace SharpHook.R3;

/// <summary>
/// Adapts an <see cref="IGlobalHook" /> to the <see cref="IR3GlobalHook" /> interface.
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="IR3GlobalHook" />
public sealed class R3GlobalHookAdapter : IGlobalHook, IR3GlobalHook
{
    private readonly IGlobalHook hook;

    private readonly Subject<HookEventArgs> hookEnabledSubject = new();
    private readonly Subject<HookEventArgs> hookDisabledSubject = new();

    private readonly Subject<KeyboardHookEventArgs> keyTypedSubject = new();
    private readonly Subject<KeyboardHookEventArgs> keyPressedSubject = new();
    private readonly Subject<KeyboardHookEventArgs> keyReleasedSubject = new();

    private readonly Subject<MouseHookEventArgs> mouseClickedSubject = new();
    private readonly Subject<MouseHookEventArgs> mousePressedSubject = new();
    private readonly Subject<MouseHookEventArgs> mouseReleasedSubject = new();
    private readonly Subject<MouseHookEventArgs> mouseMovedSubject = new();
    private readonly Subject<MouseHookEventArgs> mouseDraggedSubject = new();

    private readonly Subject<MouseWheelHookEventArgs> mouseWheelSubject = new();

    private readonly IDisposable subscriptions;

    /// <summary>
    /// Initializes a new instance of <see cref="R3GlobalHookAdapter" />.
    /// </summary>
    /// <param name="hook">The hook to adapt.</param>
    /// <param name="defaultTimeProvider">
    /// The default time provider for observables, or <see langword="null" /> to use the default one
    /// (as defined in <see cref="ObservableSystem.DefaultTimeProvider" />).
    /// </param>
    public R3GlobalHookAdapter(IGlobalHook hook, TimeProvider? defaultTimeProvider = null)
    {
        this.hook = hook ?? throw new ArgumentNullException(nameof(hook));

        defaultTimeProvider ??= ObservableSystem.DefaultTimeProvider;

        var subscriptions = Disposable.CreateBuilder();

        subscriptions.Add(
            Observable.FromEventHandler<HookEventArgs>(
                h => this.hook.HookEnabled += h, h => this.hook.HookEnabled -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.hookEnabledSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<HookEventArgs>(
                h => this.hook.HookDisabled += h, h => this.hook.HookDisabled -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.hookDisabledSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<KeyboardHookEventArgs>(
                h => this.hook.KeyTyped += h, h => this.hook.KeyTyped -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.keyTypedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<KeyboardHookEventArgs>(
                h => this.hook.KeyPressed += h, h => this.hook.KeyPressed -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.keyPressedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<KeyboardHookEventArgs>(
                h => this.hook.KeyReleased += h, h => this.hook.KeyReleased -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.keyReleasedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseHookEventArgs>(
                h => this.hook.MouseClicked += h, h => this.hook.MouseClicked -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mouseClickedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseHookEventArgs>(
                h => this.hook.MousePressed += h, h => this.hook.MousePressed -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mousePressedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseHookEventArgs>(
                h => this.hook.MouseReleased += h, h => this.hook.MouseReleased -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mouseReleasedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseHookEventArgs>(
                h => this.hook.MouseMoved += h, h => this.hook.MouseMoved -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mouseMovedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseHookEventArgs>(
                h => this.hook.MouseDragged += h, h => this.hook.MouseDragged -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mouseDraggedSubject.AsObserver()));

        subscriptions.Add(
            Observable.FromEventHandler<MouseWheelHookEventArgs>(
                h => this.hook.MouseWheel += h, h => this.hook.MouseWheel -= h)
                .Select(this.SelectEventArgs)
                .Subscribe(this.mouseWheelSubject.AsObserver()));

        this.subscriptions = subscriptions.Build();

        this.HookEnabled = this.hookEnabledSubject.ObserveOn(defaultTimeProvider);
        this.HookDisabled = this.hookDisabledSubject.ObserveOn(defaultTimeProvider);

        this.KeyTyped = this.keyTypedSubject.ObserveOn(defaultTimeProvider);
        this.KeyPressed = this.keyPressedSubject.ObserveOn(defaultTimeProvider);
        this.KeyReleased = this.keyReleasedSubject.ObserveOn(defaultTimeProvider);

        this.MouseClicked = this.mouseClickedSubject.ObserveOn(defaultTimeProvider);
        this.MousePressed = this.mousePressedSubject.ObserveOn(defaultTimeProvider);
        this.MouseReleased = this.mouseReleasedSubject.ObserveOn(defaultTimeProvider);
        this.MouseMoved = this.mouseMovedSubject.ObserveOn(defaultTimeProvider);
        this.MouseDragged = this.mouseDraggedSubject.ObserveOn(defaultTimeProvider);

        this.MouseWheel = this.mouseWheelSubject.ObserveOn(defaultTimeProvider);
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
    /// The observable emits a value when the <see cref="Run" /> or <see cref="RunAsync" /> method is called and
    /// then immediately completes.
    /// </remarks>
    public Observable<HookEventArgs> HookEnabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is disabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is disabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IDisposable.Dispose" /> method is called and then
    /// immediately completes.
    /// </remarks>
    public Observable<HookEventArgs> HookDisabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is typed.
    /// </summary>
    /// <value>An observable which emits a value when a key is typed.</value>
    public Observable<KeyboardHookEventArgs> KeyTyped { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a key is pressed.</value>
    public Observable<KeyboardHookEventArgs> KeyPressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is released.
    /// </summary>
    /// <value>An observable which emits a value when a key is released.</value>
    public Observable<KeyboardHookEventArgs> KeyReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is clicked.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is clicked.</value>
    public Observable<MouseHookEventArgs> MouseClicked { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is pressed.</value>
    public Observable<MouseHookEventArgs> MousePressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is released.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is released.</value>
    public Observable<MouseHookEventArgs> MouseReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is moved.</value>
    public Observable<MouseHookEventArgs> MouseMoved { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is dragged.</value>
    public Observable<MouseHookEventArgs> MouseDragged { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse wheel is scrolled.
    /// </summary>
    /// <value>An observable which emits a value when the mouse wheel is scrolled.</value>
    public Observable<MouseWheelHookEventArgs> MouseWheel { get; }

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be stopped by calling the
    /// <see cref="Stop" /> or the <see cref="IDisposable.Dispose" /> methods.
    /// </summary>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public void Run()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        this.hook.Run();
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be stopped by calling the
    /// <see cref="Stop" /> or the <see cref="IDisposable.Dispose" /> methods.
    /// </summary>
    /// <returns>A task which is completed when the hook is stopped.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// The hook is started on a separate thread.
    /// </remarks>
    public Task RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        return this.hook.RunAsync();
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

        this.hookDisabledSubject.Subscribe(_ =>
        {
            this.subscriptions.Dispose();
            this.DisposeAllSubjects();
        });

        this.hook.Dispose();
    }

    private TArgs SelectEventArgs<TArgs>((object? Sender, TArgs Args) e) =>
        e.Args;

    private void DisposeAllSubjects()
    {
        this.hookEnabledSubject.Dispose();
        this.hookDisabledSubject.Dispose();

        this.keyTypedSubject.Dispose();
        this.keyPressedSubject.Dispose();
        this.keyReleasedSubject.Dispose();

        this.mouseClickedSubject.Dispose();
        this.mousePressedSubject.Dispose();
        this.mouseReleasedSubject.Dispose();
        this.mouseMovedSubject.Dispose();
        this.mouseDraggedSubject.Dispose();

        this.mouseWheelSubject.Dispose();
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

    Task IGlobalHook.RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        return this.hook.RunAsync();
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
