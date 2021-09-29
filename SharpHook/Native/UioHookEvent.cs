using System;
using System.Runtime.InteropServices;

namespace SharpHook.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UioHookEvent : IEquatable<UioHookEvent>
    {
        [FieldOffset(0)]
        public EventType Type;

        [FieldOffset(8)]
        public ushort Time;

        [FieldOffset(16)]
        public ushort Mask;

        [FieldOffset(18)]
        public ushort Reserved;

        [FieldOffset(20)]
        public KeyboardEventData Keyboard;

        [FieldOffset(20)]
        public MouseEventData Mouse;

        [FieldOffset(20)]
        public MouseWheelEventData Wheel;

        public override bool Equals(object? obj) =>
            obj is UioHookEvent e && this.Equals(e);

        public bool Equals(UioHookEvent e) =>
            this.Type == e.Type &&
                this.Time == e.Time &&
                this.Mask == e.Mask &&
                this.Reserved == e.Reserved &&
                this.Keyboard == e.Keyboard;

        public override int GetHashCode() =>
            HashCode.Combine(this.Type, this.Time, this.Mask, this.Reserved, this.Keyboard, this.Mouse, this.Wheel);

        public static bool operator ==(UioHookEvent left, UioHookEvent right) =>
            left.Equals(right);

        public static bool operator !=(UioHookEvent left, UioHookEvent right) =>
            !(left == right);
    }
}
