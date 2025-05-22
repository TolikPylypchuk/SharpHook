namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal sealed class TaskQueue(int parallelismLevel) : IDisposable
{
#if NET9_0_OR_GREATER
    private readonly Lock syncRoot = new();
#else
    private readonly object syncRoot = new();
#endif

    private readonly SemaphoreSlim semaphore = new(parallelismLevel);

    private bool disposed = false;

    public async void Enqueue(Func<Task> taskGenerator)
    {
        this.ThrowIfDisposed();

        await this.semaphore.WaitAsync();

        try
        {
            await taskGenerator();
        } finally
        {
            lock (this.syncRoot)
            {
                if (!this.disposed)
                {
                    this.semaphore.Release();
                }
            }
        }
    }

    public void Dispose()
    {
        if (!this.disposed)
        {
            lock (this.syncRoot)
            {
                this.disposed = true;
                this.semaphore.Dispose();
            }
        }
    }

    private void ThrowIfDisposed([CallerMemberName] string? method = null)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(
                this.GetType().Name, $"Cannot call {method} - the object is disposed");
        }
    }
}
