using PhoneFileTransfer.Models;

namespace PhoneFileTransfer.Services.FileRemoverService
{
    public interface IFileRemover
    {
        public void ExecuteRemove(bool skipAlreadyRemoved);
        public event EventHandler<string> FileRemoving;
        public event EventHandler RemoveCompleted;
        public void PauseRemove();
        public void ResumeRemove();
        public void CancelRemove();
        public WorkerStatus RemoveStatus { get; }
    }
}