namespace SharpHook;

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
    /// Simulates pressing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    public void SimulateKeyPress(KeyCode keyCode);

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    public void SimulateKeyRelease(KeyCode keyCode);

    /// <summary>
    /// Simulates pressing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    public void SimulateMousePress(MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    public void SimulateMouseRelease(MouseButton button);

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    public void SimulateMouseMovement(short x, short y);

    /// <summary>
    /// Simulates scrolling a mouse wheel.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="amount">The scrolling amount.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <remarks>
    /// A positive <paramref name="rotation" /> value indicates that the wheel was rotated forward, away from the user;
    /// a negative value indicates that the wheel was rotated backward, toward the user.
    /// </remarks>
    public void SimulateMouseWheel(short x, short y, ushort amount, short rotation);
}
