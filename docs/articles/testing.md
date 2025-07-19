# Testing

SharpHook provides two classes which make testing easier. They aren't required since mocks can be used instead, but
unlike mocks, no setup is required to use these classes, and they respect the invariants of their real counterparts.

> [!NOTE]
> Testing classes are contained directly in the `SharpHook` package. This increases the package size, but these classes
> will be removed if trimming is enabled.

## Test Global Hook

`SharpHook.Testing.TestGlobalHook` provides an implementation of `IGlobalHook` and `IEventSimulator` which can be used
for testing. When the `Run` or `RunAsync` method is called, it will dispatch events using the various `Simulate` methods
from `IEventSimulator`.

Here's a very simple example of a test which utilizes `TestGlobalHook`:

```c#
[Fact]
public async Task TestLastPressedKey()
{
    var hook = new TestGlobalHook();
    var keyCode = KeyCode.VcA;
    var componentUnderTest = new SomeClassWhichUsesGlobalHookEvents(hook);

    // Run the test hook asynchronously and ignore the result
    var hookStopped = hook.RunAsync();

    hook.SimulateKeyPress(keyCode);

    hook.Dispose();
    await hookStopped;

    Assert.Equal(keyCode, componentUnderTest.LastPressedKey);
}
```

If this class is used as an `IEventSimulator` in the tested code, then the `SimulatedEvents` property can be checked to
see which events were simulated using the test instance.

`TestGlobalHook` can be used with two different threading modes – the simple mode and the event loop mode.

When running in simple mode (the default mode) the hook will dispatch events immediately in the same thread which
simulates them.

When running the event loop, the hook will run an actual even loop on the thread on which `Run` was called. When
simulating events, they will be posted to the event loop, and then dispatched in the hook thread. This mode is much more
difficult to use correctly, so it is useful only when it is important for event handlers to run in the same thread on
which the hook itself is running. It's recommended to stop the global hook and wait for it to stop before asserting any
events. Since these actions are done in different threads, it is not guaranteed that an event will be dispatched
immediately after it is simulated. `Run` and the task returned by `RunAsync` will complete after every event has been
dispatched.

Other than that, members of `TestGlobalHook` are quite straightforward; the API reference should be viewed for more
info.

If an `IReactiveGlobalHook` is needed for testing, then `ReactiveGlobalHookAdapter` can be used to adapt an instance of
`TestGlobalHook`.

## Test Low-Level Functionality Provider

If the low-level functionality of SharpHook should be mocked, or mocking should be pushed as far away as possible,
then `SharpHook.Testing.TestProvider` can be used. It implements every interface in the `SharpHook.Providers` namespace,
and as such, it can be used instead of normal low-level functionality providers.

Like `TestProvider`, this class can post events using the `PostEvent` method and dispatch them if `Run` has been
called. It also contains the `PostedEvents` property.

`TestProvider` can be used with two different threading modes – the simple mode and the event loop mode.

When running in simple mode (the default mode) the provider will dispatch events immediately in the same thread which
simulates them.

When running the event loop, the provider will run an actual even loop on the thread on which `Run` was called. When
simulating events, they will be posted to the event loop, and then dispatched in the hook thread. This mode is much more
difficult to use correctly, so it is useful only when it is important for event handlers to run in the same thread on
which the hook itself is running. It's recommended to stop the provider and wait for it to stop before asserting any
events. Since these actions are done in different threads, it is not guaranteed that an event will be dispatched
immediately after it is simulated. `Run` and the task returned by `RunAsync` will complete after every event has been
dispatched.

All classes in SharpHook use providers instead of directly using the `UioHook` class for low-level functionality.
The providers are selectable, so e.g., the following global hook can be used for testing:

```c#
var testProvider = new TestProvider();

// Calls to methods in testProvider will be reflected in the hook
var hook = new SimpleGlobalHook(globalHookProvider: testProvider);

// Calls to methods in testProvider will be reflected in the simulator
var simulator = new EventSimulator(simulationProvider: testProvider);
```

> [!NOTE]
> `TaskPoolGlobalHook` shouldn't be used this way since its event handlers are asynchronous and there is no built-in
> way to know when they are actually executed. As such, it's difficult to check event handler results. If you want
> to use a real hook, e.g. for integration testing, then use `SimpleGlobalHook` instead.
