namespace SharpHook.Native;

using System;

/// <summary>
/// Represents a virtual modifier mask.
/// </summary>
/// <seealso cref="UioHookEvent" />
[Flags]
public enum ModifierMask : ushort
{
    None = 0,

    LeftShift = 1 << 0,
    LeftCtrl = 1 << 1,
    LeftMeta = 1 << 2,
    LeftAlt = 1 << 3,

    RightShift = 1 << 4,
    RightCtrl = 1 << 5,
    RightMeta = 1 << 6,
    RightAlt = 1 << 7,

    Shift = LeftShift | RightShift,
    Ctrl = LeftCtrl | RightCtrl,
    Meta = LeftMeta | RightMeta,
    Alt = LeftAlt | RightAlt,

    Button1 = 1 << 8,
    Button2 = 1 << 9,
    Button3 = 1 << 10,
    Button4 = 1 << 11,
    Button5 = 1 << 12,

    NumLock = 1 << 13,
    CapsLock = 1 << 14,
    ScrollLock = 1 << 15
}
