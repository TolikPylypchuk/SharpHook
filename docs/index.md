# Introduction

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

## Installation

```
dotnet add package SharpHook
dotnet add package SharpHook.Reactive
dotnet add package SharpHook.ReactiveUI
dotnet add package SharpHook.R3
dotnet add package SharpHook.Testing
```

## Supported Platforms

SharpHook targets .NET 10+, .NET Framework 4.7.2+, and .NET Standard 2.0. The following table describes
the availability of SharpHook on various platforms:

<table>
  <tr>
    <th></th>
    <th>Windows</th>
    <th>macOS</th>
    <th>Linux</th>
  </tr>
  <tr>
    <th>x86</th>
    <td>Yes</td>
    <td>N/A</td>
    <td>No</td>
  </tr>
  <tr>
    <th>x64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm32</th>
    <td>No</td>
    <td>N/A</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
</table>

Platform support notes:

- Windows 11 is supported, though SharpHook should work on older versions of Windows as well. Support for Windows on
Arm32 was removed in version 5.0 since it was
[removed in .NET 5](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md).

- macOS 10.15+ is required, though only the currently supported macOS versions are supported by SharpHook. Mac Catalyst
is also supported, and version 13.1+ is required. macOS requires that the access to the Accessibility API be enabled for
the application if it wants to create a global hook or simulate events.

- Linux distributions supported by .NET are supported by SharpHook. Linux on x86 is
[not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself. Both X11 and Wayland are supported, but
Wayland support lacks several features and requires elevated privileges. On Linux, multiple libuiohook backends are
present and the backend choice can be customized.

More info on OS support can be found in [an article on OS-specific constraints](articles/os-constraints.md).
