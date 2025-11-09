namespace SharpHook;

/// <summary>
/// Represents an object that can simulate a predetermined sequence of events.
/// </summary>
public interface IEventSimulationSequenceTemplate
{
    /// <summary>
    /// Simulates the predetermined sequence of events.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    UioHookResult Simulate();
}
