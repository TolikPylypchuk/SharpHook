# Migration Guide

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

## Version 2 to 3

Version 3 contains several breaking changes, so you may need to change your code to upgrade.

libuiohook is now at version 1.3 and contains breaking changes which propagated to the `UioHook` class and a few other
types.

`UioHook.SetDispatchProc` now receives a pointer to user-supplied data. This pointer is then passed to the callback, so
`DispatchProc` also receives it. You shouldn't ever use this functionality (you should pass `IntPtr.Zero` to
`SetDispatchProc`). View the API docs of `UioHook` and `DispatchProc` for more info.

Previously event masks were ignored when simulating events on Windows. Now event masks are always ignored when
simulating events, so the methods of `IEventSimulator` and `EventSimulator` don't have the optional second parameter
which specifies the event's mask anymore. It's not possible to simulate Windows events with masks directly the way macOS
and Linux allow it, so this functionality was removed from libuiohook on other OSes as well.

`UioHook.PostEvent` now returns `UioHookResult` instead of being `void`. Thus, all methods of `IEventSimulator` and
`EventSimulator` also return `UioHookResult`.

`UioHookEvent.Time` now contains the event's UNIX timestamp, and `HookEventArgs` now contains the `EventTime`
property - a `DateTimeOffset` object derived from the timestamp.

You can now get libuiohook logs if you need to log its execution, which was not possible before.

Other chages were done independently of libuiohook.

`IGlobalHook` and `IReactiveGlobalHook` now contain the `IsDisposed` property. If you have classes derived from these
interfaces then you need to add this property.

The `HookEvent<TArgs>` class was removed from SharpHook.Reactive. The observables of `IReactiveGlobalHook` now emit
`HookEventArgs` or a derived type directly. If you need the sender of the event, then use closures.

Next article: [About SharpHook](about.md).
