namespace SharpHook.Native;

using System;

/// <summary>
/// Represents possible values of the <see cref="UioHookEvent.Reserved" /> field.
/// </summary>
[Flags]
public enum EventReservedValueMask : ushort
{
    /// <summary>
    /// No value.
    /// </summary>
    None = 0,

    /// <summary>
    /// If this flag is set in the event handler then the event will be suppressed from further propagation.
    /// </summary>
    /// <remarks>Suppressing events works only on Windows and macOS.</remarks>
    SuppressEvent = 1
}
