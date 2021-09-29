using System;

namespace SharpHook.Native
{
    public struct ScreenData : IEquatable<ScreenData>
    {
        public byte Number;
        public short X;
        public short Y;
        public ushort Width;
        public ushort Height;

        public override bool Equals(object? obj) =>
            obj is ScreenData data && this.Equals(data);

        public bool Equals(ScreenData data) =>
            this.Number == data.Number &&
                this.X == data.X &&
                this.Y == data.Y &&
                this.Width == data.Width &&
                this.Height == data.Height;

        public override int GetHashCode() =>
            HashCode.Combine(this.Number, this.X, this.Y, this.Width, this.Height);

        public static bool operator ==(ScreenData left, ScreenData right) =>
            left.Equals(right);

        public static bool operator !=(ScreenData left, ScreenData right) =>
            !(left == right);
    }
}
