# Global Hooks

This article describes how to use global hooks provided by SharpHook.

## The Interface

SharpHook provides the `IGlobalHook` interface along with three default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```csharp
using SharpHook;

var hook = new EventLoopGlobalHook();

// Events of type EventHandler<HookEventArgs>:
hook.HookEnabled += OnHookEnabled;
hook.HookDisabled += OnHookDisabled;

// Events of type EventHandler<KeyboardHookEventArgs>:
hook.KeyTyped += OnKeyTyped; // Disabled by default
hook.KeyPressed += OnKeyPressed;
hook.KeyReleased += OnKeyReleased;

// Events of type EventHandler<MouseHookEventArgs>:
hook.MouseClicked += OnMouseClicked;
hook.MousePressed += OnMousePressed;
hook.MouseReleased += OnMouseReleased;
hook.MouseMoved += OnMouseMoved;
hook.MouseMovedRelative += OnMouseMovedRelative;
hook.MouseDragged += OnMouseDragged;
hook.MouseDraggedRelative += OnMouseDraggedRelative;

// Events of type EventHandler<MouseWheelHookEventArgs>:
hook.MouseWheel += OnMouseWheel;

hook.Run();
// or
await hook.RunAsync();
```

`IGlobalHook` contains separate events for every event type that can be raised by libuiohook. The sender of these
events is the `IGlobalHook` itself.

It also contains the `Run` and `RunAsync` methods which run the global hook. `Run` runs it on the current thread,
blocking it until the global hook is stopped. `RunAsync` runs the global hook in a non-blocking way and returns a
`Task` – this task is finished when the hook is stopped or disposed. Since the underlying native API is blocking, the
only way to run the hook in a non-blocking way is to run it on a separate thread, and all default implementations do
just that.

You can specify whether `RunAsync` should create a background thread or not. Background threads don't block the
application from exiting if all other threads have finished executing. By default, the created thread will not be a
background thread.

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

You can create a keyboard-only or a mouse-only hook by passing a `GlobalHookType` to `Run` or `RunAsync`. On Windows,
there are two different global hooks – a keyboard hook and a mouse hook. On macOS and Linux, there is one hook for all
events, and this enables filtering keyboard or mouse events out on these OSes, though on Wayland, the connection to the
compositor will be established only if mouse events are enabled since they need to know the screen size and bounds.

## The Default Implementations

SharpHook provides three implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.EventLoopGlobalHook` runs all of its event handlers on a separate dedicated thread. On backpressure it will
queue the remaining events which means that the hook will be able to process all events. This implementation should be
preferred to `SimpleGlobalHook` except for very simple use-cases. But it has a downside – suppressing event propagation
will be ignored since event handlers are executed on another thread.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining events which
means that the hook will be able to process all events. Like with `EventLoopGlobalHook`, suppressing event propagation
will be ignored since event handlers are executed on other threads. In general, `EventLoopGlobalHook` should be
preferred instead, as this class provides benefits only if events should be processed in parallel, which is rarely the
case.

## Mouse Events on Wayland

On Wayland, absolute mouse position is usually not available, so you should subscribe to the `MouseMovedRelative` and
`MouseDraggedRelative` in addition to `MouseMoved` and `MouseDragged` if you want to listen to mouse movement. Mice are
relative pointer devices, so they only raise relative motion events. Absolute motion events will only be raised by
absolute pointer devices like touchscreens, or in virtual machines.

On other platforms, relative motion events are never raised.

Also, events of type `MousePressed`, `MouseReleased`, `MouseClicked`, and `MouseWheel` don't include mouse coordinates –
they are always set to (0, 0).

You can query support for absolute mouse coordinates with `IFeatureProvider.GetOptionalFeatureSupport` – if it returns
`AbsoluteMouseMovement` as one of the supported features, then mouse motion events will always be absolute. If not, then
you should subscribe to `MouseMovedRelative` and `MouseDraggedRelative`. If it returns `AbsoluteMouseButtonCoordinates`
as one of the supported features, then the cursor position will be included in `MousePressed`, `MouseReleased`,
`MouseClicked`, and `MouseWheel` events. If not, then the coordinates will always be set to (0, 0).
