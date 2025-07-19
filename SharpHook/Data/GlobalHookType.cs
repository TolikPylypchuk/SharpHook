namespace SharpHook.Data;

/// <summary>
/// Represents a global hook type – keyboard hook, mouse hook, or both.
/// </summary>
[Flags]
public enum GlobalHookType
{
    /// <summary>A global hook for both keyboard and mouse events.</summary>
    All,

    /// <summary>A global hook for keyboard events only.</summary>
    Keyboard,

    /// <summary>A global hook for mouse events only.</summary>
    Mouse
}
