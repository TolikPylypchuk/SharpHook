# SharpHook.Reactive

SharpHook is a library which provides a cross-platform global keyboard and mouse hook for .NET. It is a thin wrapper of
[libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as well as a higher-level
interface and classes to work with it.

## Supported Platforms

<table>
  <tr>
    <th></th>
    <th>Windows</th>
    <th>macOS</th>
    <th>Linux</th>
  </tr>
  <tr>
    <th>x86</th>
    <td>Yes</td>
    <td>N/A</td>
    <td>No<sup>1</sup></td>
  </tr>
  <tr>
    <th>x64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm32</th>
    <td>Yes<sup>2</sup></td>
    <td>N/A</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm64</th>
    <td>No<sup>3</sup></td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
</table>

[1] - Linux on x86 is [not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself.

[2] - Windows Arm32 support was
[dropped](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md) in .NET 5 so it will most
probably be dropped by this library in a future version as well.

[3] - Windows on Arm64 is not yet supported by libuiohook.

libuiohook only supports X11 on Linux. Wayland support [may be coming](https://github.com/kwhat/libuiohook/issues/100),
but it's not yet here.

## Usage

### Basic Usage

Referer to the `SharpHook` package README for the basic usage guidelines.

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

## Limitations

You have to remember that libuiohook binaries should be present in the curent working directory. This is how P/Invoke
works, and it can cause the 'library not found' issues when running your client app from a different directory.

Another thing is that libuiohook supports hooking into its logging capabilities so that you can get its logs. This
library doesn't support this. The reason is that you should call `hook_set_logger_proc` and pass your callback for
logging. This is similar to `hook_set_dispatch_proc`, but this time the callback should accept a variable number of
arguments (using C's `...` syntax) and the client decides how to format the log message. Supporting native
variable arguments in callbacks is next to impossible in C#, and the payoff is not worth spending a lot of effort to
implement this feature.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
