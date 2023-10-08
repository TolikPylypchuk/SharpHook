# Event and Text Entry Simulation

SharpHook provides the ability to simulate keyboard and mouse events, as well as text entry, in a cross-platform way as
well. It provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events by default (though it's configurable). The methods in this interface return
a `UioHookResult` to specify whether the event was simulated successfully, or not.

## Event Simulation

Input event simulation is quite straightforward. Here's a quick example:

```C#
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
    direction: MouseWheelScrollDirection.Vertical, // MouseWheelScrollDirection.Vertical by default
    type: MouseWheelScrollType.UnitScroll); // MouseWheelScrollType.UnitScroll by default
```

Mouse wheel simulation is a little more complex than other events.

A positive `rotation` value indicates that the wheel will be rotated up or left, and a negative value indicates that
the wheel will be rotated down or right.

On Windows the value 120 represents the default wheel step. As such, multiples of 120 can be used as the rotation value,
but it's not required. The value of `type` is ignored.

On macOS it's recommended to use values between -10 and 10. This will result in quite a small scroll amount with pixel
scrolling, so `MouseWheelScrollType.BlockScroll` is recommended for line scrolling instead of pixel scrolling.

On Linux there is no fixed recommendation, but multiples of 100 can be used. The value of `type` is ignored.

## Text Entry Simulation

Starting with version 5.0.0, SharpHook also provides text entry simulation. `IEventSimulator` contains the
`SimulateTextEntry` method which accepts a `string`. The text to simulate doesn't depend on the current keyboard layout.
The full range of UTF-16 (including surrogate pairs, e.g. emojis) is supported.

Text entry simulation may not work well on Linux. More info can be found in the article on
[OS-specific constraints](os-constraints.md).
