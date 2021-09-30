using SharpHook.Native;

namespace SharpHook
{
    public sealed class KeyboardHookEventArgs : HookEventArgs
    {
        public KeyboardHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Keyboard;

        public KeyboardEventData Data { get; }
    }
}
