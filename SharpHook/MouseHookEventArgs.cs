using SharpHook.Native;

namespace SharpHook
{
    public sealed class MouseHookEventArgs : HookEventArgs
    {
        public MouseHookEventArgs(MouseEventData data, UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = data;

        public MouseEventData Data { get; }
    }
}
