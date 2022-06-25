# SharpHook Changelog

## [v3.0.1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.1) (June 25, 2022)

- A memory issue which arised only when debugging was fixed ([#12](https://github.com/TolikPylypchuk/SharpHook/issues/12)).

- libuiohook was updated to commit [de3f683](https://github.com/kwhat/libuiohook/tree/de3f68346781b1f3347b44ce8e370a5f0a603f89).

## [v3.0.0](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v3.0.0) (March, 27 2022)

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
