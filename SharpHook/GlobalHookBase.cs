namespace SharpHook;

/// <summary>
/// Represents an abstract implementation of <see cref="IGlobalHook" /> which defines everything needed except for
/// a strategy for dispatching events and uses a global hook provider (libuiohook by default).
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
/// <seealso cref="BasicGlobalHookBase" />
public abstract class GlobalHookBase : BasicGlobalHookBase, IGlobalHook
{
    /// <summary>
    /// Initializes a new instance of <see cref="GlobalHookBase" />.
    /// </summary>
    /// <param name="globalHookType">The global hook type.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IBasicGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    protected GlobalHookBase(
        GlobalHookType globalHookType = GlobalHookType.All,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
        : base(globalHookType, globalHookProvider, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// When implemented in a derived class, represents a strategy for handling a hook event.
    /// </summary>
    /// <param name="e">The event to handle.</param>
    /// <remarks>
    /// Derived classes should call <see cref="DispatchEvent(ref UioHookEvent)" /> inside this method to raise the
    /// appropriate event.
    /// </remarks>
    protected override abstract void HandleHookEvent(ref UioHookEvent e);

    /// <summary>
    /// Returns a value which indicates whether there are any subscribers to an event which corresponds to the hook
    /// event type.
    /// </summary>
    /// <param name="e">The event to check.</param>
    /// <returns>
    /// <see langword="true" /> if there are any subscribers to an event which corresponds to the hook event type.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// Derived classes may call this method as an optimization before attempting to dispatch the hook event.
    /// </remarks>
    protected bool ShouldDispatchEvent(ref UioHookEvent e) =>
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
        }

        if (args is not null && args.SuppressEvent)
        {
            e.Mask |= EventMask.SuppressEvent;
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
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>
    /// This event is raised when the <see cref="IBasicGlobalHook.Run" /> or <see cref="IBasicGlobalHook.RunAsync" />
    /// method is called.
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
