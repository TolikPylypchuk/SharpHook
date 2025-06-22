# SharpHook

[![NuGet](https://img.shields.io/nuget/v/SharpHook.svg?label=SharpHook)](https://www.nuget.org/packages/SharpHook)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.Reactive.svg?label=SharpHook.Reactive)](https://www.nuget.org/packages/SharpHook.Reactive)

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

## Installation

```
dotnet add package SharpHook
dotnet add package SharpHook.Reactive
```

## Upgrading

A [migration guide](https://sharphook.tolik.io/articles/migration.html) is available for upgrading between major
versions.

## Docs

You can find more information (including the API reference) in the docs at
[https://sharphook.tolik.io](https://sharphook.tolik.io).

## Supported Platforms

SharpHook targets .NET 8+, .NET Framework 4.7.2+, and .NET Standard 2.0. The following table describes
the availability of SharpHook on various platforms:

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
    <td>No</td>
  </tr>
  <tr>
    <th>x64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm32</th>
    <td>No</td>
    <td>N/A</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
</table>

Platform support notes:

- Windows 10/11 is supported. Support for Windows on Arm32 was removed in version 5.0 since it was
[removed in .NET 5](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md).

- macOS 10.15+ is supported. Mac Catalyst 13.1+ is also supported. macOS requires that the access to the Accessibility
API be enabled for the application if it wants to create a global hook or simulate events.

- Linux distributions supported by .NET are supported by SharpHook. Linux on x86 is
[not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself. Only X11 is supported - Wayland support
[may be coming](https://github.com/kwhat/libuiohook/issues/196), but it's not yet here.

More info on OS support can be found in
[an article on OS-specific constraints](https://sharphook.tolik.io/articles/os-constraints.html).

## Usage

### Native Functions of libuiohook

SharpHook exposes the functions of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Data` namespace
contains types which represent the data used by libuiohook.

In general, you don't need to use the native methods directly. Instead, use the higher-level interfaces and classes
provided by SharpHook. However, you should still read this section to know how the high-level features work under
the hood.

If you want to use the low-level functionality, you don't need to use the `UioHook` class directly. Instead you can use
interfaces in the `SharpHook.Providers` namespace. The methods in those interfaces are the same as in the `UioHook`
class. `SharpHook.Providers.UioHookProvider` implements all of these interfaces and simply calls the corresponding
methods in `UioHook`. This should be done to decouple your code from `UioHook` and make testing easier.

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.
- `Run` - creates a keyboard and mouse global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunKeyboard` - creates a keyboard-only global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunMouse` - creates a mouse-only global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` - destroys the global hook.

> [!IMPORTANT]
> You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
> previously set one.

Additionally, `UioHook` contains the `PostEvent` method for simulating input events.

`UioHook` also contains the `PostText` method which simulates text entry. The text to simulate doesn't depend on the
current keyboard layout. The full range of UTF-16 (including surrogate pairs, e.g. emojis) is supported.

libuiohook also provides functions to get various system properties. The corresponding methods are also present in
`UioHook`.

### Global Hooks

SharpHook provides the `IGlobalHook` interface along with two default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```c#
using SharpHook;

// ...

var hook = new TaskPoolGlobalHook();

hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>

hook.KeyTyped += OnKeyTyped;           // EventHandler<KeyboardHookEventArgs>
hook.KeyPressed += OnKeyPressed;       // EventHandler<KeyboardHookEventArgs>
hook.KeyReleased += OnKeyReleased;     // EventHandler<KeyboardHookEventArgs>

hook.MouseClicked += OnMouseClicked;   // EventHandler<MouseHookEventArgs>
hook.MousePressed += OnMousePressed;   // EventHandler<MouseHookEventArgs>
hook.MouseReleased += OnMouseReleased; // EventHandler<MouseHookEventArgs>
hook.MouseMoved += OnMouseMoved;       // EventHandler<MouseHookEventArgs>
hook.MouseDragged += OnMouseDragged;   // EventHandler<MouseHookEventArgs>

hook.MouseWheel += OnMouseWheel;       // EventHandler<MouseWheelHookEventArgs>

hook.Run();
// or
await hook.RunAsync();
```

First, you create the hook, then subscribe to its events, and then run it. The `Run` method runs the hook on the current
thread, blocking it. The `RunAsync()` method runs the hook on a separate thread and returns a `Task` which is finished
when the hook is stopped. You can subscribe to events after the hook is started.

`IGlobalHook` contains the `Stop` method to stop the global hook. After stopping, the global hook can be started again
by calling the `Run` or `RunAsync` method. Calling `Stop` when the hook is not running won't do anything.

`IGlobalHook` extends `IDisposable`. When you call the `Dispose` method on a hook, it's disposed and stopped if it was
running. Once a hook has been disposed, it cannot be started again - you'll have to create a new instance. Calling
`Dispose` when the hook is not running won't do anything other than marking the instance as disposed.

Hook events are of type `HookEventArgs` or a derived type which contains more info. It's possible to suppress event
propagation by setting the `SuppressEvent` property to `true` inside the event handler. This must be done synchronously
and is only supported on Windows and macOS. You can check the event time and whether the event is real or simulated with
the `EventTime` and `IsEventSimulated` properties respectively.

> [!IMPORTANT]
> Always use one instance of `IGlobalHook` at a time in the entire application since they all must use the same static
> method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global hook
> when another global hook is already running will corrupt the internal global state of libuiohook.

You can create a keyboard-only or a mouse-only hook by passing a `GlobalHookType` to the hook's constructor. This makes
a real difference only on Windows where there are two different global hooks - a keyboard hook and a mouse hook. On
macOS and Linux, there is one hook for all events, and this simply enables filtering keyboard or mouse events out on
these OSes.

SharpHook provides two implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining handlers.
This means that the hook will be able to process all events. This implementation should be preferred to
`SimpleGlobalHook` except for very simple use-cases. But it has a downside - suppressing event propagation will be
ignored since event handlers are run on other threads.

The library also provides the `SharpHook.GlobalHookBase` class which you can extend to create your own implementation
of the global hook. It calls the appropriate event handlers, and you only need to implement a strategy for dispatching
the events. It also keeps a reference to a running global hook so that it's not garbage-collected.

### Reactive Global Hooks

If you're using Rx.NET, you can use the SharpHook.Reactive package to integrate SharpHook with Rx.NET.

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.Reactive provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.Reactive.SimpleReactiveGlobalHook`. Since we're dealing with observables, it's up to you to decide when
and where to handle the events through schedulers. A default scheduler can be specified for all observables.

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default scheduler can be specified for all observables.

### Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. Here's a quick
example:

```c#
using SharpHook;
using SharpHook.Native;

// ...

var simulator = new EventSimulator();

// Press Ctrl+C
simulator.SimulateKeyPress(KeyCode.VcLeftControl);
simulator.SimulateKeyPress(KeyCode.VcC);

// Release Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC);
simulator.SimulateKeyRelease(KeyCode.VcLeftControl);

// Press the left mouse button
simulator.SimulateMousePress(MouseButton.Button1);

// Release the left mouse button
simulator.SimulateMouseRelease(MouseButton.Button1);

// Press the left mouse button at (0, 0)
simulator.SimulateMousePress(0, 0, MouseButton.Button1);

// Release the left mouse button at (0, 0)
simulator.SimulateMouseRelease(0, 0, MouseButton.Button1);

// Move the mouse pointer to (0, 0)
simulator.SimulateMouseMovement(0, 0);

// Move the mouse pointer 50 pixels to the right and 100 pixels down
simulator.SimulateMouseMovementRelative(50, 100);

// Scroll the mouse wheel
simulator.SimulateMouseWheel(
    rotation: -120,
    direction: MouseWheelScrollDirection.Vertical, // Vertical by default
    type: MouseWheelScrollType.UnitScroll); // UnitScroll by default
```

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events.

### Text Entry Simulation

SharpHook also provides text entry simulation. `IEventSimulator` contains the `SimulateTextEntry` method which accepts
a `string`. The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
surrogate pairs, e.g., emojis) is supported.

### Logging

libuiohook can log messages throughout its execution. By default the messages are not logged anywhere, but you can get
these logs by using the `ILogSource` interface and its default implementation, `LogSource`:

```c#
using SharpHook.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

As with global hooks, you should use only one `LogSource` object at a time. `ILogSource` extends `IDisposable` - you
can dispose of a log source to stop receiving libuiohook messages.

An `EmptyLogSource` class is also available - this class doesn't listen to the libuiohook logs and can be used instead
of `LogSource` in release builds.

SharpHook.Reactive contains the `IReactiveLogSource` and `ReactiveLogSourceAdapter` so you can use them in a more
reactive way:

```C#
using SharpHook.Logging;
using SharpHook.Reactive.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### Testing

SharpHook provides two classes which make testing easier. They aren't required since mocks can be used instead, but
unlike mocks, no setup is required to use these classes.

`SharpHook.Testing.TestGlobalHook` provides an implementation of `IGlobalHook` and `IEventSimulator` which can be used
for testing. When the `Run` or `RunAsync` method is called, it will dispatch events using the various `Simulate` methods
from `IEventSimulator`.

If this class is used as an `IEventSimulator` in the tested code, then the `SimulatedEvents` property can be checked to
see which events were simulated using the test instance.

If an `IReactiveGlobalHook` is needed for testing, then `ReactiveGlobalHookAdapter` can be used to adapt an instance of
`TestGlobalHook`.

If the low-level functionality of SharpHook should be mocked, or mocking should be pushed as far away as possible,
then `SharpHook.Testing.TestProvider` can be used. It implements every interface in the `SharpHook.Providers` namespace
and as such it can be used instead of a normal low-level functionality provider.

Like `TestGlobalHook`, this class can post events using the `PostEvent` method and dispatch them if `Run` was called.
It also contains the `PostedEvents` property.

## Library Status

If you've noticed that this library hasn't gotten new commits in some time, rest assured that it's not abandoned!
I'm not giving up on this library any time soon.

## Building from Source

In order to build this library, you'll first need to get libuiohook binaries. You you can get a
[nightly build from this repository](https://github.com/TolikPylypchuk/SharpHook/actions/workflows/build.yml), or you
can build them yourself as instructed in the [libuiohook fork](https://github.com/TolikPylypchuk/libuiohook) that
SharpHook uses (not recommended as it's non-trivial, and you should most probably use the same options that the build in
this repository uses anyway).

Place the binaries into the appropriate directories in the `SharpHook` project, as described in the following table:

<table>
  <tr>
    <th>OS</th>
    <th>File</th>
    <th>Source directory</th>
    <th>Target directory</th>
  </tr>
  <tr>
    <th>Windows</th>
    <td>uiohook.dll</td>
    <td>windows/&lt;platform&gt;/bin</td>
    <td>lib/win-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>macOS</th>
    <td>libuiohook.dylib</td>
    <td>macos/&lt;platform&gt;/lib</td>
    <td>lib/osx-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>Mac Catalyst</th>
    <td>libuiohook.dylib</td>
    <td>catalyst/&lt;platform&gt;/lib</td>
    <td>lib/maccatalyst-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>Linux</th>
    <td>libuiohook.so</td>
    <td>linux/&lt;platform&gt;/lib</td>
    <td>lib/linux-&lt;platform&gt;</td>
  </tr>
</table>

With libuiohook in place you can build SharpHook using your usual methods, e.g. with Visual Studio or the `dotnet` CLI.
You need .NET 9 to build SharpHook.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
