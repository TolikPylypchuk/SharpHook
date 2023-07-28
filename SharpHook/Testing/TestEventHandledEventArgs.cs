namespace SharpHook.Testing;

using System;

using SharpHook.Native;

/// <summary>
/// Event args for <see cref="TestProvider.TestEventHandled" /> and <see cref="TestGlobalHook.TestEventHandled" />.
/// </summary>
public class TestEventHandledEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestEventHandledEventArgs" /> class.
    /// </summary>
    /// <param name="event">The event which was handled.</param>
    /// <param name="hookCalled">The value which specifies whether the hook was called for this event.</param>
    public TestEventHandledEventArgs(UioHookEvent @event, bool hookCalled)
    {
        this.Event = @event;
        this.HookCalled = hookCalled;
    }

    /// <summary>
    /// Gets the event which was handled.
    /// </summary>
    /// <value>The event which was handled.</value>
    public UioHookEvent Event { get; }

    /// <summary>
    /// Gets the value which specifies whether the hook was called for this event - <see langword="true" /> if the test
    /// provider was running when <see cref="TestProvider.PostEvent(ref UioHookEvent)" /> was called, and
    /// <see langword="false" /> otherwise.
    /// </summary>
    /// <value>The value which specifies whether the hook was called for this event</value>
    public bool HookCalled { get; }
}
