# Introduction

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry simulation for
.NET. It is a wrapper of [libuiohook](https://github.com/TolikPylypchuk/libuiohook) and provides direct access to its
features as well as higher-level types to work with it.

## Installation

```
dotnet add package SharpHook --version 5.3.5
dotnet add package SharpHook.Reactive --version 5.3.5
```

## Supported Platforms

SharpHook targets .NET 6+, .NET Framework 4.6.2+, and .NET Standard 2.0. The following table describes
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

- Windows 10/11 is supported. Support for Windows on Arm32 was removed in version 5.0.0 since it was
[removed in .NET 5](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md).

- macOS 10.15+ is supported. Mac Catalyst is also supported (13.1+). macOS requires that the accessibility API be
enabled for the application if it wants to create a global hook.

- Linux distributions supported by .NET are supported by SharpHook. Linux on x86 is
[not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself. Only X11 is supported - Wayland support
[may be coming](https://github.com/kwhat/libuiohook/issues/100), but it's not yet here.

More info on OS support can be found in [an article on OS-specific constraints](articles/os-constraints.md).
