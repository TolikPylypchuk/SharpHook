# SharpHook

[![NuGet](https://img.shields.io/nuget/v/SharpHook.svg?label=SharpHook)](https://www.nuget.org/packages/SharpHook)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.Reactive.svg?label=SharpHook.Reactive)](https://www.nuget.org/packages/SharpHook.Reactive)
[![NuGet](https://img.shields.io/nuget/v/SharpHook.R3.svg?label=SharpHook.R3)](https://www.nuget.org/packages/SharpHook.R3)

SharpHook 为 .NET 提供了一个跨平台的全局键盘和鼠标钩子、事件模拟和文本输入模拟功能。
它是 [libuiohook](https://github.com/TolikPylypchuk/libuiohook) 的封装，并提供对其功能的直接访问以及更高级别的类型来使用它。

## 安装

```
dotnet add package SharpHook
dotnet add package SharpHook.Reactive
dotnet add package SharpHook.R3
```

## 升级

跨主版本升级时，可以参考[迁移指南](https://sharphook.tolik.io/articles/migration.html)。

## 文档

你可以在 [https://sharphook.tolik.io](https://sharphook.tolik.io) 找到更多信息（包括 API 参考）。

## 支持的平台

SharpHook 面向 .NET 8+、.NET Framework 4.7.2+ 以及 .NET Standard 2.0。下表描述了 SharpHook 在各个平台上的支持情况：

<table>
  <tr>
    <th></th>
    <th>Windows</th>
    <th>macOS</th>
    <th>Linux</th>
  </tr>
  <tr>
    <th>x86</th>
    <td>支持</td>
    <td>不适用</td>
    <td>不支持</td>
  </tr>
  <tr>
    <th>x64</th>
    <td>支持</td>
    <td>支持</td>
    <td>支持</td>
  </tr>
  <tr>
    <th>Arm32</th>
    <td>不支持</td>
    <td>不适用</td>
    <td>支持</td>
  </tr>
  <tr>
    <th>Arm64</th>
    <td>支持</td>
    <td>支持</td>
    <td>支持</td>
  </tr>
</table>

平台支持说明：

- Windows 10/11 受支持。对 32 位 ARM 架构的 Windows 支持在 5.0 版本中移除，因为它在 .NET 5 中已被[移除](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md)。

- macOS 10.15+ 受支持，同时支持 Mac Catalyst 13.1+。如果应用程序想要创建全局钩子或模拟事件，
macOS 要求开启辅助功能（Accessibility）权限。

- Linux 只要是 .NET 支持的发行版均受 SharpHook 支持。[.NET 已不再支持 32 位 x86 架构的 Linux 系统](https://github.com/dotnet/runtime/issues/7335)。
仅支持 X11 – Wayland[即将支持](https://github.com/kwhat/libuiohook/issues/196)，但目前暂不支持。

> [!注意]
> 对于官方已停止维护的操作系统版本，会尽可能提供支持，但某些仅影响旧版本的问题视情况而定。

更多关于操作系统支持的信息可以在
[关于操作系统特定限制的文章](https://sharphook.tolik.io/articles/os-constraints.html) 中找到。

## 使用方法

### libuiohook 的原生函数

SharpHook 在 `SharpHook.Native.UioHook` 类中暴露了 libuiohook 的函数。`SharpHook.Data` 命名空间包含了
表示 libuiohook 所使用数据的类型。

通常不需要直接使用原生方法。优先应使用 SharpHook 提供的高级接口和类。
但了解底层逻辑，能帮助你更好地理解整体运行机制。

如果你想使用底层功能，你不需要直接使用 `UioHook` 类。你可以使用 `SharpHook.Providers` 命名空间中的接口。
这些接口中的方法与 `UioHook` 类中的方法相同。`SharpHook.Providers.UioHookProvider` 实现了所有这些接口，
内部会转发调用到 `UioHook` 中的相应方法。这样做可以解耦业务与底层实现，使测试更容易。

`UioHook` 包含以下用于处理全局钩子的方法：

- `SetDispatchProc` – 设置当 libuiohook 引发事件时将被调用的函数。
- `Run` – 创建一个键盘和鼠标全局钩子并在当前线程上运行，阻塞该线程直到调用 `Stop`。
- `RunKeyboard` – 创建一个仅键盘全局钩子并在当前线程上运行，阻塞该线程直到调用 `Stop`。
- `RunMouse` – 创建一个仅鼠标全局钩子并在当前线程上运行，阻塞该线程直到调用 `Stop`。
- `Stop` – 停止全局钩子。

> [!重要]
> 你必须记住，一次只能存在一个全局钩子，因为调用 `SetDispatchProc` 会覆盖先前设置的回调。
> 此外，在另一个全局钩子已经运行时运行全局钩子会破坏 libuiohook 的内部全局状态。

此外，`UioHook` 包含用于模拟输入事件的 `PostEvent` 方法。

`UioHook` 还包含用于模拟文本输入的 `PostText` 方法。要模拟的文本不依赖于当前的键盘布局。
支持完整的 UTF-16 (包括代理项对，例如表情符号 Emoji)。

libuiohook 还提供了获取各种系统属性的函数。`UioHook` 中也提供了相应的方法。

### 全局钩子

SharpHook 提供了 `IGlobalHook` 接口以及三个默认实现，你可以使用它们来控制钩子并订阅其事件。
下面是一个基本用法示例：

```csharp
using SharpHook;
using SharpHook.Providers;

// KeyTyped 事件可能导致系统范围的副作用，因此如果不需要应该禁用它。
UioHookProvider.Instance.KeyTypedEnabled = false; // or true

var hook = new EventLoopGlobalHook();

hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>

hook.KeyTyped += OnKeyTyped;           // EventHandler<KeyboardHookEventArgs>
hook.KeyPressed += OnKeyPressed;       // EventHandler<KeyboardHookEventArgs>
hook.KeyReleased += OnKeyReleased;     // EventHandler<KeyboardHookEventArgs>

hook.MouseClicked += OnMouseClicked;   // EventHandler<MouseHookEventArgs>
hook.MousePressed += OnMousePressed;   // EventHandler<MouseHookEventArgs>
hook.MouseReleased += OnMouseReleased; // EventHandler<MouseHookEventArgs>
hook.MouseMoved += OnMouseMoved;       // EventHandler<MouseHookEventArgs>
hook.MouseDragged += OnMouseDragged;   // EventHandler<MouseHookEventArgs>

hook.MouseWheel += OnMouseWheel;       // EventHandler<MouseWheelHookEventArgs>

hook.Run();
// 或者
await hook.RunAsync();
```

首先，你创建钩子，然后订阅其事件，最后运行它。 `Run` 方法在当前线程上运行钩子，阻塞该线程。
`RunAsync()` 方法在单独的线程上运行钩子，并返回一个在钩子停止时完成的 `Task`。你可以在钩子启动后订阅事件。

`IGlobalHook` 包含用于停止全局钩子的 `Stop` 方法。停止后，可以通过调用 `Run` 或 `RunAsync` 方法再次启动全局钩子。
当钩子未运行时调用 `Stop` 不会起任何作用。

`IGlobalHook` 扩展了 `IDisposable`。当你在钩子上调用 `Dispose` 方法时，它会被释放，如果正在运行也会被停止。
一旦钩子被释放，就不能再启动——你必须创建一个新实例。当钩子未运行时调用 `Dispose` 除了将实例标记为已释放外不会起任何作用。

钩子事件的类型为 `HookEventArgs` 或派生类型，其中包含更多信息。可以通过在事件处理程序内部将 `SuppressEvent`
属性设置为 `true` 来抑制事件传播。这必须同步完成，并且仅在 Windows 和 macOS 上受支持。你可以分别使用 `EventTime` 和
`IsEventSimulated` 属性检查事件时间以及事件是真实的还是模拟的。

> [!重要]
> 始终在整个应用程序中一次使用一个 `IGlobalHook` 实例，因为它们都必须使用相同的静态方法来设置 libuiohook 的钩子回调，
> 因此一次只能有一个回调。在另一个全局钩子已经运行时运行全局钩子会破坏 libuiohook 的内部全局状态。

你可以通过向钩子构造函数传递 `GlobalHookType` 来创建仅键盘或仅鼠标的钩子。这仅在 Windows 上真正有区别，
因为那里有两个不同的全局钩子 – 键盘钩子和鼠标钩子。在 macOS 和 Linux 上，只有一个钩子用于所有事件，
这仅是在这些操作系统上过滤掉键盘或鼠标事件。

SharpHook 提供了三个 `IGlobalHook`的实现：

- `SharpHook.SimpleGlobalHook` 在其运行钩子的同一线程上运行所有事件处理程序。这意味着处理程序通常应该很快，
因为它们如果运行时间过长会阻塞钩子处理后续事件。

- `SharpHook.EventLoopGlobalHook` 在单独的专用线程上运行所有事件处理程序。当事件处理不过来时，会排队处理剩余事件，
这意味着钩子将能够处理所有事件。除了非常简单的用例之外，这个实现应该优先于 `SimpleGlobalHook`。
但它有一个缺点——由于事件处理程序在其他线程上运行，抑制事件传播将被忽略。

- `SharpHook.TaskPoolGlobalHook` 在任务默认线程池内的其他线程上运行所有事件处理程序。处理程序的并行级别可以配置。
在处理不过来时，会排队处理剩余的事件，这意味着钩子将能够处理所有事件。除了非常简单的用例之外，
这个实现应该优先于 `SimpleGlobalHook`。但它有一个缺点——由于事件处理程序在其他线程上运行，抑制事件传播将被忽略。
通常，应该优先使用 `EventLoopGlobalHook`，因为这个类仅在事件应该并行处理（这种情况很少见）时才提供好处。

该库还提供了 `SharpHook.GlobalHookBase` 类，你可以扩展此类来创建自己的全局钩子实现。它会调用适当的事件处理程序，
你只需要实现发送事件的策略。它还会保留对正在运行的全局钩子的引用，以便它不会被垃圾回收。

该库还提供了 `IBasicGlobalHook` 接口和 `BasicGlobalHookBase` 类。可以扩展此类来创建自定义全局钩子，
该钩子具有与 `IGlobalHook` 中不同形式的事件。

### Reactive 全局钩子

#### Rx.NET

如果你正在使用 [Rx.NET](https://github.com/dotnet/reactive)，你可以使用 SharpHook.Reactive 包将 SharpHook 与 Rx.NET 集成。

SharpHook.Reactive 提供了 `SharpHook.Reactive.IReactiveGlobalHook` 接口以及一个默认实现，
你可以使用它来控制钩子并订阅其可观察对象。下面是一个基本示例：

```csharp
using SharpHook.Reactive;

// ...

var hook = new ReactiveGlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);

hook.MouseMoved
    .Throttle(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseMoved);

hook.MouseDragged
    .Throttle(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseDragged);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// 或者
await hook.RunAsync();
```

Reactive 全局钩子基本上与默认全局钩子相同，并且相同的规则适用于它们。

SharpHook.Reactive 提供了两个 `IReactiveGlobalHook` 的实现：

- `SharpHook.Reactive.ReactiveGlobalHook`。由于我们处理的是可观察对象，由你决定何时何地通过调度器来处理事件。
可以为所有可观察对象指定默认调度器。

- `SharpHook.Reactive.ReactiveGlobalHookAdapter` 将 `IGlobalHook` 适配为 `IReactiveGlobalHook`。
所有订阅和更改都会传播到被适配的钩子。没有从 `IReactiveGlobalHook` 到 `IGlobalHook` 的默认适配器。
可以为所有可观察对象指定默认调度器。

#### R3

如果你正在使用 [R3](https://github.com/Cysharp/R3)，你可以使用 SharpHook.R3 包将 SharpHook 与 R3 集成。

SharpHook.R3 提供了 `SharpHook.R3.IR3GlobalHook` 接口以及一个默认实现，你可以使用它来控制钩子并订阅其可观察对象。
下面是一个基本示例：

```csharp
using SharpHook.R3;

// ...

var hook = new R3GlobalHook();

hook.HookEnabled.Subscribe(OnHookEnabled);
hook.HookDisabled.Subscribe(OnHookDisabled);

hook.KeyTyped.Subscribe(OnKeyTyped);
hook.KeyPressed.Subscribe(OnKeyPressed);
hook.KeyReleased.Subscribe(OnKeyReleased);

hook.MouseClicked.Subscribe(OnMouseClicked);
hook.MousePressed.Subscribe(OnMousePressed);
hook.MouseReleased.Subscribe(OnMouseReleased);

hook.MouseMoved
    .Debouce(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseMoved);

hook.MouseDragged
    .Debouce(TimeSpan.FromSeconds(0.5))
    .Subscribe(OnMouseDragged);

hook.MouseWheel.Subscribe(OnMouseWheel);

hook.Run();
// 或者
await hook.RunAsync();
```

R3 全局钩子基本上与默认全局钩子相同，并且相同的规则适用于它们。

SharpHook.R3 提供了两个 `IR3GlobalHook` 的实现：

- `SharpHook.R3.R3GlobalHook`。由于我们处理的是可观察对象，由你决定何时何地通过时间提供器来处理事件。
可以为所有可观察对象指定默认时间提供器。

- `SharpHook.R3.R3GlobalHookAdapter` 将 `IGlobalHook` 适配为 `IR3GlobalHook`。
所有订阅和更改都会传播到被适配的钩子。没有从 `IR3GlobalHook` 到 `IGlobalHook`的默认适配器。
可以为所有可观察对象指定默认时间提供器。

### 事件模拟

SharpHook 提供了以跨平台方式模拟键盘和鼠标事件的能力。这是一个快速示例：

```csharp
using SharpHook;
using SharpHook.Native;

// ...

var simulator = new EventSimulator();

// 按下 Ctrl+C
simulator.SimulateKeyPress(KeyCode.VcLeftControl);
simulator.SimulateKeyPress(KeyCode.VcC);

// 放开 Ctrl+C
simulator.SimulateKeyRelease(KeyCode.VcC);
simulator.SimulateKeyRelease(KeyCode.VcLeftControl);

// 模拟按下 Ctrl, 再按下 C, 然后放开 C, 再放开 Ctrl
simulator.SimulateKeyStroke(KeyCode.VcLeftControl, KeyCode.VcC);

// 按下鼠标左键
simulator.SimulateMousePress(MouseButton.Button1);

// 放开鼠标左键
simulator.SimulateMouseRelease(MouseButton.Button1);

// 在 (0, 0) 坐标按下鼠标左键
simulator.SimulateMousePress(0, 0, MouseButton.Button1);

// 在 (0, 0) 坐标放开鼠标左键
simulator.SimulateMouseRelease(0, 0, MouseButton.Button1);

// 移动鼠标指针到 (0, 0) 坐标
simulator.SimulateMouseMovement(0, 0);

// 将鼠标指针向左移动 50px，向下移动 100px
simulator.SimulateMouseMovementRelative(50, 100);

// 滚动鼠标滚轮
simulator.SimulateMouseWheel(
    rotation: -120,
    direction: MouseWheelScrollDirection.Vertical, // Vertical by default
    type: MouseWheelScrollType.UnitScroll); // UnitScroll by default
```

SharpHook 提供了 `IEventSimulator` 接口，以及默认实现 `EventSimulator`，它默认调用 `UioHook.PostEvent` 来模拟事件。

### 模拟文本输入

SharpHook 还提供了文本输入模拟。 `IEventSimulator` 包含 `SimulateTextEntry` 方法，该方法接受一个 `string`参数。
要模拟的文本不依赖于当前的键盘布局。支持完整的 UTF-16 范围（包括代理项对，例如表情符号 Emoji）。

### 日志记录

libuiohook 可以在其执行过程中记录消息。默认情况下，消息不会记录在任何地方，但你可以通过使用 `ILogSource`
接口及其默认实现 `LogSource` 来获取这些日志：

```csharp
using SharpHook.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
logSource.MessageLogged += this.OnMessageLogged;

private void OnMessageLogged(object? sender, LogEventArgs e) =>
    this.logger.Log(this.AdaptLogLevel(e.LogEntry.Level), e.LogEntry.FullText);
```

一次只能使用一个 `LogSource` 对象。 `ILogSource` 扩展了 `IDisposable` – 你可以处置日志源以停止接收 libuiohook 消息。

还提供了一个 `EmptyLogSource` 类 – 这个类不监听 libuiohook 日志，可以在发布版本中代替 `LogSource` 使用。

SharpHook.Reactive 包含了 `IReactiveLogSource` 和 `ReactiveLogSourceAdapter` ，因此你可以以更具响应性的方式使用它们：

```csharp
using SharpHook.Logging;
using SharpHook.Reactive.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new ReactiveLogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

SharpHook.R3 包含了 `IR3LogSource` 和 `R3LogSourceAdapter` ，因此你也可以以更具响应性的方式使用它们：

```csharp
using SharpHook.Logging;
using SharpHook.R3.Logging;

// ...

var logSource = LogSource.RegisterOrGet(minLevel: LogLevel.Info);
var reactiveLogSource = new R3LogSourceAdapter(logSource);
reactiveLogSource.MessageLogged.Subscribe(this.OnMessageLogged);
```

### 测试

SharpHook 提供了两个使测试更容易的类。它们不是必需的，因为可以改用模拟对象，但与模拟对象不同，
使用这些类不需要任何设置。

`SharpHook.Testing.TestGlobalHook` 提供了 `IGlobalHook` 和 `IEventSimulator` 的实现，可用于测试。
当调用 `Run` 或 `RunAsync` 方法时，它将使用 `IEventSimulator` 的各种 `Simulate` 方法来发布事件。

如果这个类在被测试的代码中用作 `IEventSimulator` ，那么可以检查 `SimulatedEvents`
属性以查看使用了哪个测试实例模拟了哪些事件。

如果测试需要 `IReactiveGlobalHook`，可以使用 `ReactiveGlobalHookAdapter` 来适配 `TestGlobalHook` 的实例。

如果测试需要 `IR3GlobalHook`，可以使用 `R3GlobalHookAdapter` 来适配 `TestGlobalHook` 的实例。

如果需要模拟 SharpHook 的低级功能，或者希望尽可能将模拟推远，那么可以使用 `SharpHook.Testing.TestProvider`。
它实现了 `SharpHook.Providers` 命名空间中的每个接口，因此可以用它来代替正常的低级功能提供程序。

与 `TestGlobalHook` 一样，这个类可以使用 `PostEvent` 方法发布事件，并在调用 `Run` 时分派它们。
它还包含 `PostedEvents` 属性。

## 仓库状态

如果你注意到这个仓库有一段时间没有新提交了，请放心，它没有被放弃！短期内没有放弃这个仓库的计划。

## 编译源码

为了编译这个库，你首先需要获取 libuiohook 的二进制文件。
你可以从[此仓库的 nightly 获取](https://github.com/TolikPylypchuk/SharpHook/actions/workflows/build.yml)编译，
或者你可以按照 SharpHook 使用的 [libuiohook 分支](https://github.com/TolikPylypchuk/libuiohook)
中的说明自行编译（不推荐，因为它并不简单，而且你很可能应该使用与此仓库中相同编译的选项）。

将二进制文件放入 `SharpHook` 项目中的适当目录，如下表所述：

<table>
  <tr>
    <th>操作系统</th>
    <th>文件</th>
    <th>源目录</th>
    <th>目标目录</th>
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

有了 libuiohook 之后，你可以使用常规方法，例如使用 Visual Studio 或 `dotnet` CLI 编译 SharpHook。
你需要 .NET 9 来编译 SharpHook。

## 图标

图标由 [Freepik](https://www.freepik.com) 制作，来自 [www.flaticon.com](https://www.flaticon.com)。
