namespace SharpHook.Native;

using System;
using System.Runtime.InteropServices;

using SharpHook.Internal;

/// <summary>
/// Represents the data of a mouse wheel-related event.
/// </summary>
/// <seealso cref="MouseWheelHookEventArgs" />
/// <seealso cref="MouseWheelScrollType" />
/// <seealso cref="MouseWheelScrollDirection" />
[StructLayout(LayoutKind.Sequential)]
public struct MouseWheelEventData : IEquatable<MouseWheelEventData>
{
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
    /// The wheel scroll type.
    /// </summary>
    /// <value>The wheel scroll type.</value>
    public MouseWheelScrollType Type;

    /// <summary>
    /// The wheel rotation.
    /// </summary>
    /// <value>The wheel rotation.</value>
    /// <remarks>
    /// A positive value indicates that the wheel was rotated down and a negative value indicates that the wheel was
    /// rotated up.
    /// </remarks>
    public short Rotation;

    /// <summary>
    /// The scroll delta.
    /// </summary>
    /// <value>The scroll delta.</value>
    public ushort Delta;

    /// <summary>
    /// The wheel scroll direction.
    /// </summary>
    /// <value>The wheel scroll direction.</value>
    public MouseWheelScrollDirection Direction;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly override bool Equals(object? obj) =>
        obj is MouseWheelEventData data && this.Equals(data);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="data">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly bool Equals(MouseWheelEventData data) =>
        this.X == data.X &&
            this.Y == data.Y &&
            this.Type == data.Type &&
            this.Rotation == data.Rotation &&
            this.Delta == data.Delta &&
            this.Direction == data.Direction;

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public readonly override int GetHashCode() =>
        HashCodeUtil.GetHashCode(this.X, this.Y, this.Type, this.Rotation, this.Delta, this.Direction);

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public readonly override string ToString() =>
        $"{nameof(MouseWheelEventData)}: {nameof(this.X)} = {this.X}; {nameof(this.Y)} = {this.Y}; " +
        $"{nameof(this.Type)} = {this.Type}; {nameof(this.Rotation)} = {this.Rotation}; " +
        $"{nameof(this.Delta)} = {this.Delta}; {nameof(this.Direction)} = {this.Direction}";

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(MouseWheelEventData left, MouseWheelEventData right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(MouseWheelEventData left, MouseWheelEventData right) =>
        !(left == right);
}
