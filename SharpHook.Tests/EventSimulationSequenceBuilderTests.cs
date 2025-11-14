namespace SharpHook;

public sealed class EventSimulationSequenceBuilderTests
{
    [Property(DisplayName = "AddEvent and Simulate should post an event")]
    public void AddEvent(UioHookEvent @event)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddEvent(@event)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);
        Assert.Equal(@event, provider.PostedEvents[0]);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddEvents and Simulate should post events")]
    public void AddEvents(NonNull<UioHookEvent[]> events)
    {
        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

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
        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddEvents(eventsToSimulate.AsEnumerable())
            .Simulate();

        // Assert

        Assert.Equal(eventsToSimulate, provider.PostedEvents);
        Assert.Equal(UioHookResult.Success, result);
    }

    [Fact(DisplayName = "AddEvents should throw if the events array is null")]
    public void AddEventsNull()
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => builder.AddEvents(null!));
    }

    [Fact(DisplayName = "AddEvents should throw if the events enumerable is null")]
    public void AddEventsEnumerableNull()
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => builder.AddEvents((IEnumerable<UioHookEvent>)null!));
    }

    [Property(DisplayName = "AddKeyPress and Simulate should post a key press event")]
    public void AddKeyPress(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddKeyPress(keyCode)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.KeyPressed, actualEvent.Type);
        Assert.Equal(keyCode, actualEvent.Keyboard.KeyCode);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddKeyRelease and Simulate should post a key release event")]
    public void AddKeyRelease(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddKeyRelease(keyCode)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.KeyReleased, actualEvent.Type);
        Assert.Equal(keyCode, actualEvent.Keyboard.KeyCode);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress and Simulate should post a mouse button press event")]
    public void AddMousePress(MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMousePress(mouseButton)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MousePressedIgnoreCoordinates, actualEvent.Type);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(0, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress with clicks and Simulate should post a mouse button press event")]
    public void AddMousePressClicks(MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMousePress(mouseButton, clicks)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MousePressedIgnoreCoordinates, actualEvent.Type);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(clicks, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMousePress with coordinates and Simulate should post a mouse button press event")]
    public void AddMousePressCoordinates(short x, short y, MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMousePress(x, y, mouseButton)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MousePressed, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(0, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName =
        "AddMousePress with coordinates and clicks and Simulate should post a mouse button press event")]
    public void AddMousePressCoordinatessClicks(short x, short y, MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMousePress(x, y, mouseButton, clicks)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MousePressed, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(clicks, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease and Simulate should post a mouse button release event")]
    public void AddMouseRelease(MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseRelease(mouseButton)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, actualEvent.Type);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(0, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease with clicks and Simulate should post a mouse button release event")]
    public void AddMouseReleaseClicks(MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseRelease(mouseButton, clicks)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, actualEvent.Type);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(clicks, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseRelease with coordinates and Simulate should post a mouse button release event")]
    public void AddMouseReleaseCoordinates(short x, short y, MouseButton mouseButton)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseRelease(x, y, mouseButton)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseReleased, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(0, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName =
        "AddMouseRelease with coordinates and clicks and Simulate should post a mouse button release event")]
    public void AddMouseReleaseCoordinatessClicks(short x, short y, MouseButton mouseButton, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseRelease(x, y, mouseButton, clicks)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseReleased, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);
        Assert.Equal(mouseButton, actualEvent.Mouse.Button);
        Assert.Equal(clicks, actualEvent.Mouse.Clicks);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseMovement and Simulate should post a mouse movement event")]
    public void AddMouseMovement(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseMovement(x, y)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseMoved, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseMovementRelative and Simulate should post a relative mouse movement event")]
    public void AddMouseMovementRelative(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseMovementRelative(x, y)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseMovedRelativeToCursor, actualEvent.Type);
        Assert.Equal(x, actualEvent.Mouse.X);
        Assert.Equal(y, actualEvent.Mouse.Y);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "AddMouseWheel and Simulate should post a mouse wheel event")]
    public void AddMouseWheel(short rotation, MouseWheelScrollDirection direction, MouseWheelScrollType type)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddMouseWheel(rotation, direction, type)
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.Equal(EventType.MouseWheel, actualEvent.Type);
        Assert.Equal(rotation, actualEvent.Wheel.Rotation);
        Assert.Equal(direction, actualEvent.Wheel.Direction);
        Assert.Equal(type, actualEvent.Wheel.Type);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Property(DisplayName = "RemoveEvent should remove an event from the sequence")]
    public void RemoveEvent(UioHookEvent @event)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

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
        // Arrange

        var eventsToSimulate = events.Get;

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

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

        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder.Simulate();

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(failedResult.Value, result);
    }

    [Property(DisplayName = "CreateTemplate should create a template which posts an event")]
    public void CreateTemplate(UioHookEvent @event)
    {
        // Arrange

        var provider = new TestProvider();
        var builder = new EventSimulationSequenceBuilder(provider);

        // Act

        var result = builder
            .AddEvent(@event)
            .CreateTemplate()
            .Simulate();

        // Assert

        Assert.Single(provider.PostedEvents);
        Assert.Equal(@event, provider.PostedEvents[0]);
        Assert.Equal(UioHookResult.Success, result);
    }
}
