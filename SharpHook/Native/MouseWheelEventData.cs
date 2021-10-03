using System;

namespace SharpHook.Native
{
    /// <summary>
    /// Represents the data of a mouse wheel-related event.
    /// </summary>
    /// <seealso cref="MouseWheelHookEventArgs" />
    public struct MouseWheelEventData : IEquatable<MouseWheelEventData>
    {
        /// <summary>
        /// The number of clicks.
        /// </summary>
        public ushort Clicks;

        /// <summary>
        /// The X-coordinate of the mouse cursor.
        /// </summary>
        public short X;

        /// <summary>
        /// The Y-coordinate of the mouse cursor.
        /// </summary>
        public short Y;

        /// <summary>
        /// The type.
        /// </summary>
        public byte Type;

        /// <summary>
        /// The amount.
        /// </summary>
        public ushort Amount;

        /// <summary>
        /// The rotation.
        /// </summary>
        public ushort Rotation;

        /// <summary>
        /// The direction.
        /// </summary>
        public byte Direction;

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj) =>
            obj is MouseWheelEventData data && this.Equals(data);

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="data">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
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
            HashCode.Combine(this.Clicks, this.X, this.Y, this.Type, this.Amount, this.Rotation, this.Direction);

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator ==(MouseWheelEventData left, MouseWheelEventData right) =>
            left.Equals(right);

        /// <summary>
        /// Compares two objects for inequality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are not equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator !=(MouseWheelEventData left, MouseWheelEventData right) =>
            !(left == right);
    }
}
