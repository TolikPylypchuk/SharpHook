namespace SharpHook;

/// <summary>
/// Represents an implementation of <see cref="IGlobalHook" /> which raises events on the same thread on which
/// the hook itself runs.
/// </summary>
/// <remarks>
/// The event handlers should generally be fast and simple so that the thread can process all raised events. If you
/// need to do a lot of processing in the event handlers then consider using another implementation, e.g.,
/// <see cref="TaskPoolGlobalHook" />.
/// </remarks>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="EventLoopGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
public sealed class SimpleGlobalHook : GlobalHookBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    /// <param name="globalHookType">The global hook type.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IBasicGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public SimpleGlobalHook(
        GlobalHookType globalHookType = GlobalHookType.All,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
        : base(globalHookType, globalHookProvider, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Handles the hook event.
    /// </summary>
    /// <param name="e">The event to handle.</param>
    protected override void HandleHookEvent(ref UioHookEvent e) =>
        this.DispatchEvent(ref e);
}
