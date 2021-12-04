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
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateKeyPress(KeyCode keyCode, ModifierMask mask = ModifierMask.None);

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateKeyRelease(KeyCode keyCode, ModifierMask mask = ModifierMask.None);

    /// <summary>
    /// Simulates pressing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMousePress(MouseButton button, ModifierMask mask = ModifierMask.None);

    /// <summary>
    /// Simulates releasing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMouseRelease(MouseButton button, ModifierMask mask = ModifierMask.None);

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMouseMovement(short x, short y, ModifierMask mask = ModifierMask.None);

    /// <summary>
    /// Simulates scrolling a mouse wheel.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="amount">The scrolling amount.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    /// <remarks>
    /// A positive <paramref name="rotation" /> value indicates that the wheel was rotated forward, away from the user;
    /// a negative value indicates that the wheel was rotated backward, toward the user.
    /// </remarks>
    public void SimulateMouseWheel(
        short x,
        short y,
        ushort amount,
        short rotation,
        ModifierMask mask = ModifierMask.None);
}
