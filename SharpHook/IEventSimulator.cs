namespace SharpHook;

/// <summary>
/// Represents an object which can simulate keyboard and mouse events.
/// </summary>
/// <remarks>
/// The methods of this interface correspond to constants defined in the <see cref="EventType" /> enum.
/// </remarks>
public interface IEventSimulator
{
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
    /// On Windows and macOS, this property is ignored.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value" /> represents a negative time span.
    /// </exception>
    TimeSpan TextSimulationDelayOnX11 { get; set; }

    /// <summary>
    /// Simulates pressing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateKeyPress(KeyCode keyCode);

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateKeyRelease(KeyCode keyCode);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(MouseButton button);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(MouseButton button, ushort clicks);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(short x, short y, MouseButton button);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(short x, short y, MouseButton button, ushort clicks);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(MouseButton button, ushort clicks);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(short x, short y, MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="clicks">The click count (used only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(short x, short y, MouseButton button, ushort clicks);

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseMovement(short x, short y);

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseMovementRelative(short x, short y);

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
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll);

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
    /// simulation on Linux works slowly and is not guaranteed to be correct. <see cref="TextSimulationDelayOnX11" />
    /// can be used to increase (or decrease) the delay if needed – longer delays add consistency but may be more
    /// jarring to end users. <see cref="TextSimulationDelayOnX11" /> can also be used to get the currently configured
    /// delay – the default is 50 milliseconds.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    UioHookResult SimulateTextEntry(string text);

    /// <summary>
    /// Initializes a builder for a sequence of events that can be simulated together.
    /// </summary>
    /// <returns>A builder for a sequence of events that can be simulated together.</returns>
    IEventSimulationSequenceBuilder Sequence();
}
