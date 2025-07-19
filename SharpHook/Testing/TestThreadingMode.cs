namespace SharpHook.Testing;

/// <summary>
/// Represents the mode of dispatching simulated events in testing hooks – either on the same thread which simulates
/// them, or on the thread which runs the testing hook.
/// </summary>
public enum TestThreadingMode
{
    /// <summary>
    /// Simulated events will be dispatched immediately on the same thread which simulates them.
    /// </summary>
    Simple,

    /// <summary>
    /// Simulated events will be posted to an event loop and dispatched on the thread on which the testing hook runs.
    /// </summary>
    EventLoop
}
