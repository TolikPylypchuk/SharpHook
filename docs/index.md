# SharpHook

SharpHook provides a cross-platform global keyboard and mouse hook, event simulation, and text entry for .NET.
It is a wrapper of [libuiohook](https://github.com/kwhat/libuiohook) and provides direct access to its features as
well as higher-level types to work with it.

The following articles can help you learn how to use this library.

## GitHub Repository

https://github.com/TolikPylypchuk/SharpHook

## Installation

```
dotnet add package SharpHook --version 5.0.0
dotnet add package SharpHook.Reactive --version 5.0.0
```


## Supported Platforms

SharpHook targets .NET 6+, .NET Framework 4.6.1+, and .NET Standard 2.0. The following table describes
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
    <td>No<sup>1</sup></td>
  </tr>
  <tr>
    <th>x64</th>
    <td>Yes</td>
    <td>Yes</td>
    <td>Yes</td>
  </tr>
  <tr>
    <th>Arm32</th>
    <td>No<sup>2</sup></td>
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

[1] - Linux on x86 is [not supported](https://github.com/dotnet/runtime/issues/7335) by .NET itself.

[2] - Windows Arm32 support was
[dropped](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md) in .NET 5.

libuiohook only supports X11 on Linux. Wayland support [may be coming](https://github.com/kwhat/libuiohook/issues/100),
but it's not yet here.
