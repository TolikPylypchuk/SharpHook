namespace SharpHook;

public sealed class TaskPoolGlobalHookTests
{
    [Property(DisplayName = "IsRunning should be true only if the hook is running")]
    public void IsRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        // Act + Assert

        Assert.False(hook.IsRunning);

        this.RunHookAndWaitForStart(hook, provider);
        Assert.True(hook.IsRunning);

        this.StopHookAndWaitForStop(hook);
        Assert.False(hook.IsRunning);

        this.RunHookAndWaitForStart(hook, provider);
        Assert.True(hook.IsRunning);

        this.DisposeHookAndWaitForStop(hook);
        Assert.False(hook.IsRunning);
    }

    [Property(DisplayName = "IsDisposed should be true only if the hook is disposed")]
    public void IsDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        // Act + Assert

        Assert.False(hook.IsDisposed);

        hook.Dispose();

        Assert.True(hook.IsDisposed);
    }

    [Property(DisplayName = "HookEnabled events should be raised")]
    public void HookEnabled(GlobalHookType globalHookType, DateTimeAfterEpoch dateTime, EventMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        hook.Run();

        // Assert

        Assert.Same(hook, actualSender);

        Assert.NotNull(actualEventArgs);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(EventType.HookEnabled, actualEventArgs.RawEvent.Type);
        Assert.Equal(time, actualEventArgs.RawEvent.Time);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);
    }

    [Property(DisplayName = "HookDisabled events should be raised")]
    public void HookDisabled(GlobalHookType globalHookType, DateTimeAfterEpoch dateTime, EventMask mask)
    {
        // Arrange

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => mask
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        hook.Run();

        // Assert

        Assert.Same(hook, actualSender);

        Assert.NotNull(actualEventArgs);
        Assert.Equal(dateTime.Value, actualEventArgs.EventTime);
        Assert.Equal(EventType.HookDisabled, actualEventArgs.RawEvent.Type);
        Assert.Equal(time, actualEventArgs.RawEvent.Time);
        Assert.Equal(mask, actualEventArgs.RawEvent.Mask);
    }

    [Property(DisplayName = "KeyPressed events should be raised only if the global hook type includes keyboard")]
    public async Task KeyPressed(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        KeyboardHookEventArgs? actualArgs = null;

        var keyPressed = new TaskCompletionSource<object?>();

        hook.KeyPressed += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            keyPressed.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Mouse)
        {
            await keyPressed.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "KeyReleased events should be raised only if the global hook type includes keyboard")]
    public async Task KeyReleased(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        KeyboardHookEventArgs? actualArgs = null;

        var keyReleased = new TaskCompletionSource<object?>();

        hook.KeyReleased += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            keyReleased.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Mouse)
        {
            await keyReleased.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(KeyboardEventData.UndefinedChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "KeyTyped events should be raised only if the global hook type includes keyboard")]
    public async Task KeyTyped(
        GlobalHookType globalHookType,
        KeyCode keyCode,
        ushort rawCode,
        char keyChar,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        KeyboardHookEventArgs? actualArgs = null;

        var keyTyped = new TaskCompletionSource<object?>();

        hook.KeyTyped += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            keyTyped.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Mouse)
        {
            await keyTyped.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Mouse)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(keyCode, actualArgs.Data.KeyCode);
            Assert.Equal(rawCode, actualArgs.Data.RawCode);
            Assert.Equal(keyChar, actualArgs.Data.KeyChar);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MousePressed events should be raised only if the global hook type includes mouse")]
    public async Task MousePressed(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseHookEventArgs? actualArgs = null;

        var mousePressed = new TaskCompletionSource<object?>();

        hook.MousePressed += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mousePressed.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mousePressed.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MouseReleased events should be raised only if the global hook type includes mouse")]
    public async Task MouseReleased(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseHookEventArgs? actualArgs = null;

        var mouseReleased = new TaskCompletionSource<object?>();

        hook.MouseReleased += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mouseReleased.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mouseReleased.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MouseClicked events should be raised only if the global hook type includes mouse")]
    public async Task MouseClicked(
        GlobalHookType globalHookType,
        MouseButton button,
        short x,
        short y,
        ushort clicks,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseHookEventArgs? actualArgs = null;

        var mouseClicked = new TaskCompletionSource<object?>();

        hook.MouseClicked += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mouseClicked.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mouseClicked.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(button, actualArgs.Data.Button);
            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
            Assert.Equal(clicks, actualArgs.Data.Clicks);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MouseMoved events should be raised only if the global hook type includes mouse")]
    public async Task MouseMoved(
        GlobalHookType globalHookType,
        short x,
        short y,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseHookEventArgs? actualArgs = null;

        var mouseMoved = new TaskCompletionSource<object?>();

        hook.MouseMoved += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mouseMoved.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mouseMoved.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MouseDragged events should be raised only if the global hook type includes mouse")]
    public async Task MouseDragged(
        GlobalHookType globalHookType,
        short x,
        short y,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseHookEventArgs? actualArgs = null;

        var mouseDragged = new TaskCompletionSource<object?>();

        hook.MouseDragged += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mouseDragged.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mouseDragged.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

            Assert.NotNull(actualArgs);

            Assert.Equal(e, actualArgs.RawEvent);
            Assert.Equal(dateTime.Value, actualArgs.EventTime);

            Assert.Equal(x, actualArgs.Data.X);
            Assert.Equal(y, actualArgs.Data.Y);
        } else
        {
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "MouseWheel events should be raised only if the global hook type includes mouse")]
    public async Task MouseWheel(
        GlobalHookType globalHookType,
        short x,
        short y,
        short rotation,
        ushort delta,
        MouseWheelScrollDirection direction,
        MouseWheelScrollType type,
        DateTimeAfterEpoch dateTime,
        EventMask mask)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

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

        object? actualSender = null;
        MouseWheelHookEventArgs? actualArgs = null;

        var mouseWheel = new TaskCompletionSource<object?>();

        hook.MouseWheel += (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
            mouseWheel.SetResult(null);
        };

        // Act

        this.RunHookAndWaitForStart(hook, provider);

        provider.PostEvent(ref e);

        if (globalHookType != GlobalHookType.Keyboard)
        {
            await mouseWheel.Task;
        }

        // Assert

        if (globalHookType != GlobalHookType.Keyboard)
        {
            Assert.Same(hook, actualSender);

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
            Assert.Null(actualSender);
            Assert.Null(actualArgs);
        }
    }

    [Property(DisplayName = "Run should throw if the hook failed to start")]
    public void RunFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Run);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "RunAsync should throw if the hook failed to start")]
    public async Task RunAsyncFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        // Act + Assert

        var exception = await Assert.ThrowsAsync<HookException>(hook.RunAsync);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "Run should throw if the hook is already running")]
    public void RunRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        Assert.Throws<InvalidOperationException>(hook.Run);
    }

    [Property(DisplayName = "RunAsync should throw if the hook is already running")]
    public async Task RunAsyncRunning(GlobalHookType globalHookType)
    {
        // Arrange

        var provider = new TestProvider();
        using var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(hook.RunAsync);
    }

    [Property(DisplayName = "Run should throw if the hook is disposed")]
    public void RunDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new SimpleGlobalHook(globalHookType);
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Run);
    }

    [Property(DisplayName = "RunAsync should throw if the hook is disposed")]
    public async Task RunAsyncDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new SimpleGlobalHook(globalHookType);
        hook.Dispose();

        // Act + Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(hook.RunAsync);
    }

    [Property(DisplayName = "Stop should throw if the hook failed to stop")]
    public void StopFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Stop);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "Stop should throw if the hook is disposed")]
    public void StopDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType);
        hook.Dispose();

        // Act + Assert

        Assert.Throws<ObjectDisposedException>(hook.Stop);
    }

    [Property(DisplayName = "Dispose should throw if the hook failed to stop")]
    public void DisposeFail(GlobalHookType globalHookType, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: provider);

        this.RunHookAndWaitForStart(hook, provider);

        // Act + Assert

        var exception = Assert.Throws<HookException>(hook.Dispose);
        Assert.Equal(result.Value, exception.Result);
    }

    [Property(DisplayName = "Dispose should do nothing if the hook is disposed")]
    public void DisposeDisposed(GlobalHookType globalHookType)
    {
        // Arrange

        var hook = new TaskPoolGlobalHook(globalHookType: globalHookType);
        hook.Dispose();

        // Act

        var exception = Record.Exception(hook.Dispose);

        // Assert

        Assert.Null(exception);
    }

    [Property(DisplayName = "TaskPoolGlobalHook should not throw if the provider is null")]
    public void ProviderNull(GlobalHookType globalHookType)
    {
        var exception = Record.Exception(() =>
            new TaskPoolGlobalHook(globalHookType: globalHookType, globalHookProvider: null));

        Assert.Null(exception);
    }

    private void RunHookAndWaitForStart(TaskPoolGlobalHook hook, TestProvider provider)
    {
        hook.RunAsync();

        while (!provider.IsRunning)
        {
            Thread.Yield();
        }
    }

    private void StopHookAndWaitForStop(TaskPoolGlobalHook hook)
    {
        hook.Stop();

        while (hook.IsRunning)
        {
            Thread.Yield();
        }
    }

    private void DisposeHookAndWaitForStop(TaskPoolGlobalHook hook)
    {
        hook.Dispose();

        while (hook.IsRunning)
        {
            Thread.Yield();
        }
    }
}
