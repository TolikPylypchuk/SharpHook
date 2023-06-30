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
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateKeyPress(KeyCode keyCode);

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateKeyRelease(KeyCode keyCode);

    /// <summary>
    /// Simulates pressing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(MouseButton button);

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(MouseButton button);

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button);

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseMovement(short x, short y);

    /// <summary>
    /// Simulates moving a mouse pointer relative to the current cursor position.
    /// </summary>
    /// <param name="x">The X-coordinate offset.</param>
    /// <param name="y">The Y-coordinate offset.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseMovementRelative(short x, short y);

    /// <summary>
    /// Simulates scrolling the mouse wheel.
    /// </summary>
    /// <param name="delta">The scroll delta.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// A positive <paramref name="rotation" /> value indicates that the wheel will be rotated down and a negative value
    /// indicates that the wheel will be rotated up.
    /// </remarks>
    public UioHookResult SimulateMouseWheel(ushort delta, short rotation);
}
