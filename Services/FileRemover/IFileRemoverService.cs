using PhoneFileTransfer.Models;

namespace PhoneFileTransfer.Services.FileRemover
{
    public interface IFileRemoverService
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