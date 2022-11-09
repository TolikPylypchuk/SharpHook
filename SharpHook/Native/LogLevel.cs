namespace SharpHook.Native;

/// <summary>
/// Represents a log level.
/// </summary>
public enum LogLevel : uint
{
    /// <summary>
    /// Debug information
    /// </summary>
    Debug = 1,

    /// <summary>
    /// General-purpose information
    /// </summary>
    Info,

    /// <summary>
    /// Warning
    /// </summary>
    Warn,

    /// <summary>
    /// Error
    /// </summary>
    Error
}
