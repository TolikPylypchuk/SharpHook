namespace SharpHook.Simulation;

public sealed class EventSimulatorTests
{
    private const string ApplicationName = "TestApp";
    private const int NanosecondsPerTick = 100;

    [Property(DisplayName = "EventSimulator should simulate key presses")]
    public void SimulateKeyPress(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateKeyPress(keyCode);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.KeyPressed, postedEvent.Type);
        Assert.Equal(keyCode, postedEvent.Keyboard.KeyCode);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating a key press fails")]
    public void SimulateKeyPressFail(KeyCode keyCode, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateKeyPress(keyCode);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate key releases")]
    public void SimulateKeyRelease(KeyCode keyCode)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateKeyRelease(keyCode);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.KeyReleased, postedEvent.Type);
        Assert.Equal(keyCode, postedEvent.Keyboard.KeyCode);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating a key release fails")]
    public void SimulateKeyReleaseFail(KeyCode keyCode, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateKeyRelease(keyCode);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse presses")]
    public void SimulateMousePress(MouseButton button)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMousePress(button);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(button, postedEvent.Mouse.Button);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating a mouse press fails")]
    public void SimulateMousePressFail(MouseButton button, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMousePress(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse presses with explicit clicks")]
    public void SimulateMousePressExplicitClicks(MouseButton button, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMousePress(button, clicks);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(button, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating a mouse press with explicit clicks fails")]
    public void SimulateMousePressExplicitClicksFail(MouseButton button, ushort clicks, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMousePress(button, clicks);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse presses with explicit coordinates")]
    public void SimulateMousePressExplicitCoordinates(short x, short y, MouseButton button)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMousePress(x, y, button);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressed, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(button, postedEvent.Mouse.Button);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating a mouse press with explicit coordinates fails")]
    public void SimulateMousePressExplicitCoordinatesFail(
        short x,
        short y,
        MouseButton button,
        FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMousePress(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse presses with explicit coordinates and clicks")]
    public void SimulateMousePressExplicitCoordinatesAndClicks(short x, short y, MouseButton button, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMousePress(x, y, button, clicks);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MousePressed, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(button, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating a mouse press with explicit coordinates and clicks fails")]
    public void SimulateMousePressExplicitCoordinatesAndClicksFail(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMousePress(x, y, button, clicks);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse releases")]
    public void SimulateMouseRelease(MouseButton button)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseRelease(button);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(button, postedEvent.Mouse.Button);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating a mouse release fails")]
    public void SimulateMouseReleaseFail(MouseButton button, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseRelease(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse releases with explicit clicks")]
    public void SimulateMouseReleaseExplicitClicks(MouseButton button, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseRelease(button, clicks);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleasedIgnoreCoordinates, postedEvent.Type);
        Assert.Equal(button, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating a mouse press with explicit clicks fails")]
    public void SimulateMouseReleaseExplicitClicksFail(MouseButton button, ushort clicks, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMousePress(button, clicks);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse releases with explicit coordinates")]
    public void SimulateMouseReleaseExplicitCoordinates(short x, short y, MouseButton button)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseRelease(x, y, button);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleased, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(button, postedEvent.Mouse.Button);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating a mouse release with explicit coordinates fails")]
    public void SimulateMouseReleaseExplicitCoordinatesFail(
        short x,
        short y,
        MouseButton button,
        FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseRelease(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse releases with explicit coordinates and clicks")]
    public void SimulateMouseReleaseExplicitCoordinatesAndClicks(short x, short y, MouseButton button, ushort clicks)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseRelease(x, y, button, clicks);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseReleased, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
        Assert.Equal(button, postedEvent.Mouse.Button);
        Assert.Equal(clicks, postedEvent.Mouse.Clicks);
    }

    [Property(DisplayName =
        "EventSimulator should return an error if simulating " +
        "a mouse release with explicit coordinates and clicks fails")]
    public void SimulateMouseReleaseExplicitCoordinatesAndClicksFail(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseRelease(x, y, button, clicks);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse movement")]
    public void SimulateMouseMovement(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseMovement(x, y);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseMoved, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating mouse movement fails")]
    public void SimulateMouseMovementFail(short x, short y, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseMovement(x, y);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate relative mouse movement")]
    public void SimulateMouseMovementRelative(short x, short y)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseMovementRelative(x, y);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseMovedRelative, postedEvent.Type);
        Assert.Equal(x, postedEvent.Mouse.X);
        Assert.Equal(y, postedEvent.Mouse.Y);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating relative mouse movement fails")]
    public void SimulateMouseMovementRelativeFail(short x, short y, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseMovementRelative(x, y);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate mouse wheel")]
    public void SimulateMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateMouseWheel(rotation, direction, type);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedEvent = Assert.Single(provider.PostedEvents);

        Assert.Equal(EventType.MouseWheel, postedEvent.Type);
        Assert.Equal(rotation, postedEvent.Wheel.Rotation);
        Assert.Equal(direction, postedEvent.Wheel.Direction);
        Assert.Equal(type, postedEvent.Wheel.Type);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating mouse wheel fails")]
    public void SimulateMouseWheelFail(
        short rotation,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type,
        FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateMouseWheel(rotation, direction, type);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "EventSimulator should simulate text entry")]
    public void SimulateTextEntry(NonNull<string> text)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var result = simulator.SimulateTextEntry(text.Get);

        // Assert

        Assert.Equal(UioHookResult.Success, result);

        var postedText = Assert.Single(provider.PostedText);

        Assert.Equal(text.Get, postedText);
    }

    [Property(DisplayName = "EventSimulator should return an error if simulating text entry fails")]
    public void SimulateTextEntryFail(NonNull<string> text, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostTextResult = result.Value
        };

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act

        var actualResult = simulator.SimulateTextEntry(text.Get);

        // Assert

        Assert.Equal(result.Value, actualResult);
    }

    [Fact(DisplayName = "EventSimulator should throw when simulating null text entry")]
    public void SimulateTextEntryNull()
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => simulator.SimulateTextEntry(null!));
    }

    [Property(DisplayName = "EventSimulator should return a correct text simulation delay on Linux")]
    public void GetTextSimulationDelayOnLinux(ulong delay)
    {
        // Arrange

        var provider = new TestProvider();
        ((IEventSimulationProvider)provider).PostTextDelayLinux = delay;

        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act + Assert

        Assert.Equal(
            delay / NanosecondsPerTick * NanosecondsPerTick,
            (ulong)(simulator.TextSimulationDelayOnLinux.Ticks * NanosecondsPerTick));
    }

    [Property(DisplayName = "EventSimulator should set a correct text simulation delay on Linux or throw an exception")]
    public void SetTextSimulationDelayOnLinux(TimeSpan timeSpan)
    {
        // Arrange

        var provider = new TestProvider();
        var simulator = EventSimulator.Create(ApplicationName, provider);

        // Act + Assert

        if (timeSpan >= TimeSpan.Zero)
        {
            simulator.TextSimulationDelayOnLinux = timeSpan;
            Assert.Equal(
                (ulong)(timeSpan.Ticks * NanosecondsPerTick),
                ((IEventSimulationProvider)provider).PostTextDelayLinux);
        } else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => simulator.TextSimulationDelayOnLinux = timeSpan);
        }
    }

    [Fact(DisplayName = "EventSimulator should not throw if the application name is null")]
    public void NullApplicationNameError()
    {
        var provider = new TestProvider();
        var exception = Record.Exception(() => EventSimulator.Create(null!, provider));
        Assert.Null(exception);
    }

    [Fact(DisplayName = "EventSimulator should not throw if the provider is null")]
    public void NullProviderError()
    {
        var exception = Record.Exception(() => EventSimulator.Create(ApplicationName, null));
        Assert.Null(exception);
    }
}
