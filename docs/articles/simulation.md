# Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. It provides the
`IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls `UioHook.PostEvent` to
simulate the events. The methods in this interface return a `UioHookResult` to specify whether the event was simulated
successfully, or not.

Here's a quick example:

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

// Move the mouse pointer to (0, 0)
simulator.SimulateMouseMovement(0, 0);

// Move the mouse pointer 50 pixels to the right and 100 pixels down
simulator.SimulateMouseMovementRelative(50, 100);

// Scroll the mouse wheel
simulator.SimulateMouseWheel(2, -120);
```

Windows defines a single 'step' of a mouse wheel as rotation value 120.

Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of libuiohook
for more details.
