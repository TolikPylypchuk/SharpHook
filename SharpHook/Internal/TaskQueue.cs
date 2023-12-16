namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal sealed class TaskQueue(int parallelismLevel) : IDisposable
{
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
