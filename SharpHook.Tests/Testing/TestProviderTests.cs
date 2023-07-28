namespace SharpHook.Testing;

public sealed class TestProviderTests
{
    [Fact(DisplayName = "SetDispatchProc, Run, and PostEvent should work together")]
    public async void SetDispatchProc()
    {
        // Arrange

        var eventToPost = CreateUioHookEvent();
        var userData = (IntPtr)1;

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

        await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.Equal(eventToPost, actualEvent);
        Assert.Equal(userData, actualUserData);

        // Clean up

        provider.Stop();
    }

    [Fact(DisplayName = "Events should be suppressible")]
    public async void SuppressEvent()
    {
        // Arrange

        var eventToPost = CreateUioHookEvent();

        var provider = new TestProvider();

        // Act

        provider.SetDispatchProc(
            (ref UioHookEvent e, IntPtr data) => e.Reserved |= EventReservedValueMask.SuppressEvent,
            IntPtr.Zero);

        await provider.RunAndWaitForStart();

        var result = await provider.PostEventAndWaitForHandler(ref eventToPost);

        // Assert

        Assert.NotNull(result);
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

    [Theory(DisplayName = "Run should return an error if configured to do so")]
    [ClassData(typeof(FailedUioHookResultsData))]
    public void RunFail(UioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            RunResult = result
        };

        // Act

        var actualResult = provider.Run();

        // Assert

        Assert.False(provider.IsRunning);
        Assert.Equal(result, actualResult);
    }

    [Theory(DisplayName = "Stop should return an error if configured to do so")]
    [ClassData(typeof(FailedUioHookResultsData))]
    public async void StopFail(UioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            StopResult = result
        };

        await provider.RunAndWaitForStart();

        // Act

        var actualResult = provider.Stop();

        // Assert

        Assert.Equal(result, actualResult);
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

    [Fact(DisplayName = "HookDisabled should be raised when the hook is stopped")]
    public async void HookDisabled()
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

        provider.Stop();

        // Assert

        var hookDisabledEvents = provider.PostedEvents
            .Where(e => e.Type == EventType.HookDisabled)
            .ToList();

        Assert.Single(hookDisabledEvents);

        var hookDisabledEvent = hookDisabledEvents[0];

        Assert.Equal(dateTime.ToUnixTimeMilliseconds(), (long)hookDisabledEvent.Time);
        Assert.Equal(modifierMask, hookDisabledEvent.Mask);
        Assert.Equal(EventReservedValueMask.None, hookDisabledEvent.Reserved);
    }

    [Fact(DisplayName = "PostEvent should post an event")]
    public void PostEvent()
    {
        // Arrange

        var eventToPost = CreateUioHookEvent();

        var provider = new TestProvider();

        // Act

        var actualResult = provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Equal(UioHookResult.Success, actualResult);
        Assert.Single(provider.PostedEvents);
        Assert.Equal(eventToPost, provider.PostedEvents[0]);
    }

    [Theory(DisplayName = "PostEvent should return an error if configured to do so")]
    [ClassData(typeof(FailedUioHookResultsData))]
    public void PostEventFail(UioHookResult result)
    {
        // Arrange

        var eventToPost = CreateUioHookEvent();

        var provider = new TestProvider
        {
            PostEventResult = result
        };

        // Act

        var actualResult = provider.PostEvent(ref eventToPost);

        // Assert

        Assert.Empty(provider.PostedEvents);
        Assert.Equal(result, actualResult);
    }

    [Fact(DisplayName = "PostText should post text")]
    public void PostText()
    {
        // Arrange

        const string text = "text";
        var provider = new TestProvider();

        // Act

        var actualResult = provider.PostText(text);

        // Assert

        Assert.Equal(UioHookResult.Success, actualResult);
        Assert.Single(provider.PostedText);
        Assert.Equal(text, provider.PostedText[0]);
    }

    [Theory(DisplayName = "PostText should return an error if configured to do so")]
    [ClassData(typeof(FailedUioHookResultsData))]
    public void PostTextFail(UioHookResult result)
    {
        // Arrange

        var provider = new TestProvider
        {
            PostTextResult = result
        };

        // Act

        var actualResult = provider.PostText("text");

        // Assert

        Assert.Empty(provider.PostedText);
        Assert.Equal(result, actualResult);
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

    [Fact(DisplayName = "Post text delay should be settable through the property")]
    public void PostTextDelayX11()
    {
        // Arrange

        const ulong postTextDelay = 10;

        // Act

        var provider = new TestProvider
        {
            PostTextDelayX11 = postTextDelay
        };

        // Assert

        Assert.Equal(postTextDelay, provider.PostTextDelayX11);
        Assert.Equal(postTextDelay, ((IEventSimulationProvider)provider).GetPostTextDelayX11());
    }

    [Fact(DisplayName = "Post text delay should be settable through the method")]
    public void SetPostTextDelayX11()
    {
        // Arrange

        const ulong postTextDelay = 10;

        // Act

        var provider = new TestProvider();
        ((IEventSimulationProvider)provider).SetPostTextDelayX11(postTextDelay);

        // Assert

        Assert.Equal(postTextDelay, provider.PostTextDelayX11);
        Assert.Equal(postTextDelay, ((IEventSimulationProvider)provider).GetPostTextDelayX11());
    }

    [Fact(DisplayName = "Screen info should be settable")]
    public void ScreenInfo()
    {
        // Arrange

        var screenInfo = new[] { new ScreenData { Number = 1, X = 0, Y = 0, Width = 1920, Height = 1080 } };

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

    [Fact(DisplayName = "Auto-repeat rate should be settable")]
    public void AutoRepeatRate()
    {
        // Arrange

        const int autoRepeatRate = 10;

        // Act

        var provider = new TestProvider
        {
            AutoRepeatRate = autoRepeatRate
        };

        // Assert

        Assert.Equal(autoRepeatRate, provider.AutoRepeatRate);
        Assert.Equal(autoRepeatRate, ((IMouseInfoProvider)provider).GetAutoRepeatRate());
    }

    [Fact(DisplayName = "Auto-repeat delay should be settable")]
    public void AutoRepeatDelay()
    {
        // Arrange

        const int autoRepeatDelay = 10;

        // Act

        var provider = new TestProvider
        {
            AutoRepeatDelay = autoRepeatDelay
        };

        // Assert

        Assert.Equal(autoRepeatDelay, provider.AutoRepeatDelay);
        Assert.Equal(autoRepeatDelay, ((IMouseInfoProvider)provider).GetAutoRepeatDelay());
    }

    [Fact(DisplayName = "Pointer acceleration multiplier should be settable")]
    public void PointerAccelerationMultiplier()
    {
        // Arrange

        const int multiplier = 10;

        // Act

        var provider = new TestProvider
        {
            PointerAccelerationMultiplier = multiplier
        };

        // Assert

        Assert.Equal(multiplier, provider.PointerAccelerationMultiplier);
        Assert.Equal(multiplier, ((IMouseInfoProvider)provider).GetPointerAccelerationMultiplier());
    }

    [Fact(DisplayName = "Pointer acceleration threshold should be settable")]
    public void PointerAccelerationThreshold()
    {
        // Arrange

        const int threshold = 10;

        // Act

        var provider = new TestProvider
        {
            PointerAccelerationThreshold = threshold
        };

        // Assert

        Assert.Equal(threshold, provider.PointerAccelerationThreshold);
        Assert.Equal(threshold, ((IMouseInfoProvider)provider).GetPointerAccelerationThreshold());
    }

    [Fact(DisplayName = "Pointer sensitivity should be settable")]
    public void PointerSensitivity()
    {
        // Arrange

        const int sensitivity = 10;

        // Act

        var provider = new TestProvider
        {
            PointerSensitivity = sensitivity
        };

        // Assert

        Assert.Equal(sensitivity, provider.PointerSensitivity);
        Assert.Equal(sensitivity, ((IMouseInfoProvider)provider).GetPointerSensitivity());
    }

    [Fact(DisplayName = "Multi-click time should be settable")]
    public void MultiClickTime()
    {
        // Arrange

        const int multiClickTime = 10;

        // Act

        var provider = new TestProvider
        {
            MultiClickTime = multiClickTime
        };

        // Assert

        Assert.Equal(multiClickTime, provider.MultiClickTime);
        Assert.Equal(multiClickTime, ((IMouseInfoProvider)provider).GetMultiClickTime());
    }

    private static UioHookEvent CreateUioHookEvent() =>
        new()
        {
            Type = EventType.KeyPressed,
            Time = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Mask = ModifierMask.None,
            Reserved = EventReservedValueMask.None,
            Keyboard = new KeyboardEventData
            {
                KeyCode = KeyCode.Vc1,
                RawCode = 1,
                RawKeyChar = 0xFF
            }
        };
}
