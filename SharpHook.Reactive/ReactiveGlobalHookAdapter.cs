namespace SharpHook.Reactive;

using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// <summary>
/// Adapts an <see cref="IGlobalHook" /> to the <see cref="IReactiveGlobalHook" /> interface.
/// </summary>
/// <remarks>This class preserves the sender of hook events - the original hook will be the sender.</remarks>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="IReactiveGlobalHook" />
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
    private readonly Subject<MouseHookEventArgs> mouseDraggedSubject = new();

    private readonly Subject<MouseWheelHookEventArgs> mouseWheelSubject = new();

    private readonly CompositeDisposable subscriptions = new();

    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of <see cref="ReactiveGlobalHookAdapter" />.
    /// </summary>
    /// <param name="hook">The hook to adapt.</param>
    /// <exception cref="ArgumentNullException"><paramref name="hook" /> is <see langword="null" />.</exception>
    public ReactiveGlobalHookAdapter(IGlobalHook hook)
    {
        this.hook = hook ?? throw new ArgumentNullException(nameof(hook));

        this.subscriptions.Add(
            Observable.FromEventPattern<HookEventArgs>(
                h => this.hook.HookEnabled += h, h => this.hook.HookEnabled -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.hookEnabledSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<HookEventArgs>(
                h => this.hook.HookDisabled += h, h => this.hook.HookDisabled -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.hookDisabledSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyTyped += h, h => this.hook.KeyTyped -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.keyTypedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyPressed += h, h => this.hook.KeyPressed -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.keyPressedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyReleased += h, h => this.hook.KeyReleased -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.keyReleasedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseClicked += h, h => this.hook.MouseClicked -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mouseClickedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MousePressed += h, h => this.hook.MousePressed -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mousePressedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseReleased += h, h => this.hook.MouseReleased -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mouseReleasedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseMoved += h, h => this.hook.MouseMoved -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mouseMovedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseDragged += h, h => this.hook.MouseDragged -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mouseDraggedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseWheelHookEventArgs>(
                h => this.hook.MouseWheel += h, h => this.hook.MouseWheel -= h)
                .Select(e => e.EventArgs)
                .Subscribe(this.mouseWheelSubject));

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
    /// Destroys the global hook if it's running.
    /// </summary>
    ~ReactiveGlobalHookAdapter() =>
        this.Dispose(false);

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning => this.hook.IsRunning;

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

        this.hook.Run();
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
    /// The hook is started on a separate thread. The returned observable emits a single value and then immediately
    /// completes when the hook is destroyed.
    /// </remarks>
    public IObservable<Unit> RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        var start = Observable.FromAsync(this.hook.RunAsync).Publish();
        start.Connect();
        return start.AsObservable();
    }

    /// <summary>
    /// Destroys the global hook.
    /// </summary>
    /// <remarks>
    /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
    /// of <see cref="IGlobalHook" />.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;

        this.hookDisabledSubject.Subscribe(_ =>
        {
            this.CompleteAllSubjects();

            if (disposing)
            {
                this.subscriptions.Dispose();
                this.DisposeAllSubjects();
            }
        });

        if (disposing)
        {
            this.hook.Dispose();
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
        if (this.disposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} - the object is disposed");
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
