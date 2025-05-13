# Logging

libuiohook can log messages throughout its execution. By default the messages are not logged anywhere, but you can get
these logs by using `UioHook.SetLoggerProc`, or the `ILogSource` interface and its default implementation, `LogSource`.

libuiohook logs contain the log level (debug, info, warning, error), message format, and message arguments.

The message structure is the following:

```
function [line]: message
```

`function` is the function name in the libuiohook source code, and `line` is the source code line.

## Using High-Level Types

The easiest way to subscribe to libuiohook's logs is to use the `LogSource` class and its interface - `ILogSource`. The
interface contains the `MessageLogged` event, and extends `IDisposable`. Calling the `Dispose` method will stop the log
source from receiving the logs. The `IsDisposed` property is also available.

`LogSource` also contains the `MinLevel` property which can be set to filter log messages by level. It's not recommended
to use the debug level for long periods of time since a debug message is logged for every single input event.

Here's a usage example:

```c#
using SharpHook.Data;
using SharpHook.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

You can get an instance of `LogSource` by using the static `RegisterOrGet` method. Subsequent calls to this method will
return the same registered instance. You can dispose of a log source to stop receiving libuiohook messages. After that,
calling `RegisterOrGet` will register a new instance.

The `MessageLogged` event contains event args of type `LogEventArgs` which contains just one property of type
`LogEntry`. This class contains the actual log message.

The simplest way to use `LogEntry` is to use its `Level` and `FullText` properties. `FullText` is created using the log
message format and arguments so you don't have to do it yourself.

SharpHook.Reactive contains the `IReactiveLogSource` and its implementation - `ReactiveLogSourceAdapter`. Here's a
usage example:

```c#
using SharpHook.Logging;
using SharpHook.Native;
using SharpHook.Reactive.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

`IReactiveLogSource` is basically the same as `ILogSource`, but `MessageLogged` is an observable of `LogEntry` instead
of an event. `ReactiveLogSourceAdapter` adapts an `ILogSource` to the `IReactiveLogSource` interface. A default
scheduler can be set for the `MessageLogged` observable.

## Using the Low-Level Functionality

The logging functionality works by using `UioHook.SetLoggerProc`. This method sets the log callback - a delegate of
type `LoggerProc`, which will be called to log the messages of libuiohook. `LoggerProc` receives the log level, a
pointer to the message format, and a pointer to the message arguments. It also receives a pointer to user-supplied data
(which is set in the `UioHook.SetLoggerProc`), but you usually shouldn't use it.

It is highly recommended to use `LogEntryParser` in order to create a log entry out of the pointers to the message
format and arguments. This way you won't have to handle these pointers directly. The problem with handling them directly
is that the log callback receives a variable number of arguments. In C# you can use the `params` keyword for that, but
native functions do that in an entirely different way, and .NET doesn't have a default way to handle that (there is an
undocumented `__arglist` keyword, but it can't be used in delegates and callbacks). `LogEntryParser` handles all that -
its code is based on the log handling code of [LibVLCSharp](https://github.com/videolan/libvlcsharp). Basically it calls
the native `vsprintf` function from the C runtime and lets it deal with formatting the log message with native variable
arguments. It then parses the log message and the log format and extracts the arguments.

If you want to use your own callback, then its form should be the following:

```c#
private void OnLog(LogLevel level, IntPtr userData, IntPtr format, IntPtr args)
{
    // Filter by log level if needed

    var logEntry = LogEntryParser.Instance.ParseNativeLogEntry(level, format, args);

    // Handle the log entry instead of the native format and arguments
}
```

## Advanced Usage

If you use structured logging, then you may want to use the message format and arguments directly, instead of using the
formatted result. `LogEntry` contains properties which can help you with that:

- `Format` - the format of the log message which can be passed to `String.Format`.
- `RawFormat` - the raw native format of the log message (which uses argument placeholders for C's `printf` function).
- `Arguments` - the strongly-typed arguments of the log message.
- `RawArguments` - the arguments of the log message as they appear in the formatted log message.
- `ArgumentPlaceholders` - the placeholders extracted from the native log format (e.g. `%d` for a number).

`String.Format(entry.Format, entry.RawAguments.ToArray())` is equal to `entry.FullText`.

`String.Format(entry.Format, entry.Aguments.ToArray())` is not necessarily equal to `entry.FullText` since some
formatting information is discarded, but using `Arguments` instead of `RawArguments` is better suited for structured
logging.

`Arguments` contains parsed message arguments which can be of one of the types listed below, according to the argument
placeholders. Only the specifier and length are considered (see the C's `printf` docs for reference).

<table>
  <tr>
    <th>Type</th>
    <th>Placeholder</th>
  </tr>
  <tr>
    <td><code>int</code></td>
    <td><code>%d</code>, <code>%i</code></td>
  </tr>
  <tr>
    <td><code>sbyte</code></td>
    <td><code>%hhd</code>, <code>%hhi</code></td>
  </tr>
  <tr>
    <td><code>short</code></td>
    <td><code>%hd</code>, <code>%hi</code></td>
  </tr>
  <tr>
    <td><code>long</code></td>
    <td>
        <code>%ld</code>, <code>%li</code>, <code>%lld</code>, <code>%lli</code>, <code>%jd</code>, <code>%ji</code>
    </td>
  </tr>
  <tr>
    <td><code>uint</code></td>
    <td><code>%u</code>, <code>%o</code>, <code>%x</code>, <code>%X</code></td>
  </tr>
  <tr>
    <td><code>byte</code></td>
    <td><code>%hhu</code>, <code>%hho</code>, <code>%hhx</code>, <code>%hhX</code></td>
  </tr>
  <tr>
    <td><code>ushort</code></td>
    <td><code>%hu</code>, <code>%ho</code>, <code>%hx</code>, <code>%hX</code></td>
  </tr>
  <tr>
    <td><code>ulong</code></td>
    <td>
        <code>%lu</code>, <code>%lo</code>, <code>%lx</code>, <code>%lX</code>, <code>%llu</code>, <code>%llo</code>,
        <code>%llx</code>, <code>%llX</code>, <code>%ju</code>, <code>%jo</code>, <code>%jx</code>, <code>%jX</code>
    </td>
  </tr>
  <tr>
    <td><code>double</code></td>
    <td><code>%f</code>, <code>%F</code>, <code>%e</code>, <code>%E</code>, <code>%g</code>, <code>%G</code></td>
  </tr>
  <tr>
    <td><code>decimal</code></td>
    <td><code>%Lf</code>, <code>%LF</code>, <code>%Le</code>, <code>%LE</code>, <code>%Lg</code>, <code>%LG</code></td>
  </tr>
  <tr>
    <td><code>char</code></td>
    <td><code>%c</code></td>
  </tr>
  <tr>
    <td><code>nint</code></td>
    <td><code>%p</code></td>
  </tr>
  <tr>
    <td><code>string</code></td>
    <td>Any other placeholder, including <code>%s</code></td>
  </tr>
</table>

The `%a`, `%A`, and `%n` specifiers are not supported, as well as length `z` and `t`.

`LogEntry` also contains the `Function` and `SourceLine` properties. These are the first two arguments of the log
message - the function name in the libuiohook source code, and the source code line.
