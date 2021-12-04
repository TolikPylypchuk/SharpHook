# Event Simulation

SharpHook provides the ability to simulate keyboard and mouse events in a cross-platform way as well. Here's a quick
example:

```C#
using SharpHook;
using SharpHook.Native;

// ...

var simulator = new EventSimulator();

simulator.SimulateKeyPress(KeyCode.VcC, ModifierMask.LeftCtrl);   // Press Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC, ModifierMask.LeftCtrl); // Release Ctrl+C

simulator.SimulateMousePress(MouseButton.Button1);   // Press the left mouse button
simulator.SimulateMouseRelease(MouseButton.Button1); // Release the left mouse button

simulator.SimulateMouseMovement(0, 0);      // Move the mouse pointer to the (0, 0) point
simulator.SimulateMouseWheel(0, 0, 10, -1); // Move the mouse pointer to the (0, 0) point, and scroll the mouse wheel
```

SharpHook provides the `IEventSimulator` interface, and the default implementation, `EventSimulator`, which calls
`UioHook.PostEvent` to simulate the events.

Next article: [Limitations](limitations.md).
