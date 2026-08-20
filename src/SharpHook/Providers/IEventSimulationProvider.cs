namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level event simulation functionality.
/// </summary>
public interface IEventSimulationProvider
{
    /// <summary>
    /// Gets or sets the delay (in nanoseconds) between posting individual characters when posting text on Linux.
    /// </summary>
    /// <value>The delay (in nanoseconds) between posting individual characters when posting text on Linux.</value>
    /// <remarks>
    /// <para>
    /// X11 doesn't support posting arbitrary Unicode characters directly. Instead, for each character,
    /// an unused key code is remapped to that character, and then key press/release is simulated. Since the receiving
    /// application must react to the remapping, and may not do so instantaneously, a delay is needed for accurate
    /// simulation.
    /// </para>
    /// <para>
    /// The default delay is 50 milliseconds.
    /// </para>
    /// <para>
    /// On Windows and macOS, as well as Wayland, this property does nothing and always returns <c>0</c>.
    /// </para>
    /// </remarks>
    ulong PostTextDelayLinux { get; set; }

    /// <summary>
    /// Posts a fake input event.
    /// </summary>
    /// <param name="event">The event to post.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The instance of the event doesn't need all fields to have value. Only <see cref="UioHookEvent.Type" />,
    /// <see cref="UioHookEvent.Keyboard" />/<see cref="UioHookEvent.Mouse" />/<see cref="UioHookEvent.Wheel" /> should
    /// be present.
    /// </para>
    /// <para>
    /// The following table describes the specifics of simulating each event type.
    /// <list type="table">
    /// <listheader>
    /// <term>Event type</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term><see cref="EventType.HookEnabled" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.HookDisabled" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyPressed" /></term>
    /// <term>Only <see cref="KeyboardEventData.KeyCode" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyReleased" /></term>
    /// <term>Only <see cref="KeyboardEventData.KeyCode" /> is considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.KeyTyped" /></term>
    /// <term>
    /// Events of this type are ignored. <see cref="PostText(string)" /> should be used to post Unicode characters.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MousePressed" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.X" />, <see cref="MouseWheelEventData.Y" />,
    /// and <see cref="MouseEventData.Button" /> are considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MousePressedIgnoreCoordinates" /></term>
    /// <term>
    /// Only <see cref="MouseEventData.Button" /> is considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseReleased" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.X" />, <see cref="MouseWheelEventData.Y" />,
    /// and <see cref="MouseEventData.Button" /> are considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseReleasedIgnoreCoordinates" /></term>
    /// <term>
    /// Only <see cref="MouseEventData.Button" /> is considered.
    /// </term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseClicked" /></term>
    /// <term>Events of this type are ignored.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseMoved" /></term>
    /// <term>Only <see cref="MouseEventData.X" /> and <see cref="MouseEventData.Y" /> are considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseMovedRelative" /></term>
    /// <term>Only <see cref="MouseEventData.X" /> and <see cref="MouseEventData.Y" /> are considered.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDragged" /></term>
    /// <term>Not recommended to use; same as <see cref="EventType.MouseMoved" />.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseDraggedRelative" /></term>
    /// <term>Not recommended to use; same as <see cref="EventType.MouseMovedRelative" />.</term>
    /// </item>
    /// <item>
    /// <term><see cref="EventType.MouseWheel" /></term>
    /// <term>
    /// Only <see cref="MouseWheelEventData.Rotation" />, <see cref="MouseWheelEventData.Direction" />, and
    /// <see cref="MouseWheelEventData.Type" /> are considered.
    /// </term>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="PostEvents" />
    UioHookResult PostEvent(ref UioHookEvent @event);

    /// <summary>
    /// Posts a sequence of fake input events.
    /// </summary>
    /// <param name="events">The events to post.</param>
    /// <param name="size">The number of events to post.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// All the same rules apply as to <see cref="PostEvent" />. The sequence of events must not contain events of types
    /// <see cref="EventType.KeyTyped" /> or <see cref="EventType.MouseClicked" />, or the method will fail, potentially
    /// in the middle of the simulation sequence.
    /// </remarks>
    /// <seealso cref="PostEvent" />
    UioHookResult PostEvents(UioHookEvent[] events, uint size);

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
    /// surrogate pairs, e.g., emojis) is supported.
    /// </para>
    /// <para>
    /// On Windows, text simulation should work correctly and consistently.
    /// </para>
    /// <para>
    /// On macOS, applications are not required to process text simulation, but most of them should handle it correctly.
    /// </para>
    /// <para>
    /// X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to
    /// that character, and then key press/release is simulated. Since the receiving application must react to the
    /// remapping, and may not do so instantaneously, a delay is needed for accurate simulation. This means that text
    /// entry on Linux works slowly and is not guaranteed to be correct. <see cref="PostTextDelayLinux" /> can be used
    /// to get or set the delay if needed – longer delays add consistency but may be more jarring to end
    /// users – the default is 50 milliseconds.
    /// </para>
    /// </remarks>
    UioHookResult PostText(string text);

    /// <summary>
    /// Initializes virtual input devices used for event simulation.
    /// </summary>
    /// <param name="applicationName">The application name which is used to identify the virtual devices.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Virtual input devices are required on Linux when using a uinput-based backend. On Windows, macOS, and the
    /// XRecord-based X11 backend, this method does nothing and always returns <see cref="UioHookResult.Success" />.
    /// </para>
    /// <para>
    /// Initializing virtual input devices is expensive, so it should generally be done once early in the application's
    /// lifetime. If virtual devices are initialzed, then calling this method again will increase the reference counter
    /// – the devices will be destroyed only when the reference counter reaches zero, i.e., when the same number of
    /// calls to <see cref="DestroyVirtualDevices" /> are made.
    /// </para>
    /// </remarks>
    /// <seealso cref="DestroyVirtualDevices" />
    UioHookResult InitializeVirtualDevices(string applicationName);

    /// <summary>
    /// Destroys virtual input devices used for event simulation.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Virtual input devices are required on Linux when using a uinput-based backend. On Windows, macOS, and the
    /// XRecord-based X11 backend, this method does nothing and always returns <see cref="UioHookResult.Success" />.
    /// </para>
    /// <para>
    /// If multiple calls to <see cref="InitializeVirtualDevices" /> were made, then this method must be called the same
    /// number of times, and will actually destroy the virtual devices only when the reference counter reaches zero.
    /// </para>
    /// <para>
    /// When virtual devices are not initialized, this method does nothing.
    /// </para>
    /// </remarks>
    /// <seealso cref="DestroyVirtualDevices" />
    UioHookResult DestroyVirtualDevices();
}
