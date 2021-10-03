using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using SharpHook.Internal;
using SharpHook.Native;

namespace SharpHook
{
    /// <summary>
    /// Represents a default thread pool-based implementation of the global keyboard and mouse hook.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The event handlers will run sequentially on separate threads inside the default thread pool for tasks. This is
    /// done so that the hook itself will not be blocked if one of the handlers is long-running. The exception is the
    /// <see cref="HookDisabled" /> event which will run on the same thread that called the <see cref="Dispose()" />
    /// method since at that point it doesn't matter anymore that the hook is not blocked.
    /// </para>
    /// <para>
    /// The <see cref="UioHookEvent" /> instance passed to the handlers will be a copy of the original
    /// data passed from libuiohook.
    /// </para>
    /// </remarks>
    public sealed class GlobalHook : IGlobalHook
    {
        private readonly TaskQueue taskQueue = new();

        private bool disposed = false;

        /// <summary>
        /// Unregisteres the global hook if it's registered.
        /// </summary>
        ~GlobalHook() =>
            this.Dispose(false);

        /// <summary>
        /// Starts the global hook. The hook can be destroyed by calling the <see cref="IDisposable.Dispose" /> method.
        /// </summary>
        /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
        /// <exception cref="HookException">Starting the global hook has failed.</exception>
        /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
        /// <remarks>
        /// The hook is started on a separate thread.
        /// </remarks>
        [SuppressMessage(
            "Design",
            "CA1031:Do not catch general exception types",
            Justification = "No need to rethrow the exception since it's set as the task source's exception")]
        public Task Start()
        {
            this.ThrowIfDisposed();

            var source = new TaskCompletionSource<object?>();

            var thread = new Thread(() =>
            {
                try
                {
                    UioHook.SetDispatchProc(this.HandleHookEvent);
                    var result = UioHook.Run();

                    if (result == UioHookResult.Success)
                    {
                        source.SetResult(null);
                    } else
                    {
                        source.SetException(new HookException(result, this.FormatFailureMessage("starting", result)));
                    }
                } catch (Exception e)
                {
                    source.SetException(e);
                }
            });

            thread.Start();

            return source.Task;
        }

        /// <summary>
        /// Destroys the global hook.
        /// </summary>
        /// <exception cref="HookException">Stopping the hook has failed.</exception>
        /// <remarks>
        /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
        /// of <see cref="GlobalHook" />.
        /// </remarks>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.disposed = true;
            }
        }

        private void HandleHookEvent(ref UioHookEvent e)
        {
            if (this.ShouldDispatchEvent(in e))
            {
                if (e.Type != EventType.HookDisabled)
                {
                    var copy = e;
                    this.taskQueue.Enqueue(() => Task.Run(() => this.DispatchEvent(in copy)));
                } else
                {
                    this.DispatchEvent(in e);
                }
            }
        }

        private void DispatchEvent(in UioHookEvent e)
        {
            switch (e.Type)
            {
                case EventType.HookEnabled:
                    this.HookEnabled?.Invoke(this, new HookEventArgs(e));
                    break;
                case EventType.HookDisabled:
                    this.HookDisabled?.Invoke(this, new HookEventArgs(e));
                    break;
                case EventType.KeyPressed:
                    this.KeyPressed?.Invoke(this, new KeyboardHookEventArgs(e));
                    break;
                case EventType.KeyReleased:
                    this.KeyReleased?.Invoke(this, new KeyboardHookEventArgs(e));
                    break;
                case EventType.MouseClicked:
                    this.MouseClicked?.Invoke(this, new MouseHookEventArgs(e));
                    break;
                case EventType.MousePressed:
                    this.MousePressed?.Invoke(this, new MouseHookEventArgs(e));
                    break;
                case EventType.MouseReleased:
                    this.MouseReleased?.Invoke(this, new MouseHookEventArgs(e));
                    break;
                case EventType.MouseMoved:
                    this.MouseMoved?.Invoke(this, new MouseHookEventArgs(e));
                    break;
                case EventType.MouseDragged:
                    this.MouseDragged?.Invoke(this, new MouseHookEventArgs(e));
                    break;
                case EventType.MouseWheel:
                    this.MouseWheel?.Invoke(this, new MouseWheelHookEventArgs(e));
                    break;
            };
        }

        private bool ShouldDispatchEvent(in UioHookEvent e) =>
            e.Type switch
            {
                EventType.HookEnabled => this.HookEnabled != null,
                EventType.HookDisabled => this.HookDisabled != null,
                EventType.KeyTyped => this.KeyTyped != null,
                EventType.KeyPressed => this.KeyPressed != null,
                EventType.KeyReleased => this.KeyReleased != null,
                EventType.MouseClicked => this.MouseClicked != null,
                EventType.MousePressed => this.MousePressed != null,
                EventType.MouseReleased => this.MouseReleased != null,
                EventType.MouseMoved => this.MouseMoved != null,
                EventType.MouseDragged => this.MouseDragged != null,
                EventType.MouseWheel => this.MouseWheel != null,
                _ => false
            };

        private void Dispose(bool disposing)
        {
            var result = UioHook.Stop();

            if (disposing)
            {
                this.taskQueue.Dispose();

                if (result != UioHookResult.Success)
                {
                    throw new HookException(result, this.FormatFailureMessage("stopping", result));
                }
            }
        }

        private void ThrowIfDisposed([CallerMemberName] string? method = null)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(
                    this.GetType().Name, $"Cannot call {method} - the object is disposed");
            }
        }

        private string FormatFailureMessage(string action, UioHookResult result) =>
            $"Failed {action} the global hook: {result} ({(int)result:x})";

        /// <summary>
        /// An event which is raised when the global hook is enabled.
        /// </summary>
        /// <remarks>This event is raised when the <see cref="Start" /> method is called.</remarks>
        public event EventHandler<HookEventArgs>? HookEnabled;

        /// <summary>
        /// An event which is raised when the global hook is disabled.
        /// </summary>
        public event EventHandler<HookEventArgs>? HookDisabled;

        /// <summary>
        /// An event which is raised when a key is typed.
        /// </summary>
        public event EventHandler<KeyboardHookEventArgs>? KeyTyped;

        /// <summary>
        /// An event which is raised when a key is pressed.
        /// </summary>
        public event EventHandler<KeyboardHookEventArgs>? KeyPressed;

        /// <summary>
        /// An event which is raised when a key is released.
        /// </summary>
        public event EventHandler<KeyboardHookEventArgs>? KeyReleased;

        /// <summary>
        /// An event which is raised when a mouse button is clicked.
        /// </summary>
        public event EventHandler<MouseHookEventArgs>? MouseClicked;

        /// <summary>
        /// An event which is raised when a mouse button is pressed.
        /// </summary>
        public event EventHandler<MouseHookEventArgs>? MousePressed;

        /// <summary>
        /// An event which is raised when a mouse button is released.
        /// </summary>
        public event EventHandler<MouseHookEventArgs>? MouseReleased;

        /// <summary>
        /// An event which is raised when the mouse cursor is moved.
        /// </summary>
        public event EventHandler<MouseHookEventArgs>? MouseMoved;

        /// <summary>
        /// An event which is raised when the mouse cursor is dragged.
        /// </summary>
        public event EventHandler<MouseHookEventArgs>? MouseDragged;

        /// <summary>
        /// An event which is raised when the mouse wheel is turned.
        /// </summary>
        public event EventHandler<MouseWheelHookEventArgs>? MouseWheel;
    }
}
