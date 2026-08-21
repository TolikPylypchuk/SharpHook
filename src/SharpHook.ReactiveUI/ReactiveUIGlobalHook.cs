namespace SharpHook.ReactiveUI;

/// <summary>
/// Represents a reactive global keyboard and mouse hook.
/// </summary>
/// <seealso cref="IReactiveGlobalHook" />
/// <seealso cref="IFeatureProvider" />
/// <seealso cref="IGlobalHookProvider" />
/// <seealso cref="UioHookProvider" />
public sealed class ReactiveUIGlobalHook : BasicGlobalHookBase, IReactiveGlobalHook
{
    private readonly Signal<HookEventArgs> hookEnabledSignal = new();
    private readonly Signal<HookEventArgs> hookDisabledSignal = new();

    private readonly Signal<KeyboardHookEventArgs> keyTypedSignal = new();
    private readonly Signal<KeyboardHookEventArgs> keyPressedSignal = new();
    private readonly Signal<KeyboardHookEventArgs> keyReleasedSignal = new();

    private readonly Signal<MouseHookEventArgs> mouseClickedSignal = new();
    private readonly Signal<MouseHookEventArgs> mousePressedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseReleasedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseMovedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseMovedRelativeSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseDraggedSignal = new();
    private readonly Signal<MouseHookEventArgs> mouseDraggedRelativeSignal = new();

    private readonly Signal<MouseWheelHookEventArgs> mouseWheelSignal = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ReactiveUIGlobalHook" />.
    /// </summary>
    /// <param name="defaultSequencer">
    /// The default sequencer for observables, or <see langword="null" /> to use the default one
    /// (<see cref="Sequencer.Immediate" />).
    /// </param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    public ReactiveUIGlobalHook(ISequencer? defaultSequencer = null, IGlobalHookProvider? globalHookProvider = null)
        : base(globalHookProvider)
    {
        defaultSequencer ??= Sequencer.Immediate;

        this.HookEnabled = this.hookEnabledSignal.ObserveOn(defaultSequencer);
        this.HookDisabled = this.hookDisabledSignal.ObserveOn(defaultSequencer);

        this.KeyTyped = this.keyTypedSignal.ObserveOn(defaultSequencer);
        this.KeyPressed = this.keyPressedSignal.ObserveOn(defaultSequencer);
        this.KeyReleased = this.keyReleasedSignal.ObserveOn(defaultSequencer);

        this.MouseClicked = this.mouseClickedSignal.ObserveOn(defaultSequencer);
        this.MousePressed = this.mousePressedSignal.ObserveOn(defaultSequencer);
        this.MouseReleased = this.mouseReleasedSignal.ObserveOn(defaultSequencer);
        this.MouseMoved = this.mouseMovedSignal.ObserveOn(defaultSequencer);
        this.MouseMovedRelative = this.mouseMovedRelativeSignal.ObserveOn(defaultSequencer);
        this.MouseDragged = this.mouseDraggedSignal.ObserveOn(defaultSequencer);
        this.MouseDraggedRelative = this.mouseDraggedRelativeSignal.ObserveOn(defaultSequencer);

        this.MouseWheel = this.mouseWheelSignal.ObserveOn(defaultSequencer);
    }

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
    /// Handles the hook event.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    protected override void HandleHookEvent(ref UioHookEvent @event)
    {
        HookEventArgs? args = null;

        switch (@event.Type)
        {
            case EventType.HookEnabled:
                this.hookEnabledSignal.OnNext(args = new(@event));
                break;
            case EventType.HookDisabled:
                this.hookDisabledSignal.OnNext(args = new(@event));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(@event);
                args = keyTypedArgs;
                this.keyTypedSignal.OnNext(keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(@event);
                args = keyPressedArgs;
                this.keyPressedSignal.OnNext(keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(@event);
                args = keyReleasedArgs;
                this.keyReleasedSignal.OnNext(keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(@event);
                args = mouseClickedArgs;
                this.mouseClickedSignal.OnNext(mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(@event);
                args = mousePressedArgs;
                this.mousePressedSignal.OnNext(mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(@event);
                args = mouseReleasedArgs;
                this.mouseReleasedSignal.OnNext(mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(@event);
                args = mouseMovedArgs;
                this.mouseMovedSignal.OnNext(mouseMovedArgs);
                break;
            case EventType.MouseMovedRelative:
                var mouseMovedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseMovedRelativeArgs;
                this.mouseMovedRelativeSignal.OnNext(mouseMovedRelativeArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedArgs;
                this.mouseDraggedSignal.OnNext(mouseDraggedArgs);
                break;
            case EventType.MouseDraggedRelative:
                var mouseDraggedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedRelativeArgs;
                this.mouseDraggedRelativeSignal.OnNext(mouseDraggedRelativeArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(@event);
                args = mouseWheelArgs;
                this.mouseWheelSignal.OnNext(mouseWheelArgs);
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
            this.hookDisabledSignal.Subscribe(_ => this.CompleteAllSignals());
        }

        base.Dispose(disposing);

        if (!isRunning)
        {
            this.CompleteAllSignals();
        }
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
        this.mouseMovedRelativeSignal.OnCompleted();
        this.mouseDraggedSignal.OnCompleted();
        this.mouseDraggedRelativeSignal.OnCompleted();

        this.mouseWheelSignal.OnCompleted();
    }
}
