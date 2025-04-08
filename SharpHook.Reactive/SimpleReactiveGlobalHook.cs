namespace SharpHook.Reactive;

/// <summary>
/// Represents a simple reactive global hook.
/// </summary>
/// <seealso cref="IReactiveGlobalHook" />
public sealed class SimpleReactiveGlobalHook : IReactiveGlobalHook
{
    private const string Starting = "starting";
    private const string Stopping = "stopping";

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

    private readonly IGlobalHookProvider globalHookProvider;
    private readonly DispatchProc dispatchProc;
    private readonly GlobalHookType globalHookType;
    private readonly bool runAsyncOnBackgroundThread;

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook()
        : this(false)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="defaultScheduler">The default scheduler for observables.</param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(IScheduler? defaultScheduler)
        : this(defaultScheduler, false)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="globalHookProvider">The underlying global hook provider.</param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(IGlobalHookProvider? globalHookProvider)
        : this(globalHookProvider, false)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(bool runAsyncOnBackgroundThread)
        : this(UioHookProvider.Instance, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="defaultScheduler">The default scheduler for observables.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(IScheduler? defaultScheduler, IGlobalHookProvider? globalHookProvider)
        : this(defaultScheduler, globalHookProvider, false)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="defaultScheduler">The default scheduler for observables.</param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(IScheduler? defaultScheduler, bool runAsyncOnBackgroundThread)
        : this(defaultScheduler, UioHookProvider.Instance, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(IGlobalHookProvider? globalHookProvider, bool runAsyncOnBackgroundThread)
        : this(null, globalHookProvider, runAsyncOnBackgroundThread)
    { }


    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="defaultScheduler">The default scheduler for observables.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleReactiveGlobalHook(
        IScheduler? defaultScheduler,
        IGlobalHookProvider? globalHookProvider,
        bool runAsyncOnBackgroundThread)
        : this(GlobalHookType.All, defaultScheduler, globalHookProvider, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleReactiveGlobalHook" />.
    /// </summary>
    /// <param name="globalHookType">The global hook type.</param>
    /// <param name="defaultScheduler">
    /// The default scheduler for observables, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    public SimpleReactiveGlobalHook(
        GlobalHookType globalHookType = GlobalHookType.All,
        IScheduler? defaultScheduler = null,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
    {
        this.globalHookType = globalHookType;
        this.globalHookProvider = globalHookProvider ?? UioHookProvider.Instance;
        this.dispatchProc = this.DispatchEvent;
        this.runAsyncOnBackgroundThread = runAsyncOnBackgroundThread;

        defaultScheduler ??= ImmediateScheduler.Instance;

        this.HookEnabled = this.hookEnabledSubject.Take(1).ObserveOn(defaultScheduler);
        this.HookDisabled = this.hookDisabledSubject.Take(1).ObserveOn(defaultScheduler);

        this.KeyTyped = this.keyTypedSubject.ObserveOn(defaultScheduler);
        this.KeyPressed = this.keyPressedSubject.ObserveOn(defaultScheduler);
        this.KeyReleased = this.keyReleasedSubject.ObserveOn(defaultScheduler);

        this.MouseClicked = this.mouseClickedSubject.ObserveOn(defaultScheduler);
        this.MousePressed = this.mousePressedSubject.ObserveOn(defaultScheduler);
        this.MouseReleased = this.mouseReleasedSubject.ObserveOn(defaultScheduler);
        this.MouseMoved = this.mouseMovedSubject.ObserveOn(defaultScheduler);
        this.MouseDragged = this.mouseDraggedSubject.ObserveOn(defaultScheduler);

        this.MouseWheel = this.mouseWheelSubject.ObserveOn(defaultScheduler);
    }

    /// <summary>
    /// Destroys the global hook if it's running.
    /// </summary>
    ~SimpleReactiveGlobalHook() =>
        this.Dispose(false);

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
    /// Gets an observable which emits a value when the global hook is enabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is enabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="Run" /> or <see cref="RunAsync" /> method is called and
    /// then immediately completes.
    /// </remarks>
    public IObservable<HookEventArgs> HookEnabled { get; }

    /// <summary>
    /// Gets an observable which emits a value when the global hook is disabled.
    /// </summary>
    /// <value>An observable which emits a value when the global hook is disabled.</value>
    /// <remarks>
    /// The observable emits a value when the <see cref="IDisposable.Dispose" /> method is called and then
    /// immediately completes.
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
    /// Runs the global hook on the current thread, blocking it. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public void Run()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        UioHookResult result;

        try
        {
            this.globalHookProvider.SetDispatchProc(this.dispatchProc, IntPtr.Zero);

            this.IsRunning = true;
            result = this.RunGlobalHook();
            this.IsRunning = false;
        } catch (Exception e)
        {
            this.IsRunning = false;
            throw new HookException(UioHookResult.Failure, e);
        }

        if (result != UioHookResult.Success)
        {
            throw new HookException(result, this.FormatFailureMessage(Starting, result));
        }
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>An observable which is completed when the hook is destroyed.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// The hook is started on a separate thread. The returned observable is hot. It emits a single value and then
    /// immediately completes when the hook is destroyed.
    /// </remarks>
    public IObservable<Unit> RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var hookStopped = new Subject<Unit>();

        var thread = new Thread(() =>
        {
            try
            {
                this.globalHookProvider.SetDispatchProc(this.dispatchProc, IntPtr.Zero);

                this.IsRunning = true;
                var result = this.RunGlobalHook();
                this.IsRunning = false;

                if (result == UioHookResult.Success)
                {
                    hookStopped.OnNext(Unit.Default);
                    hookStopped.OnCompleted();
                } else
                {
                    hookStopped.OnError(new HookException(result, this.FormatFailureMessage(Starting, result)));
                }
            } catch (Exception e)
            {
                this.IsRunning = false;
                hookStopped.OnError(new HookException(UioHookResult.Failure, e));
            }
        })
        {
            IsBackground = this.runAsyncOnBackgroundThread
        };

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

    private UioHookResult RunGlobalHook() =>
        this.globalHookType switch
        {
            GlobalHookType.Keyboard => this.globalHookProvider.RunKeyboard(),
            GlobalHookType.Mouse => this.globalHookProvider.RunMouse(),
            _ => this.globalHookProvider.Run()
        };

    private void DispatchEvent(ref UioHookEvent e, IntPtr userData)
    {
        HookEventArgs? args = null;

        switch (e.Type)
        {
            case EventType.HookEnabled:
                this.hookEnabledSubject.OnNext(args = new(e));
                break;
            case EventType.HookDisabled:
                this.hookDisabledSubject.OnNext(args = new(e));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(e);
                args = keyTypedArgs;
                this.keyTypedSubject.OnNext(keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(e);
                args = keyPressedArgs;
                this.keyPressedSubject.OnNext(keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(e);
                args = keyReleasedArgs;
                this.keyReleasedSubject.OnNext(keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(e);
                args = mouseClickedArgs;
                this.mouseClickedSubject.OnNext(mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(e);
                args = mousePressedArgs;
                this.mousePressedSubject.OnNext(mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(e);
                args = mouseReleasedArgs;
                this.mouseReleasedSubject.OnNext(mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(e);
                args = mouseMovedArgs;
                this.mouseMovedSubject.OnNext(mouseMovedArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(e);
                args = mouseDraggedArgs;
                this.mouseDraggedSubject.OnNext(mouseDraggedArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(e);
                args = mouseWheelArgs;
                this.mouseWheelSubject.OnNext(mouseWheelArgs);
                break;
        };

        if (args != null && args.SuppressEvent)
        {
            e.Mask |= EventMask.SuppressEvent;
        }
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDisposed)
        {
            return;
        }

        this.IsDisposed = true;

        if (this.IsRunning)
        {
            this.hookDisabledSubject.Subscribe(_ =>
            {
                this.globalHookProvider.SetDispatchProc(null, IntPtr.Zero);

                this.CompleteAllSubjects();

                if (disposing)
                {
                    this.DisposeAllSubjects();
                }
            });

            var result = this.globalHookProvider.Stop();

            if (disposing && result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatFailureMessage(Stopping, result));
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
                this.GetType().Name, $"Cannot call {method} - the object is disposed");
        }
    }

    private string FormatFailureMessage(string action, UioHookResult result) =>
        $"Failed {action} the global hook: {result} ({(int)result:x})";
}
