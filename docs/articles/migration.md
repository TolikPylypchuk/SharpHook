# Migration Guide

## Version 4 to 5

Version 5 contains multiple breaking changes, so you may need to change your code to upgrade.

Almost all `KeyCode` values were changed. New codes were added and some were removed. Before upgrading, make sure that
no numeric key code values are persisted since they will be incompatible. The actual enum values are meaningless and
may be changed again in the future. Make sure to only use the enum constant names, e.g. when persisting key codes.

Mouse wheel events were changed. `MouseWheelEventData` doesn't contain `Amount` anymore - instead only `Rotation` should
be considered the value of the scroll. Mouse wheel simulation was changed as well and doesn't accept the amount anymore.
Instead it accepts the scroll direction and type which makes it possible to simulate horizontal scrolling.

`KeyboardEventData.KeyChar` was renamed to `KeyboardEventData.RawKeyChar`. Since the key char is of type `ushort` for
marshalling reasons but should be used as a `char`, the `KeyboardEventData.KeyChar` property was added, which simply
casts `KeyboardEventData.RawKeyChar` to `char`.

`LogEntryParser` is now a singleton and doesn't have a public constructor.

The constants in the `MouseWheelScrollDirection` enum were shortened: `VerticalDirection` to `Vertical` and
`HorizontalDirection` to `Horizontal`.

All methods in all structs in the `UioHook.Native` namespace are now marked as `readonly`.

The minimum .NET Framework version was bumped to 4.6.2 since 4.6.1 is not supported anymore. Support for Windows on
Arm32 was removed since it was removed in .NET 5.

Support for Mac Catalyst was added which makes it possible to use SharpHook in MAUI apps on macOS.

SharpHook.Reactive now depends on Rx.NET 6.0.0.

## Version 3 to 4

Version 4 contains a couple breaking changes, so you may need to change your code to upgrade.

The biggest change is that simulating mouse button pressing/releasing now requires mouse pointer coordinates. They were
actually always required by libuiohook, so previously the buttons were always pressed/released at (0, 0).
`EventSimulator.SimulateMousePress` and `EventSimulator.SimulateMouseRelease` now have the following parameters:
`short x, short y, MouseButton button`.

`HookEventArgs` doesn't contain the `Reserved` property anymore as its purpose wasn't really clear. Now `HookEventArgs`
contains the `SuppressEvent` property - set it to `true` inside an event handler to suppress the event.

On .NET 7 `[LibraryImport]` is now used instead of `[DllImport]`. This change required making `UioHookEvent` a blittable
type, and as a result, the type of `KeyboardEventData.KeyChar` was changed from `char` to `ushort`. The field should
still be used as a `char`.

Explicit targets for .NET 5 and .NET Core 3.1 were removed, though the library can be used on those platforms through
.NET Standard.

`KeyCode.VcPrintscreen` was renamed to `KeyCode.VcPrintScreen`.

When simulating mouse wheel events on Windows, their rotation value was previously multiplied by 120. This behavior was
removed.

The ability to make `RunAsync` create a background thread was added - you can now specify that a running hook won't stop
the application from exiting if all other threads have finished executing.

Versioned libuiohook binaries for macOS and Linux were removed from the NuGet package as they were bit-for-bit same as
the unversioned binaries.

## Version 2 to 3

Version 3 contains several breaking changes, so you may need to change your code to upgrade.

libuiohook is now at version 1.3 and contains breaking changes which propagated to the `UioHook` class and a few other
types.

`UioHook.SetDispatchProc` now receives a pointer to user-supplied data. This pointer is then passed to the callback, so
`DispatchProc` also receives it. You shouldn't ever use this functionality (you should pass `IntPtr.Zero` to
`SetDispatchProc`). View the article about [low-level features](native.md) for more info.

Previously event masks were ignored when simulating events on Windows. Now event masks are always ignored when
simulating events, so the methods of `IEventSimulator` and `EventSimulator` don't have the optional second parameter
which specifies the event's mask anymore. It's not possible to simulate Windows events with masks directly the way macOS
and Linux allow it, so this functionality was removed from libuiohook on other OSes as well.

`UioHook.PostEvent` now returns `UioHookResult` instead of being `void`. Thus, all methods of `IEventSimulator` and
`EventSimulator` also return `UioHookResult`.

`UioHookEvent.Time` now contains the event's UNIX timestamp, and `HookEventArgs` now contains the `EventTime`
property - a `DateTimeOffset` object derived from the timestamp.

You can now get libuiohook logs if you need to log its execution, which was not possible before.

Other changes were done independently of libuiohook.

`IGlobalHook` and `IReactiveGlobalHook` now contain the `IsDisposed` property.

The `HookEvent<TArgs>` class was removed from SharpHook.Reactive. The observables of `IReactiveGlobalHook` now emit
`HookEventArgs` or a derived type directly. If you need the sender of the event, then use closures.

## Version 1 to 2

In order to migrate from version 1 to version 2 there are several things that should be done - they are all quite small.

The `Start` method was removed from `IGlobalHook` and `IReactiveGlobalHook`. Instead these interfaces now provide two
methods: `Run` and `RunAsync`. `Run` is for running the global hook on the same thread and blocking that thread.
`RunAsync` behaves the same way that `Start` did before - it starts the hook on a separate thread and doesn't block
the calling thread.

Replace the invocation of `Start` to the invocation of `RunAsync` on a global hook if you simply want to keep the
previous behavior.

`EmptyDispatchProc` was removed from `UioHook`. If you want to unset the hook callback function, then call
`UioHook.SetDispatchProc(null)`.

The type of `UioHookEvent.Reserved` was changed from `ushort` to a `ushort`-based enum so that its possible values are
more clear.

The type of `UioHookEvent.Time` previously was `ushort` which was wrong. It was corrected to `ulong`. Note that this
field does not contain the event's timestamp.

The global hooks now throw an exception if the hook is started when it's already running. Also, possible exceptions are
now part of the interface definition. All unexpected exceptions that can happen when starting and stopping the hook are
now wrapped into `HookException` with the value of `UioHookResult.Failure`.
