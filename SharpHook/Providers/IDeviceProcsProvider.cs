namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level device functionality.
/// </summary>
public interface IDeviceProcsProvider
{
    /// <summary>
    /// Sets the functions to open and close privieged devices. This can be used to provide a custom way of opening and
    /// closing devices on Linux without requiring direct file access to them. This is an advanced scenario and should
    /// generally be avoided.
    /// </summary>
    /// <param name="openProc">
    /// The function to use when opening a privileged device, or <see langword="null" /> to use the default method.
    /// </param>
    /// <param name="closeProc">
    /// The function to use when closing a privileged device, or <see langword="null" /> to use the default method.
    /// </param>
    /// <param name="userData">
    /// The data to pass to the device functions.
    /// </param>
    /// <remarks>
    /// These functions are used only on Linux, and only when the loaded backend uses libinput and uinput.
    /// </remarks>
    void SetDeviceProcs(OpenDeviceProc? openProc, CloseDeviceProc? closeProc, nint userData);
}
