namespace SharpHook;

public sealed class SimpleGlobalHookTests
{
    public SimpleGlobalHookTests() =>
        Arb.Register<Generators>();

    [Fact(DisplayName = "IsRunning should be true only if the hook is running")]
    public void IsRunning()
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new SimpleGlobalHook(provider);

        // Act + Assert

        Assert.False(hook.IsRunning);

        this.RunHookAndWaitForStart(hook);

        Assert.True(hook.IsRunning);

        this.DisposeHookAndWaitForStop(hook);

        Assert.False(hook.IsRunning);
    }

    [Fact(DisplayName = "IsDisposed should be true only if the hook is disposed")]
    public void IsDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new SimpleGlobalHook(provider);

        // Act + Assert

        Assert.False(hook.IsDisposed);

        hook.Dispose();

        Assert.True(hook.IsDisposed);
    }

    [Property(DisplayName = "HookEnabled events should be raised")]
    public async void HookEnabled(DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        var hook = new SimpleGlobalHook(provider);

        object? actualSender = null;
        HookEventArgs? actualEventArgs = null;

        ulong time = (ulong)dateTime.Value.ToUnixTimeMilliseconds();

        hook.HookEnabled += (sender, e) =>
        {
            actualSender = sender;
            actualEventArgs = e;
            hook.Dispose();
        };

        // Act

        await hook.RunAsync();

        // Assert

        Assert.Same(hook, actualSender);

        Assert.NotNull(actualEventArgs);

        var actualEvent = provider.PostedEvents.First(e => e.Type == EventType.HookEnabled);
        Assert.Equal(actualEvent, actualEventArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
    }

    [Property(DisplayName = "HookDisabled events should be raised")]
    public async void HookDisabled(DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        var hook = new SimpleGlobalHook(provider);

        object? actualSender = null;
        HookEventArgs? actualEventArgs = null;

        ulong time = (ulong)dateTime.Value.ToUnixTimeMilliseconds();

        hook.HookEnabled += (sender, e) => hook.Dispose();

        hook.HookDisabled += (sender, e) =>
        {
            actualSender = sender;
            actualEventArgs = e;
        };

        // Act

        await hook.RunAsync();

        // Assert

        Assert.Same(hook, actualSender);

        Assert.NotNull(actualEventArgs);

        var actualEvent = provider.PostedEvents.First(e => e.Type == EventType.HookDisabled);
        Assert.Equal(actualEvent, actualEventArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
    }

    [Property(DisplayName = "KeyPressed events should be raised")]
    public async void KeyPressed(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.KeyPressed,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = KeyboardEventData.RawUndefinedChar
            }
        };

        hook.KeyPressed += OnKeyPressed;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnKeyPressed(object? sender, KeyboardHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(keyCode, args.Data.KeyCode);
            Assert.Equal(rawCode, args.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, args.Data.KeyChar);
        }
    }

    [Property(DisplayName = "KeyReleased events should be raised")]
    public async void KeyReleased(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.KeyReleased,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = KeyboardEventData.RawUndefinedChar
            }
        };

        hook.KeyReleased += OnKeyReleased;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnKeyReleased(object? sender, KeyboardHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(keyCode, args.Data.KeyCode);
            Assert.Equal(rawCode, args.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, args.Data.KeyChar);
        }
    }

    [Property(DisplayName = "KeyTyped events should be raised")]
    public async void KeyTyped(
        KeyCode keyCode,
        ushort rawCode,
        char keyChar,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.KeyTyped,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = keyChar
            }
        };

        hook.KeyTyped += OnKeyTyped;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnKeyTyped(object? sender, KeyboardHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(keyCode, args.Data.KeyCode);
            Assert.Equal(rawCode, args.Data.RawCode);
            Assert.Equal(keyChar, args.Data.KeyChar);
        }
    }

    [Property(DisplayName = "MousePressed events should be raised")]
    public async void MousePressed(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MousePressed,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Mouse = new MouseEventData
            {
                Button = button,
                X = x,
                Y = y,
                Clicks = clicks
            }
        };

        hook.MousePressed += OnMousePressed;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMousePressed(object? sender, MouseHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(button, args.Data.Button);
            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
            Assert.Equal(clicks, args.Data.Clicks);
        }
    }

    [Property(DisplayName = "MouseReleased events should be raised")]
    public async void MouseReleased(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MouseReleased,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Mouse = new MouseEventData
            {
                Button = button,
                X = x,
                Y = y,
                Clicks = clicks
            }
        };

        hook.MouseReleased += OnMouseReleased;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMouseReleased(object? sender, MouseHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(button, args.Data.Button);
            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
            Assert.Equal(clicks, args.Data.Clicks);
        }
    }

    [Property(DisplayName = "MouseClicked events should be raised")]
    public async void MouseClicked(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MouseClicked,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Mouse = new MouseEventData
            {
                Button = button,
                X = x,
                Y = y,
                Clicks = clicks
            }
        };

        hook.MouseClicked += OnMouseClicked;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMouseClicked(object? sender, MouseHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(button, args.Data.Button);
            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
            Assert.Equal(clicks, args.Data.Clicks);
        }
    }

    [Property(DisplayName = "MouseMoved events should be raised")]
    public async void MouseMoved(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MouseMoved,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Mouse = new MouseEventData
            {
                X = x,
                Y = y
            }
        };

        hook.MouseMoved += OnMouseMoved;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMouseMoved(object? sender, MouseHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
        }
    }

    [Property(DisplayName = "MouseDragged events should be raised")]
    public async void MouseDragged(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MouseDragged,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Mouse = new MouseEventData
            {
                X = x,
                Y = y
            }
        };

        hook.MouseDragged += OnMouseDragged;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMouseDragged(object? sender, MouseHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
        }
    }

    [Property(DisplayName = "MouseWheel events should be raised")]
    public async void MouseWheel(
        short x,
        short y,
        short rotation,
        ushort delta,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        var e = new UioHookEvent
        {
            Type = EventType.MouseWheel,
            Time = (ulong)dateTime.Value.ToUnixTimeMilliseconds(),
            Mask = mask,
            Wheel = new MouseWheelEventData
            {
                X = x,
                Y = y,
                Rotation = rotation,
                Delta = delta,
                Direction = direction,
                Type = type
            }
        };

        hook.MouseWheel += OnMouseWheel;

        // Act

        this.RunHookAndWaitForStart(hook);

        await provider.PostEventAndWaitForHandler(ref e);

        // Assert

        void OnMouseWheel(object? sender, MouseWheelHookEventArgs args)
        {
            Assert.Same(hook, sender);

            Assert.Equal(e, args.RawEvent);
            Assert.Equal(dateTime.Value, args.EventTime);

            Assert.Equal(x, args.Data.X);
            Assert.Equal(y, args.Data.Y);
            Assert.Equal(rotation, args.Data.Rotation);
            Assert.Equal(delta, args.Data.Delta);
            Assert.Equal(direction, args.Data.Direction);
            Assert.Equal(type, args.Data.Type);
        }
    }

    [Property(DisplayName = "Run should throw if the hook failed to start")]
    public void RunFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new SimpleGlobalHook(provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Run);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "RunAsync should throw if the hook failed to start")]
    public async void RunAsyncFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new SimpleGlobalHook(provider);

        // Act + Assert

        var exception = await Assert.ThrowsAsync<HookException>(hook.RunAsync);
        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "Run should throw if the hook is already running")]
    public void RunRunning()
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        this.RunHookAndWaitForStart(hook);

        // Act + Assert

        Assert.Throws<InvalidOperationException>(hook.Run);
    }

    [Fact(DisplayName = "RunAsync should throw if the hook is already running")]
    public async void RunAsyncRunning()
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new SimpleGlobalHook(provider);

        this.RunHookAndWaitForStart(hook);

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(hook.RunAsync);
    }

    [Fact(DisplayName = "Run should throw if the hook is disposed")]
    public void RunDisposed()
    {
        // Arrange

        var hook = new SimpleGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Run);
    }

    [Fact(DisplayName = "RunAsync should throw if the hook is disposed")]
    public async void RunAsyncDisposed()
    {
        // Arrange

        var hook = new SimpleGlobalHook();
        hook.Dispose();

        // Act + Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(hook.RunAsync);
    }

    [Property(DisplayName = "Dispose should throw if the hook failed to stop")]
    public void DisposeFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var hook = new SimpleGlobalHook(provider);

        this.RunHookAndWaitForStart(hook);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Dispose);
        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "SimpleGlobalHook should throw if the provider is null")]
    public void ProviderNull() =>
        Assert.Throws<ArgumentNullException>(() => new SimpleGlobalHook(null!));

    private void RunHookAndWaitForStart(IGlobalHook hook)
    {
        hook.RunAsync();

        while (!hook.IsRunning)
        {
            Thread.Yield();
        }
    }

    private void DisposeHookAndWaitForStop(IGlobalHook hook)
    {
        hook.Dispose();

        while (hook.IsRunning)
        {
            Thread.Yield();
        }
    }
}
