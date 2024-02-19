using System.Collections.Immutable;

namespace SharpHook.Native;

public sealed class ModifierMaskExtensionsTests
{
    private static readonly ImmutableList<ModifierMask> BasicModifiers =
    [
        ModifierMask.LeftShift,
        ModifierMask.LeftCtrl,
        ModifierMask.LeftMeta,
        ModifierMask.LeftAlt,
        ModifierMask.RightShift,
        ModifierMask.RightCtrl,
        ModifierMask.RightMeta,
        ModifierMask.RightAlt,
        ModifierMask.Button1,
        ModifierMask.Button2,
        ModifierMask.Button3,
        ModifierMask.Button4,
        ModifierMask.Button5,
        ModifierMask.NumLock,
        ModifierMask.CapsLock,
        ModifierMask.ScrollLock,
    ];

    [Property(DisplayName = "HasShift should return true only for Shift")]
    public void HasShift(ModifierMask mask) =>
        Assert.True(mask.HasShift() == (mask.HasFlag(ModifierMask.LeftShift) || mask.HasFlag(ModifierMask.RightShift)));

    [Property(DisplayName = "HasCtrl should return true only for Ctrl")]
    public void HasCtrl(ModifierMask mask) =>
        Assert.True(mask.HasCtrl() == (mask.HasFlag(ModifierMask.LeftCtrl) || mask.HasFlag(ModifierMask.RightCtrl)));

    [Property(DisplayName = "HasAlt should return true only for Alt")]
    public void HasAlt(ModifierMask mask) =>
        Assert.True(mask.HasAlt() == (mask.HasFlag(ModifierMask.LeftAlt) || mask.HasFlag(ModifierMask.RightAlt)));

    [Property(DisplayName = "HasMeta should return true only for Meta")]
    public void HasMeta(ModifierMask mask) =>
        Assert.True(mask.HasMeta() == (mask.HasFlag(ModifierMask.LeftMeta) || mask.HasFlag(ModifierMask.RightMeta)));

    [Property(DisplayName = "HasAny should return true if the mask contains any specified modifier")]
    public void HasAny(ModifierMask mask1, ModifierMask mask2) =>
        Assert.True(mask1.HasAny(mask2) == mask2.Split().Any(m => mask1.HasFlag(m)));

    [Property(DisplayName = "HasAll should return true if the mask contains all specified modifiers")]
    public void HasAll(ModifierMask mask1, ModifierMask mask2) =>
        Assert.True(mask1.HasAll(mask2) == mask2.Split().All(m => mask1.HasFlag(m)));

    [Property(DisplayName = "Split should split a modifier mask into modifiers")]
    public void Split(ModifierMask mask)
    {
        var modifiers = mask.Split();
        Assert.True(BasicModifiers.All(modifier => mask.HasFlag(modifier) == modifiers.Contains(modifier)));
    }

    [Property(DisplayName = "Merge should merge modifiers into a modifier mask")]
    public void Merge(ModifierMask[] modifiers)
    {
        var mask = modifiers.Merge();
        Assert.True(Enum.GetValues<ModifierMask>()
            .All(modifier => !modifiers.Contains(modifier) || mask.HasFlag(modifier)));
    }

    [Fact(DisplayName = "Merge should throw on null")]
    public void MergeNull()
    {
        ModifierMask[] modifiers = null!;
        Assert.Throws<ArgumentNullException>(() => modifiers.Merge());
    }
}
