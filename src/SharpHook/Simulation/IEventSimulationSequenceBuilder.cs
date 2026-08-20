namespace SharpHook.Simulation;

/// <summary>
/// Represents a builder for a sequence of events that can be simulated together.
/// </summary>
public interface IEventSimulationSequenceBuilder : IStatefulDisposable
{
    /// <summary>
    /// Adds the specified event to the sequence of events to simulate.
    /// </summary>
    /// <param name="event">The event to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="event" /> has type <see cref="EventType.KeyTyped" /> or <see cref="EventType.MouseClicked" />.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddEvent(UioHookEvent @event);

    /// <summary>
    /// Adds the specified events to the sequence of events to simulate.
    /// </summary>
    /// <param name="events">The events to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="events" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// One of <paramref name="events" /> has type <see cref="EventType.KeyTyped" /> or
    /// <see cref="EventType.MouseClicked" />.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddEvents(params UioHookEvent[] events);

    /// <summary>
    /// Adds the specified events to the sequence of events to simulate.
    /// </summary>
    /// <param name="events">The events to add to the sequence.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="events" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// One of <paramref name="events" /> has type <see cref="EventType.KeyTyped" /> or
    /// <see cref="EventType.MouseClicked" />.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddEvents(IEnumerable<UioHookEvent> events);

    /// <summary>
    /// Adds a key press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddKeyPress(KeyCode keyCode);

    /// <summary>
    /// Adds a key release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddKeyRelease(KeyCode keyCode);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMousePress(MouseButton button);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMousePress(MouseButton button, ushort clicks);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMousePress(short x, short y, MouseButton button);

    /// <summary>
    /// Adds a mouse button press event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMousePress(short x, short y, MouseButton button, ushort clicks);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseRelease(MouseButton button);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseRelease(MouseButton button, ushort clicks);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseRelease(short x, short y, MouseButton button);

    /// <summary>
    /// Adds a mouse button release event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseRelease(short x, short y, MouseButton button, ushort clicks);

    /// <summary>
    /// Adds a mouse movement event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseMovement(short x, short y);

    /// <summary>
    /// Adds a relative mouse movement event to the sequence of events to simulate.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder AddMouseMovementRelative(short x, short y);

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
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
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
    IEventSimulationSequenceBuilder AddMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll);

    /// <summary>
    /// Removes the specified event from the sequence of events to simulate.
    /// </summary>
    /// <param name="event">The event to remove from the sequence.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder RemoveEvent(UioHookEvent @event);

    /// <summary>
    /// Removes the specified events from the sequence of events to simulate.
    /// </summary>
    /// <param name="predicate">The predicate to check if the event should be removed from the sequence.</param>
    /// <returns>The current builder.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceBuilder RemoveEvents(Func<UioHookEvent, bool> predicate);

    /// <summary>
    /// Simulates the events in this sequence.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    UioHookResult Simulate();

    /// <summary>
    /// Creates a template based on the events in this sequence.
    /// </summary>
    /// <returns>A template based on the events in this sequence.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    IEventSimulationSequenceTemplate CreateTemplate();
}
