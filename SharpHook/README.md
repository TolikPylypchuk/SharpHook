# SharpHook

SharpHook provides a cross-platform global keyboard and mouse hook for .NET, and the ability to simulate input events.
It is a thin wrapper of [libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as
well as higher-level types to work with it.

## Usage

### Native Functions of libuiohook

SharpHook exposes the functions of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains structs and enums which represent the data returned by libuiohook.

**Note**: In general, you don't need to use the native methods directly. Instead, use the higher-level types provided by
SharpHook.

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.
- `Run` - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` - destroys the global hook.

Additionally, `UioHook` contains the `PostEvent` method for simulating input events, and the `SetLoggerProc` method for
setting the log callback.

libuiohook also provides functions to get various system properties. The corresponding methods are also present in
`UioHook`.

**Important**: An application manifest is required on Windows to enable DPI awareness for your app. If it's not enabled
then mouse coordinates will be wrong on high-DPI screens. You can look at the sample app in this repository to see the
manifest example.

### Default Global Hooks

SharpHook provides the `IGlobalHook` interface along with two default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```C#
using SharpHook;

// ...

var hook = new TaskPoolGlobalHook();

hook.HookEnabled += OnHookEnabled;
hook.HookDisabled += OnHookDisabled;

hook.KeyTyped += OnKeyTyped;
hook.KeyPressed += OnKeyPressed;
hook.KeyReleased += OnKeyReleased;

hook.MouseClicked += OnMouseClicked;
hook.MousePressed += OnMousePressed;
hook.MouseReleased += OnMouseReleased;
hook.MouseMoved += OnMouseMoved;
hook.MouseDragged += OnMouseDragged;

hook.MouseWheel += OnMouseWheel;

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

**Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use
the same static method to set the hook callback for libuiohook, so there may only be one callback at a time.

SharpHook provides two implementations of `IGlobalHook`:

- `SimpleGlobalHook` runs all of its event handlers on the same thread where the hook itself runs. This means that the
handlers should generally be fast since they will block the hook from handling the events that follow if they run for
too long.

- `TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for tasks. The
parallelism level of the handlers can be configured. On backpressure it will queue the remaining handlers. This means
that the hook will be able to process all events. This implementation should be preferred to `SimpleGlobalHook` except
for very simple use-cases. But it has a downside - suppressing event propagation will be ignored since event handlers
are run on other threads.

The library also provides the `GlobalHookBase` class which you can extend to create your own implementation of the
global hook. It calls appropriate event handlers, and you only need to implement a strategy for dispatching the events.

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

// Press the left mouse button at (0, 0)
simulator.SimulateMousePress(0, 0, MouseButton.Button1);

// Release the left mouse button at (0, 0)
simulator.SimulateMouseRelease(0, 0, MouseButton.Button1);

// Move the mouse pointer to (0, 0)
simulator.SimulateMouseMovement(0, 0);

// Scroll the mouse wheel at (0, 0)
simulator.SimulateMouseWheel(0, 0, 2, -120);
```

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events.

**Note**: When simulating mouse button pressing/releasing or scrolling, the mouse pointer coordinates are required. If
you need to do that at the current coordinates, then simply track the coordinates with a global hook.

### Logging

libuiohook can log messages throughout its execution. By default the messages are not logged anywhere, but you can get
these logs by using the `ILogSource` interface and its default implementation, `LogSource`:

```C#
using SharpHook.Logging;

// ...

var logSource = LogSource.Register();
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

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
