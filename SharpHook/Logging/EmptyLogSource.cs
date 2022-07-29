namespace SharpHook.Logging;

using System;

/// <summary>
/// Represents a souce of libuiohook logs which never raises any events.
/// </summary>
/// <remarks>
/// This class exists primarily to be used instead of <see cref="LogSource" /> in release builds of client apps as they
/// will most probably not need any logs from libuiohook. Also, unlike <see cref="LogSource" /> this class doesn't need
/// the Visual C++ Redistributable package to be installed on Windows.
/// </remarks>
/// <seealso cref="ILogSource" />
/// <seealso cref="LogSource" />
public sealed class EmptyLogSource : ILogSource
{
    /// <summary>
    /// Gets the value which indicates whether the log source is disposed.
    /// </summary>
    /// <value><see langword="true" /> if the log source is disposed. Otherwise, <see langword="false" />.</value>
    /// <remarks>
    /// Since empty log sources don't do anything, it doesn't matter whether they are disposed.
    /// </remarks>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Marks this object as disposed.
    /// </summary>
    /// <remarks>
    /// Since empty log sources don't do anything, it doesn't matter whether they are disposed.
    /// </remarks>
    public void Dispose() =>
        this.IsDisposed = true;

    /// <summary>
    /// An event which is never raised.
    /// </summary>
    public event EventHandler<LogEventArgs>? MessageLogged;
}
