namespace SharpHook.ReactiveUI;

/// <summary>
/// Represents a reactive global keyboard and mouse hook.
/// </summary>
/// <seealso cref="IReactiveGlobalHook" />
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
    private readonly Signal<MouseHookEventArgs> mouseDraggedSignal = new();

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
        this.MouseDragged = this.mouseDraggedSignal.ObserveOn(defaultSequencer);

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
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedArgs;
                this.mouseDraggedSignal.OnNext(mouseDraggedArgs);
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
        this.mouseDraggedSignal.OnCompleted();

        this.mouseWheelSignal.OnCompleted();
    }
}
