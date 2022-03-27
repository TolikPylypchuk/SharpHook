# Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. It provides the
`IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls `UioHook.PostEvent` to
simulate the events.

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

// Move the mouse pointer to the (0, 0) point
simulator.SimulateMouseMovement(0, 0);

// Move the mouse pointer to the (0, 0) point, and scroll the mouse wheel
simulator.SimulateMouseWheel(0, 0, 10, -1);
```

Next article: [Logging](logging.md).
