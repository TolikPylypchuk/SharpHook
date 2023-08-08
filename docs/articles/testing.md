# Testing

SharpHook provides two classes which make testing easier. They aren't required since mocks can be used instead, but
unlike mocks, no setup is required to use these classes.

> [!NOTE]
> Testing classes are contained directly in the `SharpHook` package. This increases the package size, but these classes
> will be removed if trimming is enabled.

## Test Global Hook

`SharpHook.Testing.TestGlobalHook` provides an implementation of `IGlobalHook` and `IEventSimulator` which can be used
for testing. When the `Run` or `RunAsync` method is called, it starts an event queue which can be populated with events
using the various `Simulate` methods from `IEventSimulator`.

Here's a very simple example of a test which utilizes `TestGlobalHook`:

```c#
[Fact]
public async void TestLastPressedKey()
{
    var hook = new TestGlobalHook();
    var keyCode = KeyCode.VcA;
    var componentUnderTest = new SomeClassWhichUsesGlobalHookEvents(hook);

    // Run the test hook asynchronously and wait until IsRunning = true
    await hook.RunAndWaitForStart();

    // The default methods for simulation don't wait for event handlers, so methods which do wait are provided as well
    await hook.SimulateKeyPressAndWaitForHandler(keyCode);

    Assert.Equal(keyCode, componentUnderTest.LastPressedKey);
}
```

If this class is used as an `IEventSimulator` in tested code, then the `SimulatedEvents` property can be checked to see
which events were simulated using the test instance.

Members of `TestGlobalHook` are quite straightforward; the API reference should be viewed for more info.

If an `IReactiveGlobalHook` is needed for testing, then `ReactiveGlobalHookAdapter` can be used to adapt an instance of
`TestGlobalHook`.

## Test Low-Level Functionality Provider

If the low-level functionality of SharpHook should be mocked, or mocking should be pushed as far away as possible,
then `SharpHook.Testing.TestProvider` can be used. It implements every interface in the `SharpHook.Providers` namespace
and as such it can be used instead of a normal low-level functionality provider.

Like `TestGlobalHook`, this class can run an event loop using its `Run` method, and post events using the `PostEvent`
method. It also contains the `SimulatedEvents` property.

All classes in SharpHook use providers instead of directly using the `UioHook` class for low-level functionality.
The providers are selectable, so e.g. the following global hook can be used for testing:

```c#
var testProvider = new TestProvider();
var hook = new SimpleGlobalHook(testProvider); // Calls to test methods in testProvider will be reflected in the hook
```

> [!NOTE]
> `TaskPoolGlobalHook` shouldn't be used this way since its event handlers are asynchronous and there is no built-in
> way to know when they are actually executed. As such, it's difficult to check event handler results. If you want
> to use a real hook, e.g. for integration testing, then use `SimpleGlobalHook` instead.
