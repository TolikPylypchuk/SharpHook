# SharpHook

SharpHook: [![NuGet](https://img.shields.io/nuget/v/SharpHook.svg)](https://www.nuget.org/packages/SharpHook)  
SharpHook.Reactive: [![NuGet](https://img.shields.io/nuget/v/SharpHook.Reactive.svg)](https://www.nuget.org/packages/SharpHook.Reactive)

SharpHook is a library which provides a cross-platform global keyboard and mouse hook for .NET. It is a thin wrapper of
[libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as well as a higher-level
interface and classes to work with it.

## Installation

```
dotnet add package SharpHook --version 1.0.0-preview.1
dotnet add package SharpHook.Reactive --version 1.0.0-preview.1
```

## Docs

You can find more information (including the API reference) in the [docs](https://sharphook.tolik.io).

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

### Native Methods of libuiohook

SharpHook exposes the methods of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains structs and enums which represent the data returned by libuiohook.

**Note**: In general, you shouldn't use native methods directly. Instead, use the higher-level interface and classes
provided by SharpHook.

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.

- `Run` - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.

- `Stop` - destroys the global hook.

libuiohook also provides functions to get various system properties. The corresponding methods are also present in
`UioHook`.

### Default Global Hooks

SharpHook provides the `IGlobalHook` interface along with two default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```C#
using SharpHook;

// ...

var hook = new TaskPoolGlobalHook();

hook.HookEnabled += OnHookEnabled;
hook.HookDisabled += OnHookDisabled;

hook.KeyTyped += OnKeyTyped;
hook.KeyPressed += OnKeyPressed;
hook.KeyReleased += OnKeyReleased;

hook.MouseClicked += OnMouseClicked;
hook.MousePressed += OnMousePressed;
hook.MouseReleased += OnMouseReleased;
hook.MouseMoved += OnMouseMoved;
hook.MouseDragged += OnMouseDragged;

hook.MouseWheel += OnMouseWheel;

await hook.Start();
```

First, you create the hook, then subscribe to its events, and then start it. The `Start()` method returns a `Task`
which is finished when the hook is destroyed, so if you `await` it, you block the current async context until it stops.
You can subscribe to events after the hook is started.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's destroyed. The contract of
the interface is that once a hook has been destroyed, it cannot be started again - you'll have to create a new instance.

**Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use
the same static method to set the hook callback for libuiohook, and there may only be one callback at a time.

SharpHook provides two implementations of `IGlobalHook`:

- `SimpleGlobalHook` runs the hook on a separate thread, and runs all of its event handlers on that same thread. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `TaskPoolGlobalHook` runs the hook on a separate thread, and runs all of its event handlers on other threads inside
the default thread pool for tasks. The parallelism level of the handlers can be configured. On backpressure it will
queue the remaining handlers. This means that the hook will be able to process all events. This implementation should be
preferred to `SimpleGlobalHook` except for very simple use-cases.

The library also provides the `GlobalHookBase` class which you can extend to create your own implementation of the
global hook. It runs the hook on a separate thread and calls appropriate event handlers. You only need to implement a
strategy for dispatching the events.

### Reactive Global Hooks

If you're using Rx.NET, you can use the `SharpHook.Reactive` package to integrate SharpHook with Rx.NET.

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

## Building from Source

In order to build this library, you'll first need to get libuiohook binaries. You can build them yourself as instructued
in the [libuiohook](https://github.com/kwhat/libuiohook) repository, or you can get a
[nightly build](https://github.com/kwhat/libuiohook/actions/workflows/package.yml). Place the binaries into the
appropriate directories under the `lib` directory in the `SharpHook` project.

With libuiohook in place you can build SharpHook using your usual methods, e.g. with Visual Studio or the `dotnet` CLI.
Mind that you need .NET 6 and Visual Studio 2022 to build SharpHook.

The `SharpHook` project defines multiple platforms. If you want to run `SharpHook.Sample`, make sure you don't use
`AnyCPU` since the libuiohook version for it is not defined.

## Library Status

SharpHook is under active development. The code itself is basically done at this point, and I'm wating for the .NET 6 GA
release. When it's out, I'm going to release version 1.0.0.

No other features are planned, but I will maintain the library to keep up with the releases of libuiohook which uses a
rolling release model - every commit to its `1.2` branch is considered stable. That said, libuiohook itself is not
really active anymore, but is still maintained, so this library will most probably receive updates quite rarely.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
