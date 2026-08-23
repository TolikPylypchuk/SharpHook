namespace SharpHook.Simulation;

public sealed class EventSimulationSequenceBuilderTests
{
    private const string ApplicationName = "TestApp";

    [Property(DisplayName = "AddEvent and Simulate should post an event")]
    public void AddEvent(UioHookEvent @event)
    {
        if (@event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked)
        {
            return;
        }

        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvent(@event)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(@event, postedEvent);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddEvents and Simulate should post events")]
    public void AddEvents(NonNull<UioHookEvent[]> events)
    {
        if (events.Get.Any(@event => @event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked))
        {
            return;
        }

        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvents(eventsToSimulate)
            .Simulate();

        // Assert

        Assert.Equal(eventsToSimulate, provider.PostedEvents);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddEvents for IEnumerable and Simulate should post events")]
    public void AddEventsEnumerable(NonNull<UioHookEvent[]> events)
    {
        if (events.Get.Any(@event => @event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked))
        {
            return;
        }

        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvents(eventsToSimulate.AsEnumerable())
            .Simulate();

        // Assert

        Assert.Equal(eventsToSimulate, provider.PostedEvents);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddEvent should throw if the event has type KeyTyped")]
    public void AddEventKeyTyped(KeyboardEvent keyboardEvent)
    {
        // Arrange

        var @event = keyboardEvent.Value;
        @event.Type = EventType.KeyTyped;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvent(@event));
    }

    [Property(DisplayName = "AddEvent should throw if the event has type MouseClicked")]
    public void AddEventMouseClicked(MouseEvent mouseEvent)
    {
        // Arrange

        var @event = mouseEvent.Value;
        @event.Type = EventType.MouseClicked;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvent(@event));
    }

    [Fact(DisplayName = "AddEvents should throw if the events array is null")]
    public void AddEventsNull()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => builder.AddEvents(null!));
    }

    [Property(DisplayName = "AddEvents should throw if the event has type KeyTyped")]
    public void AddEventsKeyTyped(KeyboardEvent keyboardEvent)
    {
        // Arrange

        var @event = keyboardEvent.Value;
        @event.Type = EventType.KeyTyped;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvents(@event));
    }

    [Property(DisplayName = "AddEvents should throw if the event has type MouseClicked")]
    public void AddEventsMouseClicked(MouseEvent mouseEvent)
    {
        // Arrange

        var @event = mouseEvent.Value;
        @event.Type = EventType.MouseClicked;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvents(@event));
    }

    [Fact(DisplayName = "AddEvents should throw if the events enumerable is null")]
    public void AddEventsEnumerableNull()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => builder.AddEvents((IEnumerable<UioHookEvent>)null!));
    }

    [Property(DisplayName = "AddEvents with enumerable should throw if the event has type KeyTyped")]
    public void AddEventsEnumerableKeyTyped(KeyboardEvent keyboardEvent)
    {
        // Arrange

        var @event = keyboardEvent.Value;
        @event.Type = EventType.KeyTyped;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvents([@event]));
    }

    [Property(DisplayName = "AddEvents with enumerable should throw if the event has type MouseClicked")]
    public void AddEventsEnumerableMouseClicked(MouseEvent mouseEvent)
    {
        // Arrange

        var @event = mouseEvent.Value;
        @event.Type = EventType.MouseClicked;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act + Assert

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.AddEvents([@event]));
    }

    [Property(DisplayName = "AddKeyPress and Simulate should post a key press event")]
    public void AddKeyPress(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddKeyPress(keyCode)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.KeyPressed, postedEvent.Type);
        Assert.Equal(keyCode, postedEvent.Keyboard.KeyCode);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddKeyRelease and Simulate should post a key release event")]
    public void AddKeyRelease(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddKeyRelease(keyCode)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.KeyReleased, postedEvent.Type);
        Assert.Equal(keyCode, postedEvent.Keyboard.KeyCode);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress and Simulate should post a mouse button press event")]
    public void AddMousePress(MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMousePress(mouseButton)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(0, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress with clicks and Simulate should post a mouse button press event")]
    public void AddMousePressClicks(MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMousePress(mouseButton, clicks)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress with coordinates and Simulate should post a mouse button press event")]
    public void AddMousePressCoordinates(short x, short y, MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMousePress(x, y, mouseButton)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressed, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(0, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName =
        "AddMousePress with coordinates and clicks and Simulate should post a mouse button press event")]
    public void AddMousePressCoordinatessClicks(short x, short y, MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMousePress(x, y, mouseButton, clicks)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressed, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease and Simulate should post a mouse button release event")]
    public void AddMouseRelease(MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseRelease(mouseButton)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(0, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease with clicks and Simulate should post a mouse button release event")]
    public void AddMouseReleaseClicks(MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseRelease(mouseButton, clicks)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease with coordinates and Simulate should post a mouse button release event")]
    public void AddMouseReleaseCoordinates(short x, short y, MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseRelease(x, y, mouseButton)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleased, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(0, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName =
        "AddMouseRelease with coordinates and clicks and Simulate should post a mouse button release event")]
    public void AddMouseReleaseCoordinatessClicks(short x, short y, MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseRelease(x, y, mouseButton, clicks)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleased, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(mouseButton, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseMovement and Simulate should post a mouse movement event")]
    public void AddMouseMovement(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseMovement(x, y)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseMoved, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseMovementRelative and Simulate should post a relative mouse movement event")]
    public void AddMouseMovementRelative(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseMovementRelative(x, y)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseMovedRelative, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseWheel and Simulate should post a mouse wheel event")]
    public void AddMouseWheel(short rotation, MouseWheelScrollDirection direction, MouseWheelScrollType type)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddMouseWheel(rotation, direction, type)
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseWheel, postedEvent.Type);
        Assert.Equal(rotation, postedEvent.Wheel.Rotation);
        Assert.Equal(direction, postedEvent.Wheel.Direction);
        Assert.Equal(type, postedEvent.Wheel.Type);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "RemoveEvent should remove an event from the sequence")]
    public void RemoveEvent(UioHookEvent @event)
    {
        if (@event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked)
        {
            return;
        }

        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvent(@event)
            .RemoveEvent(@event)
            .Simulate();

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "RemoveEvents should remove events from the sequence")]
    public void RemoveEvents(NonEmptyArray<UioHookEvent> events)
    {
        if (events.Any(@event => @event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked))
        {
            return;
        }

        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvents(eventsToSimulate)
            .RemoveEvents(eventsToSimulate.Contains)
            .Simulate();

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "Simulate should return an error if the provider returns an error")]
    public void SimulateError(FailedUioHookResult failedResult)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = failedResult.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder.Simulate();

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(failedResult.Value, result);
    }

    [Property(DisplayName = "CreateTemplate should create a template which posts an event")]
    public void CreateTemplate(UioHookEvent @event)
    {
        if (@event.Type == EventType.KeyTyped || @event.Type == EventType.MouseClicked)
        {
            return;
        }

        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        var result = builder
            .AddEvent(@event)
            .CreateTemplate()
            .Simulate();

        // Assert

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(@event, postedEvent);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Fact(DisplayName = "EventSimulationSequenceBuilder should be disposed once its owner is disposed")]
    public void OwnerDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Act

        simulator.Dispose();

        // Assert

        Assert.True(builder.IsDisposed);
    }

    [Fact(DisplayName = "EventSimulationSequenceBuilder should be disposed instantly if its owner is disposed")]
    public void OwnerInstantlyDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        simulator.Dispose();
        var builder = new EventSimulationSequenceBuilder(simulator);

        // Assert

        Assert.True(builder.IsDisposed);
    }

    [Fact(DisplayName = "The EventSimulationSequenceBuilder constructor should throw if the owner is null")]
    public void NullOwner() =>
        Assert.Throws<ArgumentNullException>(() => new EventSimulationSequenceBuilder(null!));

}
