namespace SharpHook.Native
{
    public delegate void DispatchProc(ref UioHookEvent e);

    public delegate bool LogProc(uint level, string message);
}
