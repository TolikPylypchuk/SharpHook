# OS-Specific Constraints

This article describes OS-specific constrains and other things that should be noted about OS support.

## Windows

### Supported Versions

Only Windows 10 and 11 are supported.

### `KeyTyped` Events

It is recommended to disable events of type `KeyTyped` if they are unused since they may cause system-wide side effects:

- When a keyboard global hook is running with `KeyTyped` events enabled on versions of Windows older than Windows 10
1607, it will destroy dead keys rendering them unusable. This is because it uses the
[`ToUnicodeEx`](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-tounicodeex) function to
determine which characters are typed by key presses, and before Windows 10 1607 this function changed the dead key
state.

- When a keyboard global hook is running with `KeyTyped` events enabled, some editors may insert Unicode characters when
the user presses <kbd>Alt</kbd>+<kbd>Up Arrow</kbd> or <kbd>Alt</kbd>+<kbd>Down Arrow</kbd>. As of July 2025, on Windows
11, this issue can be reproduced in Microsoft Visual Studio, but on Windows 10, it can be reproduced in other editors as
well.

### Supported Architectures

x86, x64, and Arm64 are supported. Arm32 is not supported since its support was dropped in .NET 5.

### Visual C++ Redistributable

The libuiohook build used by SharpHook on Windows is statically linked to the C runtime which means that client apps
don't need the Visual C++ Redistributable package.

An exception is the logging functionality – `LogEntryParser` uses the `vsprintf` function from the C runtime, and as
such, it requires the Visual C++ Redistributable package on Windows, unlike the rest of SharpHook. If you don't want
your app to be dependent on this package, then you can use the `EmptyLogSource` class instead of `LogSource` in release
builds of your app. `EmptyLogSource` implements `ILogSource`, but never raises the `MessageLogged` event and doesn't
subscribe to libuiohook logs.

### DPI Awareness

An application manifest is required on Windows to enable DPI awareness for your app. If it's not enabled then mouse
coordinates will be wrong on high-DPI screens. You can look at the sample app in the SharpHook repository to see the
manifest example.

### Text Entry Simulation

On Windows, text simulation should work correctly and consistently.

## macOS

### Supported Versions

macOS 10.15+ is supported. Mac Catalyst 13.1+ is supported.

### Supported Architectures

x64 and Arm64 are supported.

### Accessibility API

macOS requires that the Accessibility API access be enabled for the application if it wants to create a global hook or
simulate events. If the Accessiblity API access is not enabled, then `Run` and `PostEvent` will fail and return
`UioHookResult.ErrorAxApiDisabled`. More info can be found in the [article on low-level functionality](native.md).

### Main Run-Loop

On macOS running the global hook requires that the main run-loop be present. libuiohook takes care of it if the hook
is run on the main thread. It's also taken care of by UI frameworks since they need an event loop on the main thread
to run. But if you're using a global hook in a console app or a background service and want to run it on some thread
other than the main one then you should take care of it yourself. You can do that by P/Invoking the native
`CFRunLoopRun` function on the main thread.

### Simulating Multiple Mouse Clicks

macOS doesn't recognize that a mouse was clicked multiple times when multiple press/release events have been simulated.
Instead, the click count must be explicitly provided. This is why `IEvenSimulator` contains the `SimulateMousePress` and
`SimulateMouseRelease` overloads with the `clicks` parameter. This parameter should be incremented for each
press/release, starting with `1`.

### Text Entry Simulation

On macOS, applications are not required to process text entry simulation, but most of them should handle it correctly.

## Linux

### Supported Distributions

Linux distributions supported by .NET are supported by SharpHook. It may work correctly on other distributions, but if
it doesn't then the problems will most probably not be fixed.

### Supported Architectures

x64, Arm32, and Arm64 are supported. x86 is not supported by .NET itself.

### X11 and Wayland

Only X11 is supported. Wayland support may be available in a future version.

### Text Entry Simulation

X11 doesn't support text simulation directly. Instead, for each character, an unused key code is remapped to that
character, and then key press/release is simulated. Since the receiving application must react to the remapping, and
may not do so instantaneously, a delay is needed for accurate simulation. This means that text simulation on Linux works
slowly and is not guaranteed to be correct. More info can be found in the
[article on low-level functionality](native.md).
