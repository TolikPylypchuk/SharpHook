# SharpHook Changelog

## [v5.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v5.0.0) (August 10, 2023)

This is probably the largest release since v1.0.0 and it contains multiple new features and bug fixes, as well as
breaking changes.

### Platform Support

- Support for Mac Catalyst was added which makes it possible to use SharpHook in .NET MAUI apps on macOS.

- The minimum .NET Framework version was bumped to 4.6.2.

- Support for Windows on Arm32 was removed.

### Breaking Changes

- Almost all `KeyCode` values were changed. New codes were added and some were removed. The actual enum values are
meaningless and may be changed again in the future.

- `MouseWheelEventData` doesn't contain `Amount` anymore.

- Mouse wheel simulation doesn't accept the amount anymore. Instead it accepts the scroll direction and type which
makes it possible to simulate horizontal scrolling. The meaning of the rotation value was changed - a positive value
indicates that the wheel is rotated up or left, and a negative value indicates that the wheel is rotated down or right.

- `KeyboardEventData.KeyChar` was renamed to `KeyboardEventData.RawKeyChar` and the `KeyboardEventData.KeyChar` property
was added.

- `LogEntryParser` is now a singleton and doesn't have a public constructor.

- The constants in the `MouseWheelScrollDirection` enum were shortened: `VerticalDirection` to `Vertical` and
`HorizontalDirection` to `Horizontal`.

### New Features

- Text entry simulation was added.

- Low-level functionality providers were added - they can be used instead of directly using the `UioHook` class.

- Horizontal mouse wheel simulation is now possible.

- Classes which help with testing were added.

- Reactive global hook can now accept a default scheduler which will be used for all observables.

- Extension methods for `SharpHook.Native.ModifierMask` were added.

### Bug Fixes

- Global hooks can now be created and disposed multiple times on Windows without problems.

- Many key code mappings to OS-specific key codes were fixed.

- `KeyTyped` events are now better at determining which characters should be produced by key presses on Windows.

### Other Changes

- libuiohook was updated to commit
[6f42599](https://github.com/TolikPylypchuk/libuiohook/tree/6f42599e28099406d279146de4720110d40be047).

- All methods in all structs in the `UioHook.Native` namespace are now marked as `readonly`.

- SharpHook.Reactive now depends on Rx.NET 6.0.0.

- Docs now use a new theme.

## [v4.2.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.2.1) (June 18, 2023)

- libuiohook was updated to commit
[34f279c](https://github.com/TolikPylypchuk/libuiohook/tree/34f279c3dc2bb0e92f2b4aca0a3214b6f2e78af9).

- Getting screen info for multiple screens was fixed on Windows.

- A memory leak from getting screen info was fixed.

## [v4.2.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.2.0) (April 8, 2023)

- libuiohook was updated to commit
[56e8cb8](https://github.com/TolikPylypchuk/libuiohook/tree/56e8cb88cbe9a337e6ad8b1e4e12e40e96fe02f4).

- The ability to simulate mouse movement relative to the current cursor position was added.

## [v4.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.1.0) (March 27, 2023)

- libuiohook was updated to commit
[3a90aeb](https://github.com/TolikPylypchuk/libuiohook/tree/3a90aebfcf6375eed220c7999997098d67eb6f98).

- The ability to simulate mouse press/release events at the current coordinates was added.

- Turns out that libuiohook has always ignored mouse coordinates when simulating mouse wheel events, so the method
which simulates them without coordinates was added, and the previous one was marked as obsolete.

## [v4.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.0.1) (March 12, 2023)

libuiohook was updated to commit
[41a17e2](https://github.com/TolikPylypchuk/libuiohook/tree/41a17e284300c411a6fa64de3cb6ab058f3a09c5) which fixes
support for multiple screens on Windows.

## [v4.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v4.0.0) (November 9, 2022)

- .NET 7 support was added and `[LibraryImport]` is used instead of `[DllImport]` on it.

- Explicit targets for .NET 5 and .NET Core 3.1 were removed, though the library can be used on those platforms through
.NET Standard.

- `HookEventArgs` now contains the `SuppressEvent` property instead of `Reserved`.

- `KeyboardEventData.KeyChar` is now of type `ushort` instead of `char` - this was changed purely for marshalling
reasons and it should still be used as a `char`.

- Simulating mouse presses and releases now requires providing mouse pointer coordinates.

- The ability to make `RunAsync` create a background thread was added.

- `KeyCode.VcPrintscreen` was renamed to `KeyCode.VcPrintScreen`.

- Versioned libuiohook binaries for macOS and Linux were removed from the NuGet package as they were bit-for-bit same as
the unversioned binaries.

- libuiohook is at commit
[1ece4c4](https://github.com/TolikPylypchuk/libuiohook/tree/1ece4c4c24958d6ede0cba867f1a1cb3387f81f8).

## [v3.1.3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.3) (October 27, 2022)

- Copying native libraries to the build output folder should now work correctly for .NET Framework-based projects
([#18](https://github.com/TolikPylypchuk/SharpHook/issues/18)).

## [v3.1.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.2) (October 19, 2022)

- A fix for posting keyboard events on Windows was added, as suggested by [FaithBeam](https://github.com/FaithBeam),
and fixes [#20](https://github.com/TolikPylypchuk/SharpHook/issues/20).

- libuiohook is at commit
[fc779b0](https://github.com/TolikPylypchuk/libuiohook/tree/fc779b0bc892f8aaf373b53a3791f1e5590b9924).

## [v3.1.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.1) (August 5, 2022)

- A fork of libuiohook which fixes issue [#9](https://github.com/TolikPylypchuk/SharpHook/issues/9) is used and is at
commit [09bae87](https://github.com/TolikPylypchuk/libuiohook/tree/09bae87ada36f4daf156c3469b787c1fcb39be92).

## [v3.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.1.0) (July 30, 2022)

- SharpHook now uses a build of libuiohook which links the C runtime statically on Windows. This means that Visual C++
Redistributable is not needed for client apps to run (the logging functionality is a exception though).
([#14](https://github.com/TolikPylypchuk/SharpHook/issues/14)).

- The `EmptyLogSource` class was added, mostly for using it instead of `LogSource` in release builds of client apps.

- A memory issue which was fixed for hooks in v3.0.1 was fixed for logging as well.

## [v3.0.2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.2) (July 1, 2022)

- Windows x86 support was fixed ([#10](https://github.com/TolikPylypchuk/SharpHook/issues/10)).

- The functions in `UioHook` which return system properties now return `int` instead of `long`. This is a tiny breaking
change, but I believe it's too small to warrant a bump of the major (or even minor) version.

## [v3.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.1) (June 25, 2022)

- A memory issue which arose only when debugging was fixed ([#12](https://github.com/TolikPylypchuk/SharpHook/issues/12)).

- libuiohook was updated to commit [de3f683](https://github.com/kwhat/libuiohook/tree/de3f68346781b1f3347b44ce8e370a5f0a603f89).

## [v3.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.0) (March 27, 2022)

- The ability to get libuiohook logs was added.

- Event simulation now ignores event masks not only on Windows, but everywhere, and returns a result.

- `UioHookEvent.Time` now contains the event's UNIX timestamp.

- `IGlobalHook` and `IReactiveGlobalHook` now contain the `IsDisposed` property.

- The `HookEvent<TArgs>` class was removed from SharpHook.Reactive.

- Several minor (but breaking) changes in the `UioHook` class.

- libuiohook is now at version 1.3 and commit [a887cde](https://github.com/kwhat/libuiohook/tree/a887cde82b3670e6ec54d6d3ff167903988a67af).

## [v2.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v2.0.0) (February 4, 2022)

- Global hooks now support both blocking and non-blocking running via the `Run` and `RunAsync` methods, and the `Start`
method was removed.

- Support for suppressing event propagation.

- `UioHookEvent.Time` now has the correct type - `ulong` instead of `ushort`.

- Global hooks now throw an exception if they are started when already running.

## [v1.1.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.1.0) (December 4, 2021)

- Cross-platform input event simulation using libuiohook.

- Support for Windows on Arm64.

- `MouseWheelEventData.Rotation` now has the correct type - `short` instead of `ushort`. This is a tiny breaking change,
but had to be fixed.

- libuiohook was updated to commit [5cf864d](https://github.com/kwhat/libuiohook/tree/5cf864d37bdee41bcef2297401c4538d9010b770).

## [v1.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.1) (November 21, 2021)

libuiohook was updated to commit [28ccf9c](https://github.com/kwhat/libuiohook/tree/28ccf9c392ca5fd872a21246b49bf9ee2c0baf15).

## [v1.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0) (November 8, 2021)

This release is basically the same as v1.0.0 Preview 4, but built with a GA release of .NET 6.

## [v1.0.0 Preview 4](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.4) (November 6, 2021)

- When `Dispose` on global hooks is called, they now reset the static hook callback function.

- `HookDisabled` is now emitted correctly for reactive global hooks.

- The assemblies are now trimmable.

## [v1.0.0 Preview 3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.3) (November 1, 2021)

- `IsRunning` was added to `SharpHook.IGlobalHook` and `SharpHook.Reactive.IReactiveGlobalHook`.

- `Dispose` is now safe to call when the hook is not running for all implementations.

- libuiohook was updated to commit [4867b8e](https://github.com/kwhat/libuiohook/tree/4867b8e768bdb0037d05993aad92254793326fae).

## [v1.0.0 Preview 2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.2) (October 26, 2021)

- `SharpHook.Native.ModifierMask.None` was added.

- Package descriptions were fixed.

## [v1.0.0 Preview 1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.1) (October 26, 2021)

- The basic functionality is implemented: native functions, default global hooks, and reactive global hooks.

- libuiohook is at commit [e2c581f](https://github.com/kwhat/libuiohook/tree/e2c581f6d3012f68580e68a9e75b14e599baca88).
