# Global Hooks

## The Interface

SharpHook provides the `SharpHook.IGlobalHook` interface along with three default implementations which you can use to
control the hook and subscribe to its events. This way is preferred to using native functions since it's more
convenient. Here's a basic usage example:

```c#
using SharpHook;

// ...

var hook = new EventLoopGlobalHook();

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

`IGlobalHook` contains separate events for every event type that can be raised by libuiohook. The sender of these
events is the `IGlobalHook` itself.

It also contains the `Run` and `RunAsync` methods which run the global hook. `Run` runs it on the current thread,
blocking it until the global hook is disposed. `RunAsync` runs the global hook in a non-blocking way and returns a
`Task` – this task is finished when the hook is stopped or disposed. Since the underlying native API is blocking, the
only way to run the hook in a non-blocking way is to run it on a separate thread, and all default implementations do
just that.

You can specify in the hook constructors whether `RunAsync` should create a background thread or not. Background threads
don't block the application from exiting if all other threads have finished executing. By default the created thread
will not be a background thread.

You can subscribe to events after the hook is started.

If you run the hook when it's already running, then an exception will be thrown. You can check whether a hook is running
using its `IsRunning` property.

`IGlobalHook` contains the `Stop` method to stop the global hook. After stopping, the global hook can be started again
by calling the `Run` or `RunAsync` method. Calling `Stop` when the hook is not running won't do anything.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's disposed and stopped if it was
running. Once a hook has been disposed, it cannot be started again – you'll have to create a new instance. Calling
`Dispose` when the hook is not running won't do anything other than marking the instance as disposed. You can check
whether the hook is disposed using the `IsDisposed` property.

The `HookEnabled` event is raised when the `Run` or `RunAsync` method is called. The `HookDisabled` event is raised when
the `Stop` or `Dispose` method is called.

Hook events are of type `HookEventArgs` or a derived type which contains more info. It's possible to suppress event
propagation by setting the `SuppressEvent` property to `true` inside the event handler. This must be done synchronously
and is only supported on Windows and macOS. You can check the event time and whether the event is real or simulated with
the `EventTime` and `IsEventSimulated` properties respectively.

> [!IMPORTANT]
> Always use one instance of `IGlobalHook` at a time in the entire application since they all must use the same static
> method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global hook
> when another global hook is already running will corrupt the internal global state of libuiohook.

You can create a keyboard-only or a mouse-only hook by passing a `GlobalHookType` to the hook's constructor. This makes
a real difference only on Windows where there are two different global hooks – a keyboard hook and a mouse hook. On
macOS and Linux, there is one hook for all events, and this simply enables filtering keyboard or mouse events out on
these OSes.

## The Default Implementations

SharpHook provides three implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.EventLoopGlobalHook` runs all of its event handlers on a separate dedicated thread. On backpressure it will
queue the remaining events which means that the hook will be able to process all events. This implementation should be
preferred to `SimpleGlobalHook` except for very simple use-cases. But it has a downside – suppressing event propagation
will be ignored since event handlers are run on another thread.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining events which
means that the hook will be able to process all events. This implementation should be preferred to `SimpleGlobalHook`
except for very simple use-cases. But it has a downside – suppressing event propagation will be ignored since event
handlers are run on other threads. In general, `EventLoopGlobalHook` should be preferred instead, as this class provides
benefits only if events should be processed in parallel, which is rarely the case.
