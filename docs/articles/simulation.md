# Event and Text Entry Simulation

SharpHook provides the ability to simulate keyboard and mouse events, as well as text entry, in a cross-platform way as
well. It provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events by default (though it's configurable). The methods in this interface return
a `UioHookResult` to specify whether the events were simulated successfully or not.

Simulated events can be distinguished from real ones in a global hook handler with the `HookEventArgs.IsEventSimulated`
property.

## Event Simulation

### Example

Input event simulation is quite straightforward. Here's a quick example:

```csharp
using SharpHook;
using SharpHook.Data;

// ...

var simulator = new EventSimulator();

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

### Mouse Wheel Simulation

Mouse wheel simulation is a little more complex than other events.

A positive `rotation` value indicates scrolling up or left, and a negative value indicates scrolling down or right.

On Windows, the value 120 represents the default wheel step. As such, multiples of 120 can be used as the rotation value,
but it's not required. The value of `type` is ignored.

On macOS, it's recommended to use values between -10 and 10. This will result in quite a small scroll amount with pixel
scrolling, so `MouseWheelScrollType.BlockScroll` is recommended for line scrolling instead of pixel scrolling.

On Linux, there is no fixed recommendation, but multiples of 100 can be used. The value of `type` is ignored.

### Simulating a Sequence of Events

`IEventSimulator` contains the `Sequence` method which returns an `IEventSimulationSequenceBuilder`. This object can be
used to build a sequence of events that will be simulated together. If contains several methods like `AddKeyPress` and
`AddMouseMovement` which add specific events to the sequence. It also contains general-purpose methods: `AddEvent` and
`AddEvents` which can add any event, as well as `RemoveEvent` and `RemoveEvents` which remove specific events from the
sequence. The events can be simulated using the `Simulate` method. For example:

```csharp
simulator.Sequence()
    .AddMouseMovementRelative(20, 20)
    .AddMousePress(MouseButton.Button1)
    .AddMouseRelease(MouseButton.Button1)
    .AddMouseMovementRelative(-20, -20)
    .Simulate();
```

Using `Sequence` should be preferred to multiple calls to various `SimulateXXX` when simulating multiple events. On
Windows, all events will be simulated using a single Windows API call. On macOS and Linux, each event will be simulated
one-by-one, but it's still slightly more efficient than multiple `PostEvent` calls as some structures will only be
initialized once for all events. `IEventSimulationSequenceBuilder.Simulate` also returns `UioHookResult` to indicate
whether it was successful or not. On Windows, either all events are simulated, or none are. On macOS and Linux, if a
failure occurs in the middle of the simulation sequence, then further events will not be simulated.

`IEventSimulationSequenceBuilder` also contains the `CreateTemplate` method which returns an
`IEventSimulationSequenceTemplate`. This object represents an immutable template for simulating a predetermined sequence
of events and contains only a single method - `Simulate`. For example:

```csharp
var diagonalScrollTemplate = simulator.Sequence()
    .AddMouseWheel(rotation: -120, direction: MouseWheelScrollDirection.Vertical)
    .AddMouseWheel(rotation: -120, direction: MouseWheelScrollDirection.Horizontal)
    .CreateTemplate();

diagonalScrollTemplate.Simulate();
diagonalScrollTemplate.Simulate();
diagonalScrollTemplate.Simulate();
```

`IEventSimulationSequenceBuilder` has the `AddKeyStroke` extension method which adds a sequence of key presses and a
reversed sequence of key releases. For example, these two snippets will have equivalent results:

```csharp
builder.AddKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);
```

```csharp
builder
    .AddKeyPress(KeyCode.VcLeftControl)
    .AddKeyPress(KeyCode.VcC)
    .AddKeyRelease(KeyCode.VcC)
    .AddKeyRelease(KeyCode.VcLeftControl);
```

`IEventSimulator` has the `SimulateKeyStroke` extension method which creates a sequence builder, calls `AddKeyStroke`
for it, and simulates this sequence of events.

## Text Entry Simulation

SharpHook also provides text entry simulation. `IEventSimulator` contains the `SimulateTextEntry` method which accepts
a `string`. The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
surrogate pairs, e.g. emojis) is supported.

X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to that
character, and then key press/release is simulated. Since the receiving application must react to the remapping, and
may not do so instantaneously, a delay is needed for accurate simulation. This means that text simulation on Linux works
slowly and is not guaranteed to be correct.

`IEventSimulator` contains the `TextSimulationDelayOnX11` property to get or set the delay if needed – longer delays add
consistency but may be more jarring to end users. The default is 50 milliseconds. Delays are configurable on a
nanosecond level. On Windows and macOS, setting this property does nothing, and it always returns 0.
