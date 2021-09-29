using SharpHook.Native;

namespace SharpHook
{
    public sealed class MouseWheelHookEventArgs : HookEventArgs
    {
        public MouseWheelHookEventArgs(MouseWheelEventData data, UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = data;

        public MouseWheelEventData Data { get; }
    }
}
