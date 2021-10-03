using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Event args for mouse-related hook events.
    /// </summary>
    public sealed class MouseHookEventArgs : HookEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseHookEventArgs" /> class.
        /// </summary>
        /// <param name="rawEvent">The raw event data.</param>
        public MouseHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Mouse;

        /// <summary>
        /// The event data.
        /// </summary>
        public MouseEventData Data { get; }
    }
}
