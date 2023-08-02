namespace SharpHook;

public sealed class HookEventArgsTests
{
    public HookEventArgsTests() =>
        Arb.Register<Generators>();

    [Property(DisplayName = "FromEvent should create a correct instance of HookEventArgs")]
    public void FromEvent(UioHookEvent uioHookEvent)
    {
        // Act

        var args = HookEventArgs.FromEvent(uioHookEvent);

        // Assert

        Assert.Equal(args.RawEvent, uioHookEvent);
        Assert.Equal((ulong)args.EventTime.ToUnixTimeMilliseconds(), uioHookEvent.Time);
        Assert.False(args.SuppressEvent);

        if (uioHookEvent.Type is EventType.HookEnabled or EventType.HookDisabled)
        {
            Assert.IsType<HookEventArgs>(args);
        } else if (uioHookEvent.Type is EventType.KeyPressed or EventType.KeyReleased or EventType.KeyTyped)
        {
            Assert.IsType<KeyboardHookEventArgs>(args);
            var keyboardArgs = args as KeyboardHookEventArgs;
            Assert.Equal(uioHookEvent.Keyboard, keyboardArgs!.Data);
        } else if (uioHookEvent.Type is EventType.MouseWheel)
        {
            Assert.IsType<MouseWheelHookEventArgs>(args);
            var keyboardArgs = args as MouseWheelHookEventArgs;
            Assert.Equal(uioHookEvent.Wheel, keyboardArgs!.Data);
        } else
        {
            Assert.IsType<MouseHookEventArgs>(args);
            var keyboardArgs = args as MouseHookEventArgs;
            Assert.Equal(uioHookEvent.Mouse, keyboardArgs!.Data);
        }
    }
}
