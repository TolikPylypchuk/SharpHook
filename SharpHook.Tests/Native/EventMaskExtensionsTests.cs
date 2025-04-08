using System.Collections.Immutable;

namespace SharpHook.Native;

public sealed class EventMaskExtensionsTests
{
    private static readonly ImmutableList<EventMask> BasicFlags =
    [
        EventMask.LeftShift,
        EventMask.LeftCtrl,
        EventMask.LeftMeta,
        EventMask.LeftAlt,
        EventMask.RightShift,
        EventMask.RightCtrl,
        EventMask.RightMeta,
        EventMask.RightAlt,
        EventMask.Button1,
        EventMask.Button2,
        EventMask.Button3,
        EventMask.Button4,
        EventMask.Button5,
        EventMask.NumLock,
        EventMask.CapsLock,
        EventMask.ScrollLock,
        EventMask.SimulatedEvent,
        EventMask.SuppressEvent,
    ];

    [Property(DisplayName = "HasShift should return true only for Shift")]
    public void HasShift(EventMask mask) =>
        Assert.True(mask.HasShift() == (mask.HasFlag(EventMask.LeftShift) || mask.HasFlag(EventMask.RightShift)));

    [Property(DisplayName = "HasCtrl should return true only for Ctrl")]
    public void HasCtrl(EventMask mask) =>
        Assert.True(mask.HasCtrl() == (mask.HasFlag(EventMask.LeftCtrl) || mask.HasFlag(EventMask.RightCtrl)));

    [Property(DisplayName = "HasAlt should return true only for Alt")]
    public void HasAlt(EventMask mask) =>
        Assert.True(mask.HasAlt() == (mask.HasFlag(EventMask.LeftAlt) || mask.HasFlag(EventMask.RightAlt)));

    [Property(DisplayName = "HasMeta should return true only for Meta")]
    public void HasMeta(EventMask mask) =>
        Assert.True(mask.HasMeta() == (mask.HasFlag(EventMask.LeftMeta) || mask.HasFlag(EventMask.RightMeta)));

    [Property(DisplayName = "HasAny should return true if the mask contains any specified flag")]
    public void HasAny(EventMask mask1, EventMask mask2) =>
        Assert.True(mask1.HasAny(mask2) == mask2.Split().Any(m => mask1.HasFlag(m)));

    [Property(DisplayName = "HasAll should return true if the mask contains all specified flags")]
    public void HasAll(EventMask mask1, EventMask mask2) =>
        Assert.True(mask1.HasAll(mask2) == mask2.Split().All(m => mask1.HasFlag(m)));

    [Property(DisplayName = "Split should split a modifier mask into flags")]
    public void Split(EventMask mask)
    {
        var modifiers = mask.Split();
        Assert.True(BasicFlags.All(modifier => mask.HasFlag(modifier) == modifiers.Contains(modifier)));
    }

    [Property(DisplayName = "Merge should merge flags into a mask")]
    public void Merge(EventMask[] modifiers)
    {
        var mask = modifiers.Merge();
        Assert.True(Enum.GetValues<EventMask>()
            .All(modifier => !modifiers.Contains(modifier) || mask.HasFlag(modifier)));
    }

    [Fact(DisplayName = "Merge should throw on null")]
    public void MergeNull()
    {
        EventMask[] modifiers = null!;
        Assert.Throws<ArgumentNullException>(() => modifiers.Merge());
    }
}
