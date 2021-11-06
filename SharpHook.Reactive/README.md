# SharpHook.Reactive

SharpHook is a library which provides a cross-platform global keyboard and mouse hook for .NET. It is a thin wrapper of
[libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as well as higher-level
types to work with it.

If you're using Rx.NET, you can use the SharpHook.Reactive package to integrate SharpHook with Rx.NET.

## Usage

### Basic Usage

Referer to the [SharpHook](https://www.nuget.org/packages/SharpHook) package for the basic usage guidelines.

### Reactive Global Hooks

SharpHook.Reactive provides the `IReactiveGlobalHook` interface along with a default implementation and an adapter
which you can use to use to control the hook and subscribe to its observables. Here's a basic example:

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SimpleReactiveGlobalHook` runs the hook on a separate thread. Since we are dealing with observables, it's up to
you to decide when and where to handle the events through schedulers.

- `ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All subscriptions and changes
are propagated to the adapted hook.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
