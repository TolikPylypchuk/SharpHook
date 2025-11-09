namespace SharpHook;

/// <summary>
/// Represents a builder for a sequence of events that can be simulated together.
/// </summary>
public sealed class EventSimulationSequenceBuilder : IEventSimulationSequenceBuilder
{
    private readonly List<UioHookEvent> events = [];
    private readonly IEventSimulationProvider simulationProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSimulationSequenceBuilder" /> class.
    /// </summary>
    /// <param name="simulationProvider">
    /// The simulation functionality provider (or <see cref="UioHookProvider.Instance" /> if <see langword="null" />).
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public EventSimulationSequenceBuilder(IEventSimulationProvider? simulationProvider = null) =>
        this.simulationProvider = simulationProvider ?? UioHookProvider.Instance;

    /// <summary>
    /// Adds the specified event to the sequence of events to simulate.
    /// </summary>
    /// <param name="e">The event to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddEvent(UioHookEvent e)
    {
        this.events.Add(e);
        return this;
    }

    /// <summary>
    /// Adds the specified events to the sequence of events to simulate.
    /// </summary>
    /// <param name="events">The events to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddEvents(params UioHookEvent[] events)
    {
        this.events.AddRange(events);
        return this;
    }

    /// <summary>
    /// Adds the specified events to the sequence of events to simulate.
    /// </summary>
    /// <param name="events">The events to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddEvents(IEnumerable<UioHookEvent> events)
    {
        this.events.AddRange(events);
        return this;
    }

    /// <summary>
    /// Adds a key press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddKeyPress(KeyCode keyCode)
    {
        this.events.Add(new()
        {
            Type = EventType.KeyPressed,
            Keyboard = new() { KeyCode = keyCode }
        });

        return this;
    }

    /// <summary>
    /// Adds a key release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddKeyRelease(KeyCode keyCode)
    {
        this.events.Add(new()
        {
            Type = EventType.KeyReleased,
            Keyboard = new() { KeyCode = keyCode }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMousePress(MouseButton button) =>
        this.AddMousePress(button, 0);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMousePress(MouseButton button, ushort clicks)
    {
        this.events.Add(new()
        {
            Type = EventType.MousePressedIgnoreCoordinates,
            Mouse = new() { Button = button, Clicks = clicks }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMousePress(short x, short y, MouseButton button) =>
        this.AddMousePress(x, y, button, 0);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMousePress(short x, short y, MouseButton button, ushort clicks)
    {
        this.events.Add(new()
        {
            Type = EventType.MousePressed,
            Mouse = new() { Button = button, X = x, Y = y, Clicks = clicks }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseRelease(MouseButton button) =>
        this.AddMouseRelease(button, 0);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseRelease(MouseButton button, ushort clicks)
    {
        this.events.Add(new()
        {
            Type = EventType.MouseReleasedIgnoreCoordinates,
            Mouse = new() { Button = button, Clicks = clicks }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseRelease(short x, short y, MouseButton button) =>
        this.AddMouseRelease(x, y, button, 0);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseRelease(short x, short y, MouseButton button, ushort clicks)
    {
        this.events.Add(new()
        {
            Type = EventType.MouseReleased,
            Mouse = new() { Button = button, X = x, Y = y, Clicks = clicks }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse movement event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseMovement(short x, short y)
    {
        this.events.Add(new()
        {
            Type = EventType.MouseMoved,
            Mouse = new() { X = x, Y = y }
        });

        return this;
    }

    /// <summary>
    /// Adds a relative mouse movement event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder AddMouseMovementRelative(short x, short y)
    {
        this.events.Add(new()
        {
            Type = EventType.MouseMovedRelativeToCursor,
            Mouse = new() { X = x, Y = y }
        });

        return this;
    }

    /// <summary>
    /// Adds a mouse wheel scroll event to the sequence of events to simulate.
    /// </summary>
    /// <param name="rotation">
    /// The wheel rotation. A positive value indicates that the wheel will be rotated up or left,
    /// and a negative value indicates that the wheel will be rotated down or right.
    /// </param>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="type">The scroll type (considered only on macOS).</param>
    /// <returns>The current builder.</returns>
    /// <remarks>
    /// <para>
    /// On Windows, the value <c>120</c> represents the default wheel step. As such, multiples of <c>120</c> can be
    /// used, but it's not required. The value of <paramref name="type" /> is ignored.
    /// </para>
    /// <para>
    /// On macOS it's recommended to use values between <c>-10</c> and <c>10</c>. This will result in quite a small
    /// scroll amount with pixel scrolling, so <see cref="MouseWheelScrollType.BlockScroll" /> is recommended for line
    /// scrolling instead of pixel scrolling.
    /// </para>
    /// <para>
    /// On Linux there is no fixed recommendation, but multiples of <c>100</c> can be used. The value of
    /// <paramref name="type" /> is ignored.
    /// </para>
    /// </remarks>
    public IEventSimulationSequenceBuilder AddMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll)
    {
        this.events.Add(new()
        {
            Type = EventType.MouseWheel,
            Wheel = new() { Rotation = rotation, Direction = direction, Type = type }
        });

        return this;
    }

    /// <summary>
    /// Removes the specified event from the sequence of events to simulate.
    /// </summary>
    /// <param name="e">The event to remove from the sequence.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder RemoveEvent(UioHookEvent e)
    {
        this.events.Remove(e);
        return this;
    }

    /// <summary>
    /// Removes the specified events from the sequence of events to simulate.
    /// </summary>
    /// <param name="predicate">The predicate to check if the event should be removed from the sequence or not.</param>
    /// <returns>The current builder.</returns>
    public IEventSimulationSequenceBuilder RemoveEvents(Func<UioHookEvent, bool> predicate)
    {
        this.events.RemoveAll(e => predicate(e));
        return this;
    }

    /// <summary>
    /// Simulates the events in this sequence.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Simulate() =>
        this.simulationProvider.PostEvents([.. this.events], (uint)this.events.Count);

    /// <summary>
    /// Creates a template based on the events in this sequence.
    /// </summary>
    /// <returns>A template based on the events in this sequence.</returns>
    public IEventSimulationSequenceTemplate CreateTemplate() =>
        new EventSimulationSequenceTemplate(this.events, this.simulationProvider);
}
