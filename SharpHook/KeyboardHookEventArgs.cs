using SharpHook.Native;

namespace SharpHook
{
    public sealed class KeyboardHookEventArgs : HookEventArgs
    {
        public KeyboardHookEventArgs(KeyboardEventData data, UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = data;

        public KeyboardEventData Data { get; }
    }
}
