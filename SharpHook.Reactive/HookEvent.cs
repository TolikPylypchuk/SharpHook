using System.Reactive;

namespace SharpHook.Reactive
{
    /// <summary>
    /// Represents an event which is raised by <see cref="IReactiveGlobalHook" />.
    /// </summary>
    /// <typeparam name="TArgs">The type of the event's data.</typeparam>
    public sealed class HookEvent<TArgs>
        where TArgs : HookEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IReactiveGlobalHook" />.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The data of the event.</param>
        public HookEvent(object? sender, TArgs args)
        {
            this.Sender = sender;
            this.Args = args;
        }

        public object? Sender { get; }
        public TArgs Args { get; }
    }

    internal static class HookEvent
    {
        internal static HookEvent<TArgs> FromEventPattern<TArgs>(EventPattern<TArgs> pattern)
            where TArgs : HookEventArgs =>
            new(pattern.Sender, pattern.EventArgs);
    }
}
