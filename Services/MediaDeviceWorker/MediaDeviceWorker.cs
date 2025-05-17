using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.MediaDeviceWorker
{
    public class MediaDeviceWorker : IMediaDeviceWorker
    {
        private readonly BlockingCollection<Func<Task>> _tasks = new();
        private readonly Thread _workerThread;

        public MediaDeviceWorker()
        {
            _workerThread = new Thread(Run)
            {
                IsBackground = true,
                Name = "MediaDeviceWorker"
            };
            _workerThread.Start();
        }

        public Task Enqueue(Func<Task> taskFunc)
        {
            var tcs = new TaskCompletionSource();
            _tasks.Add(async () =>
            {
                try
                {
                    await taskFunc();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        private void Run()
        {
            foreach (var task in _tasks.GetConsumingEnumerable())
            {
                try
                {
                    task().Wait(); // serializza l'esecuzione
                }
                catch
                {
                    // Log, ma non bloccare il thread
                }
            }
        }

        public void Stop()
        {
            _tasks.CompleteAdding();
        }
    }

}
