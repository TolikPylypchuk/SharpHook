using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;

using SharpHook.Native;

namespace SharpHook.Reactive
{
    /// <summary>
    /// Represents a simple reactive global hook.
    /// </summary>
    /// <seealso cref="IReactiveGlobalHook" />
    public sealed class SimpleReactiveGlobalHook : IReactiveGlobalHook
    {
        private readonly Subject<HookEvent<HookEventArgs>> hookEnabledSubject = new();
        private readonly Subject<HookEvent<HookEventArgs>> hookDisabledSubject = new();

        private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyTypedSubject = new();
        private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyPressedSubject = new();
        private readonly Subject<HookEvent<KeyboardHookEventArgs>> keyReleasedSubject = new();

        private readonly Subject<HookEvent<MouseHookEventArgs>> mouseClickedSubject = new();
        private readonly Subject<HookEvent<MouseHookEventArgs>> mousePressedSubject = new();
        private readonly Subject<HookEvent<MouseHookEventArgs>> mouseReleasedSubject = new();
        private readonly Subject<HookEvent<MouseHookEventArgs>> mouseMovedSubject = new();
        private readonly Subject<HookEvent<MouseHookEventArgs>> mouseDraggedSubject = new();

        private readonly Subject<HookEvent<MouseWheelHookEventArgs>> mouseWheelSubject = new();

        private bool disposed = false;

        public SimpleReactiveGlobalHook()
        {
            this.HookEnabled = this.hookEnabledSubject.AsObservable().Take(1);
            this.HookDisabled = this.hookDisabledSubject.AsObservable().Take(1);

            this.KeyTyped = this.keyTypedSubject.AsObservable();
            this.KeyPressed = this.keyPressedSubject.AsObservable();
            this.KeyReleased = this.keyReleasedSubject.AsObservable();

            this.MouseClicked = this.mouseClickedSubject.AsObservable();
            this.MousePressed = this.mousePressedSubject.AsObservable();
            this.MouseReleased = this.mouseReleasedSubject.AsObservable();
            this.MouseMoved = this.mouseMovedSubject.AsObservable();
            this.MouseDragged = this.mouseDraggedSubject.AsObservable();

            this.MouseWheel = this.mouseWheelSubject.AsObservable();
        }

        /// <summary>
        /// Unregisteres the global hook if it's registered.
        /// </summary>
        ~SimpleReactiveGlobalHook() =>
            this.Dispose(false);

        /// <summary>
        /// Gets an observable which emits a value when the global hook is enabled.
        /// </summary>
        /// <remarks>
        /// The observable emits a value when the <see cref="Start" /> method is called and then immediately completes.
        /// </remarks>
        public IObservable<HookEvent<HookEventArgs>> HookEnabled { get; }

        /// <summary>
        /// Gets an observable which emits a value when the global hook is disabled.
        /// </summary>
        /// <remarks>
        /// The observable emits a value when the <see cref="IDisposable.Dispose" /> method is called and then
        /// immediately completes.
        /// </remarks>
        public IObservable<HookEvent<HookEventArgs>> HookDisabled { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is typed.
        /// </summary>
        public IObservable<HookEvent<KeyboardHookEventArgs>> KeyTyped { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is pressed.
        /// </summary>
        public IObservable<HookEvent<KeyboardHookEventArgs>> KeyPressed { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is released.
        /// </summary>
        public IObservable<HookEvent<KeyboardHookEventArgs>> KeyReleased { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is clicked.
        /// </summary>
        public IObservable<HookEvent<MouseHookEventArgs>> MouseClicked { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is pressed.
        /// </summary>
        public IObservable<HookEvent<MouseHookEventArgs>> MousePressed { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is released.
        /// </summary>
        public IObservable<HookEvent<MouseHookEventArgs>> MouseReleased { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse cursor is moved.
        /// </summary>
        public IObservable<HookEvent<MouseHookEventArgs>> MouseMoved { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse cursor is dragged.
        /// </summary>
        public IObservable<HookEvent<MouseHookEventArgs>> MouseDragged { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse wheel is scrolled.
        /// </summary>
        public IObservable<HookEvent<MouseWheelHookEventArgs>> MouseWheel { get; }

        /// <summary>
        /// Starts the global hook. The hook can be destroyed by calling the <see cref="IDisposable.Dispose" /> method.
        /// </summary>
        /// <returns>An observable which is completed when the hook is destroyed.</returns>
        /// <exception cref="HookException">Starting the global hook has failed.</exception>
        /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
        /// <remarks>
        /// The hook is started on a separate thread. The returned observable emits a sinlge value and then immediately
        /// completes when the hook is destroyed.
        /// </remarks>
        [SuppressMessage(
            "Design",
            "CA1031:Do not catch general exception types",
            Justification = "No need to rethrow the exception since it's set as the task source's exception")]
        public IObservable<Unit> Start()
        {
            this.ThrowIfDisposed();

            var hookStopped = new Subject<Unit>();

            var thread = new Thread(() =>
            {
                try
                {
                    UioHook.SetDispatchProc(this.DispatchEvent);
                    var result = UioHook.Run();

                    if (result == UioHookResult.Success)
                    {
                        hookStopped.OnNext(Unit.Default);
                        hookStopped.OnCompleted();
                    } else
                    {
                        hookStopped.OnError(new HookException(result, this.FormatFailureMessage("starting", result)));
                    }
                } catch (Exception e)
                {
                    hookStopped.OnError(e);
                }
            });

            thread.Start();

            return hookStopped.AsObservable();
        }

        /// <summary>
        /// Destroys the global hook.
        /// </summary>
        /// <exception cref="HookException">Stopping the hook has failed.</exception>
        /// <remarks>
        /// After calling this method, the hook cannot be started again. If you want to do that, create a new instance
        /// of <see cref="IGlobalHook" />.
        /// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void DispatchEvent(ref UioHookEvent e)
        {
            switch (e.Type)
            {
                case EventType.HookEnabled:
                    this.hookEnabledSubject.OnNext(new HookEvent<HookEventArgs>(this, new(e)));
                    break;
                case EventType.HookDisabled:
                    this.hookDisabledSubject.OnNext(new HookEvent<HookEventArgs>(this, new(e)));
                    break;
                case EventType.KeyTyped:
                    this.keyTypedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                    break;
                case EventType.KeyPressed:
                    this.keyPressedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                    break;
                case EventType.KeyReleased:
                    this.keyReleasedSubject.OnNext(new HookEvent<KeyboardHookEventArgs>(this, new(e)));
                    break;
                case EventType.MouseClicked:
                    this.mouseClickedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                    break;
                case EventType.MousePressed:
                    this.mousePressedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                    break;
                case EventType.MouseReleased:
                    this.mouseReleasedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                    break;
                case EventType.MouseMoved:
                    this.mouseMovedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                    break;
                case EventType.MouseDragged:
                    this.mouseDraggedSubject.OnNext(new HookEvent<MouseHookEventArgs>(this, new(e)));
                    break;
                case EventType.MouseWheel:
                    this.mouseWheelSubject.OnNext(new HookEvent<MouseWheelHookEventArgs>(this, new(e)));
                    break;
            };
        }

        private void Dispose(bool disposing)
        {
            var result = UioHook.Stop();

            if (!this.disposed)
            {
                this.disposed = true;

                this.hookEnabledSubject.OnCompleted();
                this.hookDisabledSubject.OnCompleted();

                this.keyTypedSubject.OnCompleted();
                this.keyPressedSubject.OnCompleted();
                this.keyReleasedSubject.OnCompleted();

                this.mouseClickedSubject.OnCompleted();
                this.mousePressedSubject.OnCompleted();
                this.mouseReleasedSubject.OnCompleted();
                this.mouseMovedSubject.OnCompleted();
                this.mouseDraggedSubject.OnCompleted();

                this.mouseWheelSubject.OnCompleted();
            }

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
    }
}
