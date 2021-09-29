using SharpHook.Native;

namespace SharpHook
{
    public abstract class HookEventArgs
    {
        private protected HookEventArgs(UioHookEvent rawEvent) =>
            this.RawEvent = rawEvent;

        public UioHookEvent RawEvent { get; }
    }
}
