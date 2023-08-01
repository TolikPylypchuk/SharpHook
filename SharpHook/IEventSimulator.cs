namespace SharpHook;

using System;

using SharpHook.Native;

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
    /// On Windows and macOS this property is ignored.
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
    /// Simulates the input of arbitrary Unicode characters.
    /// </summary>
    /// <param name="text">The text to simulate.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// Text entry simulation is not guaranteed to work correctly. View the docs for more info.
    /// </para>
    /// <para>
    /// The text to simulate doesn't depend on the current keyboard layout.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    UioHookResult SimulateTextEntry(string text);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(MouseButton button);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMousePress(short x, short y, MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    UioHookResult SimulateMouseRelease(short x, short y, MouseButton button);

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
    /// The wheel rotation. A positive value indicates that the wheel will be rotated up or right,
    /// and a negative value indicates that the wheel will be rotated down or left.
    /// </param>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="type">The scroll type (considered only on macOS).</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction = MouseWheelScrollDirection.Vertical,
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll);
}
