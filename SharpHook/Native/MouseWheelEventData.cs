using System;

namespace SharpHook.Native
{
    public struct MouseWheelEventData : IEquatable<MouseWheelEventData>
    {
        public ushort Clicks;
        public short X;
        public short Y;
        public byte Type;
        public ushort Amount;
        public ushort Rotation;
        public byte Direction;

        public override bool Equals(object? obj) =>
            obj is MouseWheelEventData data && this.Equals(data);

        public bool Equals(MouseWheelEventData data) =>
            this.Clicks == data.Clicks &&
                this.X == data.X &&
                this.Y == data.Y &&
                this.Type == data.Type &&
                this.Amount == data.Amount &&
                this.Rotation == data.Rotation &&
                this.Direction == data.Direction;

        public override int GetHashCode() =>
            HashCode.Combine(this.Clicks, this.X, this.Y, this.Type, this.Amount, this.Rotation, this.Direction);

        public static bool operator ==(MouseWheelEventData left, MouseWheelEventData right) =>
            left.Equals(right);

        public static bool operator !=(MouseWheelEventData left, MouseWheelEventData right) =>
            !(left == right);
    }
}
