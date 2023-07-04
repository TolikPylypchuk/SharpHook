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

/// <summary>
/// Contains extension methods for <see cref="ModifierMask" />.
/// </summary>
public static class ModifierMaskExtensions
{
    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftShift" /> or
    /// <see cref="ModifierMask.RightShift"/>. Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftShift" /> or
    /// <see cref="ModifierMask.RightShift"/>. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasShift(this ModifierMask mask) =>
        mask.HasAny(ModifierMask.Shift);

    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftCtrl" /> or
    /// <see cref="ModifierMask.RightCtrl"/>. Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftCtrl" /> or
    /// <see cref="ModifierMask.RightCtrl"/>. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasCtrl(this ModifierMask mask) =>
        mask.HasAny(ModifierMask.Ctrl);

    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftAlt" /> or
    /// <see cref="ModifierMask.RightAlt"/>. Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftAlt" /> or
    /// <see cref="ModifierMask.RightAlt"/>. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasAlt(this ModifierMask mask) =>
        mask.HasAny(ModifierMask.Alt);

    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftMeta" /> or
    /// <see cref="ModifierMask.RightMeta"/>. Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains either <see cref="ModifierMask.LeftMeta" /> or
    /// <see cref="ModifierMask.RightMeta"/>. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasMeta(this ModifierMask mask) =>
        mask.HasAny(ModifierMask.Meta);

    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains any of the specified modifiers.
    /// Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <param name="modifiers">The modifiers to check against.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains any of the specified modifiers.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// This method returns <see langword="false" /> is <paramref name="modifiers" /> equals
    /// <see cref="ModifierMask.None" />.
    /// </remarks>
    public static bool HasAny(this ModifierMask mask, ModifierMask modifiers) =>
        (mask & modifiers) != ModifierMask.None;

    /// <summary>
    /// Returns <see langword="true" /> if the modifier mask contains all of the specified modifiers.
    /// Otherwise, returns <see langword="false" />.
    /// </summary>
    /// <param name="mask">The modifier mask to check.</param>
    /// <param name="modifiers">The modifiers to check against.</param>
    /// <returns>
    /// <see langword="true" /> if the modifier mask contains all of the specified modifiers.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// This method returns <see langword="true" /> is <paramref name="modifiers" /> equals
    /// <see cref="ModifierMask.None" />.
    /// </remarks>
    public static bool HasAll(this ModifierMask mask, ModifierMask modifiers) =>
        modifiers.Split().All(m => mask.HasFlag(m));

    /// <summary>
    /// Splits a modifier mask into an array of individual modifiers.
    /// </summary>
    /// <param name="mask">The modifier mask to split.</param>
    /// <returns>
    /// An array of individual modifiers contained in <paramref name="mask" />,
    /// or an empty array if <paramref name="mask" /> is <see cref="ModifierMask.None" />.
    /// </returns>
    public static ModifierMask[] Split(this ModifierMask mask) =>
#if NET5_0_OR_GREATER
        Enum.GetValues<ModifierMask>()
#else
        Enum.GetValues(typeof(ModifierMask))
            .Cast<ModifierMask>()
#endif
            .Where(m =>
                m != ModifierMask.None &&
                m != ModifierMask.Shift &&
                m != ModifierMask.Ctrl &&
                m != ModifierMask.Alt &&
                m != ModifierMask.Meta)
            .Where(m => mask.HasFlag(m))
            .ToArray();

    /// <summary>
    /// Merges an array of modifiers into a single modifier mask.
    /// </summary>
    /// <param name="modifiers">The modifiers to merge.</param>
    /// <returns>A modifier mask which contains the specified modifiers.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="modifiers" /> is <see langword="null" />.
    /// </exception>
    public static ModifierMask Merge(this ModifierMask[] modifiers) =>
        modifiers != null
            ? modifiers.Aggregate(ModifierMask.None, (mask, modifier) => mask | modifier)
            : throw new ArgumentNullException(nameof(modifiers));
}
