namespace SharpHook;

/// <summary>
/// Represents basic operations for a global keyboard and mouse hook regardless of the form of its events.
/// </summary>
/// <remarks>
/// It is highly recommended not to implement this interface directly. If you want to create a custom global hook, you
/// should instead extend the <see cref="BasicGlobalHookBase" /> class as it correctly implements the invariants
/// required for advanced scenarios.
/// </remarks>
public interface IBasicGlobalHook : IDisposable
{
    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    bool IsRunning { get; }

    /// <summary>
    /// Gets the value which indicates whether the global hook has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the global hook has been disposed. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>A disposed global hook cannot be started again.</remarks>
    bool IsDisposed { get; }

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be stopped temporarily by calling
    /// <see cref="Stop" /> or stopped permanently by calling <see cref="IDisposable.Dispose" />.
    /// </summary>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    void Run();

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be stopped temporarily by calling
    /// <see cref="Stop" /> or stopped permanently by calling <see cref="IDisposable.Dispose" />.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is stopped.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// Since the underlying native API for running a global hook is blocking, the only way to run it without blocking
    /// the current thread is to run it on a separate thread.
    /// </remarks>
    Task RunAsync();

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// After stopping, the global hook can run again.
    /// </remarks>
    void Stop();
}
