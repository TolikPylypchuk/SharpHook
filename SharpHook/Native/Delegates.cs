namespace SharpHook.Native
{
    /// <summary>
    /// Represents a function which is called by libuiohook when an event occurs.
    /// </summary>
    /// <param name="e">The event that occured.</param>
    public delegate void DispatchProc(ref UioHookEvent e);

    /// <summary>
    /// Represents a function for logging in libuiohook.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The message to log.</param>
    /// <returns><see langword="true" /> if the message was logged. Otherwise, <see langword="false" /></returns>
    public delegate bool LogProc(uint level, string message);
}
