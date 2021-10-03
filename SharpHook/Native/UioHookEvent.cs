using System;
using System.Runtime.InteropServices;

namespace SharpHook.Native
{
    /// <summary>
    /// Represents an event which can be raised by libuiohook.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a union-like struct and the data it contains depends on the type of the event.
    /// </para>
    /// <para>
    /// The <see cref="Keyboard" /> field is available for the events of the following types:
    /// <list type="bullet">
    /// <item><see cref="EventType.KeyTyped" /></item>
    /// <item><see cref="EventType.KeyPressed" /></item>
    /// <item><see cref="EventType.KeyReleased" /></item>
    /// </list>
    /// </para>
    /// <para>
    /// The <see cref="Mouse" /> field is available for the events of the following types:
    /// <list type="bullet">
    /// <item><see cref="EventType.MouseClicked" /></item>
    /// <item><see cref="EventType.MousePressed" /></item>
    /// <item><see cref="EventType.MouseReleased" /></item>
    /// <item><see cref="EventType.MouseMoved" /></item>
    /// <item><see cref="EventType.MouseDragged" /></item>
    /// </list>
    /// </para>
    /// <para>
    /// The <see cref="Wheel" /> field is available for the events of the following type:
    /// <list type="bullet">
    /// <item><see cref="EventType.MouseWheel" /></item>
    /// </list>
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct UioHookEvent : IEquatable<UioHookEvent>
    {
        /// <summary>
        /// The type of the event.
        /// </summary>
        [FieldOffset(0)]
        public EventType Type;

        /// <summary>
        /// The time of the event.
        /// </summary>
        [FieldOffset(8)]
        public ushort Time;

        /// <summary>
        /// The mask of the event.
        /// </summary>
        [FieldOffset(16)]
        public ModifierMask Mask;

        /// <summary>
        /// The reserved value.
        /// </summary>
        [FieldOffset(18)]
        public ushort Reserved;

        /// <summary>
        /// The event data if this event is keyboard-related.
        /// </summary>
        [FieldOffset(20)]
        public KeyboardEventData Keyboard;

        /// <summary>
        /// The event data if this event is mouse-related.
        /// </summary>
        [FieldOffset(20)]
        public MouseEventData Mouse;

        /// <summary>
        /// The event data if this event is mouse wheel-related.
        /// </summary>
        [FieldOffset(20)]
        public MouseWheelEventData Wheel;

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj) =>
            obj is UioHookEvent e && this.Equals(e);

        /// <summary>
        /// Compares this object to another object for equality.
        /// </summary>
        /// <param name="e">The object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(UioHookEvent e) =>
            this.Type == e.Type &&
                this.Time == e.Time &&
                this.Mask == e.Mask &&
                this.Reserved == e.Reserved &&
                this.Type switch
                {
                    EventType.KeyTyped => this.Keyboard == e.Keyboard,
                    EventType.KeyPressed => this.Keyboard == e.Keyboard,
                    EventType.KeyReleased => this.Keyboard == e.Keyboard,
                    EventType.MouseClicked => this.Mouse == e.Mouse,
                    EventType.MousePressed => this.Mouse == e.Mouse,
                    EventType.MouseReleased => this.Mouse == e.Mouse,
                    EventType.MouseMoved => this.Mouse == e.Mouse,
                    EventType.MouseWheel => this.Wheel == e.Wheel,
                    _ => true,
                };

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override int GetHashCode() =>
            HashCode.Combine(this.Type, this.Time, this.Mask, this.Reserved, this.Keyboard, this.Mouse, this.Wheel);

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString() =>
            $"{nameof(UioHookEvent)}: {nameof(this.Type)} = {this.Type}; {nameof(this.Time)} = {this.Time}; " +
            $"{nameof(this.Mask)} = {this.Mask}; {nameof(this.Reserved)} = {this.Reserved}" +
            this.Type switch
            {
                EventType.KeyTyped => $"; {nameof(this.Keyboard)} = {this.Keyboard}",
                EventType.KeyPressed => $"; {nameof(this.Keyboard)} = {this.Keyboard}",
                EventType.KeyReleased => $"; {nameof(this.Keyboard)} = {this.Keyboard}",
                EventType.MouseClicked => $"; {nameof(this.Mouse)} = {this.Mouse}",
                EventType.MousePressed => $"; {nameof(this.Mouse)} = {this.Mouse}",
                EventType.MouseReleased => $"; {nameof(this.Mouse)} = {this.Mouse}",
                EventType.MouseMoved => $"; {nameof(this.Mouse)} = {this.Mouse}",
                EventType.MouseWheel => $"; {nameof(this.Wheel)} = {this.Wheel}",
                _ => String.Empty
            };

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator ==(UioHookEvent left, UioHookEvent right) =>
            left.Equals(right);

        /// <summary>
        /// Compares two objects for inequality.
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>
        /// <see langword="true" /> if and only if the objects are not equal. Otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator !=(UioHookEvent left, UioHookEvent right) =>
            !(left == right);
    }
}
