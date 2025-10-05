# SharpHook.Reactive

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

If you're using [Rx.NET](https://github.com/dotnet/reactive), you can use the SharpHook.Reactive package to integrate
SharpHook with Rx.NET.

## Usage

### Basic Usage

Refer to the [SharpHook](https://www.nuget.org/packages/SharpHook) package for the basic usage guidelines.

### Reactive Global Hooks

SharpHook.Reactive provides the `IReactiveGlobalHook` interface along with a default implementation and an adapter
which you can use to use to control the global hook and subscribe to its observables. Here's a basic example:

```csharp
using SharpHook.Reactive;

// ...

var hook = new ReactiveGlobalHook();

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.Reactive.ReactiveGlobalHook`. Since we're dealing with observables, it's up to you to decide when and where
to handle the events through schedulers. A default scheduler can be specified for all observables.

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default scheduler can be specified for all observables.

### Logging

SharpHook.Reactive contains `IReactiveLogSource` and `ReactiveLogSourceAdapter` so you can use them in a more reactive
way:

```csharp
using SharpHook.Logging;
using SharpHook.Reactive.Logging;

// ...

var logSource = LogSource.RegisterOrGet();
var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### R3 Integration

As an alternative, SharpHook also provides integration with [R3](https://github.com/Cysharp/R3) in the
[SharpHook.R3](https://www.nuget.org/packages/SharpHook.R3) package.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
