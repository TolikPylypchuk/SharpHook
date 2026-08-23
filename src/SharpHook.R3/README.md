# SharpHook.R3

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

If you're using [R3](https://github.com/Cysharp/R3), you can use the SharpHook.R3 package to integrate SharpHook with
it.

## Usage

### Basic Usage

Refer to the [SharpHook](https://www.nuget.org/packages/SharpHook) package for the basic usage guidelines.

### R3 Global Hooks

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

R3 global hooks are basically the same as the default global hooks and the same rules apply to them.

Note that SharpHook.R3 uses its own interface instead of implementing `IReactiveGlobalHook` since R3's implementation is
incompatible with the interface.

SharpHook.R3 provides two implementations of `IR3GlobalHook`:

- `SharpHook.R3.R3GlobalHook`. Since we're dealing with observables, it's up to you to decide when and where to handle
the events through time providers. A default time provider can be specified for all observables.

- `SharpHook.R3.R3GlobalHookAdapter` adapts an `IGlobalHook` to `IR3GlobalHook`. All subscriptions and changes are
propagated to the adapted hook. There is no default adapter from `IR3GlobalHook` to `IGlobalHook`. A default time
provider can be specified for all observables.

### Logging

SharpHook.R3 contains `IR3LogSource` and `R3LogSourceAdapter` so you can use them in a more reactive way:

```csharp
using SharpHook.Logging;
using SharpHook.R3.Logging;

var logSource = LogSource.RegisterOrGet();
var r3LogSource = new R3LogSourceAdapter(logSource);
r3LogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### Rx.NET Integration

As an alternative, SharpHook also provides integration with [Rx.NET](https://github.com/dotnet/reactive) in the
[SharpHook.Reactive](https://www.nuget.org/packages/SharpHook.Reactive) package.

### ReactiveUI.Primitives Integration

As an alternative, SharpHook also provides integration with
[ReactiveUI.Primitives](https://github.com/reactiveui/Primitives) in the
[SharpHook.ReactiveUI](https://www.nuget.org/packages/SharpHook.ReactiveUI) package.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
