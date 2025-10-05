# Custom Global Hooks

## Custom Implementations of `IGlobalHook`

If you want an `IGlobalHook` with a custom dispatch logic, you should extend the `SharpHook.GlobalHookBase` class. It
implements the logic for running, stopping, and disposing of the global hook, as well as raising appropriate events.
It also keeps a reference to a running global hook so that it's not garbage-collected. You only need to implement the
`HandleHookEvent` method, and call the `DispatchEvent` inside it in the way you see fit. You can also call
`ShouldDispatchEvent` before executing any logic to determine whether the event should be dispatched (i.e. it has any
subscribers).

As an example, here's the implementation of `SimpleGlobalHook` (with XML comments removed):

```csharp
namespace SharpHook;

public sealed class SimpleGlobalHook : GlobalHookBase
{
    public SimpleGlobalHook(
        GlobalHookType globalHookType = GlobalHookType.All,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
        : base(globalHookType, globalHookProvider, runAsyncOnBackgroundThread)
    { }

    protected override void HandleHookEvent(ref UioHookEvent e) =>
        this.DispatchEvent(ref e);
}
```

As you can see, it really is exceedingly simple!

`GlobalHookBase` also provides the `BeforeRun` and `AfterStop` methods to execute custom logic before a global hook
starts running and after it finishes running. You can also override its `Dispose` method. You must call the base
method inside the overriden method. For example, `EventLoopGlobalHook` overrides `BeforeRun` to start a separate thread
with an event loop, and `Dispose` to stop it.

Additionally, `GlobalHookBase` provides virtual methods that raise appropriate events like `OnHookEnabled` and
`OnKeyPressed`.

## Custom Global Hooks With Other Event Forms

SharpHook provides the `IBasicGlobalHook` interface and the `BasicGlobalHookBase` class which act as the roots of the
entire hierarchy of global hook classes and interfaces in SharpHook.

`IBasicGlobalHook` represents basic operations for a global keyboard and mouse hook regardless of the form of its
events. It extends `IDisposable` and defines three methods: `Run`, `RunAsync`, and `Stop`, as well as two properties:
`IsRunning` and `IsDisposed`. `BasicGlobalHookBase` is an abstract class which implements these methods and leaves out
only the actual handling of the events.

If you want to create a custom global hook which doesn't implement `IGlobalHook` (e.g. if you want to implement
`IReactiveGlobalHook` or `IR3GlobalHook`, or have a different interface altogether) then it's highly recommended to
extend `BasicGlobalHookBase` since this way you will only need to implement handling the events, and all other logic
(which is actually non-trivial) is handled for you.

The following methods provided in `GlobalHookBase` are actually provided in `BasicGlobalHookBase` which `GlobalHookBase`
itself extends: `HandleHookEvent`, `BeforeRun`, `AfterStop`, and `Dispose`.

As an example, here's a global hook which raises a single event for all hook events instead of different events for each
event type:

```csharp
public sealed class StraightforwardGlobalHook : BasicGlobalHookBase
{
    protected override void HandleHookEvent(ref UioHookEvent e) =>
        this.HookEvent?.Invoke(this, e);

    public event EventHandler<UioHookEvent>? HookEvent;
}
```

## Using Low-Level Functionality Directly

As mentioned above, it's highly recommended to extend `BasicGlobalHookBase` if you want to implement a custom global
hook, but you may nonetheless want to implement everything from scratch. You can read more about that in the article on
[low-level functionality](native.md).

When calling `SetDispatchProc`, the function must be wrapped into a delegate reference and the reference must be stored
to prevent garbage collection. This is because the following code:

```csharp
provider.SetDispatchProc(someObj.SomeMethod, IntPtr.Zero);
```

is actually transformed into this code by the C# compiler:

```csharp
provider.SetDispatchProc(new DispatchProc(someObj.SomeMethod), IntPtr.Zero);
```

The CLR protects the `DispatchProc` reference from being garbage-collected only until the `SetDispatchProc` methods
exits (which happens almost instantly). The CLR does not and cannot know that the reference will be used later and so it
will happily collect this reference thinking it's not needed anymore. Instead, the following should be done:

```csharp
DispatchProc dispatchProc = someObj.SomeMethod; // This reference should be stored, e.g., as a field of the object
provider.SetDispatchProc(dispatchProc, IntPtr.Zero);
```

Additionally, if you need to support Mac Catalyst, there are two conditions that the event callback must satisfy:

- The method must be `static`
- The method must be decorated with the `[ObjCRuntime.MonoPInvokeCallback(typeof(DispatchProc))]` attribute

`BasicGlobalHookBase` does all this for you so that you don't have to.

If you look through the source code of `BasicGlobalHookBase`, you will notice that even though running multiple global
hooks at the same time is expressly forbidden and this is emphasized in multiple places in the docs, the class itself
still supports running multiple global hooks at the same time. Even though the event callback is static, the class has
a dictionary of running global hooks where the key is the object's index (that's incremented in the constructor), and
this index is passed as the second parameter of `SetDispatchProc` (and this behaviour is also discouraged). So why do
that? The answer is simple – `BasicGlobalHookBase` itself just doesn't care that there is such a limitation in place.
This is a limitation of libuiohook, and in theory, you could implement a custom `IGlobalHookProvider` which supports
multiple running global hooks, and `BasicGlobalHookBase` will work without problems with such a provider. Or you could
instantiate multiple global hooks with a different provider each, and they will also work without problems.
