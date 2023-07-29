namespace SharpHook.Testing;

public class TestGlobalHookTests
{
    public TestGlobalHookTests() =>
        Arb.Register<Generators>();

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

    [Property(DisplayName = "KeyPressed events should be simulated and handled")]
    public async void HandleKeyPressed(KeyCode keyCode, ushort rawCode)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyPressed += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyPressAndWaitForHandler(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "KeyReleased events should be simulated and handled")]
    public async void HandleKeyReleased(KeyCode keyCode, ushort rawCode)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyReleased += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyReleaseAndWaitForHandler(keyCode);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualEventArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualEventArgs.Data.KeyChar);
        Assert.Equal(KeyboardEventData.RawUndefinedChar, actualEventArgs.Data.RawKeyChar);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "KeyTyped events should be simulated and handled")]
    public async void HandleKeyTyped(char keyChar, KeyCode keyCode, ushort rawCode)
    {
        // Arrange

        KeyboardHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            KeyCodeToChars = c => new[] { keyChar },
            KeyCodeToRawCode = c => rawCode
        };

        // Act

        hook.KeyTyped += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateKeyPressAndWaitForKeyTypedHandler(keyCode, keyChar);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(keyCode, actualEventArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualEventArgs.Data.RawCode);
        Assert.Equal(keyChar, actualEventArgs.Data.KeyChar);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MousePressed events should be simulated and handled")]
    public async void HandleMousePressed(short x, short y, MouseButton button)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MousePressed += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMousePressAndWaitForHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MousePressed events with explicit coordinates should be simulated and handled")]
    public async void HandleMousePressedExplicitCoordinates(short x, short y, MouseButton button)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook();

        // Act

        hook.MousePressed += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMousePressAndWaitForHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseReleased events should be simulated and handled")]
    public async void HandleMouseReleased(short x, short y, MouseButton button)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MouseReleased += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseReleased events with explicit coordinates should be simulated and handled")]
    public async void HandleMouseReleasedExplicitCoordinates(short x, short y, MouseButton button)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook();

        // Act

        hook.MouseReleased += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseClicked events should be simulated and handled")]
    public async void HandleMouseClicked(short x, short y, MouseButton button, ushort clicks)
    {
        // Arrange

        clicks = clicks != 0 ? clicks : (ushort)1;

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            MouseClickCount = clicks
        };

        // Act

        hook.MouseClicked += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseClicked events with explicit coordinates should be simulated and handled")]
    public async void HandleMouseClickedExplicitCoordinates(short x, short y, MouseButton button, ushort clicks)
    {
        // Arrange

        clicks = clicks != 0 ? clicks : (ushort)1;

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            MouseClickCount = clicks
        };

        // Act

        hook.MouseClicked += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseReleaseAndWaitForMouseClickHandler(x, y, button);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(button, actualEventArgs.Data.Button);
        Assert.Equal(clicks, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseMoved events should be simulated and handled")]
    public async void HandleMouseMovement(short x, short y)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook();

        // Act

        hook.MouseMoved += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementAndWaitForHandler(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "Relatvie MouseMoved events should be simulated and handled")]
    public async void HandleMouseMovementRelative(short x, short y, short xOffset, short yOffset)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MouseMoved += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementRelativeAndWaitForHandler(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseDragged events should be simulated and handled")]
    public async void HandleMouseDragged(short x, short y)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            DragMouseWhenMoving = true
        };

        // Act

        hook.MouseDragged += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementAndWaitForHandler(x, y);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "Relatvie MouseDragged events should be simulated and handled")]
    public async void HandleMouseDraggedRelative(short x, short y, short xOffset, short yOffset)
    {
        // Arrange

        MouseHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y,
            DragMouseWhenMoving = true
        };

        // Act

        hook.MouseDragged += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseMovementRelativeAndWaitForHandler(xOffset, yOffset);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x + xOffset, actualEventArgs.Data.X);
        Assert.Equal(y + yOffset, actualEventArgs.Data.Y);
        Assert.Equal(MouseButton.NoButton, actualEventArgs.Data.Button);
        Assert.Equal(0, actualEventArgs.Data.Clicks);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }

    [Property(DisplayName = "MouseWheel events should be simulated and handled")]
    public async void HandleMouseWheel(ushort delta, short rotation, short x, short y)
    {
        // Arrange

        MouseWheelHookEventArgs? actualEventArgs = null;

        using var hook = new TestGlobalHook
        {
            CurrentMouseX = () => x,
            CurrentMouseY = () => y
        };

        // Act

        hook.MouseWheel += (sender, e) => actualEventArgs = e;

        await hook.RunAndWaitForStart();

        var args = await hook.SimulateMouseWheelAndWaitForHandler(delta, rotation);

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(x, actualEventArgs.Data.X);
        Assert.Equal(y, actualEventArgs.Data.Y);
        Assert.Equal(delta, actualEventArgs.Data.Delta);
        Assert.Equal(rotation, actualEventArgs.Data.Rotation);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(actualEventArgs.RawEvent, args.Event);
    }
}
