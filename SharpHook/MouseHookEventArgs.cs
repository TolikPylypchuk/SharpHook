using SharpHook.Native;

namespace SharpHook
{
    public sealed class MouseHookEventArgs : HookEventArgs
    {
        public MouseHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Mouse;

        public MouseEventData Data { get; }
    }
}
