namespace SharpHook.Simulation;

/// <summary>
/// Represents an object that can simulate a predetermined sequence of events.
/// </summary>
public interface IEventSimulationSequenceTemplate : IStatefulDisposable
{
    /// <summary>
    /// Simulates the predetermined sequence of events.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="ObjectDisposedException">The builder has been disposed.</exception>
    UioHookResult Simulate();
}
