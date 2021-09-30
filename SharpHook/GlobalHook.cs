using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using SharpHook.Internal;
using SharpHook.Native;

namespace SharpHook
{
    public sealed class GlobalHook : IGlobalHook
    {
        private readonly TaskQueue taskQueue = new();

        private bool disposed = false;

        ~GlobalHook() =>
            this.Dispose(false);

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
                var copy = e;
                this.taskQueue.Enqueue(() => Task.Run(() => this.DispatchEvent(in copy)));
            }
        }

        private void DispatchEvent(in UioHookEvent e)
        {
            switch (e.Type)
            {
                case EventType.KeyTyped:
                    this.KeyTyped?.Invoke(this, new KeyboardHookEventArgs(e));
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
            if (disposing)
            {
                this.taskQueue.Dispose();
            }

            var result = UioHook.Stop();

            if (disposing && result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatFailureMessage("stopping", result));
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

        public event EventHandler<KeyboardHookEventArgs>? KeyTyped;
        public event EventHandler<KeyboardHookEventArgs>? KeyPressed;
        public event EventHandler<KeyboardHookEventArgs>? KeyReleased;

        public event EventHandler<MouseHookEventArgs>? MouseClicked;
        public event EventHandler<MouseHookEventArgs>? MousePressed;
        public event EventHandler<MouseHookEventArgs>? MouseReleased;
        public event EventHandler<MouseHookEventArgs>? MouseMoved;
        public event EventHandler<MouseHookEventArgs>? MouseDragged;

        public event EventHandler<MouseWheelHookEventArgs>? MouseWheel;
    }
}
