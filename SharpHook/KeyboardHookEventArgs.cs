using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Event args for keyboard-related hook events.
    /// </summary>
    public sealed class KeyboardHookEventArgs : HookEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHookEventArgs" /> class.
        /// </summary>
        /// <param name="rawEvent">The raw event data.</param>
        public KeyboardHookEventArgs(UioHookEvent rawEvent)
            : base(rawEvent) =>
            this.Data = rawEvent.Keyboard;

        /// <summary>
        /// The event data.
        /// </summary>
        public KeyboardEventData Data { get; }
    }
}
