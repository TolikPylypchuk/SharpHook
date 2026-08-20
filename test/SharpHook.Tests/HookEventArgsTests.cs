namespace SharpHook;

public sealed class HookEventArgsTests
{
    [Property(DisplayName = "FromEvent should create a correct instance of HookEventArgs")]
    public void FromEvent(UioHookEvent uioHookEvent)
    {
        // Act

        var args = HookEventArgs.FromEvent(uioHookEvent);

        // Assert

        Assert.Equal(args.RawEvent, uioHookEvent);
        Assert.Equal((ulong)args.EventTime.ToUnixTimeMilliseconds(), uioHookEvent.Time);
        Assert.False(args.SuppressEvent);
        Assert.Equal(uioHookEvent.Mask.HasFlag(EventMask.SimulatedEvent), args.IsEventSimulated);

        if (uioHookEvent.Type is EventType.HookEnabled or EventType.HookDisabled)
        {
            Assert.IsType<HookEventArgs>(args);
        } else if (uioHookEvent.Type is EventType.KeyPressed or EventType.KeyReleased or EventType.KeyTyped)
        {
            var keyboardArgs = Assert.IsType<KeyboardHookEventArgs>(args);
            Assert.Equal(uioHookEvent.Keyboard, keyboardArgs.Data);
        } else if (uioHookEvent.Type is EventType.MouseWheel)
        {
            var mouseWheelArgs = Assert.IsType<MouseWheelHookEventArgs>(args);
            Assert.Equal(uioHookEvent.Wheel, mouseWheelArgs.Data);
        } else
        {
            var mouseArgs = Assert.IsType<MouseHookEventArgs>(args);
            Assert.Equal(uioHookEvent.Mouse, mouseArgs.Data);
        }
    }
}
