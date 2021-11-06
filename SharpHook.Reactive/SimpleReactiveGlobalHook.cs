namespace SharpHook.Reactive;

/// <summary>
/// Represents a simple reactive global hook.
/// </summary>
/// <seealso cref="IReactiveGlobalHook" />
public sealed class SimpleReactiveGlobalHook : IReactiveGlobalHook
{
    private readonly Subject<HookEvent<HookEventArgs>> hookEnabledSubject = new();
    private readonly Subject<HookEvent<HookEventArgs>> hookDisabledSubject = new();

    private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyTypedSubject = new();
    private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyPressedSubject = new();
    private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyReleasedSubject = new();

    private readonly Subject<HookEvent<MouseHookEventArgs>> mouseClickedSubject = new();
    private readonly Subject<HookEvent<MouseHookEventArgs>> mousePressedSubject = new();
    private readonly Subject<HookEvent<MouseHookEventArgs>> mouseReleasedSubject = new();
    private readonly Subject<HookEvent<MouseHookEventArgs>> mouseMovedSubject = new();
    private readonly Subject<HookEvent<MouseHookEventArgs>> mouseDraggedSubject = new();

    private readonly Subject<HookEvent<MouseWheelHookEventArgs>> mouseWheelSubject = new();

    private bool disposed = false;

    public SimpleReactiveGlobalHook()
    {
        this.HookEnabled = this.hookEnabledSubject.AsObservable().Take(1);
        this.HookDisabled = this.hookDisabledSubject.AsObservable().Take(1);

        this.KeyTyped = this.keyTypedSubject.AsObservable();
        this.KeyPressed = this.keyPressedSubject.AsObservable();
        this.KeyReleased = this.keyReleasedSubject.AsObservable();

        this.MouseClicked = this.mouseClickedSubject.AsObservable();
        this.MousePressed = this.mousePressedSubject.AsObservable();
        this.MouseReleased = this.mouseReleasedSubject.AsObservable();
        this.MouseMoved = this.mouseMovedSubject.AsObservable();
        this.MouseDragged = this.mouseDraggedSubject.AsObservable();

        this.MouseWheel = this.mouseWheelSubject.AsObservable();
    }

    /// <summary>
    /// Unregisteres the global hook if it's registered.
    /// </summary>
    ~SimpleReactiveGlobalHook() =>
        this.Dispose(false);

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is enabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is enabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="Start" /> method is called and then immediately completes.
    /// </remarks>
    public IObservable<HookEvent<HookEventArgs>> HookEnabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is disabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is disabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IDisposable.Dispose" /> method is called and then
    /// immediately completes.
    /// </remarks>
    public IObservable<HookEvent<HookEventArgs>> HookDisabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is typed.
    /// </summary>
    /// <value>An observable which emits a value when a key is typed.</value>
    public IObservable<HookEvent<KeyboardHookEventArgs>> KeyTyped { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a key is pressed.</value>
    public IObservable<HookEvent<KeyboardHookEventArgs>> KeyPressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a key is released.
    /// </summary>
    /// <value>An observable which emits a value when a key is released.</value>
    public IObservable<HookEvent<KeyboardHookEventArgs>> KeyReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is clicked.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is clicked.</value>
    public IObservable<HookEvent<MouseHookEventArgs>> MouseClicked { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is pressed.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is pressed.</value>
    public IObservable<HookEvent<MouseHookEventArgs>> MousePressed { get; }

    /// <summary>
    /// Gets an observable which emits a value when a mouse button is released.
    /// </summary>
    /// <value>An observable which emits a value when a mouse button is released.</value>
    public IObservable<HookEvent<MouseHookEventArgs>> MouseReleased { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is moved.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is moved.</value>
    public IObservable<HookEvent<MouseHookEventArgs>> MouseMoved { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse cursor is dragged.
    /// </summary>
    /// <value>An observable which emits a value when the mouse cursor is dragged.</value>
    public IObservable<HookEvent<MouseHookEventArgs>> MouseDragged { get; }

    /// <summary>
    /// Gets an observable which emits a value when the mouse wheel is scrolled.
    /// </summary>
    /// <value>An observable which emits a value when the mouse wheel is scrolled.</value>
    public IObservable<HookEvent<MouseWheelHookEventArgs>> MouseWheel { get; }

    /// <summary>
    /// Starts the global hook. The hook can be destroyed by calling the <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>An observable which is completed when the hook is destroyed.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// The hook is started on a separate thread. The returned observable emits a single value and then immediately
    /// completes when the hook is destroyed.
    /// </remarks>
    public IObservable<Unit> Start()
    {
        this.ThrowIfDisposed();

        var hookStopped = new Subject<Unit>();

        var thread = new Thread(() =>
        {
            try
            {
                UioHook.SetDispatchProc(this.DispatchEvent);

                this.IsRunning = true;
                var result = UioHook.Run();
                this.IsRunning = false;

                if (result == UioHookResult.Success)
                {
                    hookStopped.OnNext(Unit.Default);
                    hookStopped.OnCompleted();
                }
                else
                {
                    hookStopped.OnError(new HookException(result, this.FormatFailureMessage("starting", result)));
                }
            }
            catch (Exception e)
            {
                hookStopped.OnError(e);
            }
        });

        thread.Start();

        return hookStopped.AsObservable();
    }

    /// <summary>
    /// Destroys the global hook.
    /// </summary>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    /// <remarks>
    /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
    /// of <see cref="IReactiveGlobalHook" />.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void DispatchEvent(ref UioHookEvent e)
    {
        switch (e.Type)
        {
            case EventType.HookEnabled:
                this.hookEnabledSubject.OnNext(new HookEvent<HookEventArgs>(this, new(e)));
                break;
            case EventType.HookDisabled:
                this.hookDisabledSubject.OnNext(new HookEvent<HookEventArgs>(this, new(e)));
                break;
            case EventType.KeyTyped:
                this.keyTypedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                break;
            case EventType.KeyPressed:
                this.keyPressedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                break;
            case EventType.KeyReleased:
                this.keyReleasedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                break;
            case EventType.MouseClicked:
                this.mouseClickedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                break;
            case EventType.MousePressed:
                this.mousePressedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                break;
            case EventType.MouseReleased:
                this.mouseReleasedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                break;
            case EventType.MouseMoved:
                this.mouseMovedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                break;
            case EventType.MouseDragged:
                this.mouseDraggedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                break;
            case EventType.MouseWheel:
                this.mouseWheelSubject.OnNext(new HookEvent<MouseWheelHookEventArgs>(this, new(e)));
                break;
        };
    }

    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;

        if (this.IsRunning)
        {
            this.hookDisabledSubject.Subscribe(_ =>
            {
                UioHook.SetDispatchProc(UioHook.EmptyDispatchProc);

                this.CompleteAllSubjects();

                if (disposing)
                {
                    this.DisposeAllSubjects();
                }
            });

            var result = UioHook.Stop();

            if (disposing && result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatFailureMessage("stopping", result));
            }
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
        this.mouseDraggedSubject.OnCompleted();

        this.mouseWheelSubject.OnCompleted();
    }

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

    private void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} - the object is disposed");
        }
    }

    private string FormatFailureMessage(string action, UioHookResult result) =>
        $"Failed {action} the global hook: {result} ({(int)result:x})";
}
