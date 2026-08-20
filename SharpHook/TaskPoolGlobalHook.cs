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
/// <seealso cref="EventLoopGlobalHook" />
public sealed class TaskPoolGlobalHook : GlobalHookBase
{
    private const int DefaultParallelismLevel = 1;

    private readonly TaskQueue taskQueue;

    /// <summary>
    /// Initializes a new instance of <see cref="TaskPoolGlobalHook" />.
    /// </summary>
    /// <param name="parallelismLevel">The parallelism level of event handlers.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public TaskPoolGlobalHook(
        int parallelismLevel = DefaultParallelismLevel,
        IGlobalHookProvider? globalHookProvider = null)
        : base(globalHookProvider) =>
        this.taskQueue = new(parallelismLevel);

    /// <summary>
    /// Handles the hook event.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    protected override void HandleHookEvent(ref UioHookEvent @event)
    {
        if (!this.ShouldDispatchEvent(ref @event))
        {
            return;
        }

        if (@event.Type != EventType.HookDisabled)
        {
            var copy = @event;
            this.taskQueue.Enqueue(() => Task.Run(() => this.DispatchEvent(ref copy)));
        } else
        {
            this.DispatchEvent(ref @event);
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
        try
        {
            base.Dispose(disposing);
        } finally
        {
            if (disposing)
            {
                this.taskQueue.Dispose();
            }
        }
    }
}
