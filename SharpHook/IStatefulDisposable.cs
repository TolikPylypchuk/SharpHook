namespace SharpHook;

/// <summary>
/// Provides a mechanism for disposing of resources and tracking the disposal state.
/// </summary>
public interface IStatefulDisposable : IDisposable
{
    /// <summary>
    /// Gets a value which indicates whether the object has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the object has been disposed. Otherwise, <see langword="false" />.
    /// </value>
    bool IsDisposed { get; }
}
