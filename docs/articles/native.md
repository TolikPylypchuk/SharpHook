# Native Functions

This article describes how to use the low-level features provided by SharpHook.

SharpHook exposes the functions of libuiohook in the `SharpHook.Native.UioHook` class. The `SharpHook.Native`
namespace also contains types which represent the data used by libuiohook.

In general, you don't need to use the native methods directly. Instead, use the higher-level interfaces and classes
provided by SharpHook. However, you should still read this article to know how the high-level features work under
the hood.

**Note**: The libuiohook build used by SharpHook on Windows is statically linked to the C runtime which means that
client apps don't need the Visual C++ Redistributable package. An exception is the logging functionality - this is
explained further in the article about [logging](logging.md).

## Low-Level Functionality Providers

If you want to use the low-level functionality, you don't need to use the `UioHook` class directly. Instead you can use
interfaces in the `SharpHook.Providers` namespace. The methods in those interfaces are the same as in the `UioHook`
class. `SharpHook.Providers.UioHookProvider` implements all of these interfaces and simply calls the corresponding
methods in `UioHook`. This should be done to decouple your code from `UioHook` and make testing easier.

## Working with the Hook Itself

`UioHook` contains the following methods for working with the global hook:

- `SetDispatchProc` - sets the function which will be called when an event is raised by libuiohook.
- `Run` - creates a global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` - destroys the global hook.

You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
previously set one.

`SetDispatchProc` accepts a delegate of type `SharpHook.Native.DispatchProc`. This delegate in turn accepts a
`SharpHook.Native.UioHookEvent` by reference, and returns nothing.  You can pass `null` to `SetDispatchProc` in order
to unset the callback function.

`Run` and `Stop` return a `UioHookResult` which specifies whether the result of the method was successful
(`UioHookResult.Success`) or not (any other value).

The methods described above are also defined in the `SharpHook.Providers.IGlobalHookProvider` interface.

**Note**: On macOS running the global hook requires that the main run-loop is present. libuiohook takes care of it if
the hook is run on the main thread. It's also taken care of by UI frameworks since they need an event loop on the main
thread to run. But if you're using a global hook in a console app or a background service and want to run it on some
thread other than the main one then you should take care of it yourself. You can do that by P/Invoking the native
`CFRunLoopRun` function on the main thread.

**Note**: macOS also requires that the accessibility API be enabled for the application if it wants to create a global
hook. If the accessiblity API is not enabled, then `Run` will fail and return `UioHookResult.ErrorAxApiDisabled`.

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

An application manifest is required on Windows to enable DPI awareness for your app. If it's not enabled then mouse
coordinates will be wrong on high-DPI screens. You can look at the sample app in the SharpHook repository to see the
manifest example.

**Note**: `KeyTyped` and `MouseClicked` events are not raised by the OS, but by libuiohook itself. `KeyTyped` is raised
after `KeyPressed` if the key press has caused characters to be typed. Since a single key press can cause multiple
characters to be typed, a single `KeyPressed` event can raise multiple `KeyTyped` events. `MouseClicked` is raised after
`MouseReleased` if the cursor was not dragged. Since these events are raised by libuiohook, and not the OS, suppressing
them has no effect.

## Simulating Input Events

`UioHook` contains the `PostEvent` method for simulating input events. It accepts a `UioHookEvent`, but it doesn't need
all its fields. Only `Type` and `Keyboard`/`Mouse`/`Wheel` should be present.

`PostEvent` returns `UioHookResult` to indicate whether it was successful or not.

This method is also defined in the `SharpHook.Providers.IEventSimulationProvider` interface.

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
      <td>Events of this type are ignored. The next section describes text entry simulation.</td>
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
        Only <code>MouseWheelEventData.Rotation</code>, <code>MouseWheelEventData.Direction</code>, and
        <code>MouseWheelEventData.Type</code> are considered.
      </td>
    </tr>
  </tbody>
</table>

Mouse wheel simulation is a little more complex than other events.

A positive `MouseWheelEventData.Rotation` value indicates that the wheel will be rotated up or left, and
a negative value indicates that the wheel will be rotated down or right.

On Windows the value 120 represents the default wheel step. As such, multiples of 120 can be used as the
rotation value, but it's not required. The value of `MouseWheelEventData.Type` is ignored.

On macOS it's recommended to use values between -10 and 10. This will result in quite a small scroll
amount with pixel scrolling, so `MouseWheelScrollType.BlockScroll` is recommended for line scrolling
instead of pixel scrolling.

On Linux there is no fixed recommendation, but multiples of 100 can be used. The value of `MouseWheelEventData.Type`
is ignored.

## Simulating Text Entry

Starting with version 5.0.0, SharpHook also provides text entry simulation. `UioHook` contains the `PostText` method
which accepts a `string`. The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16
(including surrogate pairs, e.g. emojis) is supported.

On Windows text simulation should work correctly and consistently.

On macOS applications are not required to process text simulation, but most of them should handle it correctly.

X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to that
character, and then key press/release is simulated. Since the receiving application must react to the remapping, and
may not do so instantaneously, a delay is needed for accurate simulation. This means that text simulation on Linux works
slowly and is not guaranteed to be correct. `UioHook` contains the `SetPostTextDelayX11` method which can be used to
increase (or decrease) the delay if needed - longer delays add consistency but may be more jarring to end users.
`UioHook` also contains the `GetPostTextDelayX11` which can be used to get the currently configured delay - the default
is 50 milliseconds. Delays are configurable on a nanosecond level. On Windows and macOS `SetPostTextDelayX11` does
nothing, and `GetPostTextDelayX11` always returns 0.

The methods described above are also defined in the `SharpHook.Providers.IEventSimulationProvider` interface.

## Logging

libuiohook can log messages throughout its execution. By default it doesn't log anything, but `UioHook` contains the
`SetLoggerProc` method to set the log callback function - it will be called by libuiohook to log messages.

This method is also defined in the `SharpHook.Providers.ILoggingProvider` interface.

`SetLoggerProc` accepts a delegate of type `SharpHook.Native.LoggerProc`. This delegate in turn accepts a log level,
the message format (as a pointer) and arguments (also as a pointer).

You can read more about how to use the `SetLoggerProc` method in the article about [logging](logging.md), though it's
not recommended to use it directly.

## Passing Custom Data to Callbacks

`SetDispatchProc` and `SetLoggerProc` also receive a pointer to user-supplied data. It is then passed to the
callbacks - both `DispatcherProc` and `LoggerProc` receive user-supplied data as well.

Do not use them.

You should always pass `IntPtr.Zero` to `SetDispatchProc` and `SetLoggerProc` and not use the respective parameters in
the callbacks.

The reason is that in order to use pointers to managed objects, they have to be pinned. As these callbacks tend to be
long-lived (probably as long as the program itself), the objects will have to be pinned for a long time as well, and
that's detrimental to the performance of the garbage collector and the memory layout of the program.

If you need to pass custom data to the callbacks then simply use closures. This feature was created with the C language
in mind, and C doesn't have closures.

## Other Functions

libuiohook also provides functions which get various pieces of information about the system, and are listed below:

- `CreateScreenInfo`
- `GetAutoRepeatRate`
- `GetAutoRepeatDelay`
- `GetPointerAccelerationMultiplier`
- `GetPointerAccelerationThreshold`
- `GetPointerSensitivity`
- `GetMultiClickTime`

These functions are quite straightforward, except for `CreateScreenInfo`. `UioHook` defines two versions of this
function. One is a native function which returns an unmanaged array of `ScreenData` objects (as an `IntPtr`) along
with its length in an output parameter. Another is a wrapper which returs a managed array. Use the second one if you
need it since it's safer. If you decide to use the native version then you must free the returned memory manually.

The safe version of `CreateScreenInfo` is also defined in the `SharpHook.Providers.IScreenInfoProvider` interface.
Other methods described above are also defined in the `SharpHook.Providers.IMouseInfoProvider` interface.
