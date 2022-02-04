# Migration Guide

In order to migrate from version 1 to version 2 there are several things that should be done - they are all quite small.

The `Start` method was removed from `IGlobalHook` and `IReactiveGlobalHook`. Instead these interfaces now provide two
methods: `Run` and `RunAsync`. `Run` is for running the global hook on the same thread and blocking that thread.
`RunAsync` behaves the same way that `Start` did before - it starts the hook on a separate thread and doesn't block
the calling thread.

Replace the invocation of `Start` to the invocation of `RunAsync` on a global hook if you simply want to keep the
previous behavior.

`EmptyDispatchProc` was removed from `UioHook`. If you want to unset the hook callback function, then call
`UioHook.SetDispatchProc(null)`.

The type of `UioHook.Reserved` was changed from `ushort` to a `ushort`-based enum so that its possible values are more
clear.

The type of `UioHook.Time` previously was `ushort` which was wrong. It was corrected to `ulong`. Note that this field
does not contain the event's timestamp.

The global hooks now throw an exception if the hook is started when it's already running. Also, possible exceptions are
now part of the interface definition. All unexpected exceptions that can happed when starting and stopping the hook are
now wrapped into `HookException` with the value of `UioHookResult.Failure`.
