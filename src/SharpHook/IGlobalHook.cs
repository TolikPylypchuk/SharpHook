namespace SharpHook;

/// <summary>
/// Represents a global keyboard and mouse hook.
/// </summary>
/// <remarks>
/// It is highly recommended not to implement this interface directly. If you want to create a custom global hook, you
/// should instead extend the <see cref="GlobalHookBase" /> class as it correctly implements the invariants required for
/// advanced scenarios.
/// </remarks>
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="EventLoopGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
/// <seealso cref="IFeatureProvider" />
/// <seealso cref="IGlobalHookProvider" />
/// <seealso cref="UioHookProvider" />
public interface IGlobalHook : IBasicGlobalHook
{
    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>
    /// This event is raised when the <see cref="IBasicGlobalHook.Run" /> or <see cref="IBasicGlobalHook.RunAsync" />
    /// method is called.
    /// </remarks>
    event EventHandler<HookEventArgs> HookEnabled;

    /// <summary>
    /// An event which is raised when the global hook is disabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IDisposable.Dispose" /> method is called.</remarks>
    event EventHandler<HookEventArgs> HookDisabled;

    /// <summary>
    /// An event which is raised when a character is typed.
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
    event EventHandler<KeyboardHookEventArgs> KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is not raised repeatedly if the key is kept down. To check whether key auto-repeat events
    /// are reported, you can use <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.KeyAutoRepeat" /> as one of the supported features, then this event will be raised
    /// repeatedly if the key is kept down.
    /// </remarks>
    event EventHandler<KeyboardHookEventArgs> KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyReleased;

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
    event EventHandler<MouseHookEventArgs> MouseClicked;

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
    event EventHandler<MouseHookEventArgs> MousePressed;

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
    event EventHandler<MouseHookEventArgs> MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseMovedRelative" /> instead.
    /// </remarks>
    event EventHandler<MouseHookEventArgs> MouseMoved;

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
    event EventHandler<MouseHookEventArgs> MouseMovedRelative;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event is raised only when using an absolute pointing device (like a touchscreen or a mouse in
    /// a virtual machine). Relative pointing devices (like most normal mice) will raise
    /// <see cref="MouseDraggedRelative" /> instead.
    /// </remarks>
    event EventHandler<MouseHookEventArgs> MouseDragged;

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
    event EventHandler<MouseHookEventArgs> MouseDraggedRelative;

    /// <summary>
    /// An event which is raised when the mouse wheel is scrolled.
    /// </summary>
    /// <remarks>
    /// On Wayland, this event does not include mouse coordinates (they are always set to (0, 0)). To check whether
    /// mouse coordinates for this event are available, you can use
    /// <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that method returns
    /// <see cref="UioHookFeature.AbsoluteMouseButtonCoordinates" /> as one of the supported features, then this event
    /// will contain non-zero mouse coordinates.
    /// </remarks>
    event EventHandler<MouseWheelHookEventArgs> MouseWheel;
}
