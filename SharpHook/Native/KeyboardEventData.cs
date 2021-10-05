using System;
using System.Runtime.InteropServices;

namespace SharpHook.Native
{
    /// <summary>
    /// Represents the data of a keyboard-related event.
    /// </summary>
    /// <seealso cref="KeyboardHookEventArgs" />
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardEventData : IEquatable<KeyboardEventData>
    {
        /// <summary>
        /// The key code.
        /// </summary>
        public KeyCode KeyCode;

        /// <summary>
        /// The raw key code.
        /// </summary>
        public ushort RawCode;

        /// <summary>
        /// The character of the key.
        /// </summary>
        public char KeyChar;

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj) =>
            obj is KeyboardEventData data && this.Equals(data);

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="data">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(KeyboardEventData data) =>
            this.KeyCode == data.KeyCode &&
               this.RawCode == data.RawCode &&
               this.KeyChar == data.KeyChar;

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override int GetHashCode() =>
            HashCode.Combine(this.KeyCode, this.RawCode, this.KeyChar);

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString() =>
            $"{nameof(KeyboardEventData)}: {nameof(this.KeyCode)} = {this.KeyCode}; " +
            $"{nameof(this.RawCode)} = {this.RawCode}; {nameof(this.KeyChar)} = {this.KeyChar}";

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator ==(KeyboardEventData left, KeyboardEventData right) =>
            left.Equals(right);

        /// <summary>
        /// Compares two objects for inequality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are not equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator !=(KeyboardEventData left, KeyboardEventData right) =>
            !(left == right);
    }
}
