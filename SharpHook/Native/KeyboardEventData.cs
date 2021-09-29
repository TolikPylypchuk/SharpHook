using System;

namespace SharpHook.Native
{
    public struct KeyboardEventData : IEquatable<KeyboardEventData>
    {
        public KeyCode KeyCode;
        public ushort RawCode;
        public ushort KeyChar;

        public override bool Equals(object? obj) =>
            obj is KeyboardEventData data && this.Equals(data);

        public bool Equals(KeyboardEventData data) =>
            this.KeyCode == data.KeyCode &&
               this.RawCode == data.RawCode &&
               this.KeyChar == data.KeyChar;

        public override int GetHashCode() =>
            HashCode.Combine(this.KeyCode, this.RawCode, this.KeyChar);

        public static bool operator ==(KeyboardEventData left, KeyboardEventData right) =>
            left.Equals(right);

        public static bool operator !=(KeyboardEventData left, KeyboardEventData right) =>
            !(left == right);
    }
}
