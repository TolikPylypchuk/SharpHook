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
        this.PostEvent(new()
        {
            Type = EventType.MousePressedIgnoreCoordinates,
            Mouse = new() { Button = button }
        });

    /// <summary>
    /// Simulates pressing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMousePress(short x, short y, MouseButton button) =>
        this.PostEvent(new()
        {
            Type = EventType.MousePressed,
            Mouse = new() { Button = button, X = x, Y = y }
        });

    /// <summary>
    /// Simulates releasing a mouse button at the current coordinates.
    /// </summary>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(MouseButton button) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseReleasedIgnoreCoordinates,
            Mouse = new() { Button = button }
        });

    /// <summary>
    /// Simulates releasing a mouse button at the specified coordinates.
    /// </summary>
    /// <param name="x">The target X-coordinate of the mouse pointer.</param>
    /// <param name="y">The target Y-coordinate of the mouse pointer.</param>
    /// <param name="button">The mouse button to release.</param>
    /// <returns>The result of the operation.</returns>
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton button) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseReleased,
            Mouse = new() { Button = button, X = x, Y = y }
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
            Type = EventType.MouseMovedRelativeToCursor,
            Mouse = new() { X = x, Y = y }
        });

    /// <summary>
    /// Simulates scrolling the mouse wheel.
    /// </summary>
    /// <param name="amount">The scrolling amount.</param>
    /// <param name="rotation">The wheel rotation.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// <para>
    /// A positive <paramref name="rotation" /> value indicates that the wheel will be rotated down and a negative value
    /// indicates that the wheel will be rotated up.
    /// </para>
    /// <para>
    /// Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of
    /// libuiohook for more details.
    /// </para>
    /// </remarks>
    public UioHookResult SimulateMouseWheel(ushort amount, short rotation) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseWheel,
            Wheel = new() { Amount = amount, Rotation = rotation }
        });

    private UioHookResult PostEvent(UioHookEvent e) =>
        UioHook.PostEvent(ref e);
}
