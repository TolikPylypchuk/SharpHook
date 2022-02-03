namespace SharpHook;

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using SharpHook.Native;

/// <summary>
/// Represents an abstract implementation of <see cref="IGlobalHook" /> which raises events only when there is at least
/// one subscriber.
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
public abstract class GlobalHookBase : IGlobalHook
{
    private const string Starting = "starting";
    private const string Stopping = "stopping";

    private bool disposed = false;

    /// <summary>
    /// Destroys the global hook if it's running.
    /// </summary>
    ~GlobalHookBase() =>
        this.Dispose(false);

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

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

        try
        {
            UioHook.SetDispatchProc(this.HandleHookEventIfNeeded);

            this.IsRunning = true;
            var result = UioHook.Run();
            this.IsRunning = false;

            if (result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatFailureMessage(Starting, result));
            }
        } catch (Exception e)
        {
            this.IsRunning = false;
            throw new HookException(UioHookResult.Failure, e);
        }
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
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
                    source.SetException(new HookException(result, this.FormatFailureMessage(Starting, result)));
                }
            }
            catch (Exception e)
            {
                this.IsRunning = false;
                source.SetException(new HookException(UioHookResult.Failure, e));
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
        HookEventArgs? args = null;

        switch (e.Type)
        {
            case EventType.HookEnabled:
                this.OnHookEnabled(args = new HookEventArgs(e));
                break;
            case EventType.HookDisabled:
                UioHook.SetDispatchProc(UioHook.EmptyDispatchProc);
                this.OnHookDisabled(args = new HookEventArgs(e));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(e);
                args = keyTypedArgs;
                this.OnKeyTyped(keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(e);
                args = keyPressedArgs;
                this.OnKeyPressed(keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(e);
                args = keyReleasedArgs;
                this.OnKeyReleased(keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(e);
                args = mouseClickedArgs;
                this.OnMouseClicked(mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(e);
                args = mousePressedArgs;
                this.OnMousePressed(mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(e);
                args = mouseReleasedArgs;
                this.OnMouseReleased(mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(e);
                args = mouseMovedArgs;
                this.OnMouseMoved(mouseMovedArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(e);
                args = mouseDraggedArgs;
                this.OnMouseDragged(mouseDraggedArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(e);
                args = mouseWheelArgs;
                this.OnMouseWheel(mouseWheelArgs);
                break;
        };

        if (args != null && args.Reserved.HasValue)
        {
            e.Reserved = args.Reserved.Value;
        }
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
                throw new HookException(result, this.FormatFailureMessage(Stopping, result));
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

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            throw new InvalidOperationException("The global hook is already running");
        }
    }

    private string FormatFailureMessage(string action, UioHookResult result) =>
        $"Failed {action} the global hook: {result} ({(int)result:x})";

    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>
    /// This event is raised when the <see cref="IGlobalHook.Run" /> or <see cref="IGlobalHook.RunAsync" /> method
    /// is called.
    /// </remarks>
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
