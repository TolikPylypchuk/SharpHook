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
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.hookEnabledSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<HookEventArgs>(
                h => this.hook.HookDisabled += h, h => this.hook.HookDisabled -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.hookDisabledSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyTyped += h, h => this.hook.KeyTyped -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.keyTypedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyPressed += h, h => this.hook.KeyPressed -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.keyPressedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<KeyboardHookEventArgs>(
                h => this.hook.KeyReleased += h, h => this.hook.KeyReleased -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.keyReleasedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseClicked += h, h => this.hook.MouseClicked -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.mouseClickedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MousePressed += h, h => this.hook.MousePressed -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.mousePressedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseReleased += h, h => this.hook.MouseReleased -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.mouseReleasedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseMoved += h, h => this.hook.MouseMoved -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.mouseMovedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseHookEventArgs>(
                h => this.hook.MouseDragged += h, h => this.hook.MouseDragged -= h)
                .Select(HookEvent.FromEventPattern)
                .Subscribe(this.mouseDraggedSubject));

        this.subscriptions.Add(
            Observable.FromEventPattern<MouseWheelHookEventArgs>(
                h => this.hook.MouseWheel += h, h => this.hook.MouseWheel -= h)
                .Select(HookEvent.FromEventPattern)
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
    /// Unregisteres the global hook if it's registered.
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
    /// <remarks>
    /// The returned observable emits a sinlge value and then immediately completes when the hook is destroyed.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public IObservable<Unit> Start()
    {
        this.ThrowIfDisposed();

        var start = Observable.FromAsync(this.hook.Start).Publish();
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

        if (disposing)
        {
            this.hook.Dispose();
        }

        this.hookDisabledSubject.Subscribe(_ =>
        {
            this.hookDisabledSubject.OnCompleted();
            this.hookDisabledSubject.Dispose();
        });

        this.CompleteAllSubjects();

        if (disposing)
        {
            this.subscriptions.Dispose();
            this.DisposeAllSubjects();
        }
    }

    private void CompleteAllSubjects()
    {
        this.hookEnabledSubject.OnCompleted();

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
        this.hookEnabledSubject.OnCompleted();

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

    Task IGlobalHook.Start()
    {
        this.ThrowIfDisposed();
        return this.hook.Start();
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
