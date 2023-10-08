# SharpHook

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

## Usage

### Native Functions of libuiohook

SharpHook exposes the functions of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains types which represent the data used by libuiohook.

In general, you don't need to use the native methods directly. Instead, use the higher-level interfaces and classes
provided by SharpHook. However, you should still read this section to know how the high-level features work under
the hood.

If you want to use the low-level functionality, you don't need to use the `UioHook` class directly. Instead you can use
interfaces in the `SharpHook.Providers` namespace. The methods in those interfaces are the same as in the `UioHook`
class. `SharpHook.Providers.UioHookProvider` implements all of these interfaces and simply calls the corresponding
methods in `UioHook`. This should be done to decouple your code from `UioHook` and make testing easier.

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.
- `Run` - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` - destroys the global hook.

You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
previously set one.

Additionally, `UioHook` contains the `PostEvent` method for simulating input events, and the `SetLoggerProc` method for
setting the log callback.

Starting with version 5.0.0, SharpHook also provides text entry simulation and `UioHook` contains the `PostText` method.
The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including surrogate pairs,
e.g. emojis) is supported.

libuiohook also provides functions to get various system properties. The corresponding methods are also present in
`UioHook`.

### Global Hooks

SharpHook provides the `IGlobalHook` interface along with two default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```C#
using SharpHook;

// ...

var hook = new TaskPoolGlobalHook();

hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>

hook.KeyTyped += OnKeyTyped;           // EventHandler<KeyboardHookEventArgs>
hook.KeyPressed += OnKeyPressed;       // EventHandler<KeyboardHookEventArgs>
hook.KeyReleased += OnKeyReleased;     // EventHandler<KeyboardHookEventArgs>

hook.MouseClicked += OnMouseClicked;   // EventHandler<MouseHookEventArgs>
hook.MousePressed += OnMousePressed;   // EventHandler<MouseHookEventArgs>
hook.MouseReleased += OnMouseReleased; // EventHandler<MouseHookEventArgs>
hook.MouseMoved += OnMouseMoved;       // EventHandler<MouseHookEventArgs>
hook.MouseDragged += OnMouseDragged;   // EventHandler<MouseHookEventArgs>

hook.MouseWheel += OnMouseWheel;       // EventHandler<MouseWheelHookEventArgs>

hook.Run();
// or
await hook.RunAsync();
```

First, you create the hook, then subscribe to its events, and then run it. The `Run` method runs the hook on the current
thread, blocking it. The `RunAsync()` method runs the hook on a separate thread and returns a `Task` which is finished
when the hook is destroyed. You can subscribe to events after the hook is started.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's destroyed. The contract of
the interface is that once a hook has been destroyed, it cannot be started again - you'll have to create a new instance.
Calling `Dispose` when the hook is not running is safe - it just won't do anything (other than marking the instance as
disposed).

Hook events are of type `HookEvent` or a derived type which contains more info. It's possible to suppress event
propagation by setting the `SuppressEvent` property to `true` inside the event handler. This must be done synchronously
and is only supported on Windows and macOS.

**Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use
the same static method to set the hook callback for libuiohook, so there may only be one callback at a time.

SharpHook provides two implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining handlers.
This means that the hook will be able to process all events. This implementation should be preferred to
`SimpleGlobalHook` except for very simple use-cases. But it has a downside - suppressing event propagation will be
ignored since event handlers are run on other threads.

The library also provides the `SharpHook.GlobalHookBase` class which you can extend to create your own implementation
of the global hook. It calls the appropriate event handlers, and you only need to implement a strategy for dispatching
the events. It also contains a finalizer which will stop the global hook if this object is not reachable anymore.

### Reactive Global Hooks

Use the [SharpHook.Reactive](https://www.nuget.org/packages/SharpHook.Reactive) package for reactive global hooks.

### Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. Here's a quick
example:

```C#
using SharpHook;
using SharpHook.Native;

// ...

var simulator = new EventSimulator();

// Press Ctrl+C
simulator.SimulateKeyPress(KeyCode.VcLeftControl);
simulator.SimulateKeyPress(KeyCode.VcC);

// Release Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC);
simulator.SimulateKeyRelease(KeyCode.VcLeftControl);

// Press the left mouse button
simulator.SimulateMousePress(MouseButton.Button1);

// Release the left mouse button
simulator.SimulateMouseRelease(MouseButton.Button1);

// Press the left mouse button at (0, 0)
simulator.SimulateMousePress(0, 0, MouseButton.Button1);

// Release the left mouse button at (0, 0)
simulator.SimulateMouseRelease(0, 0, MouseButton.Button1);

// Move the mouse pointer to (0, 0)
simulator.SimulateMouseMovement(0, 0);

// Move the mouse pointer 50 pixels to the right and 100 pixels down
simulator.SimulateMouseMovementRelative(50, 100);

// Scroll the mouse wheel
simulator.SimulateMouseWheel(
    rotation: -120,
    direction: MouseWheelScrollDirection.Vertical, // MouseWheelScrollDirection.Vertical by default
    type: MouseWheelScrollType.UnitScroll); // MouseWheelScrollType.UnitScroll by default
```

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events.

### Text Entry Simulation

Starting with version 5.0.0, SharpHook also provides text entry simulation. `IEventSimulator` contains the
`SimulateTextEntry` method which accepts a `string`. The text to simulate doesn't depend on the current keyboard layout.
The full range of UTF-16 (including surrogate pairs, e.g. emojis) is supported.

### Logging

libuiohook can log messages throughout its execution. By default the messages are not logged anywhere, but you can get
these logs by using the `ILogSource` interface and its default implementation, `LogSource`:

```C#
using SharpHook.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

As with global hooks, you should use only one `LogSource` object at a time. `ILogSource` extends `IDisposable` - you
can dispose of a log source to stop receiving libuiohook messages. You should keep a reference to an instance of
`LogSource` when you use it since it will stop receiving messages when garbage collector deletes it, to avoid memory
leaks.

An `EmptyLogSource` class is also available - this class doesn't listen to the libuiohook logs and can be used instead
of `LogSource` in release builds.

### Testing

SharpHook provides two classes which make testing easier. They aren't required since mocks can be used instead, but
unlike mocks, no setup is required to use these classes.

`SharpHook.Testing.TestGlobalHook` provides an implementation of `IGlobalHook` and `IEventSimulator` which can be used
for testing. When the `Run` or `RunAsync` method is called, it will dispatch events using the various `Simulate` methods
from `IEventSimulator`.

If this class is used as an `IEventSimulator` in tested code, then the `SimulatedEvents` property can be checked to see
which events were simulated using the test instance.

If an `IReactiveGlobalHook` is needed for testing, then `ReactiveGlobalHookAdapter` can be used to adapt an instance of
`TestGlobalHook`.

If the low-level functionality of SharpHook should be mocked, or mocking should be pushed as far away as possible,
then `SharpHook.Testing.TestProvider` can be used. It implements every interface in the `SharpHook.Providers` namespace
and as such it can be used instead of a normal low-level functionality provider.

Like `TestGlobalHook`, this class can post events using the `PostEvent` method and dispatch them if `Run` was called.
It also contains the `PostedEvents` property.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
