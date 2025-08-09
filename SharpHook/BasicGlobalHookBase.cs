using System.Collections.Concurrent;

#if MACCATALYST
using ObjCRuntime;
#endif

namespace SharpHook;

/// <summary>
/// Represents an abstract implementation of <see cref="IBasicGlobalHook" /> which defines everything needed except for
/// a strategy for handling events and uses a global hook provider (libuiohook by default).
/// </summary>
/// <seealso cref="IGlobalHook" />
/// <seealso cref="GlobalHookBase" />
public abstract class BasicGlobalHookBase : IBasicGlobalHook
{
    private static readonly DispatchProc dispatchProc = HandleHookEvent;
    private static readonly ConcurrentDictionary<nint, BasicGlobalHookBase> runningGlobalHooks = [];

    private static int currentHookIndex = 0;

    private readonly IGlobalHookProvider globalHookProvider;
    private readonly GlobalHookType globalHookType;
    private readonly bool runAsyncOnBackgroundThread;
    private readonly nint hookIndex;

    /// <summary>
    /// Initializes a new instance of <see cref="BasicGlobalHookBase" />.
    /// </summary>
    /// <param name="globalHookType">The global hook type.</param>
    /// <param name="globalHookProvider">
    /// The underlying global hook provider, or <see langword="null" /> to use the default one.
    /// </param>
    /// <param name="runAsyncOnBackgroundThread">
    /// <see langword="true" /> if <see cref="RunAsync" /> should run the hook on a background thread.
    /// Otherwise, <see langword="false" />.
    /// </param>
    [SuppressMessage(
        "Style", "IDE0290:Use primary constructor", Justification = "Primary constructors don't support XML comments")]
    protected BasicGlobalHookBase(
        GlobalHookType globalHookType = GlobalHookType.All,
        IGlobalHookProvider? globalHookProvider = null,
        bool runAsyncOnBackgroundThread = false)
    {
        this.globalHookProvider = globalHookProvider ?? UioHookProvider.Instance;
        this.globalHookType = globalHookType;
        this.runAsyncOnBackgroundThread = runAsyncOnBackgroundThread;
        this.hookIndex = Interlocked.Increment(ref currentHookIndex);
    }

    /// <summary>
    /// Stops the global hook if it's running.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~BasicGlobalHookBase() =>
        this.Dispose(false);

    /// <summary>
    /// Gets the value which indicates whether the global hook is running.
    /// </summary>
    /// <value><see langword="true" /> if the global hook is running. Otherwise, <see langword="false" />.</value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets the value which indicates whether the global hook has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if the global hook has been disposed. Otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>A disposed global hook cannot be started again.</remarks>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Runs the global hook on the current thread, blocking it. The hook can be stopped temporarily by calling the
    /// <see cref="Stop" /> method or stopped permanently by calling the <see cref="Dispose()" /> method.
    /// </summary>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    public void Run()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        this.BeforeRun();

        UioHookResult result;

        try
        {
            runningGlobalHooks[this.hookIndex] = this;
            this.globalHookProvider.SetDispatchProc(dispatchProc, this.hookIndex);

            this.IsRunning = true;
            result = this.RunGlobalHook();
        } catch (Exception e)
        {
            throw new HookException(UioHookResult.Failure, e);
        } finally
        {
            this.IsRunning = false;
            runningGlobalHooks.TryRemove(this.hookIndex, out _);
            this.globalHookProvider.SetDispatchProc(null, this.hookIndex);
        }

        if (result != UioHookResult.Success)
        {
            throw new HookException(result, this.FormatRunFailureMessage(result));
        }
    }

    /// <summary>
    /// Runs the global hook without blocking the current thread. The hook can be stopped temporarily by calling the
    /// <see cref="Stop" /> method or stopped permanently by calling the <see cref="Dispose()" /> method.
    /// </summary>
    /// <returns>A <see cref="Task" /> which finishes when the hook is destroyed.</returns>
    /// <exception cref="HookException">Starting the global hook has failed.</exception>
    /// <exception cref="InvalidOperationException">The global hook is already running.</exception>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>The hook is started on a separate thread.</remarks>
    public Task RunAsync()
    {
        this.ThrowIfRunning();
        this.ThrowIfDisposed();

        this.BeforeRun();

        var source = new TaskCompletionSource<object?>();

        var thread = new Thread(() =>
        {
            try
            {
                runningGlobalHooks[this.hookIndex] = this;
                this.globalHookProvider.SetDispatchProc(dispatchProc, this.hookIndex);

                this.IsRunning = true;
                var result = this.RunGlobalHook();
                this.IsRunning = false;

                if (result == UioHookResult.Success)
                {
                    source.SetResult(null);
                } else
                {
                    source.SetException(new HookException(result, this.FormatRunFailureMessage(result)));
                }
            } catch (Exception e)
            {
                this.IsRunning = false;
                source.SetException(new HookException(UioHookResult.Failure, e));
            } finally
            {
                runningGlobalHooks.TryRemove(this.hookIndex, out _);
                this.globalHookProvider.SetDispatchProc(null, this.hookIndex);
            }
        })
        {
            IsBackground = this.runAsyncOnBackgroundThread
        };

        thread.Start();

        return source.Task;
    }

    /// <summary>
    /// Stops the global hook.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The global hook has been disposed.</exception>
    /// <remarks>
    /// After stopping, the global hook can run again.
    /// </remarks>
    public void Stop()
    {
        this.ThrowIfDisposed();

        if (this.IsRunning)
        {
            var result = this.globalHookProvider.Stop();

            if (result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatStopFailureMessage(result));
            }
        }

        this.AfterStop();
    }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running.
    /// </summary>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    /// <remarks>
    /// After calling this method, the hook cannot run again. If you want to stop the global hook with the ability to
    /// run it again, call the <see cref="Stop" /> method instead.
    /// </remarks>
    public void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// When implemented in a derived class, represents a strategy for handling a hook event.
    /// </summary>
    /// <param name="e">The event to handle.</param>
    protected abstract void HandleHookEvent(ref UioHookEvent e);

    /// <summary>
    /// Defines actions to be done before the global hook is started. The default implementation does nothing, but
    /// it may change in a future version, so calling <see cref="BasicGlobalHookBase.BeforeRun" /> in an overriden
    /// method is recommended.
    /// </summary>
    protected virtual void BeforeRun()
    { }

    /// <summary>
    /// Defines actions to be done after the global hook is stopped. The default implementation does nothing, but
    /// it may change in a future version, so calling <see cref="BasicGlobalHookBase.BeforeRun" /> in an overriden
    /// method is recommended.
    /// </summary>
    protected virtual void AfterStop()
    { }

    /// <summary>
    /// Disposes of the global hook, stopping it if it is running. This method will not be called if the global hook is
    /// already disposed.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true" /> if the method is called from the <see cref="Dispose()" /> method.
    /// Otherwise, <see langword="false" />.
    /// </param>
    /// <exception cref="HookException">Stopping the hook has failed.</exception>
    protected virtual void Dispose(bool disposing)
    {
        this.IsDisposed = true;

        if (this.IsRunning)
        {
            var result = this.globalHookProvider.Stop();

            if (disposing && result != UioHookResult.Success)
            {
                throw new HookException(result, this.FormatStopFailureMessage(result));
            }
        }
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException" /> if this object is disposed.
    /// </summary>
    /// <param name="method">The method which calls this method.</param>
    protected void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.IsDisposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} – the object is disposed");
        }
    }

#if MACCATALYST
    [MonoPInvokeCallback(typeof(DispatchProc))]
#endif
    private static void HandleHookEvent(ref UioHookEvent e, nint hookIndex)
    {
        if (runningGlobalHooks.TryGetValue(hookIndex, out var hook))
        {
            hook.HandleHookEvent(ref e);
        }
    }

    private UioHookResult RunGlobalHook() =>
        this.globalHookType switch
        {
            GlobalHookType.Keyboard => this.globalHookProvider.RunKeyboard(),
            GlobalHookType.Mouse => this.globalHookProvider.RunMouse(),
            _ => this.globalHookProvider.Run()
        };

    private void ThrowIfRunning()
    {
        if (this.IsRunning)
        {
            throw new InvalidOperationException("The global hook is already running");
        }
    }

    private string FormatRunFailureMessage(UioHookResult result) =>
        FormatFailureMessage("running", result);

    private string FormatStopFailureMessage(UioHookResult result) =>
        FormatFailureMessage("stopping", result);

    private string FormatFailureMessage(string action, UioHookResult result) =>
        $"Failed {action} the global hook: {result} ({(int)result:x})";
}
