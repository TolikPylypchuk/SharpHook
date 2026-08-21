namespace SharpHook;

/// <summary>
/// Represents an abstract implementation of <see cref="IGlobalHook" /> which defines everything needed except for
/// a strategy for dispatching events and uses a global hook provider (libuiohook by default).
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="EventLoopGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
/// <seealso cref="BasicGlobalHookBase" />
public abstract class GlobalHookBase : BasicGlobalHookBase, IGlobalHook
{
    /// <summary>
    /// Initializes a new instance of <see cref="GlobalHookBase" />.
    /// </summary>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    protected GlobalHookBase(IGlobalHookProvider? globalHookProvider = null)
        : base(globalHookProvider)
    { }

    /// <summary>
    /// When implemented in a derived class, represents a strategy for handling a hook event.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <remarks>
    /// Derived classes should call <see cref="DispatchEvent(ref UioHookEvent)" /> inside this method to raise the
    /// appropriate event. They can also call <see cref="ShouldDispatchEvent(ref UioHookEvent)" /> to determine whether
    /// to attempt dispatching the event at all.
    /// </remarks>
    protected override abstract void HandleHookEvent(ref UioHookEvent @event);

    /// <summary>
    /// Returns a value which indicates whether there are any subscribers to an event which corresponds to the hook
    /// event type.
    /// </summary>
    /// <param name="event">The event to check.</param>
    /// <returns>
    /// <see langword="true" /> if there are any subscribers to an event which corresponds to the hook event type.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// Derived classes may call this method as an optimization before attempting to dispatch the hook event.
    /// </remarks>
    protected bool ShouldDispatchEvent(ref UioHookEvent @event) =>
        @event.Type switch
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
            EventType.MouseMovedRelative => this.MouseMovedRelative != null,
            EventType.MouseDragged => this.MouseDragged != null,
            EventType.MouseDraggedRelative => this.MouseDraggedRelative != null,
            EventType.MouseWheel => this.MouseWheel != null,
            _ => false
        };

    /// <summary>
    /// Dispatches an event from the global hook provider, i.e. raises the appropriate event.
    /// </summary>
    /// <param name="event">The event to dispatch.</param>
    protected void DispatchEvent(ref UioHookEvent @event)
    {
        HookEventArgs? args = null;

        switch (@event.Type)
        {
            case EventType.HookEnabled:
                this.OnHookEnabled(args = new HookEventArgs(@event));
                break;
            case EventType.HookDisabled:
                this.OnHookDisabled(args = new HookEventArgs(@event));
                break;
            case EventType.KeyTyped:
                var keyTypedArgs = new KeyboardHookEventArgs(@event);
                args = keyTypedArgs;
                this.OnKeyTyped(keyTypedArgs);
                break;
            case EventType.KeyPressed:
                var keyPressedArgs = new KeyboardHookEventArgs(@event);
                args = keyPressedArgs;
                this.OnKeyPressed(keyPressedArgs);
                break;
            case EventType.KeyReleased:
                var keyReleasedArgs = new KeyboardHookEventArgs(@event);
                args = keyReleasedArgs;
                this.OnKeyReleased(keyReleasedArgs);
                break;
            case EventType.MouseClicked:
                var mouseClickedArgs = new MouseHookEventArgs(@event);
                args = mouseClickedArgs;
                this.OnMouseClicked(mouseClickedArgs);
                break;
            case EventType.MousePressed:
                var mousePressedArgs = new MouseHookEventArgs(@event);
                args = mousePressedArgs;
                this.OnMousePressed(mousePressedArgs);
                break;
            case EventType.MouseReleased:
                var mouseReleasedArgs = new MouseHookEventArgs(@event);
                args = mouseReleasedArgs;
                this.OnMouseReleased(mouseReleasedArgs);
                break;
            case EventType.MouseMoved:
                var mouseMovedArgs = new MouseHookEventArgs(@event);
                args = mouseMovedArgs;
                this.OnMouseMoved(mouseMovedArgs);
                break;
            case EventType.MouseMovedRelative:
                var mouseMovedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseMovedRelativeArgs;
                this.OnMouseMovedRelative(mouseMovedRelativeArgs);
                break;
            case EventType.MouseDragged:
                var mouseDraggedArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedArgs;
                this.OnMouseDragged(mouseDraggedArgs);
                break;
            case EventType.MouseDraggedRelative:
                var mouseDraggedRelativeArgs = new MouseHookEventArgs(@event);
                args = mouseDraggedRelativeArgs;
                this.OnMouseDraggedRelative(mouseDraggedRelativeArgs);
                break;
            case EventType.MouseWheel:
                var mouseWheelArgs = new MouseWheelHookEventArgs(@event);
                args = mouseWheelArgs;
                this.OnMouseWheel(mouseWheelArgs);
                break;
        }

        if (args is not null && args.SuppressEvent)
        {
            @event.Mask |= EventMask.SuppressEvent;
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
    /// Raises the <see cref="MouseMovedRelative" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseMovedRelative(MouseHookEventArgs args) =>
        this.MouseMovedRelative?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseDragged" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseDragged(MouseHookEventArgs args) =>
        this.MouseDragged?.Invoke(this, args);

    /// <summary>
    /// Raises the <see cref="MouseDraggedRelative" /> event with this object as the sender.
    /// </summary>
    /// <param name="args">The arguments of the event.</param>
    protected virtual void OnMouseDraggedRelative(MouseHookEventArgs args) =>
        this.MouseDraggedRelative?.Invoke(this, args);

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
    public event EventHandler<KeyboardHookEventArgs>? KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is not raised repeatedly if the key is kept down. To check whether key auto-repeat events
    /// are reported, you can use <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.KeyAutoRepeat" /> as one of the supported features, then this event will be raised
    /// repeatedly if the key is kept down.
    /// </remarks>
    public event EventHandler<KeyboardHookEventArgs>? KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    public event EventHandler<KeyboardHookEventArgs>? KeyReleased;

    /// <summary>
    /// An event which is raised when a mouse button is clicked.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseClicked;

    /// <summary>
    /// An event which is raised when a mouse button is pressed.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MousePressed;

    /// <summary>
    /// An event which is raised when a mouse button is released.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseMovedRelative" /> instead.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseMoved;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved relatively to its previous position.
    /// </summary>
    /// <remarks>
    /// This event is raised only on Wayland when using a relative pointing device (which most normal mice are). To
    /// check whether the current platform can raise this event, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method does not return
    /// <see cref="UioHookFeature.AbsoluteMouseMovement" /> as one of the supported features, then the current platform
    /// may raise this event.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseMovedRelative;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseDraggedRelative" /> instead.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseDragged;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged relatively to its previous position.
    /// </summary>
    /// <remarks>
    /// This event is raised only on Wayland when using a relative pointing device (which most normal mice are). To
    /// check whether the current platform can raise this event, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method does not return
    /// <see cref="UioHookFeature.AbsoluteMouseMovement" /> as one of the supported features, then the current platform
    /// may raise this event.
    /// </remarks>
    public event EventHandler<MouseHookEventArgs>? MouseDraggedRelative;

    /// <summary>
    /// An event which is raised when the mouse wheel is turned.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    public event EventHandler<MouseWheelHookEventArgs>? MouseWheel;
}
