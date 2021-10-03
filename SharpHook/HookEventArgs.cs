using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// A base class for event args related to the global hook.
    /// </summary>
    public abstract class HookEventArgs
    {
        private protected HookEventArgs(UioHookEvent rawEvent) =>
            this.RawEvent = rawEvent;

        /// <summary>
        /// The raw event data.
        /// </summary>
        public UioHookEvent RawEvent { get; }
    }
}
