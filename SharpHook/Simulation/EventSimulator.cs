namespace SharpHook.Simulation;

/// <summary>
/// A keyboard and mouse event simulator which posts events to a simulation provider (libuiohook by default).
/// </summary>
/// <seealso cref="IEventSimulationProvider" />
/// <seealso cref="IEventSimulationSequenceBuilder" />
/// <seealso cref="IEventSimulationSequenceTemplate" />
public sealed class EventSimulator : IEventSimulator
{
    private EventSimulator(IEventSimulationProvider simulationProvider) =>
        this.SimulationProvider = simulationProvider;

    /// <summary>
    /// Destroys virtual input devices.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~EventSimulator()
    {
        try
        {
            this.SimulationProvider.DestroyVirtualDevices();
        } catch { }
    }

    /// <summary>
    /// Gets or sets the delay between simulating individual characters when simulating text on Linux.
    /// </summary>
    /// <value>The delay between simulating individual characters when simulating text on Linux.</value>
    /// <remarks>
    /// <para>
    /// X11 doesn't support simulating arbitrary Unicode characters directly. Instead, for each character,
    /// an unused key code is remapped to that character, and then key press/release is simulated. Since the receiving
    /// application must react to the remapping, and may not do so instantaneously, a delay is needed for accurate
    /// simulation.
    /// </para>
    /// <para>
    /// The default delay is 50 milliseconds.
    /// </para>
    /// <para>
    /// On Windows and macOS, as well as Wayland, this property is ignored.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value" /> represents a negative time span.
    /// </exception>
    public TimeSpan TextSimulationDelayOnLinux
    {
        get => TimeSpan.FromTicks((long)this.SimulationProvider.PostTextDelayLinux / 100);
        set
        {
            if (value.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            this.SimulationProvider.PostTextDelayLinux = (ulong)(value.Ticks * 100);
        }
    }

    /// <summary>
    /// Gets the value which indicates whether the event simulator has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the event simulator has been disposed. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>A disposed event simulator cannot be used to simulate events.</remarks>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Gets the simulation provider used by this event simulator.
    /// </summary>
    internal IEventSimulationProvider SimulationProvider { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="EventSimulator" /> class. On Linux, this method also initializes
    /// virtual input devices for event simulation.
    /// </summary>
    /// <param name="applicationName">
    /// The application name which is used to identify the virtual input devices. A <see langword="null" /> or empty
    /// string is technnically allowed, but not recommended.
    /// </param>
    /// <param name="simulationProvider">
    /// The simulation functionality provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <exception cref="HookException">Initialization of virtual devices on Linux has failed.</exception>
    /// <returns>A new instance of <see cref="EventSimulator" />.</returns>
    /// <remarks>
    /// <para>
    /// Virtual input devices are required on Linux when using a uinput-based backend. On Windows, macOS, and the
    /// XRecord-based X11 backend, device initialization is a no-op.
    /// </para>
    /// <para>
    /// Initializing virtual devices is expensive, so it is recommended to reuse the same instance of
    /// <see cref="EventSimulator" />. Virtual devices are destroyed when the <see cref="EventSimulator" /> instance is
    /// disposed.
    /// </para>
    /// <para>
    /// If an instance of <see cref="EventSimulator" /> is created when another instance is already active, virtual
    /// devices will not be initialized again. Instead, a reference counter will be incremented. Virtual devices will
    /// only be destroyed when all active instances of <see cref="EventSimulator" /> are disposed.
    /// </para>
    /// </remarks>
    public static EventSimulator Create(string applicationName, IEventSimulationProvider? simulationProvider = null)
    {
        simulationProvider ??= UioHookProvider.Instance;
        var result = simulationProvider.InitializeVirtualDevices(applicationName);

        if (result != UioHookResult.Success)
        {
            throw new HookException(result, $"Failed to initialize virtual devices: {result}");
        }

        return new(simulationProvider);
    }

    /// <summary>
    /// Simulates pressing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateKeyPress(KeyCode keyCode) =>
        this.PostEvent(new()
        {
            Type = EventType.KeyPressed,
            Keyboard = new() { KeyCode = keyCode }
        });

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateKeyRelease(KeyCode keyCode) =>
        this.PostEvent(new()
        {
            Type = EventType.KeyReleased,
            Keyboard = new() { KeyCode = keyCode }
        });

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(MouseButton button) =>
        this.SimulateMousePress(button, 0);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(MouseButton button, ushort clicks) =>
        this.PostEvent(new()
        {
            Type = EventType.MousePressedIgnoreCoordinates,
            Mouse = new() { Button = button, Clicks = clicks }
        });

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button) =>
        this.SimulateMousePress(x, y, button, 0);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button, ushort clicks) =>
        this.PostEvent(new()
        {
            Type = EventType.MousePressed,
            Mouse = new() { Button = button, X = x, Y = y, Clicks = clicks }
        });

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(MouseButton button) =>
        this.SimulateMouseRelease(button, 0);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(MouseButton button, ushort clicks) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseReleasedIgnoreCoordinates,
            Mouse = new() { Button = button, Clicks = clicks }
        });

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button) =>
        this.SimulateMouseRelease(x, y, button, 0);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button, ushort clicks) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseReleased,
            Mouse = new() { Button = button, X = x, Y = y, Clicks = clicks }
        });

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseMovement(short x, short y) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseMoved,
            Mouse = new() { X = x, Y = y }
        });

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseMovementRelative(short x, short y) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseMovedRelative,
            Mouse = new() { X = x, Y = y }
        });

    /// <summary>
    /// Simulates scrolling the mouse wheel.
    /// </summary>
    /// <param name="rotation">
    /// The wheel rotation. A positive value indicates that the wheel will be rotated up or left,
    /// and a negative value indicates that the wheel will be rotated down or right.
    /// </param>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="type">The scroll type (considered only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// On Windows, the value <c>120</c> represents the default wheel step. As such, multiples of <c>120</c> can be
    /// used, but it's not required. The value of <paramref name="type" /> is ignored.
    /// </para>
    /// <para>
    /// On macOS, it's recommended to use values between <c>-10</c> and <c>10</c>. This will result in quite a small
    /// scroll amount with pixel scrolling, so <see cref="MouseWheelScrollType.BlockScroll" /> is recommended for line
    /// scrolling instead of pixel scrolling.
    /// </para>
    /// <para>
    /// On Linux, there is no fixed recommendation, but multiples of <c>100</c> can be used. The value of
    /// <paramref name="type" /> is ignored.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseWheel,
            Wheel = new() { Rotation = rotation, Direction = direction, Type = type }
        });

    /// <summary>
    /// Simulates the input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
    /// surrogate pairs, e.g. emojis) is supported.
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
    /// simulation on Linux works slowly and is not guaranteed to be correct. <see cref="TextSimulationDelayOnLinux" />
    /// can be used to increase (or decrease) the delay if needed – longer delays add consistency but may be more
    /// jarring to end users. <see cref="TextSimulationDelayOnLinux" /> can also be used to get the currently configured
    /// delay – the default is 50 milliseconds.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    public UioHookResult SimulateTextEntry(string text)
    {
        this.ThrowIfDisposed();
        return this.SimulationProvider.PostText(text ?? throw new ArgumentNullException(nameof(text)));
    }

    /// <summary>
    /// Initializes a builder for a sequence of events that can be simulated together.
    /// </summary>
    /// <returns>A builder for a sequence of events that can be simulated together.</returns>
    /// <remarks>The builder is automatically disposed when this simulator is disposed or garbage-collected.</remarks>
    public IEventSimulationSequenceBuilder Sequence()
    {
        this.ThrowIfDisposed();
        return new EventSimulationSequenceBuilder(this);
    }

    /// <summary>
    /// Disposes of the event simulator and destroys virtual input devices on Linux.
    /// </summary>
    /// <exception cref="HookException">Destroying virtual devices on Linux has failed.</exception>
    /// <remarks>
    /// <para>
    /// This method also disposes of any <see cref="IEventSimulationSequenceBuilder" /> and
    /// <see cref="IEventSimulationSequenceTemplate" /> instances owned by this event simulator.
    /// </para>
    /// <para>
    /// Virtual devices are destroyed only when all active instances of <see cref="EventSimulator" /> are disposed.
    /// </para>
    /// </remarks>
    public void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        var result = SimulationProvider.DestroyVirtualDevices();

        if (result != UioHookResult.Success)
        {
            throw new HookException(result, $"Failed to destroy virtual devices: {result}");
        }

        GC.SuppressFinalize(this);
        this.IsDisposed = true;

        this.OnDisposed?.Invoke();
    }

    private UioHookResult PostEvent(UioHookEvent @event)
    {
        this.ThrowIfDisposed();
        return this.SimulationProvider.PostEvent(ref @event);
    }

    private void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.IsDisposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} – the object is disposed");
        }
    }

    internal event Action? OnDisposed;
}
