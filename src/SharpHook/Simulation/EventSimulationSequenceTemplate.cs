namespace SharpHook.Simulation;

/// <summary>
/// Represents an object that can simulate a predetermined sequence of events.
/// </summary>
public sealed class EventSimulationSequenceTemplate : IEventSimulationSequenceTemplate
{
    private EventSimulator owner;
    private readonly UioHookEvent[] events;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSimulationSequenceTemplate" /> class.
    /// </summary>
    /// <param name="owner">The owner of this template.</param>
    /// <param name="events">The events to simulate. They will be copied to ensure the events don't change.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="owner" /> or <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    /// If <paramref name="owner" /> is disposed, then this instance becomes instantly disposed as well.
    /// </remarks>
    public EventSimulationSequenceTemplate(EventSimulator owner, IEnumerable<UioHookEvent> events)
    {
        this.owner = owner ?? throw new ArgumentNullException(nameof(owner));

        if (events is null)
        {
            throw new ArgumentNullException(nameof(events));
        }

        if (events.Any(@event => @event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked))
        {
            throw new ArgumentOutOfRangeException(
                nameof(events), "Events of type KeyTyped or MouseClicked are not allowed");
        }

        this.events = [.. events];

        this.owner.OnDisposed += this.Dispose;
        if (this.owner.IsDisposed)
        {
            this.Dispose();
        }
    }

    /// <summary>
    /// Gets the value which indicates whether the template has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the template has been disposed. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>A disposed template cannot be used to simulate events.</remarks>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Simulates the predetermined sequence of events.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="ObjectDisposedException">The template is disposed.</exception>
    public UioHookResult Simulate()
    {
        this.ThrowIfDisposed();
        return this.owner.SimulationProvider.PostEvents(this.events, (uint)this.events.Length);
    }

    /// <summary>
    /// Disposes of this template.
    /// </summary>
    public void Dispose()
    {
        this.IsDisposed = true;
        this.owner.OnDisposed -= this.Dispose;
        this.owner = null!;
    }

    private void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.IsDisposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} – the object is disposed");
        }
    }
}
