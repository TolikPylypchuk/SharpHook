namespace SharpHook;

/// <summary>
/// Represents options for <see cref="TaskPoolGlobalHook" />.
/// </summary>
/// <seealso cref="TaskPoolGlobalHook" />
public sealed class TaskPoolGlobalHookOptions
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
    public TaskPoolGlobalHookOptions(int parallelismLevel) =>
        this.ParallelismLevel = parallelismLevel;

    /// <summary>
    /// Gets the configured parallelism level of the event handlers.
    /// </summary>
    /// <value>The parallelism level of the event handlers.</value>
    public int ParallelismLevel { get; }
}
