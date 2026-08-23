# SharpHook.ReactiveUI

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

If you're using [ReactiveUI.Primitives](https://github.com/reactiveui/Primitives), you can use the SharpHook.ReactiveUI
package to integrate SharpHook with it.

## Usage

### Basic Usage

Refer to the [SharpHook](https://www.nuget.org/packages/SharpHook) package for the basic usage guidelines.

### Reactive Global Hooks

The base SharpHook package provides the `IReactiveGlobalHook` interface.

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.ReactiveUI provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.ReactiveUI.ReactiveUIGlobalHook`. Since we're dealing with observables, it's up to you to decide when and
where to handle the events through sequencers. A default sequencer can be specified for all observables.

- `SharpHook.ReactiveUI.ReactiveUIGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default sequencer can be specified for all observables.

### Logging

The base SharpHook package provides the `IReactiveLogSource` interface.

SharpHook.ReactiveUI contains `ReactiveUILogSourceAdapter` which adapts an `ILogSource` into an `IReactiveLogSource`:

```csharp
using SharpHook.Logging;
using SharpHook.ReactiveUI.Logging;

var logSource = LogSource.RegisterOrGet();
var reactiveLogSource = new ReactiveUILogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### Rx.NET Integration

As an alternative, SharpHook also provides integration with [Rx.NET](https://github.com/dotnet/reactive) in the
[SharpHook.Reactive](https://www.nuget.org/packages/SharpHook.Reactive) package.

### R3 Integration

As an alternative, SharpHook also provides integration with [R3](https://github.com/Cysharp/R3) in the
[SharpHook.R3](https://www.nuget.org/packages/SharpHook.R3) package.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
