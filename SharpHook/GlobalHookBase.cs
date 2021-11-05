namespace SharpHook;

/// <summary>
/// Represents an abstract implementation of <see cref="IGlobalHook" /> which runs the hook on a separate thread and
/// raises events only when there is at least one subscriber.
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
public abstract class GlobalHookBase : IGlobalHook
{
    private bool disposed = false;

    /// <summary>
    /// Unregisteres the global hook if it's registered.
    /// </summary>
    ~GlobalHookBase() =>
        this.Dispose(false);

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Starts the global hook. The hook can be destroyed by calling the <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// The hook is started on a separate thread.
    /// </remarks>
    public Task Start()
    {
        this.ThrowIfDisposed();

        var source = new TaskCompletionSource<object?>();

        var thread = new Thread(() =>
        {
            try
            {
                UioHook.SetDispatchProc(this.HandleHookEventIfNeeded);

                this.IsRunning = true;
                var result = UioHook.Run();
                this.IsRunning = false;

                if (result == UioHookResult.Success)
                {
                    source.SetResult(null);
                }
                else
                {
                    source.SetException(new HookException(result, this.FormatFailureMessage("starting", result)));
                }
            }
            catch (Exception e)
            {
                source.SetException(e);
            }
        });

        thread.Start();

        return source.Task;
    }

    /// <summary>
    /// Destroys the global hook.
    /// </summary>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    /// <remarks>
    /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
    /// of <see cref="IGlobalHook" />.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// When implemented in a derived class, represents a strategy for handling a hook event.
    /// </summary>
    /// <param name="e">The event to handle.</param>
    /// <remarks>
    /// Derived classes should call <see cref="DispatchEvent(ref UioHookEvent)" /> inside this method to raise the
    /// appropriate event.
    /// </remarks>
    protected abstract void HandleHookEvent(ref UioHookEvent e);

    /// <summary>
    /// Dispatches an event from libuiohook, i.e. raises the appropriate event.
    /// </summary>
    /// <param name="e">The event to dispatch.</param>
    protected void DispatchEvent(ref UioHookEvent e)
    {
        switch (e.Type)
        {
            case EventType.HookEnabled:
                this.OnHookEnabled(new HookEventArgs(e));
                break;
            case EventType.HookDisabled:
                this.OnHookDisabled(new HookEventArgs(e));
                break;
            case EventType.KeyTyped:
                this.OnKeyTyped(new KeyboardHookEventArgs(e));
                break;
            case EventType.KeyPressed:
                this.OnKeyPressed(new KeyboardHookEventArgs(e));
                break;
            case EventType.KeyReleased:
                this.OnKeyReleased(new KeyboardHookEventArgs(e));
                break;
            case EventType.MouseClicked:
                this.OnMouseClicked(new MouseHookEventArgs(e));
                break;
            case EventType.MousePressed:
                this.OnMousePressed(new MouseHookEventArgs(e));
                break;
            case EventType.MouseReleased:
                this.OnMouseReleased(new MouseHookEventArgs(e));
                break;
            case EventType.MouseMoved:
                this.OnMouseMoved(new MouseHookEventArgs(e));
                break;
            case EventType.MouseDragged:
                this.OnMouseDragged(new MouseHookEventArgs(e));
                break;
            case EventType.MouseWheel:
                this.OnMouseWheel(new MouseWheelHookEventArgs(e));
                break;
        };
    }

    /// <summary>
    /// Raises the <see cref="HookEnabled" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnHookEnabled(HookEventArgs args) =>
        this.HookEnabled?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="HookDisabled" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnHookDisabled(HookEventArgs args) =>
        this.HookDisabled?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="KeyTyped" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnKeyTyped(KeyboardHookEventArgs args) =>
        this.KeyTyped?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="KeyPressed" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnKeyPressed(KeyboardHookEventArgs args) =>
        this.KeyPressed?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="KeyReleased" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnKeyReleased(KeyboardHookEventArgs args) =>
        this.KeyReleased?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseClicked" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseClicked(MouseHookEventArgs args) =>
        this.MouseClicked?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MousePressed" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMousePressed(MouseHookEventArgs args) =>
        this.MousePressed?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseReleased" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseReleased(MouseHookEventArgs args) =>
        this.MouseReleased?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseMoved" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseMoved(MouseHookEventArgs args) =>
        this.MouseMoved?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseDragged" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseDragged(MouseHookEventArgs args) =>
        this.MouseDragged?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseWheel" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseWheel(MouseWheelHookEventArgs args) =>
        this.MouseWheel?.Invoke(this, args);

    /// <summary>
    /// Destoys the global hook.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true" /> if the method is called from the <see cref="Dispose()" /> method.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;

        if (this.IsRunning)
        {
            var result = UioHook.Stop();

            if (disposing && result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatFailureMessage("stopping", result));
            }
        }
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException" /> if this object is disposed.
    /// </summary>
    /// <param name="method">The method which calls this method.</param>
    protected void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} - the object is disposed");
        }
    }

    private void HandleHookEventIfNeeded(ref UioHookEvent e)
    {
        if (this.ShouldDispatchEvent(ref e))
        {
            this.HandleHookEvent(ref e);
        }
    }

    private bool ShouldDispatchEvent(ref UioHookEvent e) =>
        e.Type switch
        {
            EventType.HookEnabled => this.HookEnabled != null,
            EventType.HookDisabled => this.HookDisabled != null,
            EventType.KeyTyped => this.KeyTyped != null,
            EventType.KeyPressed => this.KeyPressed != null,
            EventType.KeyReleased => this.KeyReleased != null,
            EventType.MouseClicked => this.MouseClicked != null,
            EventType.MousePressed => this.MousePressed != null,
            EventType.MouseReleased => this.MouseReleased != null,
            EventType.MouseMoved => this.MouseMoved != null,
            EventType.MouseDragged => this.MouseDragged != null,
            EventType.MouseWheel => this.MouseWheel != null,
            _ => false
        };

    private string FormatFailureMessage(string action, UioHookResult result) =>
        $"Failed {action} the global hook: {result} ({(int)result:x})";

    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IGlobalHook.Start" /> method is called.</remarks>
    public event EventHandler<HookEventArgs>? HookEnabled;

    /// <summary>
    /// An event which is raised when the global hook is disabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IDisposable.Dispose" /> method is called.</remarks>
    public event EventHandler<HookEventArgs>? HookDisabled;

    /// <summary>
    /// An event which is raised when a key is typed.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyReleased;

    /// <summary>
    /// An event which is raised when a mouse button is clicked.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseClicked;

    /// <summary>
    /// An event which is raised when a mouse button is pressed.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MousePressed;

    /// <summary>
    /// An event which is raised when a mouse button is released.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseMoved;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    public event EventHandler<MouseHookEventArgs>? MouseDragged;

    /// <summary>
    /// An event which is raised when the mouse wheel is turned.
    /// </summary>
    public event EventHandler<MouseWheelHookEventArgs>? MouseWheel;
}
