namespace SharpHook.Native;

using System;
using System.Runtime.InteropServices;

using SharpHook.Internal;

/// <summary>
/// Represents screen-related data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ScreenData : IEquatable<ScreenData>
{
    /// <summary>
    /// The ordinal number of the screen.
    /// </summary>
    /// <value>The ordinal number of the screen.</value>
    public byte Number;

    /// <summary>
    /// The X-coordinate of the screen.
    /// </summary>
    /// <value>The X-coordinate of the screen.</value>
    public short X;

    /// <summary>
    /// The Y-coordinate of the screen.
    /// </summary>
    /// <value>The Y-coordinate of the screen.</value>
    public short Y;

    /// <summary>
    /// The width of the screen.
    /// </summary>
    /// <value>The width of the screen.</value>
    public ushort Width;

    /// <summary>
    /// The height of the screen.
    /// </summary>
    /// <value>The height of the screen.</value>
    public ushort Height;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly override bool Equals(object? obj) =>
        obj is ScreenData data && this.Equals(data);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="data">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly bool Equals(ScreenData data) =>
        this.Number == data.Number &&
            this.X == data.X &&
            this.Y == data.Y &&
            this.Width == data.Width &&
            this.Height == data.Height;

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public readonly override int GetHashCode() =>
        HashCodeUtil.GetHashCode(this.Number, this.X, this.Y, this.Width, this.Height);

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public readonly override string ToString() =>
        $"{nameof(ScreenData)}: {nameof(this.Number)} = {this.Number}; {nameof(this.X)} = {this.X}; " +
        $"{nameof(this.Y)} = {this.Y}; {nameof(this.Width)} = {this.Width}; {nameof(this.Height)} = {this.Height}";

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(ScreenData left, ScreenData right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(ScreenData left, ScreenData right) =>
        !(left == right);
}
