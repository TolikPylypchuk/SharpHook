# OS-Specific Constraints

This article describes OS-specific constrains and other things that should be noted about OS support.

> [!IMPORTANT]
> Support for OS versions that are out of official support will not be provided. Issues that only affect older versions
> will most probably not be worked on, though contributions to fix them are welcome.

## Windows

### Supported Versions

Only Windows 11 is supported. SharpHook will most probably work on Windows versions as far back as at least Windows 7,
but issues that only affect older Windows versions will not be worked on.

### Supported Architectures

x86, x64, and Arm64 are supported. Arm32 is not supported since its support was dropped in .NET 5. Since x86 is quite
rare, issues that only affect x86 will most probably not be worked on.

### `KeyTyped` Events

It is recommended not to enable events of type `KeyTyped` if they are unused since they may cause system-wide side
effects:

- When a keyboard global hook is running with `KeyTyped` events enabled on versions of Windows older than Windows 10
1607, it will destroy dead keys rendering them unusable. This is because it uses the
[`ToUnicodeEx`](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-tounicodeex) function to
determine which characters are typed by key presses, and before Windows 10 1607, this function changed the dead key
state.

- When a keyboard global hook is running with `KeyTyped` events enabled, some editors may insert Unicode characters when
the user presses <kbd>Alt</kbd>+<kbd>Up Arrow</kbd> or <kbd>Alt</kbd>+<kbd>Down Arrow</kbd>. As of July 2025, on Windows
11, this issue can be reproduced in Microsoft Visual Studio, but on Windows 10, it can be reproduced in other editors as
well.

### Visual C++ Redistributable

The libuiohook build used by SharpHook on Windows is statically linked to the C runtime which means that client apps
don't need the Visual C++ Redistributable package.

An exception is the logging functionality – `LogEntryParser` uses the `vsprintf` function from the C runtime, so it
requires the Visual C++ Redistributable package on Windows, unlike the rest of SharpHook. If you don't want your app to
be dependent on this package, then you can use the `EmptyLogSource` class instead of `LogSource` in release builds of
your app. `EmptyLogSource` implements `ILogSource`, but never raises the `MessageLogged` event and doesn't subscribe to
libuiohook logs.

### DPI Awareness

An application manifest is required on Windows to enable DPI awareness for your app. If it's not enabled then mouse
coordinates will be wrong on high-DPI screens. You can look at the sample app in the SharpHook repository to see the
manifest example.

### Text Entry Simulation

On Windows, text simulation should work correctly and consistently.

## macOS

### Supported Versions

macOS 10.15+ is required, though only the currently supported macOS versions are supported by SharpHook. Issues that
only affect older versions will not be worked on.

Mac Catalyst is also supported, and version 13.1+ is required.

### Supported Architectures

x64 and Arm64 are supported, though issues that only affect x64 will not be worked on.

### Accessibility API

macOS requires that the Accessibility API access be enabled for the application if it wants to create a global hook or
simulate events. If the Accessiblity API access is not enabled, then `Run` and `PostEvent` will fail and return
`UioHookResult.ErrorAxApiDisabled`. More info can be found in the article on [low-level functionality](native.md).

### `KeyTyped` Events

It is recommended not to enable events of type `KeyTyped` if they are unused even though they cause no known system-wide
side effects on macOS.

### Main Run-Loop

On macOS, running the global hook requires that the main run-loop be present. libuiohook takes care of it if the hook
is run on the main thread. It's also taken care of by UI frameworks since they need an event loop on the main thread
to run. But if you're using a global hook in a console app or a background service and want to run it on some thread
other than the main one (e.g., `hook.RunAsync()` will run the hook on a different thread), then you should take care of
it yourself. You can do that by P/Invoking the native `CFRunLoopRun` function on the main thread:

```csharp
internal static partial class CoreFoundation
{
    private const string CoreFoundationLib = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

    // It's better to use a type derived from SafeHandle as the return type, but it's omitted for brevity
    [LibraryImport(CoreFoundationLib)]
    public static partial IntPtr CFRunLoopGetCurrent();

    [LibraryImport(CoreFoundationLib)]
    public static partial void CFRunLoopRun();

    [LibraryImport(CoreFoundationLib)]
    public static partial void CFRunLoopStop(IntPtr rl);
}

// ...

// This method must be called on the main thread
public static void RunMainLoop(CancellationToken token)
{
    var loop = CoreFoundation.CFRunLoopGetCurrent();
    token.Register(() => CoreFoundation.CFRunLoopStop(loop));
    CoreFoundation.CFRunLoopRun(); // This method will block the current thread until CFRunLoopStop is called
}

// ...

var tokenSource = new CancellationTokenSource();
hook.HookDisabled += (sender, e) => tokenSource.Cancel();
_ = hook.RunAsync(); // Ignore the result of RunAsync, do not await it
RunMainLoop(tokenSource.Token);
```

### Simulating Multiple Mouse Clicks

macOS doesn't recognize that a mouse was clicked multiple times when multiple press/release events have been simulated.
Instead, the click count must be explicitly provided. This is why `IEvenSimulator` contains the `SimulateMousePress` and
`SimulateMouseRelease` overloads with the `clicks` parameter. This parameter should be incremented for each
press/release, starting with `1`.

### Text Entry Simulation

On macOS, applications are not required to process text entry simulation, but most of them should handle it correctly.

## Linux

### Supported Distributions

Linux distributions supported by .NET are supported by SharpHook. It may work on other distributions, but if it doesn't,
then the problems will most probably not be worked on.

Only the currently supported OS versions are supported by SharpHook. Problems that only affect older versions will not
be worked on.

### Supported Architectures

x64, Arm32, and Arm64 are supported. x86 is not supported by .NET itself. Problems that only affect Arm32 will most
probably not be worked on.

### libuiohook Backends

Both X11 and Wayland are supported, though there are some limitations on Wayland.

On Linux, SharpHook provides 4 libuiohook files: libuiohook.so, libuiohook-xrecord.so, libuiohook-x11.so, and
libuiohook-wayland.so. The first file is a thin loader which simply loads one of the other 3 files and delegates all
functionality to it.

The backends have the following behaviour:

- The Wayland backend (libuiohook-wayland.so) uses libinput for the global hook and uinput for event simulation. It
needs elevated privileges to work, as described below. This is the default backend on Wayland sessions.

- The X11 backend (libuiohook-x11.so) also uses libinput for the global hook and uinput for event simulation. It also
needs elevated privileges to work and uses X11 APIs to bridge some gaps in the low-level functionality.

- The XRecord backend (libuiohook-xrecord.so) uses only X11 APIs: XRecord for the global hook and XTest for event
simulation. This is the default backend on X11 sessions and has the same behaviour as SharpHook before version 8.

The X11 backend is more constrained than the XRecord backend (for example, it cannot see events simulated by other
applications via XTest), but it may be useful to have permission parity with the Wayland backend, or if edge cases are
not important.

The backend can be selected using `ILinuxBackendProvider.SetLinuxMode`. This method must be called before any other
SharpHook API is used since any other method or property will cause a selected backend to be loaded, and a loaded
backend cannot be changed. There are 2 exceptions: calling `ILoggingProvider.SetLoggerProc` or
`IGlobalHookProvider.SetDispatchProc` will not cause a backend to be loaded, and the respective functions will be passed
on to the backend once its loaded.

### Wayland Limitations

Wayland support is much more limited than X11 support since Wayland itself is much more restrictive.

Since Wayland does not provide a way to get the absolute mouse coordinates, they are mostly not supported. More info can
be found in the article on [global hooks](hooks.md).

`KeyTyped` events are not supported on Wayland. Text entry simulation is also not supported on Wayland.

Mouse pointer properties defined in the `IMouseInfoProvider` are not supported. `GetPointerAccelerationMultiplier`,
`GetPointerAccelerationThreshold`, and `GetPointerSensitivity` always return -1. `GetMultiClickTime` always returns
a hard-coded value of 400ms which is the default multi-click time on GNOME and KDE.

Key pressed events are not auto-repeated by libinput, so the Wayland and X11 backends do not support auto-repeat.

The connection to a Wayland compositor is needed only when running a mouse hook and getting screen info or keyboard
info.

### Required Permissions

The XRecord backend does not require any special permissions. The X11 and Wayland backends require access to
`/dev/input` for global hooks and access to `/dev/uinput` for event simulation.

This does not mean that `root` access is needed – there are other ways to achieve this. If you decide to run your
application as `root` anyway, then bear in mind that you will need to pass the existing connection to a Wayland
compositor or an X11 display to your application since it won't have those by default.

Another way to get the necessary permissions is to create a udev rule which grants them to your user. This is
recommended for development as it simplifies your workflow, but this also grants permissions to read and write input
events globally to all applications that you run, so it kind of defeats Wayland's security model.

Alternatively, you can add your user to the `input` group, but it's not recommended since udev rules are a more modern
alternative, and the `input` group does not cover access to `/dev/uinput` anyway.

> [!IMPORTANT]
> This article does not cover all possible ways to run your application with the necessary permissions as this is out of
> scope of SharpHook. You need to decide for yourself which permission model works for your application.

As an example, here's what's needed to grant permissions using udev rules to the currently logged in user.

1. Create a file named `/etc/udev/rules.d/70-[your-app].rules` (substituting `[your-app]` with the name of your choice)
with the following content:

```
ACTION=="remove", GOTO="app_end"

SUBSYSTEM=="input", KERNEL=="event*", TAG+="uaccess"
KERNEL=="uinput", SUBSYSTEM=="misc", TAG+="uaccess"

LABEL="app_end"
```

2. Run the following command in the terminal:

```sh
sudo udevadm control --reload-rules && sudo udevadm trigger
```

The filename must start with a number less than 73, because udev rules are applied in order, and rule 73 applies
permissions for the uaccess tag.

Note that this is just an example that can be used to quickstart development on Wayland, or the low-level X11 backend.
As stated above, you must decide how to grant permissions to your application on your own.

### Virtual Input Devices

On the Wayland and the low-level X11 backend, virtual input devices are required to write events to `/dev/uinput`. They
are initialized automatically when creating an instance of `EventSimulator`, but if you use low-level functionality
providers, then you must initialize and destroy them yourself using methods `IEventSimulationProvider`.

### `KeyTyped` Events

It is recommended not to enable events of type `KeyTyped` if they are unused since these events interfere with Chinese
input methods on Linux.

### Text Entry Simulation

X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to that
character, and then key press/release is simulated. Since the receiving application must react to the remapping, and
may not do so instantaneously, a delay is needed for accurate simulation. This means that text simulation on Linux works
slowly and is not guaranteed to be correct. More info can be found in the article on
[low-level functionality](native.md).

### Suppressing Events

Suppressing events is not supported on Linux – neither on X11 nor on Wayland.
