# Global Hooks

## The Interface

SharpHook provides the `SharpHook.IGlobalHook` interface along with two default implementations which you can use
to control the hook and subscribe to its events. This way is preferred to using native functions since it's more
convenient. Here's a basic usage example:

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

`IGlobalHook` contains separate events for every event type that can be raised by libuiohook. The sender of these
events is the `IGlobalHook` itself.

It also contains the `Run` and `RunAsync` methods which, well, run the global hook. `Run` runs it on the current thread,
blocking it until the global hook is disposed. `RunAsync` runs the global hook in a non-blocking way and returns a
`Task` - this task is finished when the hook is destroyed. Since the underlying native API is blocking, the only way to
run the hook in a non-blocking way is to run it on a separate thread, and all default implementations do just that.

You can subscribe to events after the hook is started.

If you run the hook when it's already running, then an exception will be thrown. You can check whether a hook is running
using its `IsRunning` property.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's destroyed. The contract of
the interface is that once a hook has been destroyed, it cannot be started again - you'll have to create a new instance.
Calling `Dispose` when the hook is not running is safe - it just won't do anything (other than marking the instance as
disposed). You can check whether the hook is disposed using the `IsDisposed` property.

**Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use
the same static method to set the hook callback for libuiohook, and there may only be one callback at a time.

## The Default Implementations

SharpHook provides two implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining handlers.
This means that the hook will be able to process all events. This implementation should be preferred to
`SimpleGlobalHook` except for very simple use-cases. But it has a downside - setting the `Reserved` field to suppress
event propagation will be ignored since event handlers are run on other threads.

The library also provides the `SharpHook.GlobalHookBase` class which you can extend to create your own implementation
of the global hook. It calls the appropriate event handlers, and you only need to implement a strategy for dispatching
the events. It also contains a destructor which will stop the global hook if this object is not reachable anymore.

Next article: [Reactive Global Hooks](reactive.md).
