namespace SharpHook
{
    /// <summary>
    /// Represents options for <see cref="ThreadPoolGlobalHook" />.
    /// </summary>
    public sealed class ThreadPoolGlobalHookOptions
    {
        /// <summary>
        /// The default options with the parallelism level of 1.
        /// </summary>
        public static readonly ThreadPoolGlobalHookOptions Sequential = new(1);

        /// <summary>
        /// Initializes a new instance of <see cref="ThreadPoolGlobalHookOptions" />.
        /// </summary>
        /// <param name="parallelismLevel">The parallelism level of the event handlers.</param>
        public ThreadPoolGlobalHookOptions(int parallelismLevel) =>
            this.ParallelismLevel = parallelismLevel;

        /// <summary>
        /// Gets the configured parallelism level of the event handlers.
        /// </summary>
        public int ParallelismLevel { get; }

    }
}
