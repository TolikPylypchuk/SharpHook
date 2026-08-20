namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level keyboard info functionality.
/// </summary>
public interface IKeyboardInfoProvider
{
    /// <summary>
    /// Gets the key auto-repeat rate.
    /// </summary>
    /// <returns>The key auto-repeat rate.</returns>
    int GetAutoRepeatRate();

    /// <summary>
    /// Gets the key auto-repeat delay.
    /// </summary>
    /// <returns>The key auto-repeat delay.</returns>
    int GetAutoRepeatDelay();
}
