namespace SharpHook.Native;

/// <summary>
/// Represents a mouse wheel scroll type.
/// </summary>
/// <seealso cref="MouseWheelEventData" />
public enum MouseWheelScrollType : byte
{
    /// <summary>
    /// Unit scroll
    /// </summary>
    UnitScroll = 1,

    /// <summary>
    /// Block scroll
    /// </summary>
    BlockScroll = 2,
}
