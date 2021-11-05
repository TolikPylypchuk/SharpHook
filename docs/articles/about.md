# About SharpHook

## Library Status

SharpHook is under active development. The code itself is basically done at this point, and I'm wating for the .NET 6 GA
release. When it's out, I'm going to release version 1.0.0.

No other features are planned, but I will maintain the library to keep up with the releases of libuiohook which uses a
rolling release model - every commit to its `1.2` branch is considered stable. That said, libuiohook itself is not
really active anymore, but is still maintained, so this library will most probably receive updates quite rarely.

## Changelog

## [v1.0.0 Preview 3](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.3) (01 November 2021)

- `IsRunning` was added to `SharpHook.IGlobalHook` and `SharpHook.Reactive.IReactiveGlobalHook`.

- `Dispose` is now safe to call when the hook is not running for all implementations.

- libuiohook was updated to commit [4867b8e](https://github.com/kwhat/libuiohook/tree/4867b8e768bdb0037d05993aad92254793326fae).

## [v1.0.0 Preview 2](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.2) (26 October 2021)

- `SharpHook.Native.ModifierMask.None` was added.

- Package descriptions were fixed.

## [v1.0.0 Preview 1](https://github.com/TolikPylypchuk/SharpHook/releases/tag/v1.0.0-preview.1) (26 October 2021)

- The basic functionality is implemented: native functions, default global hooks, and reactive global hooks.

- libuiohook is at commit [e2c581f](https://github.com/kwhat/libuiohook/tree/e2c581f6d3012f68580e68a9e75b14e599baca88).
