namespace SharpHook;

/// <summary>
/// Represents an implementation of <see cref="IGlobalHook" /> which raises events on the same thread on which
/// the hook itself runs.
/// </summary>
/// <remarks>
/// The event handlers should generally be fast and simple so that the thread can process all raised events. If you
/// need to do a lot of processing in the event handlers then consider using another implementation, e.g.
/// <see cref="TaskPoolGlobalHook" />.
/// </remarks>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="TaskPoolGlobalHook" />
public sealed class SimpleGlobalHook : GlobalHookBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public SimpleGlobalHook()
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    /// <param name="globalHookProvider">The underlying global hook provider.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="globalHookProvider"/> is <see langword="null" />.
    /// </exception>
    [ExcludeFromCodeCoverage]
    public SimpleGlobalHook(IGlobalHookProvider? globalHookProvider)
        : base(globalHookProvider)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [ExcludeFromCodeCoverage]
    public SimpleGlobalHook(bool runAsyncOnBackgroundThread)
        : base(runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    /// <param name="globalHookProvider">The underlying global hook provider.</param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="globalHookProvider"/> is <see langword="null" />.
    /// </exception>
    [ExcludeFromCodeCoverage]
    public SimpleGlobalHook(IGlobalHookProvider globalHookProvider, bool runAsyncOnBackgroundThread)
        : base(globalHookProvider, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="SimpleGlobalHook" />.
    /// </summary>
    /// <param name="globalHookType">The global hook type.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
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
