namespace SharpHook;

using System;
using System.Threading.Tasks;

/// <summary>
/// Represents a global keyboard and mouse hook.
/// </summary>
/// <seealso cref="GlobalHookBase" />
/// <seealso cref="SimpleGlobalHook" />
/// <seealso cref="TaskPoolGlobalHook" />
public interface IGlobalHook : IDisposable
{
    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    bool IsRunning { get; }

    /// <summary>
    /// Gets the value which indicates whether the global hook is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>A disposed global hook cannot be started again.</remarks>
    bool IsDisposed { get; }

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    void Run();

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be destroyed by calling the
    /// <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// Since the underlying native API for running a global hook is blocking, the only way to run it without blocking
    /// the current thread is to run it on a separate thread.
    /// </remarks>
    Task RunAsync();

    /// <summary>
    /// An event which is raised when the global hook is enabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="Run" /> or <see cref="RunAsync" /> method is called.</remarks>
    event EventHandler<HookEventArgs> HookEnabled;

    /// <summary>
    /// An event which is raised when the global hook is disabled.
    /// </summary>
    /// <remarks>This event is raised when the <see cref="IDisposable.Dispose" /> method is called.</remarks>
    event EventHandler<HookEventArgs> HookDisabled;

    /// <summary>
    /// An event which is raised when a key is typed.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyTyped;

    /// <summary>
    /// An event which is raised when a key is pressed.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyPressed;

    /// <summary>
    /// An event which is raised when a key is released.
    /// </summary>
    event EventHandler<KeyboardHookEventArgs> KeyReleased;

    /// <summary>
    /// An event which is raised when a mouse button is clicked.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseClicked;

    /// <summary>
    /// An event which is raised when a mouse button is pressed.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MousePressed;

    /// <summary>
    /// An event which is raised when a mouse button is released.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseReleased;

    /// <summary>
    /// An event which is raised when the mouse cursor is moved.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseMoved;

    /// <summary>
    /// An event which is raised when the mouse cursor is dragged.
    /// </summary>
    event EventHandler<MouseHookEventArgs> MouseDragged;

    /// <summary>
    /// An event which is raised when the mouse wheel is scrolled.
    /// </summary>
    event EventHandler<MouseWheelHookEventArgs> MouseWheel;
}
