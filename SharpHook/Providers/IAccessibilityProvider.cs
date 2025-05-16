namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level macOS Accessibility API functionality.
/// </summary>
public interface IAccessibilityProvider
{
    /// <summary>
    /// Gets or sets the value which indicates whether global hooks or event simulation should prompt the user when they
    /// try to request access to macOS Accessibility API, and it is disabled. The default value is
    /// <see langword="true" />.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if global hooks and event simulation should prompt the user for access to macOS
    /// Accessibility API when it is disabled. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>
    /// On Windows and Linux, this property does nothing and always returns <see langword="false" />.
    /// </remarks>
    bool PromptUserIfAxApiDisabled { get; set; }

    /// <summary>
    /// Gets or sets the frequency in seconds for polling the access to the macOS Accessibility API when the global hook
    /// is running. The default value is 1 second.
    /// </summary>
    /// <value>
    /// The frequency in seconds for polling the access to the macOS Accessibility API when the global hook is running.
    /// </value>
    /// <remarks>
    /// On Windows and Linux, this property does nothing and always returns <c>0</c>.
    /// </remarks>
    uint AxPollFrequency { get; set; }

    /// <summary>
    /// Checks whether access to macOS Accessibility API is enabled for the process, optionally prompting the user
    /// if it is disabled.
    /// </summary>
    /// <param name="promptUserIfDisabled">Prompt the user if access to macOS Accessibility API is disabled.</param>
    /// <returns>
    /// <see langword="true" /> if access to macOS Accessibility API is enabled for the process which means that
    /// global hooks and event simulation can be used. Otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    /// On Windows and Linux, this method does nothing and always returns <see langword="true" />.
    /// </remarks>
    bool IsAxApiEnabled(bool promptUserIfDisabled);
}
