namespace SharpHook;

/// <summary>
/// Contains extension methods for <see cref="IEventSimulator" /> and <see cref="IEventSimulationSequenceBuilder" />.
/// </summary>
public static class EventSimulationExtensions
{
    /// <summary>
    /// Adds a sequecne of key press and release events which represent a single key stroke to the sequence of events to
    /// simulate.
    /// </summary>
    /// <param name="builder">The event simulation sequence builder.</param>
    /// <param name="keyCodes">The codes of the keys to press and release.</param>
    /// <returns>The builder.</returns>
    /// <remarks>
    /// As an example, if the method is called with the following parameters:
    /// <code>
    /// builder.AddKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);
    /// </code>
    /// then this will be equivalent to calling the following method sequence:
    /// <code>
    /// builder
    ///     .AddKeyPress(KeyCode.VcLeftControl)
    ///     .AddKeyPress(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcLeftControl);
    /// </code>
    /// which means that this method will add pressing the left control, then pressing C, then releasing C, then
    /// releasing the left control.
    /// </remarks>
    public static IEventSimulationSequenceBuilder AddKeyStroke(
        this IEventSimulationSequenceBuilder builder,
        params KeyCode[] keyCodes)
    {
        foreach (var keyCode in keyCodes)
        {
            builder.AddKeyPress(keyCode);
        }

        foreach (var keyCode in keyCodes.Reverse())
        {
            builder.AddKeyRelease(keyCode);
        }

        return builder;
    }

    /// <summary>
    /// Adds a sequence of key press and release events which represent a single key stroke to the sequence of events to
    /// simulate.
    /// </summary>
    /// <param name="builder">The event simulation sequence builder.</param>
    /// <param name="keyCodes">The codes of the keys to press and release.</param>
    /// <returns>The builder.</returns>
    /// <remarks>
    /// As an example, if the method is called with the following parameters:
    /// <code>
    /// builder.AddKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);
    /// </code>
    /// then this will be equivalent to calling the following method sequence:
    /// <code>
    /// builder
    ///     .AddKeyPress(KeyCode.VcLeftControl)
    ///     .AddKeyPress(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcLeftControl);
    /// </code>
    /// which means that this method will add pressing the left control, then pressing C, then releasing C, then
    /// releasing the left control.
    /// </remarks>
    public static IEventSimulationSequenceBuilder AddKeyStroke(
        this IEventSimulationSequenceBuilder builder,
        IEnumerable<KeyCode> keyCodes) =>
        builder.AddKeyStroke([.. keyCodes]);

    /// <summary>
    /// Simulates a sequence of key press and release events which represent a single key stroke.
    /// </summary>
    /// <param name="simulator">The event simulator.</param>
    /// <param name="keyCodes">The codes of the keys to press and release.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// As an example, if the method is called with the following parameters:
    /// <code>
    /// simulator.SimulateKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);
    /// </code>
    /// then this will be equivalent to calling the following method sequence:
    /// <code>
    /// simulator.Sequence()
    ///     .AddKeyPress(KeyCode.VcLeftControl)
    ///     .AddKeyPress(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcLeftControl)
    ///     .Simulate();
    /// </code>
    /// which means that this method will simualte pressing the left control, then pressing C, then releasing C, then
    /// releasing the left control.
    /// </remarks>
    public static UioHookResult SimulateKeyStroke(this IEventSimulator simulator, params KeyCode[] keyCodes) =>
        simulator.Sequence()
            .AddKeyStroke(keyCodes)
            .Simulate();

    /// <summary>
    /// Simulates a sequence of key press and release events which represent a single key stroke.
    /// </summary>
    /// <param name="simulator">The event simulator.</param>
    /// <param name="keyCodes">The codes of the keys to press and release.</param>
    /// <returns>The result of the operation.</returns>
    /// <remarks>
    /// As an example, if the method is called with the following parameters:
    /// <code>
    /// simulator.SimulateKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);
    /// </code>
    /// then this will be equivalent to calling the following method sequence:
    /// <code>
    /// simulator.Sequence()
    ///     .AddKeyPress(KeyCode.VcLeftControl)
    ///     .AddKeyPress(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcC)
    ///     .AddKeyRelease(KeyCode.VcLeftControl)
    ///     .Simulate();
    /// </code>
    /// which means that this method will simualte pressing the left control, then pressing C, then releasing C, then
    /// releasing the left control.
    /// </remarks>
    public static UioHookResult SimulateKeyStroke(this IEventSimulator simulator, IEnumerable<KeyCode> keyCodes) =>
        simulator.Sequence()
            .AddKeyStroke(keyCodes)
            .Simulate();
}
