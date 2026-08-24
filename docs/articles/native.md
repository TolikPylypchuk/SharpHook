# Native Functions

This article describes how to use the low-level features provided by SharpHook.

SharpHook exposes the functionality of libuiohook through low-level providers in the `SharpHook.Providers` namespace.
The `SharpHook.Data` namespace contains types which represent the data used by libuiohook.

> [!NOTE]
> In general, you don't need to use the low-level types directly. Instead, use the higher-level interfaces and classes
> provided by SharpHook. However, you should still read this article to know how the high-level features work under
> the hood.

There are multiple interfaces in `SharpHook.Providers`, each of which represents a part of libuiohook's functionality.
`SharpHook.Providers.UioHookProvider` implements all of these interfaces and simply calls the corresponding functions in
libuiohook. In order to use this class, use `UioHookProvider.Instance` – its single instance.

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

## Working with the Hook Itself

`IGlobalHookProvider` contains the following methods for working with the global hook:

- `SetDispatchProc` – sets the function which will be called when an event is raised by libuiohook.
- `Run` – creates a keyboard and mouse global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunKeyboard` – creates a keyboard-only global hook and runs it on the current thread, blocking it until `Stop` is
called.
- `RunMouse` – creates a mouse-only global hook and runs it on the current thread, blocking it until `Stop` is called.
- `Stop` – stops the global hook.

You have to remember that only one global hook can exist at a time since calling `SetDispatchProc` will override the
previously set one. Also, running a global hook when another global hook is already running will corrupt the internal
global state of libuiohook.

`SetDispatchProc` accepts a delegate of type `SharpHook.Data.DispatchProc`. This delegate in turn accepts a
`SharpHook.Data.UioHookEvent` by reference, and returns nothing. You can pass `null` to `SetDispatchProc` in order
to unset the callback function.

`Run`, `RunKeyboard`, `RunMouse`, and `Stop` return a `SharpHook.Data.UioHookResult` which specifies whether the result
of the method was successful (`UioHookResult.Success`) or not (any other value).

> [!NOTE]
> There are several considerations to have in mind when using these methods directly. You can read more in the article
> on [custom global hooks](custom.md).

## Input Events

The `SharpHook.Data.UioHookEvent` struct contains information about events that have occured.

There are several event types supported by libuiohook (contained in the event's `Type` field and defined in the
`SharpHook.Data.EventType` enum).

The following are the general-purpose events:

- `HookEnabled` – raised when the `Run`/`RunKeyboard`/`RunMouse` method is called.
- `HookDisabled` – raised when the `Stop` method is called.

The following are the keyboard events, and `UioHookEvent` will contain more infomration in its `Keyboard` field:

- `KeyPressed` – raised when a key is pressed (its state changed from up to down).
- `KeyReleased` – raised when a key is released (its state changed from down to up).
- `KeyTyped` – raised when a key press may have caused a character to be typed regardless of whether a character was
actually typed.

The following are the mouse events, and `UioHookEvent` will contain more infomration in its `Mouse` field:

- `MouseClicked` – raised when a mouse button is clicked (pressed and released without dragging).
- `MousePressed` – raised when a mouse button is pressed (its state changed from up to down).
- `MouseReleased` – raised when a mouse button is released (its state changed from down to up).
- `MouseMoved` – raised when the mouse cursor is moved.
- `MouseMovedRelative` – raised when the mouse cursor is moved relatively to its previous location.
- `MouseDragged` – raised when the mouse cursor is dragged (moved while a button is down).
- `MouseDraggedRelative` – raised when the mouse cursor is dragged (moved while a button is down) relatively to its
previous location.

And the last one is also a mouse event, but `UioHookEvent` will contain more information in its `Wheel` field since it
has more information:

- `MouseWheel` – raised when the mouse wheel is scrolled.

`EventType` defines more types, but they are used only when simulating events.

`UioHookEvent` also contains the `Time` field which is the event's Unix timestamp.

`UioHookEvent` also contains the `Mask` field which contains the state of keyboard modifiers and the mouse state at the
time of the event. It also contains other bit flags with information about the event. Currently two such flags are
present:

- Distinguishing real events from simulated events (`EventMask.SimulatedEvent`). If this flag is set, then the event
is simulated. Otherwise, the event is real.

- Suppressing event propagation (`EventMask.SuppressEvent`). If it's set in the event handler then libuiohook will not
propagate the event further and it will effectively be blocked. This bit should be set synchronously, i.e., on the same
thread which handles the event. Supressing events works only on Windows and macOS. You can query support for event
suppression with `IFeatureProvider.GetOptionalFeatureSupport` – if it returns `EventSuppression` as one of the supported
features, then event suppression is supported.

> [!NOTE]
> `KeyTyped` and `MouseClicked` events are not raised by the OS, but by libuiohook itself. `KeyTyped` is raised after
> `KeyPressed` if the key press may have caused characters to be typed. Since a single key press can cause multiple
> characters to be typed, a single `KeyPressed` event can raise multiple `KeyTyped` events. `MouseClicked` is raised
> after `MouseReleased` if the cursor was not dragged. Since these events are raised by libuiohook, and not the OS,
> suppressing them has no effect.

`IGlobalHookProvider` also contains the `KeyTypedEnabled` property which specifies whether events of type `KeyTyped` are
enabled. The default value is `false` because when these events are enabled, they may cause slight system-wide side
effects. More information can be found in the article on [OS-specific constraints](os-constraints.md).

On Wayland, `KeyTyped` events are not suppored at all, so setting this property does nothing. You can query support for
`KeyTyped` events with `IFeatureProvider.GetOptionalFeatureSupport` – if it returns `KeyTypedEvents` as one of the
supported features, then these events are supported.

> [!NOTE]
> There are some limitations on Wayland regarding mouse events. You can read more in the article on
> [global hooks](hooks.md).

## Simulating Input Events

`IEventSimulationProvider` contains the `PostEvent` method for simulating input events. It accepts a `UioHookEvent`, but
it doesn't need all its fields. Only `Type` and `Keyboard`/`Mouse`/`Wheel` should be present. `PostEvent` returns
`UioHookResult` to indicate whether it was successful or not.

`IEventSimulationProvider` also contains the `PostEvents` method for simulating sequences of input events. It accepts an
array of `UioHookEvent` and its size. This method should be preferred to multiple calls to `PostEvent` when simulating
multiple events. On Windows, it simulates all events using a single Windows API call. On macOS and Linux, it simulates
each event one-by-one, but it's still slightly more efficient than multiple `PostEvent` calls as some structures are
only initialized once for all events. `PostEvent` also returns `UioHookResult` to indicate whether it was successful or
not. On Windows, either all events are simulated, or none are. On macOS and Linux, if a failure occurs in the middle of
the simulation sequence, then further events will not be simulated.

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
        Only <code>MouseEventData.X</code>, <code>MouseEventData.Y</code>, and
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
        Only <code>MouseEventData.X</code>, <code>MouseEventData.Y</code>, and
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
      <td><code>MouseMovedRelative</code></td>
      <td>Only <code>MouseEventData.X</code> and <code>MouseEventData.Y</code> are considered.</td>
    </tr>
    <tr>
      <td><code>MouseDragged</code></td>
      <td>Not recommended to use; same as <code>MouseMoved</code>.</td>
    </tr>
    <tr>
      <td><code>MouseDraggedRelative</code></td>
      <td>Not recommended to use; same as <code>MouseMovedRelative</code>.</td>
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

A positive `MouseWheelEventData.Rotation` value indicates scrolling up or left, and a negative value indicates scrolling
down or right.

On Windows, the value 120 represents the default wheel step. As such, multiples of 120 can be used as the
rotation value, but it's not required. The value of `MouseWheelEventData.Type` is ignored.

On macOS, it's recommended to use values between -10 and 10. This will result in quite a small scroll amount with pixel
scrolling, so `MouseWheelScrollType.BlockScroll` is recommended for line scrolling instead of pixel scrolling.

On Linux, like Windows, multiples of 120 should generally be used. The value of `MouseWheelEventData.Type` is ignored.

## Simulating Text Entry

SharpHook also provides text entry simulation. `IEventSimulationProvider` contains the `PostText` method which accepts
a `string`. The text to simulate doesn't depend on the current keyboard layout. The full range of UTF-16 (including
surrogate pairs, e.g., emojis) is supported.

X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to that
character, and then key press/release is simulated. Since the receiving application must react to the remapping, and
may not do so instantaneously, a delay is needed for accurate simulation. This means that text simulation on Linux works
slowly and is not guaranteed to be correct.

`IEventSimulationProvider` contains the `PostTextDelayLinux` property which can be used to increase (or decrease) the
delay if needed - longer delays add consistency but may be more jarring to end users. The default delay is 50
milliseconds. Delays are configurable on a nanosecond level. On Windows and macOS, `PostTextDelayLinux` does nothing and
always returns 0.

On Wayland, text entry simulation is not supported at all.

You can query support for text entry simulation with `IFeatureProvider.GetOptionalFeatureSupport` – if it returns
`PostText` as one of the supported features, then you can simulate text entry.

## Virtual Input Devices on Linux

On Linux low-level backends, virtual input devices are required to simulate events. The `InitializeVirtualDevices`
method in `IEventSimulationProvider` initializes these devices. It accepts the application name which is used to easily
identify these devices in case debugging is needed. The name can technically be `null` or empty, but it's discouraged
since the devices will not be easily identifyable. The name will also be truncated if its length is more than 62
characters.

The `DestroyVirtualDevices` method in `IEventSimulationProvider` destroys the virtual input devices. A disposed
simulator cannot be used anymore to simulate events.

Calling `InitializeVirtualDevices` multiple times is allowed, and it will not initialize virtual devices multiple times.
Instead, it will increment the reference counter, and then `DestroyVirtualDevices` will decrement it. Once the counter
reaches zero, the virtual devices will be destroyed.

On Windows and macOS, initializing and destroying virtual devices is a no-op, so the application name is not used at
all.

## Selecting a Linux Backend

On Linux, libuiohook supports three different backends and loads one of them at runtime when any libuiohook function is
called for the first time:

- Wayland - this backend uses libinput for the global hook and uinput for event simulation with some Wayland-specific
APIs.
- X11 - this backend uses libinput for the global hook and uinput for event simulation with some X11-specific APIs.
- XRecord - this backend uses XRecord for the global hook and XTest for event simulation. It is the same backend that
was used before version 8.

You can set the mode for loading the Linux backend using the `SetLinuxMode` method in `ILinuxBackendProvider`. It
accepts the `LinuxMode` enum which has the following values:

- `AutoXRecord` - this mode uses the Wayland backend on Wayland, and the XRecord backend on X11. This is the default
mode.
- `AutoLowLevel` - this mode uses the Wayland backend on Wayland, and the X11 backend on X11. It can be useful if you
want to have the same device access requirements regardless of the session type, and don't care about edge cases.
- `XRecord` - this mode always uses the XRecord backend, even on Wayland, as basically sets the behaviour of SharpHook
to be the same as before version 8.
- `X11` - this mode always uses the X11 backend.
- `Wayland` - this mode always uses the Wayland backend.

`SetLinuxMode` returns `UioHookResult` and can return one of the following values:

- `Success` - the mode has been changed successfully.
- `Failure` - the mode that was passed into the method is an invalid enum value.
- `ErrorLinuxLoadBackend` - the backend has already been loaded and changing the mode is not allowed anymore.

`ILinuxBackendProvider` also contains the `GetLinuxMode` method which returns the currently configured mode, as well as
the `GetLoadedLinuxBackend` method which returns the backend that was actually loaded.

> [!NOTE]
> The Wayland and X11 backends require elevated privileges. You can read more in the article on
> [OS constraints](os-constraints.md).

## macOS Accessibility API

macOS requires that the processes that use global hooks or event simulation have access to its Accessibility API. When
starting a global hook or simulating an event, libuiohook first checks whether the process has such access. If it
doesn't, then it returns `UioHookResult.ErrorAxApiDisabled`.

By default, when this error happens, a system dialog will appear and prompt the user to enable access to the
Accessibility API. However, if this is undesired, the `PromptUserIfAxApiDisabled` property in `IAccessibilityProvider`
can be set to control this behaviour. If it is set to `false` then all future tries will not result in a system dialog
appearing.

`IAccessibilityProvider` also contains the `IsAxApiEnabled` method to proactively check access to the Accessibility API.
If `true` is passed to this method, then a system dialog will appear if this access is disabled.

When a global hook starts on macOS, it creates a separate thread which continuously checks whether the Accessibility API
is still enabled for the process. If the user disables the access, then the hook will stop and return
`UioHookResult.ErrorAxApiRevoked`. The polling frequency can be set using the `AxPollFrequency` in
`IAccessibilityProvider`, the default is 1 second. It is recommended not to set this to a high value, because if the
user revokes Accessibility API access while the hook is running, then the entire OS will freeze until the hook is
destroyed. So, setting this value to, e.g., 10 seconds means that the user may experience up to 10 seconds of OS freeze.

These methods only make sense on macOS, and as such, their behaviour on Windows and Linux is the following:

- `IsAxApiEnabled` does nothing and always returns `true`.
- `PromptUserIfAxApiDisabled` does nothing and always returns `false`.
- `AxPollFrequency` does nothing and always returns 0.

## Querying Optional Feature Support

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
always return -1.

Windows and macOS support all of these features. X11 supports all of them, except `EventSuppression`. Wayland supports
none of these features.

## Logging

libuiohook can log messages throughout its execution. By default, it doesn't log anything, but `ILoggingProvider`
contains the `SetLoggerProc` method to set the log callback function – it will be called by libuiohook to log messages.

`SetLoggerProc` accepts a delegate of type `SharpHook.Data.LoggerProc`. This delegate in turn accepts a log level,
the message format (as a pointer) and arguments (also as a pointer).

> [!NOTE]
> You can read more about how to use the `SetLoggerProc` method in the article about [logging](logging.md), though it's
> not recommended to use it directly.

## Passing Custom Data to Callbacks

`SetDispatchProc` and `SetLoggerProc` also receive a pointer to user-supplied data as a parameter of type `nint`. It is
then passed to the callbacks – both `DispatcherProc` and `LoggerProc` receive user-supplied data as well.

In general, it's not recommended to use them. You should usually pass `IntPtr.Zero` to `SetDispatchProc` and
`SetLoggerProc` and not use the respective parameters in the callbacks.

The reason is that in order to use pointers to managed objects, they have to be pinned. As these callbacks tend to be
long-lived (probably as long as the program itself), the objects will have to be pinned for a long time as well, and
that's detrimental to the performance of the garbage collector and the memory layout of the program.

If you need to pass custom data to the callbacks, then use closures.

One of the ways this parameter may be useful, however, is to help with constraints of Mac Catalyst applications. These
applications require ahead-of-time compilation and they also require all callbacks that are called from native code
to be static and annotated with the `[MonoPInvokeCallback]` attribute. Since these methods must be static and annotated,
there is no way to pass additional data through closures. However, the user-supplied data can be used as a number (e.g.,
a key in a static dictionary which contains relevant data objects), not as a pointer. This is exactly how higher-level
classes use this parameter.
