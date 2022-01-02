# SharpHook

SharpHook provides a cross-platform global keyboard and mouse hook for .NET, and the ability to simulate input events.
It is a thin wrapper of [libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as
well as higher-level types to work with it.

## Usage

### Native Methods of libuiohook

SharpHook exposes the methods of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains structs and enums which represent the data returned by libuiohook.

**Note**: In general, you shouldn't use native methods directly. Instead, use the higher-level types provided by
SharpHook.

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.
- `Run` - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` - destroys the global hook.

Additionally, `UioHook` contains the `PostEvent` method for simulating input events.

libuiohook also provides functions to get various system properties. The corresponding methods are also present in
`UioHook`.

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

await hook.Start();
```

First, you create the hook, then subscribe to its events, and then start it. The `Start()` method returns a `Task`
which is finished when the hook is destroyed, so if you `await` it, you block the current async context until it stops.
You can subscribe to events after the hook is started.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's destroyed. The contract of
the interface is that once a hook has been destroyed, it cannot be started again - you'll have to create a new instance.

**Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use
the same static method to set the hook callback for libuiohook, and there may only be one callback at a time.

SharpHook provides two implementations of `IGlobalHook`:

- `SimpleGlobalHook` runs the hook on a separate thread, and runs all of its event handlers on that same thread. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `TaskPoolGlobalHook` runs the hook on a separate thread, and runs all of its event handlers on other threads inside
the default thread pool for tasks. The parallelism level of the handlers can be configured. On backpressure it will
queue the remaining handlers. This means that the hook will be able to process all events. This implementation should be
preferred to `SimpleGlobalHook` except for very simple use-cases.

The library also provides the `GlobalHookBase` class which you can extend to create your own implementation of the
global hook. It runs the hook on a separate thread and calls appropriate event handlers. You only need to implement a
strategy for dispatching the events.

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

simulator.SimulateKeyPress(KeyCode.VcC, ModifierMask.LeftCtrl);   // Press Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC, ModifierMask.LeftCtrl); // Release Ctrl+C

simulator.SimulateMousePress(MouseButton.Button1);   // Press the left mouse button
simulator.SimulateMouseRelease(MouseButton.Button1); // Release the left mouse button

simulator.SimulateMouseMovement(0, 0);      // Move the mouse pointer to the (0, 0) point
simulator.SimulateMouseWheel(0, 0, 10, -1); // Move the mouse pointer to the (0, 0) point, and scroll the mouse wheel
```

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events.

**Important**: libuiohook [ignores](https://github.com/kwhat/libuiohook/issues/111) modifier masks on Windows, so you
need to simulate pressing/releasing modifier keys manually.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
