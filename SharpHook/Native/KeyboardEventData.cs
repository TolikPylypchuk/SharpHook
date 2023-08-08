namespace SharpHook.Native;

/// <summary>
/// Represents the data of a keyboard-related event.
/// </summary>
/// <seealso cref="KeyboardHookEventArgs" />
/// <seealso cref="SharpHook.Native.KeyCode" />
[ExcludeFromCodeCoverage]
[StructLayout(LayoutKind.Sequential)]
public struct KeyboardEventData : IEquatable<KeyboardEventData>
{
    /// <summary>
    /// The value of <see cref="RawKeyChar" /> in events of type <see cref="EventType.KeyPressed" /> and
    /// <see cref="EventType.KeyReleased" />.
    /// </summary>
    public static readonly ushort RawUndefinedChar = 0xFF;

    /// <summary>
    /// The value of <see cref="KeyChar" /> in events of type <see cref="EventType.KeyPressed" /> and
    /// <see cref="EventType.KeyReleased" />.
    /// </summary>
    public static readonly char UndefinedChar = (char)RawUndefinedChar;

    /// <summary>
    /// The key code.
    /// </summary>
    /// <value>The key code.</value>
    public KeyCode KeyCode;

    /// <summary>
    /// The OS-specific key code.
    /// </summary>
    /// <value>The OS-specific key code.</value>
    public ushort RawCode;

    /// <summary>
    /// The character of the key.
    /// </summary>
    /// <value>The character of the key.</value>
    /// <remarks>
    /// <para>This field is available only for the <see cref="EventType.KeyTyped" /> event.</para>
    /// <para>
    /// In general <see cref="KeyChar" /> should be used instead of this field. This field is of type
    /// <see cref="UInt16" /> purely for marshalling reasons. It should be used as a <see cref="Char" />.
    /// </para>
    /// </remarks>
    public ushort RawKeyChar;

    /// <summary>
    /// The character of the key.
    /// </summary>
    /// <value>The character of the key.</value>
    /// <remarks>
    /// <para>This property is available only for the <see cref="EventType.KeyTyped" /> event.</para>
    /// </remarks>
    public readonly char KeyChar =>
        (char)this.RawKeyChar;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly override bool Equals(object? obj) =>
        obj is KeyboardEventData data && this.Equals(data);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="data">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly bool Equals(KeyboardEventData data) =>
        this.KeyCode == data.KeyCode &&
           this.RawCode == data.RawCode &&
           this.RawKeyChar == data.RawKeyChar;

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public readonly override int GetHashCode() =>
        HashCodeUtil.GetHashCode(this.KeyCode, this.RawCode, this.RawKeyChar);

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public readonly override string ToString() =>
        $"{nameof(KeyboardEventData)}: {nameof(this.KeyCode)} = {this.KeyCode}; " +
        $"{nameof(this.RawCode)} = {this.RawCode}; {nameof(this.KeyChar)} = {this.KeyChar}";

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(KeyboardEventData left, KeyboardEventData right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(KeyboardEventData left, KeyboardEventData right) =>
        !(left == right);
}
