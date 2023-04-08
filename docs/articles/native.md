# Native Functions

This article describes how to use the low-level stuff provided by SharpHook.

SharpHook exposes the functions of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains types which represent the data used by libuiohook.

In general, you don't need to use the native methods directly. Instead, use the higher-level interfaces and classes
provided by SharpHook. However, you should still read this article to know how the high-level features work under the hood.

**Note**: Starting with version 3.1.0 the libuiohook build used by SharpHook on Windows is statically linked to the C
runtime which means that client apps don't need the Visual C++ Redistributable package. An exception is the logging
functionality - this is explained further in the article about [logging](logging.md).

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

`EventType` defines three more types, but they are used only when simulating events.

`UioHookEvent` also contains the `Time` and `Mask` fields. `Time` is the event's UNIX timestamp. `Mask` contains the
state of keyboard modifiers and the mouse state at the time of the event.

Lastly, `UioHookEvent` contains the `Reserved` field. This field can be set inside the event handler and libuiohook
will consume it. Currently only one setting is supported - suppressing the event propagation. If it's set then
libuiohook will not propagate the event further and it will effectively be blocked. The `Reserved` field should be set
synchronously i.e. on the same thread which handles the event. Supressing events works only on Windows and macOS.

An application manifest is required on Windows to enable DPI awareness for your app. If it's not enabled
then mouse coordinates will be wrong on high-DPI screens. You can look at the sample app in the SharpHook repository to
see the manifest example.

Windows defines a single 'step' of a mouse wheel as rotation value 120.

## Simulating Input Events

`UioHook` contains the `PostEvent` method for simulating input events. It accepts a `UioHookEvent`, but it doesn't need
all its fields. Only `Type` and `Keyboard`/`Mouse`/`Wheel` should be present.

`PostEvent` returns `UioHookResult` to indicate whether it was successful or not.

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
      <td>Events of this type are ignored.</td>
    </tr>
    <tr>
      <td><code>MousePressed</code></td>
      <td>
        Only <code>MouseWheelEventData.X</code>, <code>MouseWheelEventData.Y</code>, and
        <code>MouseEventData.Button</code> are considered.
      </td>
    </tr>
    <tr>
      <td><code>MousePressedIgnoreCoordinates</code></td>
      <td>
        Only <code>MouseEventData.Button</code> is considered.
      </td>
    </tr>
    <tr>
      <td><code>MouseReleased</code></td>
      <td>
        Only <code>MouseWheelEventData.X</code>, <code>MouseWheelEventData.Y</code>, and
        <code>MouseEventData.Button</code> are considered.
      </td>
    </tr>
    <tr>
      <td><code>MouseReleasedIgnoreCoordinates</code></td>
      <td>
        Only <code>MouseEventData.Button</code> is considered.
      </td>
    </tr>
    <tr>
      <td><code>MouseClicked</code></td>
      <td>Events of this type are ignored.</td>
    </tr>
    <tr>
      <td><code>MouseMoved</code></td>
      <td>Only <code>MouseEventData.X</code> and <code>MouseEventData.Y</code> are considered.</td>
    </tr>
    <tr>
      <td><code>MouseMovedRelativeToCursor</code></td>
      <td>Only <code>MouseEventData.X</code> and <code>MouseEventData.Y</code> are considered.</td>
    </tr>
    <tr>
      <td><code>MouseDragged</code></td>
      <td>Not recommended to use; same as <code>MouseMoved</code>.</td>
    </tr>
    <tr>
      <td><code>MouseWheel</code></td>
      <td>
        Only <code>MouseWheelEventData.Amount</code> and <code>MouseWheelEventData.Rotation</code> are considered.
      </td>
    </tr>
  </tbody>
</table>

Mouse wheel simulation is a little inconsistent across platforms, and not documented. View the source code of libuiohook
for more details.

## Logging

libuiohook can log messages throughout its execution. By default it doesn't log anything, but `UioHook` contains the
`SetLoggerProc` method to set the log callback function - it will be called by libuiohook to log messages.

`SetLoggerProc` accepts a delegate of type `SharpHook.Native.LoggerProc`.  This delegate in turn accepts a log level,
the message format (as a pointer) and arguments (also as a pointer).

You can read more about how to use the `SetLoggerProc` method in the article about [logging](logging.md), though it's
not recommended to use it directly.

## Passing Custom Data to Callbacks

`SetDispatchProc` and `SetLoggerProc` also receive a pointer to user-supplied data. It is then passed to the
callbacks - both `DispatcherProc` and `LoggerProc` receive user-supplied data as well.

**Do not use them.**

You should always pass `IntPtr.Zero` to `SetDispatchProc` and `SetLoggerProc` and not use the respective parameters in
the callbacks.

The reason is that in order to use pointers to managed objects, they have to be pinned. As these callbacks tend to be
long-lived (probably as long as the program itself), the objects will have to be pinned for a long time as well, and
that's detrimental to the performance of the garbage collector and the memory layout of the program.

If you need to pass custom data to the callbacks then simply use closures. This feature was created with the C language
in mind, and C doesn't have closures.

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
