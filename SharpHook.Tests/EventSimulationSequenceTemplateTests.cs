namespace SharpHook;

public sealed class EventSimulationSequenceTemplateTests
{
    [Property(DisplayName = "Simulate should post events using the simulation provider")]
    public void Simulate(NonNull<UioHookEvent[]> events)
    {
        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var template = new EventSimulationSequenceTemplate(eventsToSimulate, provider);

        // Act

        template.Simulate();

        // Assert

        Assert.Equal(eventsToSimulate, provider.PostedEvents);
    }

    [Fact(DisplayName = "The EventSimulationSequenceTemplate constructor should throw if the events are null")]
    public void NullEvents() =>
        Assert.Throws<ArgumentNullException>(() => new EventSimulationSequenceTemplate(null!));
}
