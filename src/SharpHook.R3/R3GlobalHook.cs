namespace SharpHook.R3;

/// <summary>
/// Represents a reactive R3 global keyboard and mouse hook.
/// </summary>
/// <seealso cref="IR3GlobalHook" />
/// <seealso cref="IFeatureProvider" />
/// <seealso cref="IGlobalHookProvider" />
/// <seealso cref="UioHookProvider" />
public sealed class R3GlobalHook : BasicGlobalHookBase, IR3GlobalHook
{
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

    /// <summary>
    /// Initializes a new instance of <see cref="R3GlobalHook" />.
    /// </summary>
    /// <param name="defaultTimeProvider">
    /// The default time provider for observables, or <see langword="null" /> to use the default one as defined in
    /// <see cref="ObservableSystem.DefaultTimeProvider" />.
    /// </param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    public R3GlobalHook(TimeProvider? defaultTimeProvider = null, IGlobalHookProvider? globalHookProvider = null)
        : base(globalHookProvider)
    {
        defaultTimeProvider ??= ObservableSystem.DefaultTimeProvider;

        this.HookEnabled = this.hookEnabledSubject.ObserveOn(defaultTimeProvider);
        this.HookDisabled = this.hookDisabledSubject.ObserveOn(defaultTimeProvider);

        this.KeyTyped = this.keyTypedSubject.ObserveOn(defaultTimeProvider);
        this.KeyPressed = this.keyPressedSubject.ObserveOn(defaultTimeProvider);
        this.KeyReleased = this.keyReleasedSubject.ObserveOn(defaultTimeProvider);

        this.MouseClicked = this.mouseClickedSubject.ObserveOn(defaultTimeProvider);
        this.MousePressed = this.mousePressedSubject.ObserveOn(defaultTimeProvider);
        this.MouseReleased = this.mouseReleasedSubject.ObserveOn(defaultTimeProvider);
        this.MouseMoved = this.mouseMovedSubject.ObserveOn(defaultTimeProvider);
        this.MouseMovedRelative = this.mouseMovedRelativeSubject.ObserveOn(defaultTimeProvider);
        this.MouseDragged = this.mouseDraggedSubject.ObserveOn(defaultTimeProvider);
        this.MouseDraggedRelative = this.mouseDraggedRelativeSubject.ObserveOn(defaultTimeProvider);

        this.MouseWheel = this.mouseWheelSubject.ObserveOn(defaultTimeProvider);
    }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is enabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is enabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IBasicGlobalHook.Run" /> or
    /// <see cref="IBasicGlobalHook.RunAsync" /> method is called.
    /// </remarks>
    public Observable<HookEventArgs> HookEnabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is disabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is disabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IBasicGlobalHook.Stop" /> or <see cref="IDisposable.Dispose" />
    /// method is called.
    /// </remarks>
    public Observable<HookEventArgs> HookDisabled { get; }

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
    public Observable<KeyboardHookEventArgs> KeyTyped { get; }

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
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public Observable<MouseHookEventArgs> MouseClicked { get; }

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
    public Observable<MouseHookEventArgs> MousePressed { get; }

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
    public Observable<MouseHookEventArgs> MouseReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is moved.</value>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseMovedRelative" /> instead.
    /// </remarks>
    public Observable<MouseHookEventArgs> MouseMoved { get; }

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
    public Observable<MouseHookEventArgs> MouseMovedRelative { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is dragged.</value>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseDraggedRelative" /> instead.
    /// </remarks>
    public Observable<MouseHookEventArgs> MouseDragged { get; }

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
    public Observable<MouseHookEventArgs> MouseDraggedRelative { get; }

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
    public Observable<MouseWheelHookEventArgs> MouseWheel { get; }

    /// <summary>
    /// Handles the hook event.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    protected override void HandleHookEvent(ref UioHookEvent @event)
    {
        HookEventArgs? args = null;

        switch (@event.Type)
        {
            case EventType.HookEnabled:
                this.hookEnabledSubject.OnNext(args = new(@event));
                break;
            case EventType.HookDisabled:
                this.hookDisabledSubject.OnNext(args = new(@event));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(@event);
                args = keyTypedArgs;
                this.keyTypedSubject.OnNext(keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(@event);
                args = keyPressedArgs;
                this.keyPressedSubject.OnNext(keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(@event);
                args = keyReleasedArgs;
                this.keyReleasedSubject.OnNext(keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(@event);
                args = mouseClickedArgs;
                this.mouseClickedSubject.OnNext(mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(@event);
                args = mousePressedArgs;
                this.mousePressedSubject.OnNext(mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(@event);
                args = mouseReleasedArgs;
                this.mouseReleasedSubject.OnNext(mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(@event);
                args = mouseMovedArgs;
                this.mouseMovedSubject.OnNext(mouseMovedArgs);
                break;
            case EventType.MouseMovedRelative:
                var mouseMovedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseMovedRelativeArgs;
                this.mouseMovedRelativeSubject.OnNext(mouseMovedRelativeArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedArgs;
                this.mouseDraggedSubject.OnNext(mouseDraggedArgs);
                break;
            case EventType.MouseDraggedRelative:
                var mouseDraggedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedRelativeArgs;
                this.mouseDraggedRelativeSubject.OnNext(mouseDraggedRelativeArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(@event);
                args = mouseWheelArgs;
                this.mouseWheelSubject.OnNext(mouseWheelArgs);
                break;
        }

        if (args is not null && args.SuppressEvent)
        {
            @event.Mask |= EventMask.SuppressEvent;
        }
    }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true" /> if the method is called from the <see cref="IDisposable.Dispose" /> method.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    protected override void Dispose(bool disposing)
    {
        if (this.IsDisposed)
        {
            return;
        }

        bool isRunning = this.IsRunning;

        if (isRunning)
        {
            this.hookDisabledSubject.Subscribe(_ => this.CompleteAllSubjects());
        }

        base.Dispose(disposing);

        if (!isRunning)
        {
            this.CompleteAllSubjects();
        }
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
}
