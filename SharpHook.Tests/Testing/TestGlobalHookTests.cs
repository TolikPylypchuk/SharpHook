namespace SharpHook.Testing;

public sealed class TestGlobalHookTests
{
    public TestGlobalHookTests() =>
        Arb.Register<Generators>();

    [Property(DisplayName = "KeyPressed events should be simulated and handled")]
    public void HandleKeyPressed(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyPressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateKeyPress(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "KeyPressed events should be simulated and handled for null characters")]
    public void HandleKeyPressedNullCharacters(
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            KeyCodeToRawCode = c => rawCode,
            KeyCodeToChars = c => null!
        };

        // Act

        hook.KeyPressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateKeyPress(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "KeyReleased events should be simulated and handled")]
    public void HandleKeyReleased(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateKeyRelease(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "KeyTyped events should be simulated and handled")]
    public void HandleKeyTyped(
        char keyChar,
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            KeyCodeToChars = c => [keyChar],
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyTyped += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateKeyPress(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(hook.KeyCodeToChars(keyCode).First(), actualEventArgs.Data.KeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Equal(2, hook.SimulatedEvents.Count);

        var actualEvent = hook.SimulatedEvents[1];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MousePressed events should be simulated and handled")]
    public void HandleMousePressed(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            MouseClickCount = clicks
        };

        // Act

        hook.MousePressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMousePress(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MousePressed events with explicit clicks should be simulated and handled")]
    public void HandleMousePressedExplicitClicks(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MousePressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMousePress(button, clicks);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MousePressed events with explicit coordinates should be simulated and handled")]
    public void HandleMousePressedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            MouseClickCount = clicks
        };

        // Act

        hook.MousePressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMousePress(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MousePressed events with explicit coordinates and clicks should be simulated and handled")]
    public void HandleMousePressedExplicitCoordinatesAndClicks(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        // Act

        hook.MousePressed += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMousePress(x, y, button, clicks);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseReleased events should be simulated and handled")]
    public void HandleMouseReleased(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            MouseClickCount = clicks,
            RaiseMouseClicked = false
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseReleased events with explicit clicks should be simulated and handled")]
    public void HandleMouseReleasedExplicitClicks(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            RaiseMouseClicked = false
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(button, clicks);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseReleased events with explicit coordinates should be simulated and handled")]
    public void HandleMouseReleasedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            MouseClickCount = clicks,
            RaiseMouseClicked = false
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(
        DisplayName = "MouseReleased events with explicit coordinates and clicks should be simulated and handled")]
    public void HandleMouseReleasedExplicitCoordinatesAndClicks(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            RaiseMouseClicked = false
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(x, y, button, clicks);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseClicked events should be simulated and handled")]
    public void HandleMouseClicked(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            MouseClickCount = clicks,
            RaiseMouseClicked = true
        };

        // Act

        hook.MouseClicked += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Equal(2, hook.SimulatedEvents.Count);

        var actualEvent = hook.SimulatedEvents[1];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseClicked events with explicit coordinates should be simulated and handled")]
    public void HandleMouseClickedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            MouseClickCount = clicks,
            RaiseMouseClicked = true
        };

        // Act

        hook.MouseClicked += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseRelease(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Equal(2, hook.SimulatedEvents.Count);

        var actualEvent = hook.SimulatedEvents[1];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseMoved events should be simulated and handled")]
    public void HandleMouseMovement(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        // Act

        hook.MouseMoved += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseMovement(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "Relatvie MouseMoved events should be simulated and handled")]
    public void HandleMouseMovementRelative(
        short x,
        short y,
        short xOffset,
        short yOffset,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MouseMoved += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseMovementRelative(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseDragged events should be simulated and handled")]
    public void HandleMouseDragged(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            DragMouseWhenMoving = true
        };

        // Act

        hook.MouseDragged += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseMovement(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "Relatvie MouseDragged events should be simulated and handled")]
    public void HandleMouseDraggedRelative(
        short x,
        short y,
        short xOffset,
        short yOffset,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            DragMouseWhenMoving = true
        };

        // Act

        hook.MouseDragged += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseMovementRelative(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "MouseWheel events should be simulated and handled")]
    public void HandleMouseWheel(
        short rotation,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseWheelHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        // Act

        hook.MouseWheel += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        _ = hook.RunAsync();

        hook.SimulateMouseWheel(rotation, direction, type);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(rotation, actualEventArgs.Data.Rotation);
        Assert.Equal(direction, actualEventArgs.Data.Direction);
        Assert.Equal(type, actualEventArgs.Data.Type);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.Single(hook.SimulatedEvents);

        var actualEvent = hook.SimulatedEvents[0];
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);
    }

    [Property(DisplayName = "SimulateTextEntry should add it to the simulated text collection")]
    public void SimulateTextEntry(NonNull<string> text)
    {
        // Arrange

        var hook = new TestGlobalHook();

        // Act

        var actualResult = hook.SimulateTextEntry(text.Get);

        // Assert

        Assert.Single(hook.SimulatedText);
        Assert.Equal(text.Get, hook.SimulatedText[0]);
        Assert.Equal(UioHookResult.Success, actualResult);
    }

    [Property(DisplayName = "Run should throw an exception if configured to do so")]
    public void RunFail(FailedUioHookResult result)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            RunResult = result.Value
        };

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Run);

        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "RunAsync should throw an exception if configured to do so")]
    public async void RunAsyncFail(FailedUioHookResult result)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            RunResult = result.Value
        };

        // Act + Assert

        var exception = await Assert.ThrowsAsync<HookException>(hook.RunAsync);

        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "Run should throw an exception if the hook is disposed")]
    public void RunDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Run);
    }

    [Fact(DisplayName = "RunAsync should throw an exception if the hook is disposed")]
    public async Task RunAsyncDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act + Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(hook.RunAsync);
    }

    [Property(DisplayName = "Dispose should throw an exception if configured to do so")]
    public void DisposeFail(FailedUioHookResult result)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            DisposeResult = result.Value
        };

        // Act + Assert

        _ = hook.RunAsync();

        var exception = Assert.Throws<HookException>(hook.Dispose);

        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "Dispose should not throw an exception if the hook is disposed")]
    public void DisposeDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act

        var exception = Record.Exception(hook.Dispose);

        // Assert

        Assert.Null(exception);
    }

    [Property(DisplayName = "SimulateKeyPress should return an error if configured to do so")]
    public void SimulateKeyPressFail(FailedUioHookResult result, KeyCode keyCode)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateKeyPressResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateKeyPress(keyCode);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateKeyRelease should return an error if configured to do so")]
    public void SimulateKeyReleaseFail(FailedUioHookResult result, KeyCode keyCode)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateKeyReleaseResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateKeyRelease(keyCode);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateMousePress should return an error if configured to do so")]
    public void SimulateMousePressFail(FailedUioHookResult result, MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMousePressResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMousePress(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(
        DisplayName = "SimulateMousePress with explicit coordinates should return an error if configured to do so")]
    public void SimulateMousePressExplicitCoordinatesFail(
        FailedUioHookResult result,
        short x,
        short y,
        MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMousePressResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMousePress(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateMouseRelease should return an error if configured to do so")]
    public void SimulateMouseReleaseFail(FailedUioHookResult result, MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseReleaseResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseRelease(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(
        DisplayName = "SimulateMouseRelease with explicit coordinates should return an error if configured to do so")]
    public void SimulateMouseReleaseExplicitCoordinatesFail(
        FailedUioHookResult result,
        short x,
        short y,
        MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseReleaseResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseRelease(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateMouseMovement should return an error if configured to do so")]
    public void SimulateMouseMovementFail(FailedUioHookResult result, short x, short y)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseMovementResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseMovement(x, y);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateMouseMovementRelative should return an error if configured to do so")]
    public void SimulateMouseMovementRelativeFail(FailedUioHookResult result, short xOffset, short yOffset)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseMovementResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseMovementRelative(xOffset, yOffset);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateMouseWheel should return an error if configured to do so")]
    public void SimulateMouseWheelFail(
        FailedUioHookResult result,
        short rotation,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseWheelResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseWheel(rotation, direction, type);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
    }

    [Property(DisplayName = "SimulateTextEntry should return an error if configured to do so")]
    public void SimulateTextEntryFail(NonNull<string> text, FailedUioHookResult result)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateTextEntryResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateTextEntry(text.Get);

        // Assert

        Assert.Empty(hook.SimulatedText);
        Assert.Equal(result.Value, actualResult);
    }

    [Fact(DisplayName = "SimulateTextEntry should throw on null")]
    public void SimulateTextEntryNull()
    {
        // Arrange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.SimulateTextEntry(null!));
    }

    [Fact(DisplayName = "EventDateTime should throw on null")]
    public void EventDateTimeNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.EventDateTime = null!);
    }

    [Fact(DisplayName = "EventMask should throw on null")]
    public void EventMaskNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.EventMask = null!);
    }

    [Fact(DisplayName = "KeyCodeToChars should throw on null")]
    public void KeyCodeToCharsNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.KeyCodeToChars = null!);
    }

    [Fact(DisplayName = "KeyCodeToRawCode should throw on null")]
    public void KeyCodeToRawCodeNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.KeyCodeToRawCode = null!);
    }

    [Fact(DisplayName = "CurrentMouseX should throw on null")]
    public void CurrentMouseXNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.CurrentMouseX = null!);
    }

    [Fact(DisplayName = "CurrentMouseY should throw on null")]
    public void CurrentMouseYNull()
    {
        // Arange

        var hook = new TestGlobalHook();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => hook.CurrentMouseY = null!);
    }

    [Property(DisplayName = "Text simulation delay should be settable through the property")]
    public void TextSimulationDelayOnX11(TimeSpan delay)
    {
        // Arrange

        IEventSimulator hook = new TestGlobalHook();

        // Act

        hook.TextSimulationDelayOnX11 = delay;

        // Assert

        Assert.Equal(delay, hook.TextSimulationDelayOnX11);
    }
}
