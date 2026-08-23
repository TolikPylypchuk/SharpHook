# SharpHook

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

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

> **Important**: Always use one instance of `IGlobalHook` at a time in the entire application since they all must use the
> same static method to set the hook callback for libuiohook, so there may only be one callback at a time. Running a
> global hook when another global hook is already running will corrupt the internal global state of libuiohook.

You can run a keyboard-only or a mouse-only hook by passing a `GlobalHookType` to the hook's `Run` and `RunAsync`
methods. This makes a real difference only on Windows where there are two different global hooks – a keyboard hook and a
mouse hook. On macOS and Linux, there is one hook for all events, and this simply enables filtering keyboard or mouse
events out on these OSes.

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

Use the [SharpHook.Reactive](https://www.nuget.org/packages/SharpHook.Reactive) package for reactive global hooks with
the [Rx.NET](https://github.com/dotnet/reactive) integration.

Use the [SharpHook.ReactiveUI](https://www.nuget.org/packages/SharpHook.ReactiveUI) package for reactive global hooks
with the [ReactiveUI.Primitives](https://github.com/reactiveui/Primitives) integration.

Use the [SharpHook.R3](https://www.nuget.org/packages/SharpHook.R3) package for reactive global hooks with the
[R3](https://github.com/Cysharp/R3) integration.

### Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. Here's a quick
example:

```csharp
using SharpHook;
using SharpHook.Data;

using var simulator = EventSimulator.Create("You application name");

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

In general, you don't need to use the low-level types directly. Instead, use the higher-level interfaces and classes
provided by SharpHook. However, you should still read this section to know how the high-level features work under
the hood.

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
always return `-1`.

Windows and macOS support all of these features. X11 supports all of them, except `EventSuppression`. Wayland supports
none of these features.

### Logging

libuiohook can log messages throughout its execution. By default the messages are not logged anywhere, but you can get
these logs by using the `ILogSource` interface and its default implementation, `LogSource`:

```csharp
using SharpHook.Logging;

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

`ILogSource` extends `IDisposable` – you can dispose of a log source to stop receiving libuiohook messages.

An `EmptyLogSource` class is also available – this class doesn't listen to the libuiohook logs and can be used instead
of `LogSource` in release builds.

### Testing

You can use the [SharpHook.Testing](https://www.nuget.org/packages/SharpHook.Testing) package to simplify testing as it
provides test utility classes.

## Icon

Icon made by [Freepik](https://www.freepik.com) from [www.flaticon.com](https://www.flaticon.com).
