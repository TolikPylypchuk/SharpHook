namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level screen info functionality.
/// </summary>
public interface IScreenInfoProvider
{
    /// <summary>
    /// Gets the information about screens.
    /// </summary>
    /// <returns>The information about screens.</returns>
    ScreenData[] CreateScreenInfo();
}
