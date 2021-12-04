namespace SharpHook;

using SharpHook.Native;

/// <summary>
/// A keyboard and mouse event simulator which posts events to the OS.
/// </summary>
/// <seealso cref="UioHook.PostEvent(ref UioHookEvent)" />
public class EventSimulator : IEventSimulator
{
    /// <summary>
    /// Simulates pressing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to press.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateKeyPress(KeyCode keyCode, ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.KeyPressed,
            Keyboard = new() { KeyCode = keyCode },
            Mask = mask
        });

    /// <summary>
    /// Simulates releasing a key.
    /// </summary>
    /// <param name="keyCode">The code of the key to release.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateKeyRelease(KeyCode keyCode, ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.KeyReleased,
            Keyboard = new() { KeyCode = keyCode },
            Mask = mask
        });

    /// <summary>
    /// Simulates pressing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMousePress(MouseButton button, ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.MousePressed,
            Mouse = new() { Button = button },
            Mask = mask
        });

    /// <summary>
    /// Simulates releasing a mouse button.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMouseRelease(MouseButton button, ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseReleased,
            Mouse = new() { Button = button },
            Mask = mask
        });

    /// <summary>
    /// Simulates moving a mouse pointer.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    public void SimulateMouseMovement(short x, short y, ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseMoved,
            Mouse = new() { X = x, Y = y },
            Mask = mask
        });

    /// <summary>
    /// Simulates scrolling a mouse wheel.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="amount">The scrolling amount.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <param name="mask">The modifier mask of the event.</param>
    /// <remarks>
    /// <para>
    /// A positive <paramref name="rotation" /> value indicates that the wheel was rotated forward, away from the user;
    /// a negative value indicates that the wheel was rotated backward, toward the user.
    /// </para>
    /// <para>
    /// Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of
    /// libuiohook for more details.
    /// </para>
    /// </remarks>
    public void SimulateMouseWheel(
        short x,
        short y,
        ushort amount,
        short rotation,
        ModifierMask mask = ModifierMask.None) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseWheel,
            Wheel = new() { X = x, Y = y, Amount = amount, Rotation = rotation },
            Mask = mask
        });

    private void PostEvent(UioHookEvent e) =>
        UioHook.PostEvent(ref e);
}
