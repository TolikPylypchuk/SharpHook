namespace SharpHook.Native;

using System;
using System.Linq;

/// <summary>
/// Represents a virtual modifier mask.
/// </summary>
/// <seealso cref="UioHookEvent" />
[Flags]
public enum ModifierMask : ushort
{
    /// <summary>No modifier</summary>
    None = 0,

    /// <summary>Left Shift</summary>
    LeftShift = 1 << 0,

    /// <summary>Left Control</summary>
    LeftCtrl = 1 << 1,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    LeftMeta = 1 << 2,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    LeftAlt = 1 << 3,

    /// <summary>Right Shift</summary>
    RightShift = 1 << 4,

    /// <summary>Right Control</summary>
    RightCtrl = 1 << 5,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    RightMeta = 1 << 6,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    RightAlt = 1 << 7,

    /// <summary>Left or right Shift</summary>
    Shift = LeftShift | RightShift,

    /// <summary>Left or right Control</summary>
    Ctrl = LeftCtrl | RightCtrl,

    /// <summary>
    /// Left or right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    Meta = LeftMeta | RightMeta,

    /// <summary>
    /// Left or right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    Alt = LeftAlt | RightAlt,

    /// <summary>
    /// Mouse button 1 (usually the left mouse button)
    /// </summary>
    Button1 = 1 << 8,

    /// <summary>
    /// Mouse button 2 (usually the right mouse button)
    /// </summary>
    Button2 = 1 << 9,

    /// <summary>Mouse button 3</summary>
    Button3 = 1 << 10,

    /// <summary>Mouse button 4</summary>
    Button4 = 1 << 11,

    /// <summary>Mouse button 5</summary>
    Button5 = 1 << 12,

    /// <summary>Num Lock</summary>
    NumLock = 1 << 13,

    /// <summary>Caps Lock</summary>
    CapsLock = 1 << 14,

    /// <summary>Scroll Lock</summary>
    ScrollLock = 1 << 15
}
