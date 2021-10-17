using System;
using System.Reactive;

namespace SharpHook.Reactive
{
    /// <summary>
    /// Represents a global reactive keyboard and mouse hook.
    /// </summary>
    /// <remarks>
    /// All observables of objects of this interface are completed when the object is disposed, unless stated otherwise.
    /// </remarks>
    /// <seealso cref="IGlobalHook" />
    public interface IReactiveGlobalHook : IDisposable
    {
        /// <summary>
        /// Starts the global hook. The hook can be destroyed by calling the <see cref="IDisposable.Dispose" /> method.
        /// </summary>
        /// <returns>An observable which is completed when the hook is destroyed.</returns>
        /// <remarks>
        /// The returned observable emits a sinlge value and then immediately completes when the hook is destroyed.
        /// </remarks>
        IObservable<Unit> Start();

        /// <summary>
        /// Gets an observable which emits a value when the global hook is enabled.
        /// </summary>
        /// <remarks>
        /// The observable emits a value when the <see cref="Start" /> method is called and then immediately completes.
        /// </remarks>
        IObservable<HookEvent<HookEventArgs>> HookEnabled { get; }

        /// <summary>
        /// Gets an observable which emits a value when the global hook is disabled.
        /// </summary>
        /// <remarks>
        /// The observable emits a value when the <see cref="IDisposable.Dispose" /> method is called and then
        /// immediately completes.
        /// </remarks>
        IObservable<HookEvent<HookEventArgs>> HookDisabled { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is typed.
        /// </summary>
        IObservable<HookEvent<KeyboardHookEventArgs>> KeyTyped { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is pressed.
        /// </summary>
        IObservable<HookEvent<KeyboardHookEventArgs>> KeyPressed { get; }

        /// <summary>
        /// Gets an observable which emits a value when a key is released.
        /// </summary>
        IObservable<HookEvent<KeyboardHookEventArgs>> KeyReleased { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is clicked.
        /// </summary>
        IObservable<HookEvent<MouseHookEventArgs>> MouseClicked { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is pressed.
        /// </summary>
        IObservable<HookEvent<MouseHookEventArgs>> MousePressed { get; }

        /// <summary>
        /// Gets an observable which emits a value when a mouse button is released.
        /// </summary>
        IObservable<HookEvent<MouseHookEventArgs>> MouseReleased { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse cursor is moved.
        /// </summary>
        IObservable<HookEvent<MouseHookEventArgs>> MouseMoved { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse cursor is dragged.
        /// </summary>
        IObservable<HookEvent<MouseHookEventArgs>> MouseDragged { get; }

        /// <summary>
        /// Gets an observable which emits a value when the mouse wheel is scrolled.
        /// </summary>
        IObservable<HookEvent<MouseWheelHookEventArgs>> MouseWheel { get; }
    }
}
