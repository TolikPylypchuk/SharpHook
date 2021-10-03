using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// A base class for event args related to the global hook.
    /// </summary>
    public class HookEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HookEventArgs" /> class.
        /// </summary>
        /// <param name="rawEvent">The raw event data.</param>
        public HookEventArgs(UioHookEvent rawEvent) =>
            this.RawEvent = rawEvent;

        /// <summary>
        /// The raw event data.
        /// </summary>
        public UioHookEvent RawEvent { get; }
    }
}
