namespace SharpHook.Testing;

public sealed class TestProviderTests
{
    [Property(DisplayName = "SetDispatchProc, Run, and PostEvent should work together")]
    public void Run(UioHookEvent eventToPost, IntPtr userData)
    {
        // Arrange

        if (eventToPost.Type == EventType.HookEnabled || eventToPost.Type == EventType.HookDisabled)
        {
            return;
        }

        var actualEvent = new UioHookEvent();
        var actualUserData = IntPtr.Zero;

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) =>
            {
                if (e.Type != EventType.HookEnabled && e.Type != EventType.HookDisabled)
                {
                    actualEvent = e;
                    actualUserData = data;
                }
            },
            userData);

        this.RunAndWaitForStart(provider);

        provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Equal(eventToPost, actualEvent);
        Assert.Equal(userData, actualUserData);

        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);

        // Clean up

        provider.Stop();
    }

    [Property(DisplayName = "RunAsync, SetDispatchProc, and PostEvent should work together")]
    public async Task RunAsync(UioHookEvent eventToPost, nint userData)
    {
        // Arrange

        if (eventToPost.Type == EventType.HookEnabled || eventToPost.Type == EventType.HookDisabled)
        {
            return;
        }

        var actualEvent = new UioHookEvent();
        nint actualUserData = IntPtr.Zero;

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, nint data) =>
            {
                if (e.Type != EventType.HookEnabled && e.Type != EventType.HookDisabled)
                {
                    actualEvent = e;
                    actualUserData = data;
                }
            },
            userData);

        var task = provider.RunAsync();

        provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Equal(eventToPost, actualEvent);
        Assert.Equal(userData, actualUserData);

        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "Run and PostEvent should work without SetDispatchProc")]
    public void RunPostEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        this.RunAndWaitForStart(provider);

        provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);

        // Clean up

        provider.Stop();
    }

    [Property(DisplayName = "RunAsync and PostEvent should work without SetDispatchProc")]
    public async Task RunAsyncPostEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        var task = provider.RunAsync();

        provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "RunKeyboard should not dispatch mouse events")]
    public async Task RunKeyboard(MouseEvent eventToPost, IntPtr userData)
    {
        // Arrange

        UioHookEvent? actualEvent = null;
        IntPtr? actualUserData = null;

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) =>
            {
                if (e.Type != EventType.HookEnabled)
                {
                    actualEvent = e;
                    actualUserData = data;
                }
            },
            userData);

        var task = provider.RunKeyboardAsync();

        var e = eventToPost.Value;
        provider.PostEvent(ref e);

        // Assert

        Assert.Null(actualEvent);
        Assert.Null(actualUserData);

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "RunMouse should not dispatch mouse events")]
    public async Task RunMouse(KeyboardEvent eventToPost, IntPtr userData)
    {
        // Arrange

        UioHookEvent? actualEvent = null;
        IntPtr? actualUserData = null;

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) =>
            {
                if (e.Type != EventType.HookEnabled)
                {
                    actualEvent = e;
                    actualUserData = data;
                }
            },
            userData);

        var task = provider.RunMouseAsync();

        var e = eventToPost.Value;
        provider.PostEvent(ref e);

        // Assert

        Assert.Null(actualEvent);
        Assert.Null(actualUserData);

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "Events should be suppressible")]
    public async Task SuppressEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) => e.Mask |= EventMask.SuppressEvent,
            IntPtr.Zero);

        var task = provider.RunAsync();

        provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Single(provider.PostedEvents);

        var actualEvent = provider.PostedEvents[0];
        Assert.True(actualEvent.Mask.HasFlag(EventMask.SuppressEvent));
        Assert.Equal(eventToPost, actualEvent);

        // Clean up

        provider.Stop();
        await task;
    }

    [Fact(DisplayName = "Run and Stop should change the state of the provider")]
    public async Task RunAndStop()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        var task = provider.RunAsync();

        Assert.True(provider.IsRunning);

        var result = provider.Stop();
        Assert.False(provider.IsRunning);

        Assert.Equal(UioHookResult.Success, result);

        // Clean up

        await task;
    }

    [Fact(DisplayName = "Run should throw if the provider is already running")]
    public async Task RunWhenAlreadyRunning()
    {
        // Arrange

        var provider = new TestProvider();
        var task = provider.RunAsync();

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(provider.RunAsync);

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "Run should return an error if configured to do so")]
    public void RunFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        // Act

        var actualResult = provider.Run();

        // Assert

        Assert.False(provider.IsRunning);
        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "RunAsync should return an error if configured to do so")]
    public async Task RunAsyncFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        // Act

        var actualResult = await provider.RunAsync();

        // Assert

        Assert.False(provider.IsRunning);
        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "RunKeyboard should return an error if configured to do so")]
    public void RunKeyboardFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        // Act

        var actualResult = provider.RunKeyboard();

        // Assert

        Assert.False(provider.IsRunning);
        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "RunMouse should return an error if configured to do so")]
    public void RunMouseFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result.Value
        };

        // Act

        var actualResult = provider.RunMouse();

        // Assert

        Assert.False(provider.IsRunning);
        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "Stop should return an error if configured to do so")]
    public async Task StopFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        var task = provider.RunAsync();

        // Act

        var actualResult = provider.Stop();

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.True(provider.IsRunning);

        // Clean up

        provider.StopResult = UioHookResult.Success;
        provider.Stop();
        await task;
    }

    [Fact(DisplayName = "HookEnabled should be raised when the hook is started")]
    public async Task HookEnabled()
    {
        // Arrange

        var dateTime = DateTimeOffset.UtcNow;
        var eventMask = EventMask.LeftCtrl | EventMask.LeftShift;

        // Act

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime,
            EventMask = t => eventMask
        };

        UioHookEvent actualEvent = default;

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr userData) =>
            {
                if (e.Type == EventType.HookEnabled)
                {
                    actualEvent = e;
                }
            },
            IntPtr.Zero);

        var task = provider.RunAsync();

        // Assert

        Assert.Equal(dateTime.ToUnixTimeMilliseconds(), (long)actualEvent.Time);
        Assert.Equal(eventMask, actualEvent.Mask);
        Assert.False(actualEvent.Mask.HasFlag(EventMask.SuppressEvent));

        // Clean up

        provider.Stop();
        await task;
    }

    [Property(DisplayName = "HookDisabled should be raised when the hook is stopped")]
    public async Task HookDisabled(DateTimeAfterEpoch dateTime, EventMask eventMask)
    {
        // Act

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => eventMask
        };

        UioHookEvent actualEvent = default;

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr userData) =>
            {
                if (e.Type == EventType.HookDisabled)
                {
                    actualEvent = e;
                }
            },
            IntPtr.Zero);

        var task = provider.RunAsync();

        provider.Stop();

        // Assert

        await task;

        Assert.Equal(dateTime.Value.ToUnixTimeMilliseconds(), (long)actualEvent.Time);
        Assert.Equal(eventMask, actualEvent.Mask);
        Assert.False(actualEvent.Mask.HasFlag(EventMask.SuppressEvent));
    }

    [Property(DisplayName = "PostEvent should post an event")]
    public void PostEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        var actualResult = provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Equal(UioHookResult.Success, actualResult);
        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);
    }

    [Property(DisplayName = "PostEvent should return an error if configured to do so")]
    public void PostEventFail(UioHookEvent eventToPost, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        // Act

        var actualResult = provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(result.Value, actualResult);
    }

    [Property(DisplayName = "PostText should post text")]
    public void PostText(NonNull<string> text)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        var actualResult = provider.PostText(text.Get);

        // Assert

        Assert.Equal(UioHookResult.Success, actualResult);
        Assert.Single(provider.PostedText);
        Assert.Equal(text.Get, provider.PostedText[0]);
    }

    [Property(DisplayName = "PostText should return an error if configured to do so")]
    public void PostTextFail(NonNull<string> text, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostTextResult = result.Value
        };

        // Act

        var actualResult = provider.PostText(text.Get);

        // Assert

        Assert.Empty(provider.PostedText);
        Assert.Equal(result.Value, actualResult);
    }

    [Fact(DisplayName = "PostText should return an error when text is null")]
    public void PostTextNull()
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        var actualResult = provider.PostText(null!);

        // Assert

        Assert.Equal(UioHookResult.ErrorPostTextNull, actualResult);
        Assert.Empty(provider.PostedText);
    }

    [Fact(DisplayName = "Setting the logger callback should do nothing")]
    public void SetLoggerProc() =>
        ((ILoggingProvider)new TestProvider()).SetLoggerProc(delegate { }, IntPtr.Zero);

    [Fact(DisplayName = "EventDateTime should not be settable to null")]
    public void EventDateTimeNull()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => provider.EventDateTime = null!);
    }

    [Fact(DisplayName = "EventMask should not be settable to null")]
    public void EventMaskNull()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        Assert.Throws<ArgumentNullException>(() => provider.EventMask = null!);
    }

    [Property(DisplayName = "Post text delay should be gettable and settable through the property")]
    public void PostTextDelayX11(ulong postTextDelay)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        ((IEventSimulationProvider)provider).PostTextDelayX11 = postTextDelay;

        // Assert

        Assert.Equal(postTextDelay, ((IEventSimulationProvider)provider).PostTextDelayX11);
    }

    [Property(DisplayName = "Checking if Accessibility API is disabled should depend on operation results")]
    public void IsAxApiEnabled(bool runDisabled, bool postEventDisabled, bool postTextDisabled, bool prompt)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = runDisabled ? UioHookResult.ErrorAxApiDisabled : UioHookResult.Success,
            PostEventResult = postEventDisabled ? UioHookResult.ErrorAxApiDisabled : UioHookResult.Success,
            PostTextResult = postTextDisabled ? UioHookResult.ErrorAxApiDisabled : UioHookResult.Success
        };

        // Act

        bool isAxApiEnabled = provider.IsAxApiEnabled(prompt);

        // Assert

        Assert.Equal(runDisabled || postEventDisabled || postTextDisabled, isAxApiEnabled);
    }

    [Fact(DisplayName = "Prompt user if Accessibility API is disabled should be true by default")]
    public void GetPromptUserIfAxApiDisabled()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        Assert.True(((IAccessibilityProvider)provider).PromptUserIfAxApiDisabled);
    }

    [Property(DisplayName = "Prompt user if Accessibility API is disabled should be settable through the property")]
    public void SetPromptUserIfAxApiDisabled(bool prompt)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        ((IAccessibilityProvider)provider).PromptUserIfAxApiDisabled = prompt;

        // Assert

        Assert.Equal(prompt, ((IAccessibilityProvider)provider).PromptUserIfAxApiDisabled);
    }

    [Fact(DisplayName = "Frequency for polling Accessibility API access should be 1 by default")]
    public void GetAxPollFrequency()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        Assert.Equal(1U, ((IAccessibilityProvider)provider).AxPollFrequency);
    }

    [Property(DisplayName = "Frequency for polling Accessibility API access should be settable through the property")]
    public void SetAxPollFrequency(uint frequency)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        ((IAccessibilityProvider)provider).AxPollFrequency = frequency;

        // Assert

        Assert.Equal(frequency, ((IAccessibilityProvider)provider).AxPollFrequency);
    }

    [Property(DisplayName = "Screen info should be settable")]
    public void ScreenInfo(byte number, short x, short y, ushort width, ushort height)
    {
        // Arrange

        var screenInfo = new[] { new ScreenData { Number = number, X = x, Y = y, Width = width, Height = height } };

        // Act

        var provider = new TestProvider
        {
            ScreenInfo = screenInfo
        };

        // Assert

        Assert.Single(provider.ScreenInfo);
        Assert.Equal(screenInfo[0], provider.ScreenInfo[0]);

        var createdScreenInfo = ((IScreenInfoProvider)provider).CreateScreenInfo();

        Assert.Single(createdScreenInfo);
        Assert.Equal(screenInfo[0], createdScreenInfo[0]);

    }

    [Fact(DisplayName = "Screen info should not be settable to null")]
    public void ScreenInfoNull() =>
        Assert.Throws<ArgumentNullException>(() => new TestProvider { ScreenInfo = null! });

    [Property(DisplayName = "Auto-repeat rate should be settable")]
    public void AutoRepeatRate(int autoRepeatRate)
    {
        // Act

        var provider = new TestProvider
        {
            AutoRepeatRate = autoRepeatRate
        };

        // Assert

        Assert.Equal(autoRepeatRate, provider.AutoRepeatRate);
        Assert.Equal(autoRepeatRate, ((IMouseInfoProvider)provider).GetAutoRepeatRate());
    }

    [Property(DisplayName = "Auto-repeat delay should be settable")]
    public void AutoRepeatDelay(int autoRepeatDelay)
    {
        // Act

        var provider = new TestProvider
        {
            AutoRepeatDelay = autoRepeatDelay
        };

        // Assert

        Assert.Equal(autoRepeatDelay, provider.AutoRepeatDelay);
        Assert.Equal(autoRepeatDelay, ((IMouseInfoProvider)provider).GetAutoRepeatDelay());
    }

    [Property(DisplayName = "Pointer acceleration multiplier should be settable")]
    public void PointerAccelerationMultiplier(int multiplier)
    {
        // Act

        var provider = new TestProvider
        {
            PointerAccelerationMultiplier = multiplier
        };

        // Assert

        Assert.Equal(multiplier, provider.PointerAccelerationMultiplier);
        Assert.Equal(multiplier, ((IMouseInfoProvider)provider).GetPointerAccelerationMultiplier());
    }

    [Property(DisplayName = "Pointer acceleration threshold should be settable")]
    public void PointerAccelerationThreshold(int threshold)
    {
        // Act

        var provider = new TestProvider
        {
            PointerAccelerationThreshold = threshold
        };

        // Assert

        Assert.Equal(threshold, provider.PointerAccelerationThreshold);
        Assert.Equal(threshold, ((IMouseInfoProvider)provider).GetPointerAccelerationThreshold());
    }

    [Property(DisplayName = "Pointer sensitivity should be settable")]
    public void PointerSensitivity(int sensitivity)
    {
        // Act

        var provider = new TestProvider
        {
            PointerSensitivity = sensitivity
        };

        // Assert

        Assert.Equal(sensitivity, provider.PointerSensitivity);
        Assert.Equal(sensitivity, ((IMouseInfoProvider)provider).GetPointerSensitivity());
    }

    [Property(DisplayName = "Multi-click time should be settable")]
    public void MultiClickTime(int multiClickTime)
    {
        // Act

        var provider = new TestProvider
        {
            MultiClickTime = multiClickTime
        };

        // Assert

        Assert.Equal(multiClickTime, provider.MultiClickTime);
        Assert.Equal(multiClickTime, ((IMouseInfoProvider)provider).GetMultiClickTime());
    }

    private void RunAndWaitForStart(TestProvider provider)
    {
        new Thread(() => provider.Run()).Start();

        while (!provider.IsRunning)
        {
            Thread.Yield();
        }
    }
}
