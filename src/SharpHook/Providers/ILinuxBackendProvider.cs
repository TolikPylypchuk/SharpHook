namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level Linux backend functionality.
/// </summary>
public interface ILinuxBackendProvider
{
    /// <summary>
    /// Gets the mode for selecting which Linux backend to load.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns only the selection mode. If you need to get the Linux backend that was actually loaded, then
    /// use <see cref="GetLoadedLinuxBackend()" />.
    /// </para>
    /// <para>
    /// On Windows and macOS, this method always returns <see cref="LinuxMode.AutoXRecord" /> which is the default
    /// value.
    /// </para>
    /// </remarks>
    LinuxMode GetLinuxMode();

    /// <summary>
    /// Sets the mode for selecting which Linux backend to load.
    /// </summary>
    /// <remarks>
    /// This method returns one of three possible values:
    /// <list type="bullet">
    /// <item><see cref="UioHookResult.Success" /> if the Linux mode was successfully set.</item>
    /// <item><see cref="UioHookResult.Failure" /> if the provided mode is an invalid enum value.</item>
    /// <item>
    /// <see cref="UioHookResult.ErrorLinuxLoadBackend" /> if a Linux backend has already been loaded and changing it is
    /// not allowed.
    /// </item>
    /// </list>
    /// </remarks>
    UioHookResult SetLinuxMode(LinuxMode mode);

    /// <summary>
    /// Gets the loaded Linux backend.
    /// </summary>
    /// <remarks>
    /// On Windows and macOS, this method always returns <see cref="LinuxBackend.None" />.
    /// </remarks>
    LinuxBackend GetLoadedLinuxBackend();
}
