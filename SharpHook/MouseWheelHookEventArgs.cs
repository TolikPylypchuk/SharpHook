using SharpHook.Native;

namespace SharpHook
{
    public sealed class MouseWheelHookEventArgs : HookEventArgs
    {
        public MouseWheelHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Wheel;

        public MouseWheelEventData Data { get; }
    }
}
