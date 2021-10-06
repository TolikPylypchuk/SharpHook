using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Represents an implementation of <see cref="IGlobalHook" /> which raises events on the same thread on which
    /// the hook itself runs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The event handlers should generally be fast and simple so that the thread can process all events that are raised
    /// by libuiohook. If you need to do a lot of processing in the event handlers then consider using another
    /// implementation, e.g. <see cref="TaskPoolGlobalHook" />.
    /// </para>
    /// <para>
    /// The <see cref="UioHookEvent" /> instance passed to the handlers will be a copy of the original
    /// data passed from libuiohook.
    /// </para>
    /// </remarks>
    public sealed class SimpleGlobalHook : GlobalHookBase
    {
        /// <summary>
        /// Handles the hook event.
        /// </summary>
        /// <param name="e">The event to handle.</param>
        protected override void HandleHookEventInternal(ref UioHookEvent e) =>
            this.DispatchEvent(ref e);
    }
}
