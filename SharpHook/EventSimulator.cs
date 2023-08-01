namespace SharpHook;

using System;

using SharpHook.Native;
using SharpHook.Providers;

/// <summary>
/// A keyboard and mouse event simulator which posts events to the OS.
/// </summary>
/// <seealso cref="IEventSimulationProvider" />
/// <seealso cref="UioHook.PostEvent(ref UioHookEvent)" />
public class EventSimulator : IEventSimulator
{
    private readonly IEventSimulationProvider simulationProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSimulator" /> class.
    /// </summary>
    public EventSimulator()
        : this(UioHookProvider.Instance)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSimulator" /> class.
    /// </summary>
    /// <param name="simulationProvider">The simulation functionality provider.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="simulationProvider" /> is <see langword="null" />.
    /// </exception>
    public EventSimulator(IEventSimulationProvider simulationProvider) =>
        this.simulationProvider = simulationProvider ?? throw new ArgumentNullException(nameof(simulationProvider));

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
    public TimeSpan TextSimulationDelayOnX11
    {
        get => TimeSpan.FromTicks((long)this.simulationProvider.GetPostTextDelayX11() / 100);
        set
        {
            if (value.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            this.simulationProvider.SetPostTextDelayX11((ulong)(value.Ticks * 100));
        }
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
    public UioHookResult SimulateTextEntry(string text) =>
        this.simulationProvider.PostText(text ?? throw new ArgumentNullException(nameof(text)));

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
        MouseWheelScrollType type = MouseWheelScrollType.UnitScroll) =>
        this.PostEvent(new()
        {
            Type = EventType.MouseWheel,
            Wheel = new() { Rotation = rotation, Direction = direction, Type = type }
        });

    private UioHookResult PostEvent(UioHookEvent e) =>
        this.simulationProvider.PostEvent(ref e);
}
