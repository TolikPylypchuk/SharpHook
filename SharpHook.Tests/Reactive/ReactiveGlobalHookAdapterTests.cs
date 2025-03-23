namespace SharpHook.Reactive;

public sealed class ReactiveGlobalHookAdapterTests
{
    [Property(DisplayName = "IsRunning should be true only if the hook is running", Arbitrary = [typeof(Generators)])]
    public void IsRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        // Act + Assert

        Assert.False(hook.IsRunning);

        this.RunHookAndWaitForStart(hook, provider);

        Assert.True(hook.IsRunning);

        this.DisposeHookAndWaitForStop(hook);

        Assert.False(hook.IsRunning);
    }

    [Property(DisplayName = "IsDisposed should be true only if the hook is disposed", Arbitrary = [typeof(Generators)])]
    public void IsDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        // Act + Assert

        Assert.False(hook.IsDisposed);

        hook.Dispose();

        Assert.True(hook.IsDisposed);
    }

    [Property(DisplayName = "HookEnabled events should be raised", Arbitrary = [typeof(Generators)])]
    public void HookEnabled(GlobalHookType globalHookType, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

    [Property(DisplayName = "HookDisabled events should be raised", Arbitrary = [typeof(Generators)])]
    public void HookDisabled(GlobalHookType globalHookType, DateTimeAfterEpoch dateTime, ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

    [Property(
        DisplayName = "KeyPressed events should be raised only if the global hook type includes keyboard",
        Arbitrary = [typeof(Generators)])]
    public void KeyPressed(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "KeyReleased events should be raised only if the global hook type includes keyboard",
        Arbitrary = [typeof(Generators)])]
    public void KeyReleased(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "KeyTyped events should be raised only if the global hook type includes keyboard",
        Arbitrary = [typeof(Generators)])]
    public void KeyTyped(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        char keyChar,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(keyChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MousePressed events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MousePressed(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MouseReleased events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MouseReleased(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MouseClicked events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MouseClicked(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MouseMoved events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MouseMoved(
        GlobalHookType globalHookType,
        short x,
        short y,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MouseDragged events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MouseDragged(
        GlobalHookType globalHookType,
        short x,
        short y,
        DateTimeAfterEpoch dateTime,
        ModifierMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(
        DisplayName = "MouseWheel events should be raised only if the global hook type includes mouse",
        Arbitrary = [typeof(Generators)])]
    public void MouseWheel(
        GlobalHookType globalHookType,
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
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

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

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(rotation, actualArgs.Data.Rotation);
            Assert.Equal(delta, actualArgs.Data.Delta);
            Assert.Equal(direction, actualArgs.Data.Direction);
            Assert.Equal(type, actualArgs.Data.Type);
        } else
        {
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "Run should throw if the hook failed to start", Arbitrary = [typeof(Generators)])]
    public void RunFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Run);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "RunAsync should throw if the hook failed to start", Arbitrary = [typeof(Generators)])]
    public async void RunAsyncFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        // Act + Assert

        var exception = await Assert.ThrowsAsync<HookException>(async () => await hook.RunAsync());
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "Run should throw if the hook is already running", Arbitrary = [typeof(Generators)])]
    public void RunRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        Assert.Throws<InvalidOperationException>(hook.Run);
    }

    [Property(DisplayName = "RunAsync should throw if the hook is already running", Arbitrary = [typeof(Generators)])]
    public async void RunAsyncRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await hook.RunAsync());
    }

    [Property(DisplayName = "Run should throw if the hook is disposed", Arbitrary = [typeof(Generators)])]
    public void RunDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType));
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Run);
    }

    [Property(DisplayName = "RunAsync should throw if the hook is disposed", Arbitrary = [typeof(Generators)])]
    public async void RunAsyncDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType));
        hook.Dispose();

        // Act + Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await hook.RunAsync());
    }

    [Property(DisplayName = "Dispose should throw if the hook failed to stop", Arbitrary = [typeof(Generators)])]
    public void DisposeFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var hook = new ReactiveGlobalHookAdapter(new SimpleGlobalHook(globalHookType, provider));

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Dispose);
        Assert.Equal(result.Value, exception.Result);
    }

    [Fact(DisplayName = "ReactiveGlobalHookAdapter should throw if the adapted hook is null")]
    public void AdaptedHookNull() =>
        Assert.Throws<ArgumentNullException>(() => new ReactiveGlobalHookAdapter(null!));

    [Property(
        DisplayName = "SimpleReactiveGlobalHook should not throw if the scheduler is null",
        Arbitrary = [typeof(Generators)])]
    public void SchedulerNull(GlobalHookType globalHookType)
    {
        var exception = Record.Exception(() => new ReactiveGlobalHookAdapter(
            new SimpleGlobalHook(globalHookType), null));
        Assert.Null(exception);
    }

    private void RunHookAndWaitForStart(ReactiveGlobalHookAdapter hook, TestProvider provider)
    {
        hook.RunAsync();

        while (!provider.IsRunning)
        {
            Thread.Yield();
        }
    }

    private void DisposeHookAndWaitForStop(ReactiveGlobalHookAdapter hook)
    {
        hook.Dispose();

        while (hook.IsRunning)
        {
            Thread.Yield();
        }
    }
}
