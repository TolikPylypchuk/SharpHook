using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SharpHook.Internal
{
    internal sealed class TaskQueue : IDisposable
    {
        private readonly SemaphoreSlim semaphore = new(1);

        private bool disposed = false;

        public async void Enqueue(Func<Task> taskGenerator)
        {
            this.ThrowIfDisposed();

            await semaphore.WaitAsync();

            try
            {
                await taskGenerator();
            } finally
            {
                semaphore.Release();
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
}
