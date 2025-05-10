namespace SharpHook.Data;

/// <summary>
/// Represents a mouse button.
/// </summary>
/// <seealso cref="MouseEventData" />
public enum MouseButton : ushort
{
    /// <summary>
    /// No button
    /// </summary>
    NoButton = 0,

    /// <summary>
    /// Button 1 (usually the left mouse button)
    /// </summary>
    Button1 = 1,

    /// <summary>
    /// Button 2 (usually the right mouse button)
    /// </summary>
    Button2 = 2,

    /// <summary>
    /// Button 3 (usually the middle mouse button)
    /// </summary>
    Button3 = 3,

    /// <summary>
    /// Button 4
    /// </summary>
    Button4 = 4,

    /// <summary>
    /// Button 5
    /// </summary>
    Button5 = 5
}
