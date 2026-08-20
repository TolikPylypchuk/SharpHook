using System.Collections.Concurrent;

namespace SharpHook;

/// <summary>
/// Represents an implementation of <see cref="IGlobalHook" /> which dispatches events to an event loop running on a
/// separate dedicated thread.
/// </summary>
/// <remarks>
/// <para>
/// The event handlers will run on a separate thread. This way the hook itself will not be blocked if the handlers are
/// long-running. The exception is the <see cref="IGlobalHook.HookDisabled" /> event which will run on the same thread
/// on which the hook itself is running since at that point it doesn't matter anymore that the hook is not blocked.
/// </para>
/// <para>
/// Setting <see cref="HookEventArgs.SuppressEvent" /> inside the handlers will have no effect as they are run
/// on another thread.
/// </para>
/// </remarks>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
public sealed class EventLoopGlobalHook : GlobalHookBase
{
#if NET9_0_OR_GREATER
    private readonly Lock syncRoot = new();
#else
    private readonly object syncRoot = new();
#endif

    private readonly BlockingCollection<UioHookEvent> eventLoop = [];
    private bool eventLoopStarted = false;

    /// <summary>
    /// Initializes a new instance of <see cref="EventLoopGlobalHook" />.
    /// </summary>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public EventLoopGlobalHook(IGlobalHookProvider? globalHookProvider = null)
        : base(globalHookProvider)
    { }

    /// <summary>
    /// Starts the event loop.
    /// </summary>
    protected override void BeforeRun()
    {
        if (!this.eventLoopStarted)
        {
            lock (this.syncRoot)
            {
                if (!this.eventLoopStarted)
                {
                    new Thread(this.RunEventLoop).Start();
                    this.eventLoopStarted = true;
                }
            }
        }
    }

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
            this.eventLoop.Add(@event);
        } else
        {
            this.DispatchEvent(ref @event);
        }
    }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running, and completing the event loop.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true" /> if the method is called from the <see cref="BasicGlobalHookBase.Dispose()" /> method.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    protected override void Dispose(bool disposing)
    {
        lock (this.syncRoot)
        {
            this.eventLoop.CompleteAdding();
        }

        base.Dispose(disposing);
    }

    private void RunEventLoop()
    {
        foreach (var @event in this.eventLoop.GetConsumingEnumerable())
        {
            var e = @event;
            this.DispatchEvent(ref e);
        }

        lock (this.syncRoot)
        {
            this.eventLoop.Dispose();
        }
    }
}
