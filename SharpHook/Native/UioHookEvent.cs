namespace SharpHook.Native;

using System;
using System.Runtime.InteropServices;

using SharpHook.Internal;

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
/// <item><description><see cref="EventType.KeyTyped" /></description></item>
/// <item><description><see cref="EventType.KeyPressed" /></description></item>
/// <item><description><see cref="EventType.KeyReleased" /></description></item>
/// </list>
/// </para>
/// <para>
/// The <see cref="Mouse" /> field is available for the events of the following types:
/// <list type="bullet">
/// <item><description><see cref="EventType.MouseClicked" /></description></item>
/// <item><description><see cref="EventType.MousePressed" /></description></item>
/// <item><description><see cref="EventType.MouseReleased" /></description></item>
/// <item><description><see cref="EventType.MouseMoved" /></description></item>
/// <item><description><see cref="EventType.MouseDragged" /></description></item>
/// </list>
/// </para>
/// <para>
/// The <see cref="Wheel" /> field is available for the events of the following type:
/// <list type="bullet">
/// <item><description><see cref="EventType.MouseWheel" /></description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso cref="EventType" />
/// <seealso cref="ModifierMask" />
/// <seealso cref="KeyboardEventData" />
/// <seealso cref="MouseEventData" />
/// <seealso cref="MouseWheelEventData" />
[StructLayout(LayoutKind.Explicit)]
public struct UioHookEvent : IEquatable<UioHookEvent>
{
    /// <summary>
    /// The type of the event.
    /// </summary>
    /// <value>The type of the event.</value>
    [FieldOffset(0)]
    public EventType Type;

    /// <summary>
    /// The UNIX timestamp of the event.
    /// </summary>
    /// <value>The UNIX timestamp of the event.</value>
    [FieldOffset(8)]
    public ulong Time;

    /// <summary>
    /// The mask of the event.
    /// </summary>
    /// <value>The mask of the event.</value>
    [FieldOffset(16)]
    public ModifierMask Mask;

    /// <summary>
    /// The reserved value which can be set in event handlers.
    /// </summary>
    /// <value>The reserved value which can be set in event handlers.</value>
    /// <remarks>The value for this field should be set on the same thread which handles the event.</remarks>
    [FieldOffset(18)]
    public EventReservedValueMask Reserved;

    /// <summary>
    /// The event data if this event is keyboard-related.
    /// </summary>
    /// <value>The event data if this event is keyboard-related.</value>
    [FieldOffset(20)]
    public KeyboardEventData Keyboard;

    /// <summary>
    /// The event data if this event is mouse-related.
    /// </summary>
    /// <value>The event data if this event is mouse-related.</value>
    [FieldOffset(20)]
    public MouseEventData Mouse;

    /// <summary>
    /// The event data if this event is mouse wheel-related.
    /// </summary>
    /// <value>The event data if this event is mouse wheel-related.</value>
    [FieldOffset(20)]
    public MouseWheelEventData Wheel;

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly override bool Equals(object? obj) =>
        obj is UioHookEvent e && this.Equals(e);

    /// <summary>
    /// Compares this object to another object for equality.
    /// </summary>
    /// <param name="e">The object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public readonly bool Equals(UioHookEvent e) =>
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
                _ => true
            };

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public readonly override int GetHashCode() =>
        this.Type switch
        {
            EventType.KeyTyped => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Keyboard),
            EventType.KeyPressed => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Keyboard),
            EventType.KeyReleased => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Keyboard),
            EventType.MouseClicked => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Mouse),
            EventType.MousePressed => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Mouse),
            EventType.MouseReleased => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Mouse),
            EventType.MouseMoved => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Mouse),
            EventType.MouseWheel => HashCodeUtil.GetHashCode(
                this.Type, this.Time, this.Mask, this.Reserved, this.Wheel),
            _ => 1
        };

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public readonly override string ToString() =>
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
    /// <see langword="true" /> if the objects are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(UioHookEvent left, UioHookEvent right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two objects for inequality.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>
    /// <see langword="true" /> if the objects are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(UioHookEvent left, UioHookEvent right) =>
        !(left == right);
}
