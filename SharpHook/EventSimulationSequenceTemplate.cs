namespace SharpHook;

/// <summary>
/// Represents an object that can simulate a predetermined sequence of events.
/// </summary>
public sealed class EventSimulationSequenceTemplate : IEventSimulationSequenceTemplate
{
    private readonly UioHookEvent[] events;
    private readonly IEventSimulationProvider simulationProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSimulationSequenceTemplate" /> class.
    /// </summary>
    /// <param name="events">The events to simulate. They will be copied to ensure the events don't change.</param>
    /// <param name="simulationProvider">
    /// The simulation functionality provider (or <see cref="UioHookProvider.Instance" /> if <see langword="null" />).
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    public EventSimulationSequenceTemplate(
        IEnumerable<UioHookEvent> events,
        IEventSimulationProvider? simulationProvider = null)
    {
        this.events = [.. events ?? throw new ArgumentNullException(nameof(events))];
        this.simulationProvider = simulationProvider ?? UioHookProvider.Instance;
    }

    /// <summary>
    /// Simulates the predetermined sequence of events.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public UioHookResult Simulate() =>
        this.simulationProvider.PostEvents(this.events, (uint)this.events.Length);
}
