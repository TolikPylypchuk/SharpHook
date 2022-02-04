# Native Functions

This article describes how to use the low-level stuff provided by SharpHook.

SharpHook exposes the methods of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains structs and enums which represent the data returned by libuiohook.

In general, you shouldn't use native methods directly. Instead, use the higher-level interfaces and classes provided by
SharpHook. However, you should still read this article to know how the high-level features work under the hood.

## Working with the Hook Itself

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` (`hook_set_dispatch_proc`) - sets the function which will be called when an event is raised by
libuiohook.
- `Run` (`hook_run`) - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` (`hook_stop`) - destroys the global hook.

You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
previously set one.

`SetDispatchProc` accepts a delegate of type `SharpHook.Native.DispatchProc`. This delegate in turn accepts a
`SharpHook.Native.UioHookEvent` by reference, and returns nothing.  You can pass `null` to `SetDispatchProc` in order
to unset the callback function.

Note that on macOS running the global hook requires that the main run-loop is present. libuiohook takes care of it if
the hook is run on the main thread. It's also taken care of by UI frameworks since they need an event loop on the main
thread to run. But if you're using a global hook in a console app or a background service and want to run it on some
thread other than the main one then you should take care of it yourself. You can do that e.g. by P/Invoking the native
`CFRunLoopRun` function.

## Input Events

The `SharpHook.Native.UioHookEvent` struct contains information about events that have occured.

There are several event types supported by libuiohook (contained in the event's `Type` field and defined in the
`SharpHook.Native.EventType` enum).

Following are the general-purpose events:

- `HookEnabled` - raised when the `Run` method is called.
- `HookDisabled` - raised when the `Stop` method is called.

Following are the keyboard events, and `UioHookEvent` will contain more infomration in its `Keyboard` field:

- `KeyPressed` - raised when a key is pressed.
- `KeyReleased` - raised when a key is released.
- `KeyTyped` - raised when a character is typed using the keyboard.

Following are the mouse events, and `UioHookEvent` will contain more infomration in its `Mouse` field:

- `MouseClicked` - raised when a mouse button is clicked.
- `MousePressed` - raised when a mouse button is pressed.
- `MouseReleased` - raised when a mouse button is released.
- `MouseMoved` - raised when the mouse cursor is moved.
- `MouseDragged` - raised when the mouse cursor is dragged.

And the last one is also a mouse event, but `UioHookEvent` will contain more information in its `Wheel` field since it
has more information:

- `MouseWheel` - raised when the mouse wheel is scrolled.

`UioHookEvent` also contains the `Time` and `Mask` fields. `Time` represents the time of the event, but it's not a
timestamp - it's a synthetic value which should be used only to compare it to other events' times. `Mask` contains the
state of keyboard modifiers and the mouse state at the time of the event.

Lastly, `UioHookEvent` contains the `Reserved` field. This field can be set inside the event handler and libuiohook will
consume it. Currently only one setting is supported - suppressing the event propagation. If it's set then libuiohook
will not propagate the event further and it will effectively be blocked. The `Reserved` field should be set
synchronously i.e. on the same thread which handles the event.

## Simulating Input Events

`UioHook` contains the `PostEvent` method for simulating input events. It accepts a `UioHookEvent`, but it doesn't need
all its fields. Only `Type`, `Keyboard`/`Mouse`/`Wheel`, and `Mask` should be present.

The following table describes the specifics of simulating each event type.

<table>
  <thead>
    <tr>
      <th>Event type</td>
      <th>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><code>HookEnabled</code></td>
      <td>Events of this type are ignored.</td>
    </tr>
    <tr>
      <td><code>HookDisabled</code></td>
      <td>Events of this type are ignored.</td>
    </tr>
    <tr>
      <td><code>KeyPressed</code></td>
      <td>Only <code>KeyboardEventData.KeyCode</code> is considered.</td>
    </tr>
    <tr>
      <td><code>KeyReleased</code></td>
      <td>Only <code>KeyboardEventData.KeyCode</code> is considered.</td>
    </tr>
    <tr>
      <td><code>KeyTyped</code></td>
      <td>
        Not recommended to use since on some platforms events of this type are ignored, while on others they are not.
      </td>
    </tr>
    <tr>
      <td><code>MousePressed</code></td>
      <td>Only <code>MouseEventData.Button</code> is considered.</td>
    </tr>
    <tr>
      <td><code>MouseReleased</code></td>
      <td>Only <code>MouseEventData.Button</code> is considered.</td>
    </tr>
    <tr>
      <td><code>MouseClicked</code></td>
      <td>
        Not recommended to use since on some platforms events of this type are ignored, while on others they are not.
      </td>
    </tr>
    <tr>
      <td><code>MouseMoved</code></td>
      <td>Only <code>MouseEventData.X</code> and <code>MouseEventData.Y</code> are considered.</td>
    </tr>
    <tr>
      <td><code>MouseDragged</code></td>
      <td>Not recommended to use; instead, use <code>MouseMoved</code> with a modifier mask.</td>
    </tr>
    <tr>
      <td><code>MouseWheel</code></td>
      <td>
        Only <code>MouseWheelEventData.X</code>, <code>MouseWheelEventData.Y</code>,
        <code>MouseWheelEventData.Amount</code>, and <code>MouseWheelEventData.Rotation</code> are considered.
      </td>
    </tr>
  </tbody>
</table>

Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of libuiohook
for more details.

libuiohook [ignores](https://github.com/kwhat/libuiohook/issues/111) modifier masks on Windows, so you need to simulate
pressing/releasing modifier keys manually.

## Other Functions

libuiohook also provides functions which get various pieces of information about the system, and are listed below:

- `CreateScreenInfo` (`hook_create_screen_info`)
- `GetAutoRepeatRate` (`hook_get_auto_repeat_rate`)
- `GetAutoRepeatDelay` (`hook_get_auto_repeat_delay`)
- `GetPointerAccelerationMultiplier` (`hook_get_pointer_acceleration_multiplier`)
- `GetPointerAccelerationThreshold` (`hook_get_pointer_acceleration_threshold`)
- `GetPointerSensitivity` (`hook_get_pointer_sensitivity`)
- `GetMultiClickTime` (`hook_get_multi_click_time`)

These functions are quite straightforward, except for `CreateScreenInfo`. `UioHook` defines two versions of this
function. One is a native function which returns an unmanaged array of `ScreenData` objects (as an `IntPtr`) along
with its length in an output parameter. Another is a wrapper which returs a managed array. Use the second one if you
need it since it's safer.

Next article: [Global Hooks](hooks.md).
