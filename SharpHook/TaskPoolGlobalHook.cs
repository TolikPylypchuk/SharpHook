namespace SharpHook;

/// <summary>
/// Represents a task pool-based implementation of <see cref="IGlobalHook" />.
/// </summary>
/// <remarks>
/// <para>
/// The event handlers will run with the specified parallelism level on separate threads inside the default thread
/// pool for tasks. This way the hook itself will not be blocked if the handlers are long-running.
/// The exception is the <see cref="IGlobalHook.HookDisabled" /> event which will run on the same thread on which the
/// hook itself is running since at that point it doesn't matter anymore that the hook is not blocked.
/// </para>
/// <para>
/// Setting <see cref="HookEventArgs.SuppressEvent" /> inside the handlers will have no effect as they are run
/// on other threads.
/// </para>
/// </remarks>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="SimpleGlobalHook" />
public sealed class TaskPoolGlobalHook : GlobalHookBase
{
    private const int DefaultParallelismLevel = 1;

    private readonly TaskQueue taskQueue;

    /// <summary>
    /// Initializes a new instance of <see cref="TaskPoolGlobalHook" />.
    /// </summary>
    /// <param name="parallelismLevel">The parallelism level of event handlers.</param>
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
    public TaskPoolGlobalHook(
        int parallelismLevel = DefaultParallelismLevel,
        GlobalHookType globalHookType = GlobalHookType.All,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
        : base(globalHookType, globalHookProvider, runAsyncOnBackgroundThread) =>
        this.taskQueue = new(parallelismLevel);

    /// <summary>
    /// Handles the hook event.
    /// </summary>
    /// <param name="e">The event to handle.</param>
    protected override void HandleHookEvent(ref UioHookEvent e)
    {
        if (!this.ShouldDispatchEvent(ref e))
        {
            return;
        }

        if (e.Type != EventType.HookDisabled)
        {
            var copy = e;
            this.taskQueue.Enqueue(() => Task.Run(() => this.DispatchEvent(ref copy)));
        } else
        {
            this.DispatchEvent(ref e);
        }
    }

    /// <summary>
    /// Destoys the global hook.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true" /> if the method is called from the <see cref="IDisposable.Dispose()" /> method.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    /// <remarks>
    /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
    /// of <see cref="IGlobalHook" />.
    /// </remarks>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.taskQueue.Dispose();
        }

        base.Dispose(disposing);
    }
}
