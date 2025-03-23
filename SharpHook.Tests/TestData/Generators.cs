namespace SharpHook.TestData;

public sealed class Generators
{
    public static Arbitrary<IntPtr> IntPtrs =>
        ArbMap.Default.GeneratorFor<int>().Select(v => (IntPtr)v).ToArbitrary();

    public static Arbitrary<UioHookEvent> UioHookEvents =>
        Gen.OneOf(HookEvents, KeyboardEvents, MouseEvents, WheelEvents).ToArbitrary();

    public static Arbitrary<KeyboardEvent> KeyboardUioHookEvents =>
        (from e in KeyboardEvents
         select new KeyboardEvent(e))
        .ToArbitrary();

    public static Arbitrary<MouseEvent> MouseUioHookEvents =>
        (from e in MouseEvents
         select new MouseEvent(e))
        .ToArbitrary();

    public static Arbitrary<DateTimeAfterEpoch> DateTimesAfterEpoch =>
        (from time in ArbMap.Default.GeneratorFor<DateTimeOffset>()
         where time > DateTimeOffset.UnixEpoch
         select new DateTimeAfterEpoch(time))
        .ToArbitrary();

    public static Arbitrary<UioHookResult> UioHookResults =>
        Gen.Elements(Enum.GetValues<UioHookResult>()).ToArbitrary();

    public static Arbitrary<FailedUioHookResult> FailedUioHookResults =>
        (from result in ArbMap.Default.GeneratorFor<UioHookResult>()
         where result != UioHookResult.Success
         select new FailedUioHookResult(result))
        .ToArbitrary();

    private static Gen<ulong> Timestamp =>
        from time in ArbMap.Default.GeneratorFor<DateTimeOffset>()
        where time > DateTimeOffset.UnixEpoch
        select (ulong)time.ToUnixTimeMilliseconds();

    public static Arbitrary<ModifierMask> ModifierMasks =>
        Gen.Elements(Enum.GetValues<ModifierMask>()).ToArbitrary();

    private static Gen<UioHookEvent> HookEvents =>
        from type in Gen.Elements(EventType.HookEnabled, EventType.HookDisabled)
        from time in Timestamp
        from mask in ArbMap.Default.GeneratorFor<ModifierMask>()
        from isSimulated in ArbMap.Default.GeneratorFor<bool>()
        select new UioHookEvent()
        {
            Type = type,
            Time = time,
            Mask = mask,
            Reserved = isSimulated ? EventReservedValueMask.SimulatedEvent : EventReservedValueMask.None
        };

    private static Gen<UioHookEvent> KeyboardEvents =>
        from type in Gen.Elements(EventType.KeyPressed, EventType.KeyReleased, EventType.KeyTyped)
        from time in Timestamp
        from mask in ArbMap.Default.GeneratorFor<ModifierMask>()
        from keyCode in ArbMap.Default.GeneratorFor<KeyCode>()
        from rawCode in ArbMap.Default.GeneratorFor<ushort>()
        from keyChar in ArbMap.Default.GeneratorFor<UnicodeChar>()
        where keyCode != KeyCode.VcUndefined
        select new UioHookEvent()
        {
            Type = type,
            Time = time,
            Mask = mask,
            Reserved = EventReservedValueMask.None,
            Keyboard = new KeyboardEventData
            {
                KeyCode = keyCode,
                RawCode = rawCode,
                RawKeyChar = type == EventType.KeyTyped ? keyChar.Get : KeyboardEventData.RawUndefinedChar
            }
        };

    private static Gen<UioHookEvent> MouseEvents =>
        from type in Gen.Elements(
            EventType.MousePressed,
            EventType.MouseReleased,
            EventType.MouseClicked,
            EventType.MouseMoved,
            EventType.MouseDragged)
        from time in Timestamp
        from mask in ArbMap.Default.GeneratorFor<ModifierMask>()
        from x in ArbMap.Default.GeneratorFor<short>()
        from y in ArbMap.Default.GeneratorFor<short>()
        from button in ArbMap.Default.GeneratorFor<MouseButton>()
        from clicks in Gen.Choose(1, 5)
        select new UioHookEvent()
        {
            Type = type,
            Time = time,
            Mask = mask,
            Reserved = EventReservedValueMask.None,
            Mouse = new MouseEventData
            {
                X = x,
                Y = y,
                Button = type is EventType.MousePressed or EventType.MouseReleased or EventType.MouseClicked
                    ? button
                    : MouseButton.NoButton,
                Clicks = (ushort)(type == EventType.MouseClicked ? clicks : 0)
            }
        };

    private static Gen<UioHookEvent> WheelEvents =>
        from time in Timestamp
        from mask in ArbMap.Default.GeneratorFor<ModifierMask>()
        from x in ArbMap.Default.GeneratorFor<short>()
        from y in ArbMap.Default.GeneratorFor<short>()
        from scrollType in ArbMap.Default.GeneratorFor<MouseWheelScrollType>()
        from rotation in ArbMap.Default.GeneratorFor<short>()
        from delta in ArbMap.Default.GeneratorFor<ushort>()
        from direction in ArbMap.Default.GeneratorFor<MouseWheelScrollDirection>()
        select new UioHookEvent()
        {
            Type = EventType.MouseWheel,
            Time = time,
            Mask = mask,
            Reserved = EventReservedValueMask.None,
            Wheel = new MouseWheelEventData
            {
                X = x,
                Y = y,
                Type = scrollType,
                Rotation = rotation,
                Delta = delta,
                Direction = direction
            }
        };
}
