namespace SharpHook.Testing;

public class TestGlobalHookTests
{
    public TestGlobalHookTests() =>
        Arb.Register<Generators>();

    [Property(DisplayName = "KeyPressed events should be simulated and handled")]
    public async void HandleKeyPressed(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyPressAndWaitForHandler(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "KeyPressed events should be simulated and handled for null characters")]
    public async void HandleKeyPressedNullCharacters(
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyPressAndWaitForHandler(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "KeyReleased events should be simulated and handled")]
    public async void HandleKeyReleased(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyReleaseAndWaitForHandler(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "KeyTyped events should be simulated and handled")]
    public async void HandleKeyTyped(
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
            KeyCodeToChars = c => new[] { keyChar },
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyTyped += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyPressAndWaitForKeyTypedHandler(keyCode, keyChar);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(hook.KeyCodeToRawCode(keyCode), actualEventArgs.Data.RawCode);
        Assert.Equal(hook.KeyCodeToChars(keyCode).First(), actualEventArgs.Data.KeyChar);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Equal(2, hook.SimulatedEvents.Count);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[1]);
    }

    [Property(DisplayName = "MousePressed events should be simulated and handled")]
    public async void HandleMousePressed(
        short x,
        short y,
        MouseButton button,
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMousePressAndWaitForHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MousePressed events with explicit coordinates should be simulated and handled")]
    public async void HandleMousePressedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMousePressAndWaitForHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MouseReleased events should be simulated and handled")]
    public async void HandleMouseReleased(
        short x,
        short y,
        MouseButton button,
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
            MouseClickCount = 0
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MouseReleased events with explicit coordinates should be simulated and handled")]
    public async void HandleMouseReleasedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            MouseClickCount = 0
        };

        // Act

        hook.MouseReleased += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MouseClicked events should be simulated and handled")]
    public async void HandleMouseClicked(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        clicks = clicks != 0 ? clicks : (ushort)1;

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

        hook.MouseClicked += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(hook.CurrentMouseX(), actualEventArgs.Data.X);
        Assert.Equal(hook.CurrentMouseY(), actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Equal(2, hook.SimulatedEvents.Count);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[1]);
    }

    [Property(DisplayName = "MouseClicked events with explicit coordinates should be simulated and handled")]
    public async void HandleMouseClickedExplicitCoordinates(
        short x,
        short y,
        MouseButton button,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        clicks = clicks != 0 ? clicks : (ushort)1;

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            MouseClickCount = clicks
        };

        // Act

        hook.MouseClicked += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Equal(2, hook.SimulatedEvents.Count);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[1]);
    }

    [Property(DisplayName = "MouseMoved events should be simulated and handled")]
    public async void HandleMouseMovement(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementAndWaitForHandler(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "Relatvie MouseMoved events should be simulated and handled")]
    public async void HandleMouseMovementRelative(
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementRelativeAndWaitForHandler(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MouseDragged events should be simulated and handled")]
    public async void HandleMouseDragged(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementAndWaitForHandler(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "Relatvie MouseDragged events should be simulated and handled")]
    public async void HandleMouseDraggedRelative(
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

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementRelativeAndWaitForHandler(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
    }

    [Property(DisplayName = "MouseWheel events should be simulated and handled")]
    public async void HandleMouseWheel(
        ushort delta,
        short rotation,
        short x,
        short y,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        MouseWheelHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask,
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MouseWheel += (sender, e) =>
        {
            actualEventArgs = e;
            e.SuppressEvent = true;
        };

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseWheelAndWaitForHandler(delta, rotation);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(delta, actualEventArgs.Data.Delta);
        Assert.Equal(rotation, actualEventArgs.Data.Rotation);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);

        var actualEvent = args.Event;
        Assert.True(actualEvent.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        actualEvent.Reserved = EventReservedValueMask.None;
        Assert.Equal(actualEventArgs.RawEvent, actualEvent);

        Assert.Single(hook.SimulatedEvents);
        Assert.Equal(actualEventArgs, hook.SimulatedEvents[0]);
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

    [Fact(DisplayName = "RunAsync should run the hook in a separate thread")]
    public async void RunAsync()
    {
        // Arrange

        var hook = new TestGlobalHook();

        // Act

        var result = hook.RunAsync();

        while (!hook.IsRunning)
        {
            await Task.Yield();
        }

        // Assert

        Assert.True(hook.IsRunning);

        // Clean up

        hook.Dispose();
        await result;
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

    [Fact(DisplayName = "Run should throw an exception if the hook is already running")]
    public async void RunWhenRunning()
    {
        // Arrange

        using var hook = new TestGlobalHook();
        await hook.RunAndWaitForStart();

        // Act + Assert

        Assert.Throws<InvalidOperationException>(hook.Run);
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

    [Fact(DisplayName = "RunAsync should throw an exception if the hook is already running")]
    public async void RunAsyncWhenRunning()
    {
        // Arrange

        using var hook = new TestGlobalHook();
        await hook.RunAndWaitForStart();

        // Act + Assert

        Assert.Throws<InvalidOperationException>(() => { _ = hook.RunAsync(); });
    }

    [Fact(DisplayName = "RunAsync should throw an exception if the hook is disposed")]
    public void RunAsyncDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(() => { _ = hook.RunAsync(); });
    }

    [Fact(DisplayName = "RunAndWaitForStart should throw an exception if the hook is disposed")]
    public void RunAndWaitForStartDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(() => { _ = hook.RunAndWaitForStart(); });
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

        var exception = Assert.Throws<HookException>(hook.Dispose);

        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "Dispose should throw an exception if the hook is disposed")]
    public void DisposeDisposed()
    {
        // Arrange

        var hook = new TestGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Dispose);
    }

    [Property(DisplayName = "SimulateKeyPress should return an error if configured to do so")]
    public async void SimulateKeyPressFail(FailedUioHookResult result, KeyCode keyCode, char keyChar)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateKeyPressResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateKeyPress(keyCode);
        var asyncResult1 = await hook.SimulateKeyPressAndWaitForHandler(keyCode);
        var asyncResult2 = await hook.SimulateKeyPressAndWaitForKeyTypedHandler(keyCode, keyChar);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult1);
        Assert.Null(asyncResult2);
    }

    [Property(DisplayName = "SimulateKeyRelease should return an error if configured to do so")]
    public async void SimulateKeyReleaseFail(FailedUioHookResult result, KeyCode keyCode)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateKeyReleaseResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateKeyRelease(keyCode);
        var asyncResult = await hook.SimulateKeyReleaseAndWaitForHandler(keyCode);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
    }

    [Property(DisplayName = "SimulateMousePress should return an error if configured to do so")]
    public async void SimulateMousePressFail(FailedUioHookResult result, MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMousePressResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMousePress(button);
        var asyncResult = await hook.SimulateMousePressAndWaitForHandler(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
    }

    [Property(
        DisplayName = "SimulateMousePress with explicit coordinates should return an error if configured to do so")]
    public async void SimulateMousePressExplicitCoordinatesFail(
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
        var asyncResult = await hook.SimulateMousePressAndWaitForHandler(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
    }

    [Property(DisplayName = "SimulateMouseRelease should return an error if configured to do so")]
    public async void SimulateMouseReleaseFail(FailedUioHookResult result, MouseButton button)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseReleaseResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseRelease(button);
        var asyncResult1 = await hook.SimulateMouseReleaseAndWaitForHandler(button);
        var asyncResult2 = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult1);
        Assert.Null(asyncResult2);
    }

    [Property(
        DisplayName = "SimulateMouseRelease with explicit coordinates should return an error if configured to do so")]
    public async void SimulateMouseReleaseExplicitCoordinatesFail(
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
        var asyncResult1 = await hook.SimulateMouseReleaseAndWaitForHandler(x, y, button);
        var asyncResult2 = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(x, y, button);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult1);
        Assert.Null(asyncResult2);
    }

    [Property(DisplayName = "SimulateMouseMovement should return an error if configured to do so")]
    public async void SimulateMouseMovementFail(FailedUioHookResult result, short x, short y)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseMovementResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseMovement(x, y);
        var asyncResult = await hook.SimulateMouseMovementAndWaitForHandler(x, y);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
    }

    [Property(DisplayName = "SimulateMouseMovementRelative should return an error if configured to do so")]
    public async void SimulateMouseMovementRelativeFail(FailedUioHookResult result, short xOffset, short yOffset)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseMovementResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseMovementRelative(xOffset, yOffset);
        var asyncResult = await hook.SimulateMouseMovementRelativeAndWaitForHandler(xOffset, yOffset);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
    }

    [Property(DisplayName = "SimulateMouseWheel should return an error if configured to do so")]
    public async void SimulateMouseWheelFail(FailedUioHookResult result, ushort delta, short rotation)
    {
        // Arrange

        var hook = new TestGlobalHook
        {
            SimulateMouseWheelResult = result.Value
        };

        // Act

        var actualResult = hook.SimulateMouseWheel(delta, rotation);
        var asyncResult = await hook.SimulateMouseWheelAndWaitForHandler(delta, rotation);

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.Empty(hook.SimulatedEvents);
        Assert.Null(asyncResult);
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
