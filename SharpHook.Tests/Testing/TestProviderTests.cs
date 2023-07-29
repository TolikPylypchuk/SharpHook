namespace SharpHook.Testing;

public sealed class TestProviderTests
{
    public TestProviderTests() =>
        Arb.Register<Generators>();

    [Property(DisplayName = "SetDispatchProc, Run, and PostEvent should work together")]
    public async void SetDispatchProc(UioHookEvent eventToPost, IntPtr userData)
    {
        // Arrange

        var actualEvent = new UioHookEvent();
        var actualUserData = IntPtr.Zero;

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) =>
            {
                actualEvent = e;
                actualUserData = data;
            },
            userData);

        await provider.RunAndWaitForStart();

        var args = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.Equal(eventToPost, actualEvent);
        Assert.Equal(userData, actualUserData);

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(eventToPost, args.Event);

        // Clean up

        provider.Stop();
    }

    [Property(DisplayName = "Run and PostEvent should work without SetDispatchProc")]
    public async void RunPostEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        await provider.RunAndWaitForStart();

        var args = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.NotNull(args);
        Assert.True(args.HookCalled);
        Assert.Equal(eventToPost, args.Event);

        // Clean up

        provider.Stop();
    }

    [Property(DisplayName = "Events should be suppressible")]
    public async void SuppressEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) => e.Reserved |= EventReservedValueMask.SuppressEvent,
            IntPtr.Zero);

        await provider.RunAndWaitForStart();

        var result = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.NotNull(result);
        Assert.True(result.HookCalled);
        Assert.True(result.Event.Reserved.HasFlag(EventReservedValueMask.SuppressEvent));

        // Clean up

        provider.Stop();
    }

    [Fact(DisplayName = "Run and Stop should change the state of the provider")]
    public async void RunAndStop()
    {
        // Arrange

        var provider = new TestProvider();

        // Act + Assert

        await provider.RunAndWaitForStart();

        Assert.True(provider.IsRunning);

        var result = provider.Stop();
        Assert.False(provider.IsRunning);

        Assert.Equal(UioHookResult.Success, result);
    }

    [Fact(DisplayName = "Run should throw if the provider is already running")]
    public async void RunWhenAlreadyRunning()
    {
        // Arrange

        var provider = new TestProvider();
        await provider.RunAndWaitForStart();

        // Act + Assert

        Assert.Throws<InvalidOperationException>(() => provider.Run());

        // Clean up

        provider.Stop();
    }

    [Fact(DisplayName = "RunAndWaitForStart should throw if the provider is already running")]
    public async void RunAndWaitForStartWhenAlreadyRunning()
    {
        // Arrange

        var provider = new TestProvider();
        await provider.RunAndWaitForStart();

        // Act + Assert

        await Assert.ThrowsAsync<InvalidOperationException>(() => provider.RunAndWaitForStart());

        // Clean up

        provider.Stop();
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

    [Property(DisplayName = "Stop should return an error if configured to do so")]
    public async void StopFail(FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result.Value
        };

        await provider.RunAndWaitForStart();

        // Act

        var actualResult = provider.Stop();

        // Assert

        Assert.Equal(result.Value, actualResult);
        Assert.True(provider.IsRunning);

        // Clean up

        provider.StopResult = UioHookResult.Success;
        provider.Stop();
    }

    [Fact(DisplayName = "HookEnabled should be raised when the hook is started")]
    public async void HookEnabled()
    {
        // Arrange

        var dateTime = DateTimeOffset.UtcNow;
        var modifierMask = ModifierMask.LeftCtrl | ModifierMask.LeftShift;

        // Act

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime,
            EventMask = t => modifierMask
        };

        await provider.RunAndWaitForStart();

        // Assert

        var hookEnabledEvents = provider.PostedEvents
            .Where(e => e.Type == EventType.HookEnabled)
            .ToList();

        Assert.Single(hookEnabledEvents);

        var hookEnabledEvent = hookEnabledEvents[0];

        Assert.Equal(dateTime.ToUnixTimeMilliseconds(), (long)hookEnabledEvent.Time);
        Assert.Equal(modifierMask, hookEnabledEvent.Mask);
        Assert.Equal(EventReservedValueMask.None, hookEnabledEvent.Reserved);

        // Clean up

        provider.Stop();
    }

    [Property(DisplayName = "HookDisabled should be raised when the hook is stopped")]
    public async void HookDisabled(DateTimeAfterEpoch dateTime, ModifierMask modifierMask)
    {
        // Act

        var provider = new TestProvider
        {
            EventDateTime = t => dateTime.Value,
            EventMask = t => modifierMask
        };

        await provider.RunAndWaitForStart();

        provider.Stop();

        // Assert

        var hookDisabledEvents = provider.PostedEvents
            .Where(e => e.Type == EventType.HookDisabled)
            .ToList();

        Assert.Single(hookDisabledEvents);

        var hookDisabledEvent = hookDisabledEvents[0];

        Assert.Equal(dateTime.Value.ToUnixTimeMilliseconds(), (long)hookDisabledEvent.Time);
        Assert.Equal(modifierMask, hookDisabledEvent.Mask);
        Assert.Equal(EventReservedValueMask.None, hookDisabledEvent.Reserved);
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

    [Property(DisplayName = "PostEvent should raise an event about posted event when not running")]
    public async void PostEventRaisesEvent(UioHookEvent eventToPost)
    {
        // Arrange

        var provider = new TestProvider();

        // Act

        var args = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.NotNull(args);
        Assert.False(args.HookCalled);
        Assert.Equal(eventToPost, args.Event);
    }

    [Property(DisplayName = "PostEventAndWaitForHandler should return null if configured to do so")]
    public async void PostEventAndWaitForHandlerFail(UioHookEvent eventToPost, FailedUioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostEventResult = result.Value
        };

        // Act

        var args = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.Null(args);
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

    [Property(DisplayName = "Post text delay should be settable through the property")]
    public void PostTextDelayX11(ulong postTextDelay)
    {
        // Act

        var provider = new TestProvider
        {
            PostTextDelayX11 = postTextDelay
        };

        // Assert

        Assert.Equal(postTextDelay, provider.PostTextDelayX11);
        Assert.Equal(postTextDelay, ((IEventSimulationProvider)provider).GetPostTextDelayX11());
    }

    [Property(DisplayName = "Post text delay should be settable through the method")]
    public void SetPostTextDelayX11(ulong postTextDelay)
    {
        // Act

        var provider = new TestProvider();
        ((IEventSimulationProvider)provider).SetPostTextDelayX11(postTextDelay);

        // Assert

        Assert.Equal(postTextDelay, provider.PostTextDelayX11);
        Assert.Equal(postTextDelay, ((IEventSimulationProvider)provider).GetPostTextDelayX11());
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
}
