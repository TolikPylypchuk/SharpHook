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
    /// The wheel scroll type.
    /// </summary>
    /// <value>The wheel scroll type.</value>
    public MouseWheelScrollType Type;

    /// <summary>
    /// The wheel scroll amount.
    /// </summary>
    /// <value>The wheel scroll amount.</value>
    public ushort Amount;

    /// <summary>
    /// The wheel rotation.
    /// </summary>
    /// <value>The wheel rotation.</value>
    /// <remarks>
    /// A positive value indicates that the wheel was rotated forward, away from the user; a negative value
    /// indicates that the wheel was rotated backward, toward the user.
    /// </remarks>
    public ushort Rotation;

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
    public override bool Equals(object? obj) =>
        obj is MouseWheelEventData data && this.Equals(data);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="data">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(MouseWheelEventData data) =>
        this.Clicks == data.Clicks &&
            this.X == data.X &&
            this.Y == data.Y &&
            this.Type == data.Type &&
            this.Amount == data.Amount &&
            this.Rotation == data.Rotation &&
            this.Direction == data.Direction;

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public override int GetHashCode() =>
        HashCodeUtil.GetHashCode(
            this.Clicks, this.X, this.Y, this.Type, this.Amount, this.Rotation, this.Direction);

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public override string ToString() =>
        $"{nameof(MouseWheelEventData)}: {nameof(this.Clicks)} = {this.Clicks}; {nameof(this.X)} = {this.X}; " +
        $"{nameof(this.Y)} = {this.Y}; {nameof(this.Type)} = {this.Type}; {nameof(this.Amount)} = {this.Amount}; " +
        $"{nameof(this.Rotation)} = {this.Rotation}; {nameof(this.Direction)} = {this.Direction}";

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
