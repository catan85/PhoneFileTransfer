
namespace PhoneFileTransfer.Services.MediaDeviceWorker
{
    public interface IMediaDeviceWorker
    {
        Task Enqueue(Func<Task> taskFunc);
        void Stop();
    }
}