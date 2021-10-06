using System;
using System.Threading.Tasks;

using SharpHook.Internal;
using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Represents a task pool-based implementation of <see cref="IGlobalHook" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The event handlers will run with the specified parallelism level on separate threads inside the default thread
    /// pool for tasks. This is done so that the hook itself will not be blocked if the handlers are long-running.
    /// The exception is the <see cref="IGlobalHook.HookDisabled" /> event which will run on the same thread that called
    /// the <see cref="IDisposable.Dispose()" /> method since at that point it doesn't matter anymore that the hook is
    /// not blocked.
    /// </para>
    /// <para>
    /// The <see cref="UioHookEvent" /> instance passed to the handlers will be a copy of the original
    /// data passed from libuiohook.
    /// </para>
    /// </remarks>
    public sealed class TaskPoolGlobalHook : GlobalHookBase
    {
        private readonly TaskQueue taskQueue;

        /// <summary>
        /// Initializes a new instance of <see cref="TaskPoolGlobalHook" />.
        /// </summary>
        /// <remarks>
        /// Calling this constructor is the same as passing <see cref="TaskPoolGlobalHookOptions.Sequential" /> to the
        /// other constructor.
        /// </remarks>
        public TaskPoolGlobalHook()
            : this(TaskPoolGlobalHookOptions.Sequential)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskPoolGlobalHook" />.
        /// </summary>
        /// <param name="options">The options of the hook which include its parallelism level.</param>
        public TaskPoolGlobalHook(TaskPoolGlobalHookOptions options) =>
            this.taskQueue = new(options.ParallelismLevel);

        /// <summary>
        /// Handles the hook event.
        /// </summary>
        /// <param name="e">The event to handle.</param>
        protected override void HandleHookEventInternal(ref UioHookEvent e)
        {
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
}
