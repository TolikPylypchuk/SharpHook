namespace SharpHook.Internal;

internal sealed class TaskQueue : IDisposable
{
    private readonly SemaphoreSlim semaphore;

    private bool disposed = false;

    public TaskQueue(int parallelismLevel) =>
        this.semaphore = new(parallelismLevel);

    public async void Enqueue(Func<Task> taskGenerator)
    {
        this.ThrowIfDisposed();

        await this.semaphore.WaitAsync();

        try
        {
            await taskGenerator();
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public void Dispose()
    {
        if (!this.disposed)
        {
            this.semaphore.Dispose();
            this.disposed = true;
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
