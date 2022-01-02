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

**Important**: libuiohook [ignores](https://github.com/kwhat/libuiohook/issues/111) modifier masks on Windows, so you
need to simulate pressing/releasing modifier keys manually.

Next article: [Limitations](limitations.md).
