# Reactive Global Hooks

This article describes how to use reactive global hooks provided by SharpHook.

SharpHook provides the `SharpHook.Reactive.IReactiveGlobalHook` interface, but its implementations live in separate
packages.

## Rx.NET

If you're using [Rx.NET](https://github.com/dotnet/reactive), you can use the SharpHook.Reactive package to integrate
SharpHook with it.

SharpHook.Reactive provides an implementation of `IReactiveGlobalHook` which you can use to use to control the hook and
subscribe to its observables. Here's a basic example:

```csharp
using SharpHook.Reactive;

var hook = new ReactiveGlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);
hook.MouseMoved.Subscribe(OnMouseMoved);
hook.MouseMovedRelative.Subscribe(OnMouseMovedRelative);
hook.MouseDragged.Subscribe(OnMouseDragged);
hook.MouseDraggedRelative.Subscribe(OnMouseDraggedRelative);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// or
await hook.RunAsync();
```

The observables for events emit the `EventArgs`-derived types which are the same as in the events of `IGlobalHook`.

The `Run` and `RunAsync` methods are basically the same as in `IGlobalHook`. Running the hook when it's already running
is also not allowed, and the `IsRunning` property is also available.

The `Stop` method is also available and is basically the same as in `IGlobalHook`.

`IReactiveGlobalHook` extends `IDisposable` as well and calling `Dispose` will dispose of the global hook, stopping it
if it is running. As with `IGlobalHook`, starting a disposed instance again isn't allowed. Calling `Dispose` when the
hook is not running won't do anything other than marking the instance as disposed. All observables in the hook will
complete when the hook is disposed. The `IsDisposed` property is also available.

> [!IMPORTANT]
> Always use one instance of `IReactiveGlobalHook` at a time in the entire application since they all must use the same
> static method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global
> hook when another global hook is already running will corrupt the internal global state of libuiohook.

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.Reactive.ReactiveGlobalHook`. Since we're dealing with observables, it's up to you to decide when and where
to handle the events through schedulers. A default scheduler can be specified for all observables.

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default scheduler can be specified for all observables.

#### ReactiveUI.Primitives

If you're using [ReactiveUI.Primitives](https://github.com/reactiveui/Primitives), you can use the SharpHook.ReactiveUI
package to integrate SharpHook with it.

SharpHook.ReactiveUI provides an implementation of `IReactiveGlobalHook` which you can use to use to control the hook
and subscribe to its observables. Here's a basic example:

```csharp
using SharpHook.ReactiveUI;

var hook = new ReactiveUIGlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);
hook.MouseMoved.Subscribe(OnMouseMoved);
hook.MouseMovedRelative.Subscribe(OnMouseMovedRelative);
hook.MouseDragged.Subscribe(OnMouseDragged);
hook.MouseDraggedRelative.Subscribe(OnMouseDraggedRelative);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// or
await hook.RunAsync();
```

Note that SharpHook.R3 uses its own interface instead of implementing `IReactiveGlobalHook` since R3's implementation is
incompatible with the interface.

The observables for events emit the `EventArgs`-derived types which are the same as in the events of `IGlobalHook`.

The `Run` and `RunAsync` methods are basically the same as in `IGlobalHook`. Running the hook when it's already running
is also not allowed, and the `IsRunning` property is also available.

The `Stop` method is also available and is basically the same as in `IGlobalHook`.

`IReactiveGlobalHook` extends `IDisposable` as well and calling `Dispose` will dispose of the global hook, stopping it
if it is running. As with `IGlobalHook`, starting a disposed instance again isn't allowed. Calling `Dispose` when the
hook is not running won't do anything other than marking the instance as disposed. All observables in the hook will
complete when the hook is disposed. The `IsDisposed` property is also available.

> [!IMPORTANT]
> Always use one instance of `IReactiveGlobalHook` at a time in the entire application since they all must use the same
> static method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global
> hook when another global hook is already running will corrupt the internal global state of libuiohook.

SharpHook.ReactiveUI provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.ReactiveUI.ReactiveUIGlobalHook`. Since we're dealing with observables, it's up to you to decide when and
where to handle the events through sequencers. A default sequencer can be specified for all observables.

- `SharpHook.ReactiveUI.ReactiveUIGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default sequencer can be specified for all observables.

## R3

If you're using [R3](https://github.com/Cysharp/R3), you can use the SharpHook.R3 package to integrate SharpHook with
it.

SharpHook.R3 provides the `SharpHook.R3.IR3GlobalHook` interface along with a default implementation which you can use
to use to control the hook and subscribe to its observables. Here's a basic example:

```csharp
using SharpHook.R3;

var hook = new R3GlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);
hook.MouseMoved.Subscribe(OnMouseMoved);
hook.MouseMovedRelative.Subscribe(OnMouseMovedRelative);
hook.MouseDragged.Subscribe(OnMouseDragged);
hook.MouseDraggedRelative.Subscribe(OnMouseDraggedRelative);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// or
await hook.RunAsync();
```

The observables for events emit the `EventArgs`-derived types which are the same as in the events of `IGlobalHook`.

The `Run` and `RunAsync` methods are basically the same as in `IGlobalHook`. Running the hook when it's already running
is also not allowed, and the `IsRunning` property is also available.

The `Stop` method is also available and is basically the same as in `IGlobalHook`.

`IR3GlobalHook` extends `IDisposable` as well and calling `Dispose` will dispose of the global hook, stopping it if it
is running. As with `IGlobalHook`, starting a disposed instance again isn't allowed. Calling `Dispose` when the hook is
not running won't do anything other than marking the instance as disposed. All observables in the hook will complete
when the hook is disposed. The `IsDisposed` property is also available.

> [!IMPORTANT]
> Always use one instance of `IR3GlobalHook` at a time in the entire application since they all must use the same static
> method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global hook
> when another global hook is already running will corrupt the internal global state of libuiohook.

SharpHook.R3 provides two implementations of `IR3GlobalHook`:

- `SharpHook.R3.R3GlobalHook`. Since we're dealing with observables, it's up to you to decide when and where to handle
the events through time providers. A default time provider can be specified for all observables.

- `SharpHook.R3.R3GlobalHookAdapter` adapts an `IGlobalHook` to `IR3GlobalHook`. All subscriptions and changes are
propagated to the adapted hook. A default time provider can be specified for all observables. There is no default
adapter from `IR3GlobalHook` to `IGlobalHook`.
