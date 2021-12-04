# Introduction

Creating global keyboard and mouse hooks is hard. You have to use a bunch of not-so-pretty native functions and do
low-level programming. It becomes even more difficult if you need it to work on multiple platforms. Sure, there are
libraries that make things easier, but they work only on Windows since C#'s and .NET's history is tied to this OS. Well,
now things are not quite as tied, and people need cross-platform stuff. So here we are. Meet SharpHook, a library which
enables you to easily create global hooks in a cross-platform way.

Starting with version 1.1.0, SharpHook can simulate keyboard and mouse events in a cross-platform way as well.

SharpHook doesn't implement any hooking functionality itself. It is simply a wrapper around a C library called
[libuiohook](https://github.com/kwhat/libuiohook), and a very thin one at that. However, it does provide a few types to
make things easier for you than P/Invoking native functions.

In the following articles you'll learn how to use this library.

## Supported Platforms

SharpHook targets .NET Framework 4.6.1+, .NET Standard 2.0, and .NET Core 3.1/.NET 5+. The following table describes
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
    <td>Yes<sup>2</sup></td>
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
[dropped](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md) in .NET 5 so it will most
probably be dropped by this library in a future version as well.

libuiohook only supports X11 on Linux. Wayland support [may be coming](https://github.com/kwhat/libuiohook/issues/100),
but it's not yet here.

Next article: [Native Functions](native.md).
