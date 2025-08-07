# SharpHook.R3

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

If you're using [R3](https://github.com/Cysharp/R3), you can use the SharpHook.R3 package to integrate SharpHook with
R3.

## Usage

### Basic Usage

Refer to the [SharpHook](https://www.nuget.org/packages/SharpHook) package for the basic usage guidelines.

### R3 Global Hooks

SharpHook.R3 provides the `IR3GlobalHook` interface along with a default implementation and an adapter which you can use
to use to control the global hook and subscribe to its observables. Here's a basic example:

```c#
using SharpHook.R3;

// ...

var hook = new SimpleR3GlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);

hook.MouseMoved
    .Debouce(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseMoved);

hook.MouseDragged
    .Debouce(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseDragged);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// or
await hook.RunAsync();
```

R3 global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.R3 provides two implementations of `IR3GlobalHook`:

- `SharpHook.R3.SimpleR3GlobalHook`. Since we're dealing with observables, it's up to you to decide when and where to
handle the events through time providers. A default time provider can be specified for all observables.

- `SharpHook.R3.R3GlobalHookAdapter` adapts an `IGlobalHook` to `IR3GlobalHook`. All subscriptions and changes are
propagated to the adapted hook. There is no default adapter from `IR3GlobalHook` to `IGlobalHook`. A default time
provider can be specified for all observables.

### Logging

SharpHook.R3 contains `IR3LogSource` and `R3LogSourceAdapter` so you can use them in a more reactive way:

```c#
using SharpHook.Logging;
using SharpHook.R3.Logging;

// ...

var logSource = LogSource.RegisterOrGet();
var r3LogSource = new R3LogSourceAdapter(logSource);
r3LogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### Rx.NET Integration

As an alternative, SharpHook also provides integration with [Rx.NET](https://github.com/dotnet/reactive) in the
[SharpHook.Reactive](https://www.nuget.org/packages/SharpHook.Reactive) package.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
