# Reactive Global Hooks

If you're using Rx.NET, you can use the SharpHook.Reactive package to integrate SharpHook with Rx.NET. Using this
package is preferred when possible since it's more powerful.

## The Interface

SharpHook.Reactive provides the `SharpHook.Reactive.IReactiveGlobalHook` interface along with a default implementation
which you can use to use to control the hook and subscribe to its observables. Here's a basic example:

```c#
using SharpHook.Reactive;

// ...

var hook = new SimpleReactiveGlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);

hook.MouseMoved
    .Throttle(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseMoved);

hook.MouseDragged
    .Throttle(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseDragged);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// or
hook.RunAsync().Subscribe();
```

The observables for events emit the `EventArgs`-derived types which are the same as in the events of `IGlobalHook`.
Unlike `IGlobalHook` you can't get the sender of the event by default. If you need to pass the hook itself as well, then
use a closure.

The `Run` and `RunAsync` methods are basically the same as in `IGlobalHook`, but `RunAsync` returns an
`IObservable<Unit>` instead of a `Task` (the observable is hot). This observable will emit a single value and then
complete when the global hook is destroyed. Running the hook when it's already running is also not allowed, and the
`IsRunning` property is also available.

The `Stop` method is also available and is basically the same as in `IGlobalHook`.

`IReactiveGlobalHook` extends `IDisposable` as well and calling `Dispose` will dispose of the global hook, stopping it
if it is running. As with `IGlobalHook`, starting a disposed instance again isn't allowed. Calling `Dispose` when the
hook is not running won't do anything other than marking the instance as disposed. All observables in the hook will
complete when the hook is disposed. The `IsDisposed` property is also available.

> [!IMPORTANT]
> Always use one instance of `IReactiveGlobalHook` at a time in the entire application since they all must use the same
> static method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global
> hook when another global hook is already running will corrupt the internal global state of libuiohook.

## The Default Implementations

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.Reactive.SimpleReactiveGlobalHook`. Since we're dealing with observables, it's up to you to decide when
and where to handle the events through schedulers. A default scheduler can be specified for all observables.

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. A default scheduler can be specified for all observables.
There is no default adapter from `IReactiveGlobalHook` to `IGlobalHook`.

`SimpleReactiveGlobalHook` keeps a reference to a running global hook so that it's not garbage-collected.
