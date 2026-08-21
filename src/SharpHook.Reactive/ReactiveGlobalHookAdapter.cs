namespace SharpHook.Reactive;

/// <summary>
/// Adapts an <see cref="IGlobalHook" /> to the <see cref="IReactiveGlobalHook" /> interface.
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="IReactiveGlobalHook" />
/// <seealso cref="IFeatureProvider" />
/// <seealso cref="IGlobalHookProvider" />
/// <seealso cref="UioHookProvider" />
public sealed class ReactiveGlobalHookAdapter : IGlobalHook, IReactiveGlobalHook
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
    private readonly Subject<MouseHookEventArgs> mouseMovedRelativeSubject = new();
    private readonly Subject<MouseHookEventArgs> mouseDraggedSubject = new();
    private readonly Subject<MouseHookEventArgs> mouseDraggedRelativeSubject = new();

    private readonly Subject<MouseWheelHookEventArgs> mouseWheelSubject = new();

    private readonly CompositeDisposable subscriptions = [];

    /// <summary>
    /// Initializes a new instance of <see cref="ReactiveGlobalHookAdapter" />.
    /// </summary>
    /// <param name="hook">The hook to adapt.</param>
    /// <param name="defaultScheduler">
    /// The default scheduler for observables, or <see langword="null" /> to use the default one
    /// (<see cref="Scheduler.Immediate" />).
    /// </param>
    public ReactiveGlobalHookAdapter(IGlobalHook hook, IScheduler? defaultScheduler = null)
    {
        this.hook = hook ?? throw new ArgumentNullException(nameof(hook));

        defaultScheduler ??= Scheduler.Immediate;

        Observable.FromEventPattern<HookEventArgs>(
            h => this.hook.HookEnabled += h, h => this.hook.HookEnabled -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.hookEnabledSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<HookEventArgs>(
            h => this.hook.HookDisabled += h, h => this.hook.HookDisabled -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.hookDisabledSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyTyped += h, h => this.hook.KeyTyped -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyTypedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyPressed += h, h => this.hook.KeyPressed -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyPressedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<KeyboardHookEventArgs>(
            h => this.hook.KeyReleased += h, h => this.hook.KeyReleased -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.keyReleasedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseClicked += h, h => this.hook.MouseClicked -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseClickedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MousePressed += h, h => this.hook.MousePressed -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mousePressedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseReleased += h, h => this.hook.MouseReleased -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseReleasedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseMoved += h, h => this.hook.MouseMoved -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseMovedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseMovedRelative += h, h => this.hook.MouseMovedRelative -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseMovedRelativeSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseHookEventArgs>(
            h => this.hook.MouseDragged += h, h => this.hook.MouseDragged -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseDraggedSubject)
            .DisposeWith(this.subscriptions);

        Observable.FromEventPattern<MouseWheelHookEventArgs>(
            h => this.hook.MouseWheel += h, h => this.hook.MouseWheel -= h)
            .Select(e => e.EventArgs)
            .Subscribe(this.mouseWheelSubject)
            .DisposeWith(this.subscriptions);

        this.HookEnabled = this.hookEnabledSubject.ObserveOn(defaultScheduler);
        this.HookDisabled = this.hookDisabledSubject.ObserveOn(defaultScheduler);

        this.KeyTyped = this.keyTypedSubject.ObserveOn(defaultScheduler);
        this.KeyPressed = this.keyPressedSubject.ObserveOn(defaultScheduler);
        this.KeyReleased = this.keyReleasedSubject.ObserveOn(defaultScheduler);

        this.MouseClicked = this.mouseClickedSubject.ObserveOn(defaultScheduler);
        this.MousePressed = this.mousePressedSubject.ObserveOn(defaultScheduler);
        this.MouseReleased = this.mouseReleasedSubject.ObserveOn(defaultScheduler);
        this.MouseMoved = this.mouseMovedSubject.ObserveOn(defaultScheduler);
        this.MouseMovedRelative = this.mouseMovedRelativeSubject.ObserveOn(defaultScheduler);
        this.MouseDragged = this.mouseDraggedSubject.ObserveOn(defaultScheduler);
        this.MouseDraggedRelative = this.mouseDraggedRelativeSubject.ObserveOn(defaultScheduler);

        this.MouseWheel = this.mouseWheelSubject.ObserveOn(defaultScheduler);
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
    /// <remarks>
    /// <para>
    /// This event is disabled by default. If you want to enable it, you should set the
    /// <see cref="IGlobalHookProvider.KeyTypedEnabled" /> property to <see langword="true" />.
    /// </para>
    /// <para>
    /// On Wayland, this event is not supported. You can use <see cref="IFeatureProvider.GetOptionalFeatureSupport" />
    /// to check support for this event. If that method returns <see cref="UioHookFeature.KeyTypedEvents" /> as one of
    /// the supported features, then this event is supported.
    /// </para>
    /// </remarks>
    public IObservable<KeyboardHookEventArgs> KeyTyped { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a key is pressed.</value>
    /// <remarks>
    /// On Wayland, this event is not raised repeatedly if the key is kept down. To check whether key auto-repeat events
    /// are reported, you can use <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.KeyAutoRepeat" /> as one of the supported features, then this event will be raised
    /// repeatedly if the key is kept down.
    /// </remarks>
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
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseClicked { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is pressed.</value>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MousePressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is released.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is released.</value>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is moved.</value>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseMovedRelative" /> instead.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseMoved { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved relatively to its previous position.
    /// </summary>
    /// <value>
    /// An observable which emits a value when the mouse cursor is moved relatively to its previous position.
    /// </value>
    /// <remarks>
    /// This event is raised only on Wayland when using a relative pointing device (which most normal mice are). To
    /// check whether the current platform can raise this event, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method does not return
    /// <see cref="UioHookFeature.AbsoluteMouseMovement" /> as one of the supported features, then the current platform
    /// may raise this event.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseMovedRelative { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is dragged.</value>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseDraggedRelative" /> instead.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseDragged { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged relatively to its previous position.
    /// </summary>
    /// <value>
    /// An observable which emits a value when the mouse cursor is dragged relatively to its previous position.
    /// </value>
    /// <remarks>
    /// This event is raised only on Wayland when using a relative pointing device (which most normal mice are). To
    /// check whether the current platform can raise this event, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method does not return
    /// <see cref="UioHookFeature.AbsoluteMouseMovement" /> as one of the supported features, then the current platform
    /// may raise this event.
    /// </remarks>
    public IObservable<MouseHookEventArgs> MouseDraggedRelative { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse wheel is scrolled.
    /// </summary>
    /// <value>An observable which emits a value when the mouse wheel is scrolled.</value>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
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
            this.hookDisabledSubject.Subscribe(_ => this.CompleteAllSubjectsAndDisposeSubscriptions());
        }

        this.hook.Dispose();

        if (!isRunning)
        {
            this.CompleteAllSubjectsAndDisposeSubscriptions();
        }
    }

    private void CompleteAllSubjectsAndDisposeSubscriptions()
    {
        this.CompleteAllSubjects();
        this.subscriptions.Dispose();
    }

    private void CompleteAllSubjects()
    {
        this.hookEnabledSubject.OnCompleted();
        this.hookDisabledSubject.OnCompleted();

        this.keyTypedSubject.OnCompleted();
        this.keyPressedSubject.OnCompleted();
        this.keyReleasedSubject.OnCompleted();

        this.mouseClickedSubject.OnCompleted();
        this.mousePressedSubject.OnCompleted();
        this.mouseReleasedSubject.OnCompleted();
        this.mouseMovedSubject.OnCompleted();
        this.mouseMovedRelativeSubject.OnCompleted();
        this.mouseDraggedSubject.OnCompleted();
        this.mouseDraggedRelativeSubject.OnCompleted();

        this.mouseWheelSubject.OnCompleted();
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

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseMovedRelative
    {
        add => this.hook.MouseMovedRelative += value;
        remove => this.hook.MouseMovedRelative -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseDragged
    {
        add => this.hook.MouseDragged += value;
        remove => this.hook.MouseDragged -= value;
    }

    event EventHandler<MouseHookEventArgs> IGlobalHook.MouseDraggedRelative
    {
        add => this.hook.MouseDraggedRelative += value;
        remove => this.hook.MouseDraggedRelative -= value;
    }

    event EventHandler<MouseWheelHookEventArgs> IGlobalHook.MouseWheel
    {
        add => this.hook.MouseWheel += value;
        remove => this.hook.MouseWheel -= value;
    }
}
