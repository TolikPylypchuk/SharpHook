# Reactive Global Hooks

If you're using Rx.NET, you can use the `SharpHook.Reactive` package to integrate SharpHook with Rx.NET. Using this
package is preferred when possible since it's more powerful.

## The Interface

SharpHook.Reactive provides the `SharpHook.Reactive.IReactiveGlobalHook` interface along with a default
implementation which you can use to use to control the hook and subscribe to its observables. Here's a basic example:

```C#
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

await hook.Start();
```

The observables for events have the type `HookEvent<TArgs>` where `TArgs` is the same type as for the events in
`IGlobalHook`. `HookEvent` simply contains the sender of the event (which is usually the `IReactiveGlobalHook`
itself, but can be anything), as well as the event arguments.

The `HookEnabled` and `HookDisabled` observables will emit a single event and then immediately complete afterwards.

The `Start` method is basically the same as in `IGlobalHook`, but returns `IObservable<Unit>` instead of a `Task`.
This observable will emit a single value and then complete when you destroy the global hook.

`IReactiveGlobalHook` also extends `IDisposable` and calling `Dispose` will destroy the global hook. As with
`IGlobalHook`, starting a disposed instance again shouldn't be allowed. Calling `Dispose` when the hook is not running
is safe - it just won't do anything (other than marking the instance as disposed).

## The Default Implementations

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.Reactive.SimpleReactiveGlobalHook` runs the hook on a separate thread. Since we are dealing with
observables, it's up to you to decide when and where to handle the events through schedulers.

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`.

Events emitted by `SimpleReactiveGlobalHook` will have `IReactiveGlobalHook` itself as the sender, while events
emitted by `ReactiveGlobalHookAdapter` will have the adapted `IGlobalHook` as the sender.

Both classes contain a destructor which will stop the global hook if the object is not reachable anymore. All event
observables of both classes will be completed when the hook is destroyed, be it from the `Dispose` method, or from the
destructor.

Next article: [Limitations](limitations.md).
