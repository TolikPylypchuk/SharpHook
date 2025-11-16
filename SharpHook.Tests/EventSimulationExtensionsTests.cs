namespace SharpHook;

public sealed class EventSimulationExtensionsTests
{
    [Property(DisplayName = "AddKeyStroke with an array should add events to the sequence")]
    public void AddKeyStrokeArray(KeyCode[] keyCodes)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddKeyStroke(keyCodes)
            .Simulate();

        // Assert

        AssertKeyStroke(keyCodes, provider);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddKeyStroke with an enumerable should add events to the sequence")]
    public void AddKeyStrokeEnumerable(List<KeyCode> keyCodes)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddKeyStroke(keyCodes)
            .Simulate();

        // Assert

        AssertKeyStroke(keyCodes, provider);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "SimulateKeyStroke with an array should simulate events")]
    public void SimulateKeyStrokeArray(KeyCode[] keyCodes)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = new EventSimulator(provider);

        // Act

        var result = simulator.SimulateKeyStroke(keyCodes);

        // Assert

        AssertKeyStroke(keyCodes, provider);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "SimulateKeyStroke with an enumerable should simulate events")]
    public void SimulateKeyStrokeEnumerable(List<KeyCode> keyCodes)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = new EventSimulator(provider);

        // Act

        var result = simulator.SimulateKeyStroke(keyCodes);

        // Assert

        AssertKeyStroke(keyCodes, provider);

        Assert.Equal(UioHookResult.Success, result);
    }

    private void AssertKeyStroke(IReadOnlyList<KeyCode> keyCodes, TestProvider provider)
    {
        Assert.Equal(keyCodes.Count * 2, provider.PostedEvents.Count);

        foreach (var (keyCode, actualEvent) in keyCodes.Zip(provider.PostedEvents))
        {
            Assert.Equal(EventType.KeyPressed, actualEvent.Type);
            Assert.Equal(keyCode, actualEvent.Keyboard.KeyCode);
        }

        foreach (var (keyCode, actualEvent) in
            Enumerable.Reverse(keyCodes).Zip(provider.PostedEvents.Skip(keyCodes.Count)))
        {
            Assert.Equal(EventType.KeyReleased, actualEvent.Type);
            Assert.Equal(keyCode, actualEvent.Keyboard.KeyCode);
        }
    }
}
