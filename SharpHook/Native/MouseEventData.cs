using System;

namespace SharpHook.Native
{
    public struct MouseEventData : IEquatable<MouseEventData>
    {
        public ushort Button;
        public ushort Clicks;
        public short X;
        public short Y;

        public override bool Equals(object? obj) =>
            obj is MouseEventData data && this.Equals(data);

        public bool Equals(MouseEventData data) =>
            this.Button == data.Button &&
                this.Clicks == data.Clicks &&
                this.X == data.X &&
                this.Y == data.Y;

        public override int GetHashCode() =>
            HashCode.Combine(this.Button, this.Clicks, this.X, this.Y);

        public static bool operator ==(MouseEventData left, MouseEventData right) =>
            left.Equals(right);

        public static bool operator !=(MouseEventData left, MouseEventData right) =>
            !(left == right);
    }
}
