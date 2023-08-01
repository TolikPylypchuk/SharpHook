namespace SharpHook.Native;

/// <summary>
/// Represents a mouse wheel scroll type.
/// </summary>
/// <seealso cref="MouseWheelEventData" />
public enum MouseWheelScrollType : byte
{
    /// <summary>
    /// The default scroll type on Windows and Linux, and the pixel scroll type on macOS.
    /// </summary>
    UnitScroll = 1,

    /// <summary>
    /// The page scroll type on Windows, and the line scroll type on macOS. Not available on Linux.
    /// </summary>
    BlockScroll = 2,
}
