namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level feature functionality.
/// </summary>
public interface IFeatureProvider
{
    /// <summary>
    /// Returns optional features of libuiohook that are supported on the current platform.
    /// </summary>
    /// <returns>
    /// Flags which indicate which optional features are supported on the current platform.
    /// </returns>
    UioHookFeature GetOptionalFeatureSupport();
}
