namespace SharpHook.Simulation;

public sealed class EventSimulationSequenceTemplateTests
{
    private const string ApplicationName = "TestApp";

    [Property(DisplayName = "Simulate should post events using the simulation provider")]
    public void Simulate(NonNull<UioHookEvent[]> events)
    {
        if (events.Get.Any(@event => @event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked))
        {
            return;
        }

        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var template = new EventSimulationSequenceTemplate(simulator, eventsToSimulate);

        // Act

        template.Simulate();

        // Assert

        Assert.Equal(eventsToSimulate, provider.PostedEvents);
    }

    [Property(DisplayName =
        "The EventSimulationSequenceTemplate constructor should throw if the event has type KeyTyped")]
    public void KeyTyped(KeyboardEvent keyboardEvent)
    {
        // Arrange

        var @event = keyboardEvent.Value;
        @event.Type = EventType.KeyTyped;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => new EventSimulationSequenceTemplate(simulator, [@event]));
    }

    [Property(DisplayName =
        "The EventSimulationSequenceTemplate constructor should throw if the event has type MouseClicked")]
    public void MouseClicked(MouseEvent mouseEvent)
    {
        // Arrange

        var @event = mouseEvent.Value;
        @event.Type = EventType.MouseClicked;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => new EventSimulationSequenceTemplate(simulator, [@event]));
    }

    [Fact(DisplayName = "EventSimulationSequenceTemplate should be disposed once its owner is disposed")]
    public void OwnerDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceTemplate(simulator, []);

        // Act

        simulator.Dispose();

        // Assert

        Assert.True(builder.IsDisposed);
    }

    [Fact(DisplayName = "EventSimulationSequenceTemplate should be disposed instantly if its owner is disposed")]
    public void OwnerInstantlyDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        simulator.Dispose();
        var builder = new EventSimulationSequenceTemplate(simulator, []);

        // Assert

        Assert.True(builder.IsDisposed);
    }

    [Fact(DisplayName = "The EventSimulationSequenceTemplate constructor should throw if the owner is null")]
    public void NullOwner() =>
        Assert.Throws<ArgumentNullException>(() => new EventSimulationSequenceTemplate(null!, []));

    [Fact(DisplayName = "The EventSimulationSequenceTemplate constructor should throw if the events are null")]
    public void NullEvents()
    {
        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        Assert.Throws<ArgumentNullException>(() => new EventSimulationSequenceTemplate(simulator, null!));
    }
}
