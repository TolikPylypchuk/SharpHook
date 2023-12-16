namespace SharpHook;

/// <summary>
/// Represents options for <see cref="TaskPoolGlobalHook" />.
/// </summary>
/// <param name="parallelismLevel">The parallelism level of the event handlers.</param>
/// <param name="runAsyncOnBackgroundThread">
/// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
/// Otherwise, <see langword="false" />.
/// </param>
/// <seealso cref="TaskPoolGlobalHook" />
public sealed class TaskPoolGlobalHookOptions(int parallelismLevel, bool runAsyncOnBackgroundThread)
{
    /// <summary>
    /// The default options with the parallelism level of 1.
    /// </summary>
    /// <value>The options with the parallelism level of 1.</value>
    public static readonly TaskPoolGlobalHookOptions Sequential = new(1);

    /// <summary>
    /// Initializes a new instance of <see cref="TaskPoolGlobalHookOptions" />.
    /// </summary>
    /// <param name="parallelismLevel">The parallelism level of the event handlers.</param>
    public TaskPoolGlobalHookOptions(int parallelismLevel)
        : this(parallelismLevel, false)
    { }

    /// <summary>
    /// Initializes a new instance of <see cref="TaskPoolGlobalHookOptions" />.
    /// </summary>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    public TaskPoolGlobalHookOptions(bool runAsyncOnBackgroundThread)
        : this(1, runAsyncOnBackgroundThread)
    { }

    /// <summary>
    /// Gets the configured parallelism level of the event handlers.
    /// </summary>
    /// <value>The parallelism level of the event handlers.</value>
    public int ParallelismLevel { get; } = parallelismLevel;

    /// <summary>
    /// Gets the value of whether <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if <see cref="IGlobalHook.RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </value>
    public bool RunAsyncOnBackgroundThread { get; } = runAsyncOnBackgroundThread;
}
