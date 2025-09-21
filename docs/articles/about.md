# About SharpHook

SharpHook – created by [Tolik Pylypchuk](https://tolik.io).

## Library Status

If you've noticed that this library hasn't gotten new commits in some time, rest assured that it's not abandoned!
I'm not giving up on this library any time soon.

## Building from Source

In order to build this library, you'll first need to get libuiohook binaries. You you can get a
[nightly build from this repository](https://github.com/TolikPylypchuk/SharpHook/actions/workflows/build.yml), or you
can build them yourself as instructed in the [libuiohook fork](https://github.com/TolikPylypchuk/libuiohook) that
SharpHook uses (not recommended as it's non-trivial, and you should most probably use the same options that the build in
this repository uses anyway).

Place the binaries into the appropriate directories in the `SharpHook` project, as described in the following table:

<table>
  <tr>
    <th>OS</th>
    <th>File</th>
    <th>Source directory</th>
    <th>Target directory</th>
  </tr>
  <tr>
    <th>Windows</th>
    <td>uiohook.dll</td>
    <td>windows/&lt;platform&gt;/bin</td>
    <td>lib/win-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>macOS</th>
    <td>libuiohook.dylib</td>
    <td>macos/&lt;platform&gt;/lib</td>
    <td>lib/osx-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>Mac Catalyst</th>
    <td>libuiohook.dylib</td>
    <td>catalyst/&lt;platform&gt;/lib</td>
    <td>lib/maccatalyst-&lt;platform&gt;</td>
  </tr>
  <tr>
    <th>Linux</th>
    <td>libuiohook.so</td>
    <td>linux/&lt;platform&gt;/lib</td>
    <td>lib/linux-&lt;platform&gt;</td>
  </tr>
</table>

With libuiohook in place, you can build SharpHook using your usual methods, e.g. with Visual Studio or the `dotnet` CLI.
You need .NET 9 to build SharpHook.

## Changelog

### [v7.0.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v7.0.2) (September 21, 2025)

- Mouse move events are not dropped based on previous click positions on Windows anymore.

- Simulating text entry with characters like `\n` or `\t` now works correctly on Linux.

- libuiohook was updated to commit
[feafae0](https://github.com/TolikPylypchuk/libuiohook/tree/feafae0be8bcf539cb792bb16cd8a1434f437866).

### [v7.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v7.0.1) (August 17, 2025)

- The global hook correctly discerns right and left modifier keys again on Windows.

- libuiohook was updated to commit
[e8b4814](https://github.com/TolikPylypchuk/libuiohook/tree/e8b48146042607570237df006b07d75c6e49c3f8).

### [v7.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v7.0.0) (August 10, 2025)

#### Breaking Changes

- `SimpleReactiveGlobalHook` was renamed to `ReactiveGlobalHook`.

- The `Run`, `RunAsync`, and `Stop` methods as well as the `IsRunning` and `IsDisposed` properties were moved from
`IGlobalHook` and `IReactiveGlobalHook` into `IBasicGlobalHook` which both `IGlobalHook` and `IReactiveGlobalHook` now
extend.

- Because of the above change, `RunAsync` for reactive global hooks now returns a `Task` instead of an
`IObservable<Unit>`.

- `ReactiveLogSourceAdapter` now contains a single constructor with a default parameter instead of two constructors.

#### New Features

- SharpHook.R3 – a new package for integration with R3 was added.

- `EventLoopGlobalHook` – a new implementation of `IGlobalHook` – was added.

- `IBasicGlobalHook` and `BasicGlobalHookBase` were added for easier creation of custom global hooks with different
event forms.

- `ReactiveLogSourceAdapter` now implements `ILogSource` in addition to `IReactiveLogSource`.

#### Bug Fixes

- On Windows, global hooks now correctly report the key that was pressed or released on non-QWERTY layouts.

#### Other Changes

- libuiohook was updated to commit
[a2cba5f](https://github.com/TolikPylypchuk/libuiohook/tree/a2cba5f125072dad8683e812027a561e598c7a37).

### [v6.2.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v6.2.0) (July 19, 2025)

- The ability to disable events of type `KeyTyped` was added.

- Simulation of keyboard events was fixed on macOS.

- libuiohook was updated to commit
[4217638](https://github.com/TolikPylypchuk/libuiohook/tree/42176381d991fc009b369295121bb2bccc9ff705).

### [v6.1.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v6.1.2) (June 22, 2025)

- Simulation of letter keys in different layouts was fixed on Windows – the same keys are simulated regardless of the
current keyboard layout (e.g., QWERTY or AZERTY).

- libuiohook was updated to commit
[a8d1578](https://github.com/TolikPylypchuk/libuiohook/tree/a8d1578835f0d88d751a31153a1169208c389039).

### [v6.1.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v6.1.1) (June 3, 2025)

- Simulation of mouse button press and release for buttons 2, 3, 4, and 5 was fixed on Linux.

- The location of simulated mouse events was fixed on macOS.

- libuiohook was updated to commit
[0e77cac](https://github.com/TolikPylypchuk/libuiohook/tree/0e77cac5b88369c3e1bb3cdb8c605728ea19c767).

### [v6.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v6.1.0) (May 23, 2025)

- The ability to control the behaviour of `TestGlobalHook` and `TestProvider` (simple dispatching from version 5 vs an
event loop from version 6) was added. The simple behaviour from version 5 is the default as it is much easier to use.

### [v6.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v6.0.0) (May 18, 2025)

#### Platform Support

- Support for Mac Catalyst on non-MAUI applications was added.

- The minimum .NET Framework version was bumped to 4.7.2. Direct support for .NET 6 and 7 was removed, but it's still
available through .NET Standard.

#### Breaking Changes

- All structs and enums were moved from `SharpHook.Native` to `SharpHook.Data`.

- `ModifierMask` and `EventReservedValueMask` were merged into a single enum – `EventMask`.

- `UioHookEvent.Reserved` was removed. Its values are now part of `UioHookEvent.Mask`.

- All constructors of `SimpleGlobalHook`, `TaskPoolGlobalHook`, `SimpleReactiveGlobalHook`, and
`ReactiveGlobalHookAdapter` except the main ones were removed.

- `TaskPoolGlobalHookOptions` was removed.

- All obsolete values in the `KeyCode` enum were removed.

- `LogSource.Register` was removed.

- Members of `HookException` related to binary formatters were removed.

- The behaviour of `TestGlobalHook` and `TestProvider` was changed. Instead of dispatching events in the same thread
that simulates them, they now instead add events into an event loop running on a different thread.

- `TestProvider` now throws an exception if `Run` or `RunAsync` is called and it's already running.

#### New Features

- The ability to stop global hooks without disposing of them was added.

- Several functions for working with the macOS Accessibility API were added.

- `UioHookEvent.Mask` now contains the state of modifiers and mouse buttons regardless of the global hook type
(keyboard, mouse, or both).

- Running global hooks won't be garbage-collected.

#### Bug Fixes

- Getting libuiohook logs on Mac Catalyst applications was fixed.

- The issue with global hooks breaking Alt codes on Windows was fixed.

- Getting events for Caps Lock was fixed on macOS.

- Disabling access to the Accessibility API when a global hook is running on macOS won't freeze the OS anymore.

- Disposing a `TaskPoolGlobalHook` inside an event handler doesn't throw an exception anymore.

#### Other Changes

- libuiohook was updated to commit
[c61b486](https://github.com/TolikPylypchuk/libuiohook/tree/c61b4863caa7def54349d1426ef431414466f8b9).

- SharpHook.Reactive now depends on Rx.NET 6.0.1.

- Docs for older versions were removed. From now on, only the latest version will have docs hosted on
[sharphook.tolik.io](https://sharphook.tolik.io).

### [v5.3.9](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.9) (March 29, 2025)

- The state of the modifiers is now queried on every event instead of tracking them internally which removes
inconsistencies when some events are skipped.

- .NET 9 was added as a target.

- libuiohook was updated to commit
[961ed5a](https://github.com/TolikPylypchuk/libuiohook/tree/961ed5aca62229d6fe38fd33f731dac75b3d6639).

### [v5.3.8](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.8) (September 27, 2024)

- libuiohook was updated to commit
[ce96f42](https://github.com/TolikPylypchuk/libuiohook/tree/ce96f4239e5ff7337e11f94ca5601089f82f306f)
which fixes mouse button release events for mouse buttons 2 and 3 on Linux.

### [v5.3.7](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.7) (July 9, 2024)

- libuiohook was updated to commit
[1c96539](https://github.com/TolikPylypchuk/libuiohook/tree/1c965395be9b8ac65d5f6a8d6967fa65cc709125)
which fixes a CPU usage issue on Linux.

### [v5.3.6](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.6) (May 22, 2024)

- SharpHook now provides the correct version of libuiohook for macOS x64.

### [v5.3.5](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.5) (May 20, 2024)

- Simulating mouse events when the screen configuration changes was fixed on Windows.

- Simulating text with repeating characters was fixed on Windows.

- libuiohook was updated to commit
[c7500ce](https://github.com/TolikPylypchuk/libuiohook/tree/c7500ce3b96756cddfd486e819ae576d6b05f1f5).

### [v5.3.4](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.4) (May 5, 2024)

- Simulating some keys was fixed on Windows.

- libuiohook was updated to commit
[5128c04](https://github.com/TolikPylypchuk/libuiohook/tree/5128c04655591526c3610dd5fe0fad29d2237bcd).

### [v5.3.3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.3) (April 28, 2024)

- Simulating mouse movement on multiple screens was fixed on Windows.

- libuiohook was updated to commit
[902a30b](https://github.com/TolikPylypchuk/libuiohook/tree/902a30bddacf4f955c5caf13c5f4ea83df8ae5ef).

### [v5.3.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.2) (April 2, 2024)

- Publishing applications which reference SharpHook with Visual Studio should now work correctly.

- Mouse movement simulation is now more precise on Windows.

- libuiohook was updated to commit
[5024193](https://github.com/TolikPylypchuk/libuiohook/tree/5024193088ae91d2fe6ea68c70f5e6969c7d6595).

### [v5.3.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.1) (March 3, 2024)

- Disposing an already disposed test global hook is now safe and won't throw an exception.

### [v5.3.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.3.0) (February 24, 2024)

- It's now possible to run keyboard-only or mouse-only global hooks.

- Simulating key press/release events was fixed for some keys (such as media keys) on Windows.

- Global hooks and the event simulator now don't throw an exception on null parameters and instead use default values.

- libuiohook was updated to commit
[963415f](https://github.com/TolikPylypchuk/libuiohook/tree/963415f8e9ba486bd1ff68d2f8adf903ce5b3b27).

### [v5.2.3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.2.3) (February 2, 2024)

- Windows x86 support was fixed for .NET Framework.

### [v5.2.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.2.2) (January 13, 2024)

- The package targets are now executed only for .NET Framework since they aren't needed for .NET/.NET Core.

- libuiohook resolution for Linux Arm64 was fixed in package targets.

### [v5.2.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.2.1) (January 3, 2024)

- `HookEventArgs.IsEventSimulated` has been fixed – previously it always returned `true`.

- `kCGEventTapDisabledByTimeout` doesn't break the global hook on macOS anymore.

- The packages are now marked as AOT-compatible.

- libuiohook was updated to commit
[7dbdc09](https://github.com/TolikPylypchuk/libuiohook/tree/7dbdc09cd9d5a8ef5be5bb005e853b70ce28d67f).

### [v5.2.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.2.0) (December 16, 2023)

- Simulated events can now be distinguished from real events using the `HookEventArgs.IsEventSimulated` property.

- Key codes which aren't present on modern keyboards have been deprecated.

- libuiohook was updated to commit
[d710f0f](https://github.com/TolikPylypchuk/libuiohook/tree/d710f0f57194604f98a67514b8c86f442a7bbc3d).

### [v5.1.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.1.2) (November 25, 2023)

- An issue with key events changing the way some applications handle Alt keys on Windows was fixed.

- .NET 8 was added as a target.

- libuiohook was updated to commit
[9027a77](https://github.com/TolikPylypchuk/libuiohook/tree/9027a77d597da721134d5652d77f293cec76f0b9).

### [v5.1.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.1.1) (October 13, 2023)

- An issue with key events not being simulated on Linux without a running global hook was fixed.

- libuiohook was updated to commit
[a8b6506](https://github.com/TolikPylypchuk/libuiohook/tree/a8b65064bee6f0e409058be3e4da3811868e6da0).

### [v5.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.1.0) (October 8, 2023)

- Mouse coordinates were fixed for event simulation on Windows with multiple screens.

- Simulating multiple mouse clicks on macOS was fixed.

- An issue with some events being ignored on Windows was fixed.

- Reactive log source adapter can now accept a default scheduler.

- libuiohook was updated to commit
[2712b31](https://github.com/TolikPylypchuk/libuiohook/tree/2712b31d97833309d9ad5c1df0cb1f11aecaa03e).

### [v5.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.0.0) (August 10, 2023)

#### Platform Support

- Support for Mac Catalyst was added which makes it possible to use SharpHook in .NET MAUI apps on macOS.

- The minimum .NET Framework version was bumped to 4.6.2.

- Support for Windows on Arm32 was removed.

#### Breaking Changes

- Almost all `KeyCode` values were changed. New codes were added and some were removed. The actual enum values are
meaningless and may be changed again in the future.

- `MouseWheelEventData` doesn't contain `Amount` anymore.

- Mouse wheel simulation doesn't accept the amount anymore. Instead it accepts the scroll direction and type which
makes it possible to simulate horizontal scrolling.

- The meaning of the mouse wheel rotation value was changed – a positive value indicates that the wheel is rotated up or
left, and a negative value indicates that the wheel is rotated down or right.

- `KeyboardEventData.KeyChar` was renamed to `KeyboardEventData.RawKeyChar` and the `KeyboardEventData.KeyChar` property
was added.

- `LogEntryParser` is now a singleton and doesn't have a public constructor.

- The constants in the `MouseWheelScrollDirection` enum were shortened: `VerticalDirection` to `Vertical` and
`HorizontalDirection` to `Horizontal`.

#### New Features

- Text entry simulation was added.

- Low-level functionality providers were added – they can be used instead of directly using the `UioHook` class.

- Horizontal mouse wheel simulation is now possible.

- Classes which help with testing were added.

- Reactive global hook can now accept a default scheduler which will be used for all observables.

- Extension methods for `SharpHook.Native.ModifierMask` were added.

#### Bug Fixes

- Global hooks can now be created and disposed multiple times on Windows without problems.

- Many key code mappings to OS-specific key codes were fixed.

- `KeyTyped` events are now better at determining which characters should be produced by key presses on Windows.

#### Other Changes

- libuiohook was updated to commit
[6f42599](https://github.com/TolikPylypchuk/libuiohook/tree/6f42599e28099406d279146de4720110d40be047).

- All methods in all structs in the `UioHook.Native` namespace are now marked as `readonly`.

- SharpHook.Reactive now depends on Rx.NET 6.0.0.

- Docs now use a new theme.

### [v4.2.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.2.1) (June 18, 2023)

- libuiohook was updated to commit
[34f279c](https://github.com/TolikPylypchuk/libuiohook/tree/34f279c3dc2bb0e92f2b4aca0a3214b6f2e78af9).

- Getting screen info for multiple screens was fixed on Windows.

- A memory leak from getting screen info was fixed.

### [v4.2.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.2.0) (April 8, 2023)

- libuiohook was updated to commit
[56e8cb8](https://github.com/TolikPylypchuk/libuiohook/tree/56e8cb88cbe9a337e6ad8b1e4e12e40e96fe02f4).

- The ability to simulate mouse movement relative to the current cursor position was added.

### [v4.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.1.0) (March 27, 2023)

- libuiohook was updated to commit
[3a90aeb](https://github.com/TolikPylypchuk/libuiohook/tree/3a90aebfcf6375eed220c7999997098d67eb6f98).

- The ability to simulate mouse press/release events at the current coordinates was added.

- Turns out that libuiohook has always ignored mouse coordinates when simulating mouse wheel events, so the method
which simulates them without coordinates was added, and the previous one was marked as obsolete.

### [v4.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.0.1) (March 12, 2023)

libuiohook was updated to commit
[41a17e2](https://github.com/TolikPylypchuk/libuiohook/tree/41a17e284300c411a6fa64de3cb6ab058f3a09c5) which fixes
support for multiple screens on Windows.

### [v4.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.0.0) (November 9, 2022)

- .NET 7 support was added and `[LibraryImport]` is used instead of `[DllImport]` on it.

- Explicit targets for .NET 5 and .NET Core 3.1 were removed, though the library can be used on those platforms through
.NET Standard.

- `HookEventArgs` now contains the `SuppressEvent` property instead of `Reserved`.

- `KeyboardEventData.KeyChar` is now of type `ushort` instead of `char` – this was changed purely for marshalling
reasons and it should still be used as a `char`.

- Simulating mouse presses and releases now requires providing mouse pointer coordinates.

- The ability to make `RunAsync` create a background thread was added.

- `KeyCode.VcPrintscreen` was renamed to `KeyCode.VcPrintScreen`.

- Versioned libuiohook binaries for macOS and Linux were removed from the NuGet package as they were bit-for-bit same as
the unversioned binaries.

- libuiohook is at commit
[1ece4c4](https://github.com/TolikPylypchuk/libuiohook/tree/1ece4c4c24958d6ede0cba867f1a1cb3387f81f8).

### [v3.1.3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.3) (October 27, 2022)

- Copying native libraries to the build output folder should now work correctly for .NET Framework-based projects
([#18](https://github.com/TolikPylypchuk/SharpHook/issues/18)).

### [v3.1.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.2) (October 19, 2022)

- A fix for posting keyboard events on Windows was added, as suggested by [FaithBeam](https://github.com/FaithBeam),
and fixes [#20](https://github.com/TolikPylypchuk/SharpHook/issues/20).

- libuiohook is at commit
[fc779b0](https://github.com/TolikPylypchuk/libuiohook/tree/fc779b0bc892f8aaf373b53a3791f1e5590b9924).

### [v3.1.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.1) (August 5, 2022)

- A fork of libuiohook which fixes issue [#9](https://github.com/TolikPylypchuk/SharpHook/issues/9) is used and is at
commit [09bae87](https://github.com/TolikPylypchuk/libuiohook/tree/09bae87ada36f4daf156c3469b787c1fcb39be92).

### [v3.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.0) (July 30, 2022)

- SharpHook now uses a build of libuiohook which links the C runtime statically on Windows. This means that Visual C++
Redistributable is not needed for client apps to run (the logging functionality is a exception though).
([#14](https://github.com/TolikPylypchuk/SharpHook/issues/14)).

- The `EmptyLogSource` class was added, mostly for using it instead of `LogSource` in release builds of client apps.

- A memory issue which was fixed for hooks in v3.0.1 was fixed for logging as well.

### [v3.0.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.2) (July 1, 2022)

- Windows x86 support was fixed ([#10](https://github.com/TolikPylypchuk/SharpHook/issues/10)).

- The functions in `UioHook` which return system properties now return `int` instead of `long`. This is a tiny breaking
change, but I believe it's too small to warrant a bump of the major (or even minor) version.

### [v3.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.1) (June 25, 2022)

- A memory issue which arose only when debugging was fixed ([#12](https://github.com/TolikPylypchuk/SharpHook/issues/12)).

- libuiohook was updated to commit [de3f683](https://github.com/kwhat/libuiohook/tree/de3f68346781b1f3347b44ce8e370a5f0a603f89).

### [v3.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.0) (March 27, 2022)

- The ability to get libuiohook logs was added.

- Event simulation now ignores event masks not only on Windows, but everywhere, and returns a result.

- `UioHookEvent.Time` now contains the event's UNIX timestamp.

- `IGlobalHook` and `IReactiveGlobalHook` now contain the `IsDisposed` property.

- The `HookEvent<TArgs>` class was removed from SharpHook.Reactive.

- Several minor (but breaking) changes in the `UioHook` class.

- libuiohook is now at version 1.3 and commit [a887cde](https://github.com/kwhat/libuiohook/tree/a887cde82b3670e6ec54d6d3ff167903988a67af).

### [v2.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v2.0.0) (February 4, 2022)

- Global hooks now support both blocking and non-blocking running via the `Run` and `RunAsync` methods, and the `Start`
method was removed.

- Support for suppressing event propagation.

- `UioHookEvent.Time` now has the correct type – `ulong` instead of `ushort`.

- Global hooks now throw an exception if they are started when already running.

### [v1.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.1.0) (December 4, 2021)

- Cross-platform input event simulation using libuiohook.

- Support for Windows on Arm64.

- `MouseWheelEventData.Rotation` now has the correct type – `short` instead of `ushort`. This is a tiny breaking change,
but had to be fixed.

- libuiohook was updated to commit [5cf864d](https://github.com/kwhat/libuiohook/tree/5cf864d37bdee41bcef2297401c4538d9010b770).

### [v1.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.1) (November 21, 2021)

libuiohook was updated to commit [28ccf9c](https://github.com/kwhat/libuiohook/tree/28ccf9c392ca5fd872a21246b49bf9ee2c0baf15).

### [v1.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0) (November 8, 2021)

This release is basically the same as v1.0.0 Preview 4, but built with a GA release of .NET 6.

### [v1.0.0 Preview 4](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.4) (November 6, 2021)

- When `Dispose` on global hooks is called, they now reset the static hook callback function.

- `HookDisabled` is now emitted correctly for reactive global hooks.

- The assemblies are now trimmable.

### [v1.0.0 Preview 3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.3) (November 1, 2021)

- `IsRunning` was added to `SharpHook.IGlobalHook` and `SharpHook.Reactive.IReactiveGlobalHook`.

- `Dispose` is now safe to call when the hook is not running for all implementations.

- libuiohook was updated to commit [4867b8e](https://github.com/kwhat/libuiohook/tree/4867b8e768bdb0037d05993aad92254793326fae).

### [v1.0.0 Preview 2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.2) (October 26, 2021)

- `SharpHook.Native.ModifierMask.None` was added.

- Package descriptions were fixed.

### [v1.0.0 Preview 1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.1) (October 26, 2021)

- The basic functionality is implemented: native functions, default global hooks, and reactive global hooks.

- libuiohook is at commit [e2c581f](https://github.com/kwhat/libuiohook/tree/e2c581f6d3012f68580e68a9e75b14e599baca88).
