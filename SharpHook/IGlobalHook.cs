using System;
using System.Threading.Tasks;

namespace SharpHook
{
    public interface IGlobalHook : IDisposable
    {
        Task Start();

        event EventHandler<KeyboardHookEventArgs> KeyTyped;
        event EventHandler<KeyboardHookEventArgs> KeyPressed;
        event EventHandler<KeyboardHookEventArgs> KeyReleased;

        event EventHandler<MouseHookEventArgs> MouseClicked;
        event EventHandler<MouseHookEventArgs> MousePressed;
        event EventHandler<MouseHookEventArgs> MouseReleased;
        event EventHandler<MouseHookEventArgs> MouseMoved;
        event EventHandler<MouseHookEventArgs> MouseDragged;

        event EventHandler<MouseWheelHookEventArgs> MouseWheel;
    }
}
