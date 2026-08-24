# SharpHook

[![NuGet](https://img.shields.io/nuget/v/SharpHook.svg?label=SharpHook)](https://www.nuget.org/packages/SharpHook)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.Reactive.svg?label=SharpHook.Reactive)](https://www.nuget.org/packages/SharpHook.Reactive)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.ReactiveUI.svg?label=SharpHook.ReactiveUI)](https://www.nuget.org/packages/SharpHook.ReactiveUI)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.R3.svg?label=SharpHook.R3)](https://www.nuget.org/packages/SharpHook.R3)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.Testing.svg?label=SharpHook.Testing)](https://www.nuget.org/packages/SharpHook.Testing)

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

## Installation

```
dotnet add package SharpHook
dotnet add package SharpHook.Reactive
dotnet add package SharpHook.ReactiveUI
dotnet add package SharpHook.R3
dotnet add package SharpHook.Testing
```

## Upgrading

A [migration guide](https://sharphook.tolik.io/articles/migration.html) is available for upgrading between major
versions.

## Docs

You can find more information (including the API reference) in the docs at
[https://sharphook.tolik.io](https://sharphook.tolik.io).

## Supported Platforms

SharpHook targets .NET 10+, .NET Framework 4.7.2+, and .NET Standard 2.0. The following table describes
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

- Windows 11 is supported, though SharpHook should work on older versions of Windows as well. Support for Windows on
Arm32 was removed in version 5.0 since it was
[removed in .NET 5](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md).

- macOS 10.15+ is required, though only the currently supported macOS versions are supported by SharpHook. Mac Catalyst
is also supported, and version 13.1+ is required. macOS requires that the access to the Accessibility API be enabled for
the application if it wants to create a global hook or simulate events.

- Linux distributions supported by .NET are supported by SharpHook. Linux on x86 is
[not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself. Both X11 and Wayland are supported, but
Wayland support lacks several features and requires elevated privileges. On Linux, multiple libuiohook backends are
present and the backend choice can be customized.

More info on OS support can be found in
[an article on OS-specific constraints](https://sharphook.tolik.io/articles/os-constraints.html).

## Usage

### Global Hooks

SharpHook provides the `IGlobalHook` interface along with three default implementations which you can use to control the
hook and subscribe to its events. Here's a basic usage example:

```csharp
using SharpHook;

var hook = new EventLoopGlobalHook();

// Events of type EventHandler<HookEventArgs>:
hook.HookEnabled += OnHookEnabled;
hook.HookDisabled += OnHookDisabled;

// Events of type EventHandler<KeyboardHookEventArgs>:
hook.KeyTyped += OnKeyTyped; // Disabled by default
hook.KeyPressed += OnKeyPressed;
hook.KeyReleased += OnKeyReleased;

// Events of type EventHandler<MouseHookEventArgs>:
hook.MouseClicked += OnMouseClicked;
hook.MousePressed += OnMousePressed;
hook.MouseReleased += OnMouseReleased;
hook.MouseMoved += OnMouseMoved;
hook.MouseMovedRelative += OnMouseMovedRelative;
hook.MouseDragged += OnMouseDragged;
hook.MouseDraggedRelative += OnMouseDraggedRelative;

// Events of type EventHandler<MouseWheelHookEventArgs>:
hook.MouseWheel += OnMouseWheel;

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
running. Once a hook has been disposed, it cannot be started again – you'll have to create a new instance. Calling
`Dispose` when the hook is not running won't do anything other than marking the instance as disposed.

Hook events are of type `HookEventArgs` or a derived type which contains more info. It's possible to suppress event
propagation by setting the `SuppressEvent` property to `true` inside the event handler. This must be done synchronously
and is only supported on Windows and macOS. You can check the event time and whether the event is real or simulated with
the `EventTime` and `IsEventSimulated` properties respectively.

> [!IMPORTANT]
> Always use one instance of `IGlobalHook` at a time in the entire application since they all must use the same static
> method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a global hook
> when another global hook is already running will corrupt the internal global state of libuiohook.

You can create a keyboard-only or a mouse-only hook by passing a `GlobalHookType` to `Run` or `RunAsync`. On Windows,
there are two different global hooks – a keyboard hook and a mouse hook. On macOS and Linux, there is one hook for all
events, and this enables filtering keyboard or mouse events out on these OSes, though on Wayland, the connection to the
compositor will be established only if mouse events are enabled since they need to know the screen size and bounds.

On Wayland, absolute mouse position is usually not available, so you should subscribe to the `MouseMovedRelative` and
`MouseDraggedRelative` in addition to `MouseMoved` and `MouseDragged` if you want to listen to mouse movement. On other
platforms, relative motion events are never raised.

SharpHook provides three implementations of `IGlobalHook`:

- `SharpHook.SimpleGlobalHook` runs all of its event handlers on the same thread on which the hook itself runs. This
means that the handlers should generally be fast since they will block the hook from handling the events that follow if
they run for too long.

- `SharpHook.EventLoopGlobalHook` runs all of its event handlers on a separate dedicated thread. On backpressure it will
queue the remaining events which means that the hook will be able to process all events. This implementation should be
preferred to `SimpleGlobalHook` except for very simple use-cases. But it has a downside – suppressing event propagation
will be ignored since event handlers are executed on another thread.

- `SharpHook.TaskPoolGlobalHook` runs all of its event handlers on other threads inside the default thread pool for
tasks. The parallelism level of the handlers can be configured. On backpressure it will queue the remaining events which
means that the hook will be able to process all events. Like with `EventLoopGlobalHook`, suppressing event propagation
will be ignored since event handlers are executed on other threads. In general, `EventLoopGlobalHook` should be
preferred instead, as this class provides benefits only if events should be processed in parallel, which is rarely the
case.

The library also provides the `SharpHook.GlobalHookBase` class which you can extend to create your own implementation
of the global hook. It calls the appropriate event handlers, and you only need to implement a strategy for dispatching
the events. It also keeps a reference to a running global hook so that it's not garbage-collected.

The library also provides the `IBasicGlobalHook` interface and the `BasicGlobalHookBase` class. This class can be
extended to create a custom global hook which has a different form of events from that in `IGlobalHook`.

### Reactive Global Hooks

SharpHook provides the `SharpHook.Reactive.IReactiveGlobalHook` interface, but its implementations live in separate
packages.

#### Rx.NET

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

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

Reactive global hooks are basically the same as the default global hooks and the same rules apply to them.

SharpHook.ReactiveUI provides two implementations of `IReactiveGlobalHook`:

- `SharpHook.ReactiveUI.ReactiveUIGlobalHook`. Since we're dealing with observables, it's up to you to decide when and
where to handle the events through sequencers. A default sequencer can be specified for all observables.

- `SharpHook.ReactiveUI.ReactiveUIGlobalHookAdapter` adapts an `IGlobalHook` to `IReactiveGlobalHook`. All
subscriptions and changes are propagated to the adapted hook. There is no default adapter from `IReactiveGlobalHook`
to `IGlobalHook`. A default sequencer can be specified for all observables.

#### R3

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

R3 global hooks are basically the same as the default global hooks and the same rules apply to them.

Note that SharpHook.R3 uses its own interface instead of implementing `IReactiveGlobalHook` since R3's implementation is
incompatible with the interface.

SharpHook.R3 provides two implementations of `IR3GlobalHook`:

- `SharpHook.R3.R3GlobalHook`. Since we're dealing with observables, it's up to you to decide when and where to handle
the events through time providers. A default time provider can be specified for all observables.

- `SharpHook.R3.R3GlobalHookAdapter` adapts an `IGlobalHook` to `IR3GlobalHook`. All subscriptions and changes are
propagated to the adapted hook. There is no default adapter from `IR3GlobalHook` to `IGlobalHook`. A default time
provider can be specified for all observables.

### Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. Here's a quick
example:

```csharp
using SharpHook;
using SharpHook.Data;

using var simulator = EventSimulator.Create("Your application name");

// Press Ctrl+C
simulator.SimulateKeyPress(KeyCode.VcLeftControl);
simulator.SimulateKeyPress(KeyCode.VcC);

// Release Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC);
simulator.SimulateKeyRelease(KeyCode.VcLeftControl);

// Simulate pressing Ctrl, then pressing C, then releasing C, then releasing Ctrl
simulator.SimulateKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);

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

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`.

### Text Entry Simulation

SharpHook also provides text entry simulation. `IEventSimulator` contains the `SimulateTextEntry` method which accepts
a `string`. The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
surrogate pairs, e.g., emojis) is supported.

Note that on Wayland, text entry simulation is not supported.

### Low-Level Functionality

SharpHook exposes the functionality of libuiohook through low-level providers in the `SharpHook.Providers` namespace.
The `SharpHook.Data` namespace contains types which represent the data used by libuiohook.

> [!NOTE]
> In general, you don't need to use the low-level types directly. Instead, use the higher-level interfaces and classes
> provided by SharpHook. However, you should still read this section to know how the high-level features work under
> the hood.

There are multiple interfaces in `SharpHook.Providers`, each of which represents a part of libuiohook's functionality.
`SharpHook.Providers.UioHookProvider` implements all of these interfaces and simply calls the corresponding functions in
libuiohook. In order to use this class, use `UioHookProvider.Instance` – its single instance.

`IGlobalHookProvider` contains the following members for working with the global hook:

- `SetDispatchProc` – sets the function which will be called when an event is raised by libuiohook.
- `Run` – creates a keyboard and mouse global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunKeyboard` – creates a keyboard-only global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunMouse` – creates a mouse-only global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` – stops the global hook.
- `KeyTypedEnabled` - gets or sets the value which specifies whether events of type `KeyTyped` are enabled.

> [!IMPORTANT]
> You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
> previously set one. Also, running a global hook when another global hook is already running will corrupt the internal
> global state of libuiohook.

`IEventSimulationProvider` contains the `PostEvent` method for simulating input events. It also contains the `PostText`
method which simulates text entry. On Linux, it's required to create virtual input devices before simulating input
events. `IEventSimulationProvider` contains the `InitializeVirtualDevices` and `DestroyVirtualDevices` methods for that.

All higher-level classes accept low-level providers in their constructors. By default, they use `UioHookProvider`, but
it can be swapped for a fake provider in testing, for example.

Here's the list of all interfaces in `SharpHook.Providers`:

- `IAccessibilityProvider` – provides configuration for the macOS Accessibility API.
- `IDeviceProcsProvider` – provides a way to override privileged device opening/closing functions on Linux.
- `IEventSimulationProvider` – provides the event simulation functionality.
- `IFeatureProvider` – provides information on optional features supported by the current platform.
- `IGlobalHookProvider` – provides the global hook functionality.
- `IKeyboardInfoProvider` – provides low-level information about the keyboard.
- `ILinuxBackendProvider` – provides the Linux backend configuration.
- `ILoggingProvider` – provides the libuiohook logging functionality.
- `IMouseInfoProvider` – provides low-level information about the mouse.
- `IScreenInfoProvider` – provides low-level information about screens.

### Querying Optional Feature Support

Some libuiohook features are considered 'optional' and are not supported on all platforms. The `IFeatureProvider`
interface has the `GetOptionalFeatureSupport` method which returns `UioHookFeature` – a flag enum which represents the
optional features and has the following values:

- `EventSuppression` – if this feature is supported, then event suppression works.
- `KeyTypedEvents` – if this feature is supported, then events of type `KeyTyped` work. If not, then calling
`UioHookProvider.Instance.KeyTypedEnabled = true` does nothing.
- `PostText` – if this feature is supported, then simulating text entry works.
- `KeyAutoRepeat` – if this feature is supported, then keeping a key pressed will raise repeated `KeyPressed` events.
If not, the only one `KeyPressed` event will be raised.
- `AbsoluteMouseMovement` – if this feature is supported, then mouse motion events will always include absolute mouse
coordinates, so `MouseMoved` or `MouseDragged` will always be raised. If not, then `MouseMovedRelative` or
`MouseDraggedRelative` may be raised instead.
- `AbsoluteMouseButtonCoordinates` – if this feature is supported, then mouse coordinates will be present in
`MousePressed`, `MouseReleased`, `MouseClicked`, and `MouseWheel` events. If not, then the coordinates will always be
(0, 0).
- `PointerProperties` – if this feature is supported, then the `GetPointerAccelerationMultiplier`,
`GetPointerAccelerationThreshold`, and `GetPointerSensitivity` methods in `IMouseInfoProvider` work. If not, then they
always return -1.

Windows and macOS support all of these features. X11 supports all of them, except `EventSuppression`. Wayland supports
none of these features.

### Logging

libuiohook can log messages throughout its execution. By default, the messages are not logged anywhere, but you can get
these logs by using the `ILogSource` interface and its default implementation, `LogSource`:

```csharp
using SharpHook.Data;
using SharpHook.Logging;

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

You can use only one `LogSource` object at a time. `ILogSource` extends `IDisposable` – you can dispose of a log source
to stop receiving libuiohook messages.

An `EmptyLogSource` class is also available – this class doesn't listen to the libuiohook logs and can be used instead
of `LogSource` in release builds.

SharpHook also contains the `IReactiveLogSource` interface so you can use logging in a more reactive way. Its
implementations live in SharpHook.Reactive and SharpHook.ReactiveUI.

SharpHook.Reactive contains the `ReactiveLogSourceAdapter` class:

```csharp
using SharpHook.Data;
using SharpHook.Logging;
using SharpHook.Reactive.Logging;

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

SharpHook.ReactiveUI contains the `ReactiveUILogSourceAdapter` class:

```csharp
using SharpHook.Data;
using SharpHook.Logging;
using SharpHook.ReactiveUI.Logging;

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new ReactiveUILogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

SharpHook.R3 contains the `IR3LogSource` interface and the `R3LogSourceAdapter` class so you can use logging in a more
reactive way as well:

```csharp
using SharpHook.Logging;
using SharpHook.R3.Logging;

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new R3LogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### Testing

The SharpHook.Testing package provides two classes which make testing easier. They aren't required since mocks can be
used instead, but unlike mocks, no setup is required to use these classes.

`SharpHook.Testing.TestGlobalHook` provides an implementation of `IGlobalHook` and `IEventSimulator` which can be used
for testing. When the `Run` or `RunAsync` method is called, it will dispatch events using the various `Simulate` methods
from `IEventSimulator`.

If this class is used as an `IEventSimulator` in the tested code, then the `SimulatedEvents` property can be checked to
see which events were simulated using the test instance.

If an `IReactiveGlobalHook` is needed for testing, then `ReactiveGlobalHookAdapter` or `ReactiveUIGlobalHookAdapter` can
be used to adapt an instance of `TestGlobalHook`. If an `IR3GlobalHook` is needed for testing, then
`R3GlobalHookAdapter` can be used to adapt an instance of `TestGlobalHook`.

If the low-level functionality of SharpHook should be mocked, or mocking should be pushed as far away as possible,
then `SharpHook.Testing.TestProvider` can be used. It implements every interface in the `SharpHook.Providers` namespace,
so it can be used instead of normal low-level functionality providers.

Like `TestGlobalHook`, this class can post events using the `PostEvent` method and dispatch them if `Run` was called.
It also contains the `PostedEvents` property.

## Library Status

If you've noticed that this library hasn't gotten new commits in some time, rest assured that it's not abandoned!
I'm not giving up on this library any time soon.

## Building from Source

In order to build this library, you'll first need to get libuiohook binaries. You can get a
[nightly build from this repository](https://github.com/TolikPylypchuk/SharpHook/actions/workflows/build.yml), you can
extract them from the SharpHook package, or you can build them yourself as instructed in the
[libuiohook fork](https://github.com/TolikPylypchuk/libuiohook) that SharpHook uses – you should most probably use the
same build steps as specified in SharpHook's GitHub Actions definition.

Place the binaries into the appropriate directories in the `SharpHook` project, as described in the following table.
Note that on Linux, there are four libuiohook files per platform.

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


With libuiohook in place, you can build SharpHook using your usual methods, e.g. with Visual Studio or the `dotnet` CLI.
You need .NET 10 with the `maccatalyst` workload to build SharpHook. If you are building on Linux where the
`maccatalyst` workload is not supported, or if you don't want to install it, you can set the
`SHARPHOOK_SKIP_MACCATALYST` environment variable to anything other than `false` (case-insensitive), and the Mac
Catalyst targets will be skipped.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
