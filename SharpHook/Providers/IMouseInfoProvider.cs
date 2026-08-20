namespace SharpHook.Providers;

/// <summary>
/// Represents a provider of low-level mouse info functionality.
/// </summary>
public interface IMouseInfoProvider
{
    /// <summary>
    /// Gets the pointer acceleration multiplier.
    /// </summary>
    /// <returns>The pointer acceleration multiplier.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that
    /// method returns <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is
    /// supported. Otherwise, this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    int GetPointerAccelerationMultiplier();

    /// <summary>
    /// Gets the pointer acceleration threshold.
    /// </summary>
    /// <returns>The pointer acceleration threshold.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that
    /// method returns <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is
    /// supported. Otherwise, this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    int GetPointerAccelerationThreshold();

    /// <summary>
    /// Gets the pointer sensitivity.
    /// </summary>
    /// <returns>The pointer sensitivity.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that
    /// method returns <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is
    /// supported. Otherwise, this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    int GetPointerSensitivity();

    /// <summary>
    /// Gets the multi-click time.
    /// </summary>
    /// <returns>The multi-click time.</returns>
    /// <remarks>
    /// Support for this method can be queried using <see cref="IFeatureProvider.GetOptionalFeatureSupport" />. If that
    /// method returns <see cref="UioHookFeature.PointerProperties" /> as one of its flags, then this method is
    /// supported. Otherwise, this method always returns <c>-1</c>. On Wayland, this feature is not supported.
    /// </remarks>
    int GetMultiClickTime();
}
