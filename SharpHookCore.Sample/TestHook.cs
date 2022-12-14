using SharpHook;

namespace SharpHookCore.Sample;

public class TestHook
{
    public async Task StartTestHook()
    {
        var hook = new TaskPoolGlobalHook();

        hook.HookEnabled += OnHookEnabled;

        hook.HookDisabled += OnHookDisabled;

        hook.KeyTyped += OnKeyTyped;
        hook.KeyPressed += OnKeyPressed;
        hook.KeyReleased += OnKeyReleased;

        hook.MouseClicked += OnMouseClicked;
        hook.MousePressed += OnMousePressed;
        hook.MouseReleased += OnMouseReleased;
        hook.MouseMoved += OnMouseMoved;
        hook.MouseDragged += OnMouseDragged;

        hook.MouseWheel += OnMouseWheel;

        await hook.RunAsync();
    }

    void OnMouseWheel(object? sender, MouseWheelHookEventArgs e)
    {
        Console.WriteLine("Mouse Wheel");
    }

    void OnMouseDragged(object? sender, MouseHookEventArgs e)
    {
        Console.WriteLine("Mouse Dragged");
    }

    void OnMouseMoved(object? sender, MouseHookEventArgs e)
    {
        Console.WriteLine("Mouse Moved");
    }

    void OnMouseReleased(object? sender, MouseHookEventArgs e)
    {
        Console.WriteLine("Mouse Released");
    }

    void OnMousePressed(object? sender, MouseHookEventArgs e)
    {
        Console.WriteLine("Mouse Pressed");
    }

    void OnMouseClicked(object? sender, MouseHookEventArgs e)
    {
        Console.WriteLine("Mouse Clicked");
    }

    void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        Console.WriteLine("Mouse Released");
    }

    void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        Console.WriteLine("Mouse Pressed");
    }

    void OnKeyTyped(object? sender, KeyboardHookEventArgs e)
    {
        Console.WriteLine("Key Typed");
    }

    void OnHookDisabled(object? sender, HookEventArgs e)
    {
        Console.WriteLine("Hook Disabled");
    }

    void OnHookEnabled(object? sender, HookEventArgs e)
    {
        Console.WriteLine("Hook Enabled");
    }

}
