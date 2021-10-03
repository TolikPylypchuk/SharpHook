using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Event args for mouse wheel-related hook events.
    /// </summary>
    public sealed class MouseWheelHookEventArgs : HookEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWheelHookEventArgs" /> class.
        /// </summary>
        /// <param name="rawEvent">The raw event data.</param>
        public MouseWheelHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Wheel;

        /// <summary>
        /// The event data.
        /// </summary>
        public MouseWheelEventData Data { get; }
    }
}
