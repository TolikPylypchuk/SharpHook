namespace SharpHook.Native;

using System;
using System.Runtime.InteropServices;

using SharpHook.Internal;

/// <summary>
/// Represents the data of a mouse-related event.
/// </summary>
/// <seealso cref="MouseHookEventArgs" />
/// <seealso cref="MouseButton" />
[StructLayout(LayoutKind.Sequential)]
public struct MouseEventData : IEquatable<MouseEventData>
{
    /// <summary>
    /// The mouse button.
    /// </summary>
    /// <value>The mouse button.</value>
    public MouseButton Button;

    /// <summary>
    /// The number of clicks.
    /// </summary>
    /// <value>The number of clicks.</value>
    public ushort Clicks;

    /// <summary>
    /// The X-coordinate of the mouse cursor.
    /// </summary>
    /// <value>The X-coordinate of the mouse cursor.</value>
    public short X;

    /// <summary>
    /// The Y-coordinate of the mouse cursor.
    /// </summary>
    /// <value>The Y-coordinate of the mouse cursor.</value>
    public short Y;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly override bool Equals(object? obj) =>
        obj is MouseEventData data && this.Equals(data);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="data">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly bool Equals(MouseEventData data) =>
        this.Button == data.Button &&
            this.Clicks == data.Clicks &&
            this.X == data.X &&
            this.Y == data.Y;

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public readonly override int GetHashCode() =>
        HashCodeUtil.GetHashCode(this.Button, this.Clicks, this.X, this.Y);

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public readonly override string ToString() =>
        $"{nameof(MouseEventData)}: {nameof(this.Button)} = {this.Button}; " +
        $"{nameof(this.Clicks)} = {this.Clicks}; {nameof(this.X)} = {this.X}; {nameof(this.Y)} = {this.Y}";

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(MouseEventData left, MouseEventData right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(MouseEventData left, MouseEventData right) =>
        !(left == right);
}
