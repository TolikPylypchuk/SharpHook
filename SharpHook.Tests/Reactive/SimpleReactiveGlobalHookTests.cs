namespace SharpHook.Reactive;

public sealed class SimpleReactiveGlobalHookTests
{
    public SimpleReactiveGlobalHookTests() =>
        Arb.Register<Generators>();

    [Fact(DisplayName = "IsRunning should be true only if the hook is running")]
    public void IsRunning()
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        // Act + Assert

        Assert.False(hook.IsRunning);

        this.RunHookAndWaitForStart(hook, provider);

        Assert.True(hook.IsRunning);

        this.DisposeHookAndWaitForStop(hook);

        Assert.False(hook.IsRunning);
    }

    [Fact(DisplayName = "IsDisposed should be true only if the hook is disposed")]
    public void IsDisposed()
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        // Act + Assert

        Assert.False(hook.IsDisposed);

        hook.Dispose();

        Assert.True(hook.IsDisposed);
    }

    [Property(DisplayName = "HookEnabled events should be raised")]
    public void HookEnabled(DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        HookEventArgs? actualEventArgs = null;

        ulong time = (ulong)dateTime.Value.ToUnixTimeMilliseconds();

        hook.HookEnabled.Subscribe(e =>
        {
            actualEventArgs = e;
            hook.Dispose();
        });

        // Act

        hook.Run();

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(EventType.HookEnabled, actualEventArgs.RawEvent.Type);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);
        Assert.Equal(time, actualEventArgs.RawEvent.Time);
    }

    [Property(DisplayName = "HookDisabled events should be raised")]
    public void HookDisabled(DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        HookEventArgs? actualEventArgs = null;

        ulong time = (ulong)dateTime.Value.ToUnixTimeMilliseconds();

        hook.HookEnabled.Subscribe(e => hook.Dispose());
        hook.HookDisabled.Subscribe(e => actualEventArgs = e);

        // Act

        hook.Run();

        // Assert

        Assert.NotNull(actualEventArgs);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(EventType.HookDisabled, actualEventArgs.RawEvent.Type);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);
        Assert.Equal(time, actualEventArgs.RawEvent.Time);
    }

    [Property(DisplayName = "KeyPressed events should be raised")]
    public void KeyPressed(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        KeyboardHookEventArgs? actualArgs = null;

        hook.KeyPressed.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(keyCode, actualArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
    }

    [Property(DisplayName = "KeyReleased events should be raised")]
    public void KeyReleased(KeyCode keyCode, ushort rawCode, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        KeyboardHookEventArgs? actualArgs = null;

        hook.KeyReleased.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(keyCode, actualArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualArgs.Data.RawCode);
        Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
    }

    [Property(DisplayName = "KeyTyped events should be raised")]
    public void KeyTyped(
        KeyCode keyCode,
        ushort rawCode,
        char keyChar,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        KeyboardHookEventArgs? actualArgs = null;

        hook.KeyTyped.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(keyCode, actualArgs.Data.KeyCode);
        Assert.Equal(rawCode, actualArgs.Data.RawCode);
        Assert.Equal(keyChar, actualArgs.Data.KeyChar);
    }

    [Property(DisplayName = "MousePressed events should be raised")]
    public void MousePressed(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseHookEventArgs? actualArgs = null;

        hook.MousePressed.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(button, actualArgs.Data.Button);
        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
        Assert.Equal(clicks, actualArgs.Data.Clicks);
    }

    [Property(DisplayName = "MouseReleased events should be raised")]
    public void MouseReleased(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseHookEventArgs? actualArgs = null;

        hook.MouseReleased.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(button, actualArgs.Data.Button);
        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
        Assert.Equal(clicks, actualArgs.Data.Clicks);
    }

    [Property(DisplayName = "MouseClicked events should be raised")]
    public void MouseClicked(
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseHookEventArgs? actualArgs = null;

        hook.MouseClicked.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(button, actualArgs.Data.Button);
        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
        Assert.Equal(clicks, actualArgs.Data.Clicks);
    }

    [Property(DisplayName = "MouseMoved events should be raised")]
    public void MouseMoved(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseHookEventArgs? actualArgs = null;

        hook.MouseMoved.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
    }

    [Property(DisplayName = "MouseDragged events should be raised")]
    public void MouseDragged(short x, short y, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseHookEventArgs? actualArgs = null;

        hook.MouseDragged.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
    }

    [Property(DisplayName = "MouseWheel events should be raised")]
    public void MouseWheel(
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
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        MouseWheelHookEventArgs? actualArgs = null;

        hook.MouseWheel.Subscribe(e => actualArgs = e);

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        // Assert

        Assert.NotNull(actualArgs);

        Assert.Equal(e, actualArgs.RawEvent);
        Assert.Equal(dateTime.Value, actualArgs.EventTime);

        Assert.Equal(x, actualArgs.Data.X);
        Assert.Equal(y, actualArgs.Data.Y);
        Assert.Equal(rotation, actualArgs.Data.Rotation);
        Assert.Equal(delta, actualArgs.Data.Delta);
        Assert.Equal(direction, actualArgs.Data.Direction);
        Assert.Equal(type, actualArgs.Data.Type);
    }

    [Property(DisplayName = "Run should throw if the hook failed to start")]
    public void RunFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

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

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        // Act + Assert

        var exception = await Assert.ThrowsAsync<HookException>(async () => await hook.RunAsync());
        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "Run should throw if the hook is already running")]
    public void RunRunning()
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        Assert.Throws<InvalidOperationException>(hook.Run);
    }

    [Fact(DisplayName = "RunAsync should throw if the hook is already running")]
    public async void RunAsyncRunning()
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await hook.RunAsync());
    }

    [Fact(DisplayName = "Run should throw if the hook is disposed")]
    public void RunDisposed()
    {
        // Arrange

        var hook = new SimpleReactiveGlobalHook();
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Run);
    }

    [Fact(DisplayName = "RunAsync should throw if the hook is disposed")]
    public async void RunAsyncDisposed()
    {
        // Arrange

        var hook = new SimpleReactiveGlobalHook();
        hook.Dispose();

        // Act + Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await hook.RunAsync());
    }

    [Property(DisplayName = "Dispose should throw if the hook failed to stop")]
    public void DisposeFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Dispose);
        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "SimpleReactiveGlobalHook should throw if the provider is null")]
    public void ProviderNull() =>
        Assert.Throws<ArgumentNullException>(() => new SimpleReactiveGlobalHook((IGlobalHookProvider)null!));

    [Fact(DisplayName = "SimpleReactiveGlobalHook should throw if the scheduler is null")]
    public void SchedulerNull() =>
        Assert.Throws<ArgumentNullException>(() => new SimpleReactiveGlobalHook((IScheduler)null!));

    private void RunHookAndWaitForStart(IReactiveGlobalHook hook, TestProvider provider)
    {
        hook.RunAsync();

        while (!provider.IsRunning)
        {
            Thread.Yield();
        }
    }

    private void DisposeHookAndWaitForStop(IReactiveGlobalHook hook)
    {
        hook.Dispose();

        while (hook.IsRunning)
        {
            Thread.Yield();
        }
    }
}